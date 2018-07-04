﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Specialized;

namespace AquaMasters_Billing_App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public Dictionary<string, Part> PriceSheet;
        public string savePath;
        public ObservableCollection<PurchaseSet> partsList;
        public ObservableCollection<PurchaseSet> laborList;

        public MainWindow()
        {
            InitializeComponent();
            InitializeBackendData();
            InitializeFrontendData();

        }

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


        

        // Save to file example
        private void Button_Click(object sender, RoutedEventArgs e)
        {
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

        private void REMOVE_ME_Click(object sender, RoutedEventArgs e)
        {

            List<searchAndAdd.partOrder> newParts = new List<searchAndAdd.partOrder>();
            Dictionary<string, Part> updates = new Dictionary<string, Part>();

            searchAndAdd search = new searchAndAdd(PriceSheet);
            if (search.ShowDialog().Value) {
                newParts = search.cart;
                updates = search.masterPriceList;

                writeToFile(updates);

            }
        }

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

        private void listUpdated(object sender, NotifyCollectionChangedEventArgs a) => UpdateTotals();

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
            string cost = (Decimal.Parse(this.SubtotalCostTB.Text) * 1.0635m).ToString("0.00");
            TaxTB.Text = (Decimal.Parse(this.SubtotalCostTB.Text) * .0635m).ToString("0.00");
            this.TotalCostTB.Text = cost;

        }

        private void writeToFile(Dictionary<string, Part> updates) {

            //Write an object to a json formatted string in a file
            Part json = new Part();

            json.setCost(5.00);
            json.setName("Shock");
            json.setType("Chem");

            string strPath = Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
            strPath = strPath + "\\Aquamasters\\test.ini";

            string jsonData = "";

            foreach (Part part in updates.Values) {
                jsonData += JsonConvert.SerializeObject(part, Formatting.None) + "\n";
            }
            System.IO.File.WriteAllText(strPath, jsonData);
        }

        private void OpeningButton_Click(object sender, RoutedEventArgs e) {

            List<String> newParts;
            List<decimal> newQuants;
            addOpening opening = new addOpening();

            if (opening.ShowDialog().Value) {
                newParts = opening.parts;
                newQuants = opening.quants;
                AddPartsStrings(newParts, newQuants);
            }


            
        }

        private void InitialButton_Click(object sender, RoutedEventArgs e) {

            List<String> newParts;
            List<decimal> newQuants;
            addInitial initial = new addInitial();

            if (initial.ShowDialog().Value) {
                newParts = initial.parts;
                newQuants = initial.quants;
                AddPartsStrings(newParts, newQuants);
            }


        }

        private void VacButton_Click(object sender, RoutedEventArgs e) {
            List<String> newParts;
            List<decimal> newQuants;
            addVac vac = new addVac();

            if (vac.ShowDialog().Value) {
                newParts = vac.parts;
                newQuants = vac.quantities;
                AddPartsStrings(newParts, newQuants);
            }
        }

        private void LaborDG_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e) {
            
            if (LaborDG.SelectedItem != null) {
                (sender as DataGrid).CellEditEnding -= LaborDG_CellEditEnding;
                (sender as DataGrid).CommitEdit();
                (sender as DataGrid).CommitEdit();
                (sender as DataGrid).Items.Refresh();
                (sender as DataGrid).CellEditEnding += LaborDG_CellEditEnding;
            }

            UpdateTotals();
       
        }

        private void PartsDG_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e) {

            if (PartsDG.SelectedItem != null) {
                (sender as DataGrid).CellEditEnding -= PartsDG_CellEditEnding;
                (sender as DataGrid).CommitEdit();
                (sender as DataGrid).CommitEdit();
                (sender as DataGrid).Items.Refresh();
                (sender as DataGrid).CellEditEnding += PartsDG_CellEditEnding;
            }

            UpdateTotals();
        }
    }


    /**
     * Part: the basic framework for a part or chem
     *      has a name, type and cost.
     *      fields are public to allow JsonConvert
     *      to serialize the object, and DataGrid to 
     *      Autopopulate the row fields
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
