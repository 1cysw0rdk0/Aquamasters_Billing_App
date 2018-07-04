using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Specialized;

namespace AquaMasters_Billing_App {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        public Dictionary<string, Part> PriceSheet;
        public string savePath;
        public ObservableCollection<PurchaseSet> partsList;
        public ObservableCollection<PurchaseSet> laborList;

        /**
         * Constructor for the main window
         * Initializes the components that the users sees
         * and all the supporting data 
         */
        public MainWindow() {
            InitializeComponent();
            InitializeBackendData();
            InitializeFrontendData();

        }

        /**
         * Initializes the backend data, including
         *      - The save path
         *      - Opening and reading the config file
         *      - Creating and filling the price list
         *      - Creating and binding data lists to data grids
         */
        private void InitializeBackendData() {

            // Generate save path for this computer
            this.savePath = Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
            this.savePath += "\\Aquamasters\\test.ini";

            // Initialize file to read text
            StreamReader file = File.OpenText(this.savePath);

            // Initialize price sheet with new empty dictionary
            this.PriceSheet = new Dictionary<string, Part>();

            // Read every line from the file, desearializing into parts and adding to dictonary
            while (!file.EndOfStream)
            {
                Part part = (Part)JsonConvert.DeserializeObject(file.ReadLine(), typeof(Part));
                this.PriceSheet.Add(part.name, part);
            }

            // Create and Bind data lists
            this.partsList = new ObservableCollection<PurchaseSet>();
            this.PartsDG.ItemsSource = this.partsList;

            this.laborList = new ObservableCollection<PurchaseSet>();
            this.LaborDG.ItemsSource = this.laborList;

        }

        /**
         * Initialize Frontend Data, including:
         *      - The columns for parts
         *      - The properties for parts columns
         *      - The columns for labor
         *      - The properties for labor columns
         *      - Set the update cost function to run on list update
         */
        private void InitializeFrontendData() {

            // Initialize columns for the parts display
            this.PartsDG.Columns.Add(new DataGridTextColumn { Header = "Quantity", Binding = new Binding("quantity"), Width = 56 });
            this.PartsDG.Columns.Add(new DataGridTextColumn { Header = "Name", Binding = new Binding("part.name"), Width = 242, IsReadOnly = true });
            this.PartsDG.Columns.Add(new DataGridTextColumn { Header = "Cost", Binding = new Binding("part.cost"), Width = 50, IsReadOnly = true});

            // Initialize columns for the labor display
            this.LaborDG.Columns.Add(new DataGridTextColumn { Header = "Quantity", Binding = new Binding("quantity"), Width = 56 });
            this.LaborDG.Columns.Add(new DataGridTextColumn { Header = "Service", Binding = new Binding("part.name"), Width = 242, IsReadOnly = true });
            this.LaborDG.Columns.Add(new DataGridTextColumn { Header = "Rate", Binding = new Binding("part.cost"), Width = 50, IsReadOnly = true });

            //Assign update function to both data lists
            laborList.CollectionChanged += listUpdated;
            partsList.CollectionChanged += listUpdated;


        }
        
        /// <summary>
        /// 
        /// AddPartsStrings - Adds parts to the cart from a list of strings
        /// 
        ///     This function is called after each window closes, and passes it a list
        ///     of all parts to be added, formatted as a list of strings, and a list of 
        ///     quantities, in a synchronous order.
        /// 
        /// NOTE: This method is marked to be reworked, passing two lists is terrible form!
        /// 
        /// </summary>
        /// 
        /// <param name="parts">List of part names to be added.</param>
        /// <param name="quants">List of quantities of the parts to be added.</param>
        private void AddPartsStrings(List<String> parts, List<decimal> quants) {

            for (int i = 0; i < parts.Count; i++) {

                String part = parts[i];
                Decimal quant = quants[i];

                if (this.PriceSheet.ContainsKey(part)) {
                    this.PriceSheet.TryGetValue(part, out Part temp);

                    AddPart(new Part(Decimal.Parse(temp.cost), temp.name, temp.type), quant);
                } else {
                    //Error handling?
                }
            }

        }

        /// <summary>
        /// 
        /// AddPart - Adds a single part to the correct list, when given the part object
        /// 
        ///     and a quantity of the item to add.
        ///     
        /// </summary>
        /// 
        /// <param name="newPart">The new part to add.</param>
        /// <param name="quantity">The quantity of that part to add.</param>
        private void AddPart(Part newPart, decimal quantity) {

            PurchaseSet purchaseSet = new PurchaseSet { part = newPart, quantity = quantity };

            if (purchaseSet.part.type.Equals("Labor") || purchaseSet.part.type.Equals("Service")) {
                this.laborList.Add(purchaseSet);
            } else {
                this.partsList.Add(purchaseSet);
            }

            this.PartsDG.Items.Refresh();
            this.LaborDG.Items.Refresh();

            /**
            // Uncomment this code block to condense all items of a single name into one block. otherwise, they will
            // be added as separate items to allow for easier comparison to the actual billing chart. 
            
            if (this.PartsDG.Items.Contains(purchaseSet)) {

                //Increase the quantity of this item should it already exist in the list.
                int index = this.PartsDG.Items.IndexOf(purchaseSet);
                PurchaseSet old = (PurchaseSet)this.PartsDG.Items.GetItemAt(index);
                old.quantity += purchaseSet.quantity;

                //Tempting... but that's bad style :'(
                //((PurchaseSet)this.PartsDG.Items.GetItemAt(this.PartsDG.Items.IndexOf(purchaseSet))).quantity += purchaseSet.quantity;

            }
            */

            UpdateTotals();
        }

        /// <summary>
        /// 
        /// listUpdated - Runs when a change is made to either the labor or parts lists.
        /// 
        ///     - Calls the updateTotals function to update the cost boxes.
        ///     
        /// </summary>
        /// 
        /// <param name="sender">Unused.</param>
        /// <param name="a">Unused.</param>
        private void listUpdated(object sender, NotifyCollectionChangedEventArgs a) => UpdateTotals();

        /// <summary>
        /// 
        /// updateTotals - Updates the textbox displays for the final sums
        /// 
        ///     - Loops through each item in the parts list
        ///         - Multiplies quantity by item cost
        ///         - Adds to running total
        ///     - Updates PartCostTB
        ///     - Loops through each item in the labor list
        ///         - Multiplies quantity by item cost
        ///         - Adds to running total
        ///     - Updates LaborCostTB
        ///     - Adds PartCostTB and LaborCostTB to set the SubtotalCostTB
        ///     - Sets the TotalCostTB to the SubtotalTB times tax (1.0635 as of 7/3/2018)
        ///     - Sets the TaxTB to the cost of the tax (.0635 as of 7/3/2018)
        ///     
        /// NOTE: This method is marked to be updated such that tax is not a static number
        /// 
        /// </summary>
        private void UpdateTotals() {

            decimal runningTotal = 0;
            foreach (PurchaseSet purchase in this.partsList) {
                runningTotal += Decimal.Parse(purchase.part.cost) * purchase.quantity;
            }
            this.PartCostTB.Text = runningTotal.ToString("0.00");

            runningTotal = 0;
            foreach (PurchaseSet purchase in this.laborList) {
                runningTotal += Decimal.Parse(purchase.part.cost) * purchase.quantity;
            }
            this.LaborCostTB.Text = runningTotal.ToString("0.00");

            this.SubtotalCostTB.Text = (Decimal.Parse(this.PartCostTB.Text) + Decimal.Parse(this.LaborCostTB.Text)).ToString("0.00");
            this.TotalCostTB.Text = (Decimal.Parse(this.SubtotalCostTB.Text) * 1.0635m).ToString("0.00");
            TaxTB.Text = (Decimal.Parse(this.SubtotalCostTB.Text) * .0635m).ToString("0.00");
        }

        /// <summary>
        /// 
        /// writeToFile - Saves changes made to the price list to the file
        /// 
        ///     - Creates an empty string for the file data
        ///     - Loops through every item in the price list
        ///         - Serializes the object into a json string
        ///         - Adds a newline character
        ///         - Adds to the end of the json string
        ///     - Writes json string to file
        ///     
        /// </summary>
        /// 
        /// <param name="updates">The list of parts to write to file.</param>
        private void writeToFile(Dictionary<string, Part> updates) {

            string jsonData = "";

            foreach (Part part in updates.Values) {
                jsonData += JsonConvert.SerializeObject(part, Formatting.None) + "\n";
            }
            System.IO.File.WriteAllText(savePath, jsonData);
        }

        /// <summary>
        /// 
        /// OpeningButton_Click - Handles the event that fires when the opening button is clicked
        /// 
        ///     - Constructs the new opening window
        ///     - Displays the new window
        ///         - Upon success Save the returned lists to the part lists
        ///        
        /// </summary>
        /// <param name="sender">Unused.</param>
        /// <param name="e">Unused.</param>
        private void OpeningButton_Click(object sender, RoutedEventArgs e) {

            addOpening opening = new addOpening();

            if (opening.ShowDialog().Value) {
                AddPartsStrings(opening.parts, opening.quants);
            }
        }

        /// <summary>
        /// 
        /// InitialButton_Click - Handles the event that fires when the initial button is clicked
        /// 
        ///     - Constructs the new initial window
        ///     - Displays the new window
        ///         - Upon success Save the returned lists to the part lists
        ///     
        /// </summary>
        /// <param name="sender">Unused.</param>
        /// <param name="e">Unused.</param>
        private void InitialButton_Click(object sender, RoutedEventArgs e) {

            addInitial initial = new addInitial();

            if (initial.ShowDialog().Value) {
                AddPartsStrings(initial.parts, initial.quants);
            }
        }

        /// <summary>
        /// 
        /// VacButton_Click - Handles the event that fires when the vac button is clicked
        /// 
        ///     - Constructs the new vac window
        ///     - Displays the new window
        ///         - Upong success save the returned lists to the part lists
        ///         
        /// </summary>
        /// 
        /// <param name="sender">Unused.</param>
        /// <param name="e">Unused.</param>
        private void VacButton_Click(object sender, RoutedEventArgs e) {

            addVac vac = new addVac();

            if (vac.ShowDialog().Value) { 
                AddPartsStrings(vac.parts, vac.quantities);
            }
        }

        /// <summary>
        /// 
        /// searchAndAddB_Click - Handles the event that fires when the search button is clicked
        /// 
        ///     
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void searchAndAddB_Click(object sender, RoutedEventArgs e) {

            searchAndAdd search = new searchAndAdd(PriceSheet);

            if (search.ShowDialog().Value) {
                
                writeToFile(search.masterPriceList);

                foreach (searchAndAdd.partOrder partOrder in search.cart) {
                    AddPart(partOrder.part, partOrder.quantity);
                }
            }
        }
    }

    /**
     * Part: the basic framework for a part or chem
     *      has a name, type and cost.
     *      fields are public to allow JsonConvert
     *      to serialize the object
     */
    public class Part {

        public string name { set; get; }
        public string type { set; get; }
        public string cost { set; get; }

        public string getCost() => this.cost;
        public string  getName() => this.name;
        public string  getType() => this.type;

        public void setCost(double cost) => this.cost = cost.ToString();
        public void setName(string  name) => this.name = name;
        public void setType(string  type) => this.type = type;


        public Part(decimal cost, string name, string type)
        {
            this.cost = cost.ToString("0.00");
            this.name = name;
            this.type = type;
        }

        public Part() { }
    }

    public class PurchaseSet {

        public Part part { set; get; }
        public decimal quantity { set; get; }

        public override bool Equals(Object test) {

            if (test == null || GetType() != test.GetType()) { return false; }

            PurchaseSet purchase = (PurchaseSet)test;
            return (this.part.name == purchase.part.name);
        }

        public override int GetHashCode() => base.GetHashCode();
    }
}

/**
        * // Example code demonstrating how to save a single part object to a json file
        private void Button_Click(object sender, RoutedEventArgs e) {

            //Write an object to a json formatted string in a file
            Part json = new Part();
        
            json.setCost(5.00);
            json.setName("Shock");
            json.setType("Chem");

            string strPath = Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
            strPath = strPath + "\\Aquamasters\\test.ini";

            string jsonData = JsonConvert.SerializeObject(json, Formatting.None);
            System.IO.File.WriteAllText(strPath, jsonData);
        }
        */