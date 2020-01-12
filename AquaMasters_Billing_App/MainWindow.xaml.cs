using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Specialized;
using MySql.Data.MySqlClient;


namespace AquaMasters_Billing_App {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        public Dictionary<string, Part> PriceSheet;
        public string savePath;
        public ObservableCollection<PurchaseSet> partsList;
        public ObservableCollection<PurchaseSet> laborList;
        public ObservableCollection<CustomerRecord> customerRecords;
        public ObservableCollection<ServiceRecord> serviceRecords;
        private String poolID;

		private String databaseAddress = "localhost";
		private String databaseUser = "root";
		private String databaseName = "aquamastersservice";
		private String databasePass;
		private String Connection_String;


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
         *      - Querying the database for customer list
         *      - Adding customer records to the data grid
         */
        private void InitializeBackendData() {

			// Gather Database Server Information
			//String Connection_String = @"server=localhost;userid=root;password=;database=aquamastersservice";


            // Generate save paths for this computer
            this.savePath = Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
            this.savePath += "\\Aquamasters\\";

			string priceList = this.savePath + "priceSheet.ini";
			string prefs = this.savePath + "conf.ini";


			// Initialize price list file to read text
			// TODO replace with a database call
			try {
                StreamReader file = File.OpenText(priceList);
                // Initialize price sheet with new empty dictionary
                this.PriceSheet = new Dictionary<string, Part>();

                // Read every line from the file, desearializing into parts and adding to dictonary
                while (!file.EndOfStream) {
                    Part part = (Part)JsonConvert.DeserializeObject(file.ReadLine(), typeof(Part));
                    this.PriceSheet.Add(part.name, part);
                }
            } catch {
                Console.Error.WriteLine("Unable to open price list file.");
            }


			// Initialize conf file to read text
			try {
				StreamReader file = File.OpenText(prefs);
				
				if (!file.EndOfStream) {
					databaseAddress = file.ReadLine();
				}

				if (!file.EndOfStream) {
					databaseUser = file.ReadLine();
				}

				// Additional Preference Parsing Here

			} catch {
				Console.Out.WriteLine("Unable to open conf file. Generating blank.");
				File.Create(prefs);
			}


			// Query user for missing server info
			serverInfo dialog = new serverInfo(databaseAddress, databaseUser);
			if ((bool)dialog.ShowDialog()) {
				this.databaseAddress = dialog.addressTB.Text.Trim();
				this.databaseUser = dialog.usernameTB.Text;
				this.databasePass = dialog.passwordPB.Password;
			}



			// Create and Bind data lists
			this.partsList = new ObservableCollection<PurchaseSet>();
            this.PartsDG.ItemsSource = this.partsList;

            this.laborList = new ObservableCollection<PurchaseSet>();
            this.LaborDG.ItemsSource = this.laborList;

            updateCustomerList();

        }

        /**
         * Initialize Frontend Data, including:
         *      - The columns for parts
         *      - The properties for parts columns
         *      - The columns for labor
         *      - The properties for labor columns
         *      - The columns for customers
         *      - The properties for customer columns
         *      - Set the update cost function to run on list update
         *      - Set the update customer data function 
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

            // Initialize columns for the customer display
            this.customerDB.Columns.Add(new DataGridTextColumn { Header = "Last Name", Binding = new Binding("lastName"), Width = 135 });
            this.customerDB.Columns.Add(new DataGridTextColumn { Header = "Town", Binding = new Binding("city"), Width = 110 });
            this.customerDB.Columns.Add(new DataGridTextColumn { Header = "Phone", Binding = new Binding("phone"), Width = 140 });

            // Initialize columns for the records display
            this.recordsDB.Columns.Add(new DataGridTextColumn { Header = "Date", Binding = new Binding("date"), Width = 50 });
            this.recordsDB.Columns.Add(new DataGridTextColumn { Header = "", Binding = new Binding(), Width = 50 });
            this.recordsDB.Columns.Add(new DataGridTextColumn { Header = "", Binding = new Binding(), Width = 50 });

            //Assign update function to both data lists
            this.laborList.CollectionChanged += this.listUpdated;
            this.partsList.CollectionChanged += this.listUpdated;
            this.customerDB.SelectedCellsChanged += this.customerSelected;

        }

        private void updateCustomerList() {

            this.customerRecords = new ObservableCollection<CustomerRecord>();
            this.customerDB.ItemsSource = this.customerRecords;

            this.serviceRecords = new ObservableCollection<ServiceRecord>();
            this.recordsDB.ItemsSource = this.serviceRecords;

            try {

                // Connect to database and pull list of all customers
                String query = "SELECT CustID, LastName, FirstName, Address, Town, Phone, ZipCode, AltPhone1, AltPhone2 FROM customers;";

				using (MySqlConnection conn = new MySqlConnection("server=" + databaseAddress + ";userid=" + databaseUser + ";password=" + databasePass + ";database=" + databaseName)) {
					MySqlCommand comm = new MySqlCommand(query, conn);
                    conn.Open();
                    MySqlDataReader read = comm.ExecuteReader();

                    try {
                        while (read.Read()) {
                            this.customerRecords.Add(new CustomerRecord(read["FirstName"].ToString(), read["LastName"].ToString(), read["Address"].ToString(), read["Town"].ToString(), read["ZipCode"].ToString(), read["CustID"].ToString(), read["Phone"].ToString(), read["AltPhone1"].ToString(), read["AltPhone2"].ToString()));

                        }
                    } finally { read.Close(); }

                }

            } catch {
                Console.Error.WriteLine("Unable to contact database!");
            }

        }

        /// <summary>
        /// 
        /// customerSelected - Fills in appropriate boxes with customer information
        /// 
        ///     This function is called whenever a new customer record is selected in 
        ///     the datagrid. The database is queried, and the pool data for that 
        ///     customer is pulled from the database, and is used to fill in the correct
        ///     boxes. 
        ///     In the event that multiple customers are selected, the last customer selected
        ///     is chosen to be used to fill in information.
        /// 
        /// </summary>
        /// 
        /// <param name="sender">Heck if I know man. I'm still learning this stuff. </param>
        /// <param name="e">The event args passed when a new cell is selected. </param>
        public void customerSelected(object sender, SelectedCellsChangedEventArgs e) {
            if (e.AddedCells.Count < 1) {

                custNameTB.Text = "";
                custCityTB.Text = "";
                custAddressTB.Text = "";
                custAltPhone1TB.Text = "";
                custAltPhone2TB.Text = "";
                custPhoneTB.Text = "";
                custZipCodeTB.Text = "";

                poolDimTB.Text = "";
                poolCoverTB.Text = "";
                poolMainDrainTB.Text = "";
                poolReturnsTB.Text = "";
                poolSkimmersTB.Text = "";
                poolSpaTB.Text = "";
                poolHeaterTB.Text = "";
                poolConstructionTB.Text = "";
                poolPumpTB.Text = "";
                poolFilterTB.Text = "";
                poolID = "";
                return;
            } 

            CustomerRecord customer = (CustomerRecord)e.AddedCells[0].Item;

            custNameTB.Text = customer.firstName + customer.lastName;
            custCityTB.Text = customer.city;
            custAddressTB.Text = customer.address;
            custAltPhone1TB.Text = customer.altphone;
            custAltPhone2TB.Text = customer.altphone2;
            custPhoneTB.Text = customer.phone;
            custZipCodeTB.Text = customer.zip;


            // Pull Pool Info
            String query = "SELECT PoolID, Dimensions, Construction, Cover, Spa, Heater, Returns, Skimmers, MainDrains, Pumps, BigL, Turbo, AutoFills, Controller, WaterFeatrues, LeafCatches, BuddaJet, SaltGenerator, Notes FROM pools WHERE customers_CustID = @CustID";
            using (MySqlConnection conn = new MySqlConnection("server=" + databaseAddress + ";userid=" + databaseUser + ";password=" + databasePass + ";database=" + databaseName)) {

                // Set up SQL query
                MySqlCommand command = new MySqlCommand(query, conn);
                command.Parameters.AddWithValue("@CustID", customer.id);
                conn.Open();
                MySqlDataReader reader = command.ExecuteReader();

                // Parse Response and set text boxes
                try {
                    while (reader.Read()) {
                        String main, skim, ret, spa, heater;

                        if (reader["MainDrains"].ToString() == "-1") {  main = "Unknown"; } else {  main = reader["MainDrains"].ToString(); }
                        if (reader["Skimmers"].ToString() == "-1") {  skim = "Unknown"; } else {  skim = reader["Skimmers"].ToString(); }
                        if (reader["Returns"].ToString() == "-1") {  ret = "Unknown"; } else {  ret = reader["Returns"].ToString(); }
                        if (reader["Spa"].ToString() == "-1") { spa = "Unknown"; } else if (reader["Spa"].ToString() == "1") { spa = "Yes"; } else if (reader["Spa"].ToString() == "0") { spa = "No"; } else {  spa = reader["Spa"].ToString(); }
                        if (reader["Heater"].ToString() == "-1") {  heater = "Unknown"; } else if (reader["Heater"].ToString() == "1") { heater = "Yes"; } else if (reader["Heater"].ToString() == "0") { heater = "No"; } else {  heater = reader["Heater"].ToString(); }

                        poolDimTB.Text = reader["Dimensions"].ToString();
                        poolCoverTB.Text = reader["Cover"].ToString();
                        poolMainDrainTB.Text = main;
                        poolReturnsTB.Text = ret;
                        poolSkimmersTB.Text = skim;
                        poolSpaTB.Text = spa;
                        poolHeaterTB.Text = heater;
                        poolConstructionTB.Text = reader["Construction"].ToString();
                        poolPumpTB.Text = reader["Pumps"].ToString();
                        poolFilterTB.Text = "Unknown";
                        poolID = reader["PoolID"].ToString();
                    }

                } finally { reader.Close(); }
            }

            // Pull Service History
            query = "SELECT * FROM service_records WHERE pools_PoolID = @PoolID";
            using (MySqlConnection conn = new MySqlConnection("server=" + databaseAddress + ";userid=" + databaseUser + ";password=" + databasePass + ";database=" + databaseName)) {

                // Set up SQL Query
                MySqlCommand command = new MySqlCommand(query, conn);
                command.Parameters.AddWithValue("@PoolID", poolID);
                conn.Open();
                MySqlDataReader reader = command.ExecuteReader();

                // Parse Response and populate datagrid
                try {
                    while (reader.Read()) {
                        this.serviceRecords.Add(new ServiceRecord((int)reader["RecordID"], (int)reader["customers_CustID"], poolID, (int)reader["Day"], (int)reader["Month"], (int)reader["Year"], reader["Description"].ToString(), reader["WorkType"].ToString(), reader["PartsList"].ToString(), reader["LaborList"].ToString()));

                    }
                } finally { reader.Close(); }
            }

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
        ///     - Construct the new search window
        ///     - Displays the new window
        ///     - Calls writeToFile to save changes
        ///     - Add each part to the parts lists
        ///     
        /// </summary>
        /// 
        /// <param name="sender">Unused.</param>
        /// <param name="e">Unused.</param>
        private void searchAndAddB_Click(object sender, RoutedEventArgs e) {

            searchAndAdd search = new searchAndAdd(PriceSheet);

            if (search.ShowDialog().Value) {
                
                writeToFile(search.masterPriceList);

                foreach (searchAndAdd.partOrder partOrder in search.cart) {
                    AddPart(partOrder.part, partOrder.quantity);
                }
            }
        }

        /// <summary>
        /// 
        /// Clear_Click - Handles the event that fires what the clear button is clicked
        /// 
        ///     - Clears both parts and labor lists
        ///     - Both lists automatically update the cost
        ///     
        /// </summary>
        /// 
        /// <param name="sender">Unused.</param>
        /// <param name="e">Unused.</param>
        private void Clear_Click(object sender, RoutedEventArgs e) {
            partsList.Clear();
            laborList.Clear();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e) {

        }

        /// <summary>
        /// Search_Update - Updates the list of customers based on search criteria
        /// 
        ///     Takes the input values from non-default seach boxes and applies them to
        ///     the list of customers read from the database. 
        ///     
        /// </summary>
        /// <param name="sender">*shrug*</param>
        /// <param name="e">also unused</param>
        private void Search_Update(object sender, System.Windows.Input.KeyEventArgs e) {

            String name = this.searchLastNameTB.Text.ToString().ToLower();
            String zip = this.searchZipTB.Text.ToString().ToLower();
            String phone = this.searchPhoneTB.Text.ToString().ToLower();
            ObservableCollection<CustomerRecord> customerRecordsTemp = new ObservableCollection<CustomerRecord>();

            if (name != "last name") {
                foreach (CustomerRecord customer in this.customerRecords) {
                    if (customer.lastName.ToLower().Contains(name)) {
                        customerRecordsTemp.Add(customer);
                    }
                }
            } else {
                customerRecordsTemp = this.customerRecords;
            }

            ObservableCollection<CustomerRecord> customerRecordsTemp2 = new ObservableCollection<CustomerRecord>();

            if (zip != "zip") {
                foreach (CustomerRecord customer in customerRecordsTemp) {
                    if (customer.zip.ToLower().Contains(zip)) {
                        customerRecordsTemp2.Add(customer);
                    } else if (customer.zip.ToLower().Contains("-1")) {
                        customerRecordsTemp2.Add(customer);
                    } 
                }
            } else {
                customerRecordsTemp2 = customerRecordsTemp;
            }

            customerRecordsTemp = new ObservableCollection<CustomerRecord>();
            if (phone.Length >= 9) {
                foreach (CustomerRecord customer in customerRecordsTemp2) {
                    if (customer.phone.Replace(" ","").Replace(")","").Replace("(","").Replace("-","").Contains(phone.Replace(" ","")) 
                        | customer.altphone.Replace(" ", "").Replace(")", "").Replace("(", "").Replace("-", "").Contains(phone.Replace(" ", "")) 
                        | customer.altphone2.Replace(" ", "").Replace(")", "").Replace("(", "").Replace("-", "").Contains(phone.Replace(" ", ""))) {

                        customerRecordsTemp.Add(customer);
                    }
                }
            } else {
                customerRecordsTemp = customerRecordsTemp2;
            }

            this.customerDB.ItemsSource = customerRecordsTemp;
            this.customerDB.Items.Refresh();

        }

        // The next 6 functions handle search box io
        #region Search Box IO
        private void searchLastNameTB_GotFocus(object sender, RoutedEventArgs e) {
            if (this.searchLastNameTB.Text.ToLower() == "last name") {
                this.searchLastNameTB.Text = "";
            }
        }

        private void searchLastNameTB_LostFocus(object sender, RoutedEventArgs e) {
            if (this.searchLastNameTB.Text == "") {
                this.searchLastNameTB.Text = "Last Name";
            }
        }

        private void searchZipTB_GotFocus(object sender, RoutedEventArgs e) {
            if (this.searchZipTB.Text == "Zip") {
                this.searchZipTB.Text = "";
            }
        }

        private void searchZipTB_LostFocus(object sender, RoutedEventArgs e) {
            if (this.searchZipTB.Text == "") {
                this.searchZipTB.Text = "Zip";
            }
        }

        private void searchPhoneTB_GotFocus(object sender, RoutedEventArgs e) {
            if (this.searchPhoneTB.Text == "Phone") {
                this.searchPhoneTB.Text = "";

            }
        }

        private void searchPhoneTB_LostFocus(object sender, RoutedEventArgs e) {
            if (this.searchPhoneTB.Text == "") {
                this.searchPhoneTB.Text = "Phone";
            }
        }

        #endregion Search Box IO

        private void NewCustomer_Click(object sender, RoutedEventArgs e) {

            newCustomer window = new newCustomer();
            window.ShowDialog();
            // Reload the customer list from the database
            updateCustomerList();
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


        public Part(decimal cost, string name, string type) {
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

    public class CustomerRecord {

        public string firstName { set; get; }
        public string lastName { set; get; }
        public string address { set; get; }
        public string city { set; get; }
        public string zip { set; get; }
        public string id { set; get; }
        public string phone { set; get; }
        public string altphone { set; get; }
        public string altphone2 { set; get; }
        

        public CustomerRecord(string firstName, string lastName, string address, string city, string zip, string id, string phone, string altphone, string altphone2) {
            this.firstName = firstName;
            this.lastName = lastName;
            this.address = address;
            this.city = city;

            // Handle Unknown zip codes
            if (zip == "-1") {
                this.zip = "Unknown";
            } else {
                this.zip = "0" + zip;
            }
            
            this.id = id;

            // Handles phones. Moves blank entries to the end
            List<String> phones = new List<string>();
            if (phone != "") { phones.Add(phone); }
            if (altphone != "") { phones.Add(altphone); }
            if (altphone2 != "") { phones.Add(altphone2); }
            phones.Add("");
            phones.Add("");
            phones.Add("");
            this.phone = phones[0];
            this.altphone = phones[1];
            this.altphone2 = phones[2];
        }


    }

    public class ServiceRecord {

        public int recordID { set; get; }
        public int day { set; get; }
        public int month { set; get; }
        public int year { set; get; }
        public string poolID { set; get; }
        public int custID { set; get; }
        public int cost { get; }
        public string description { set; get; }
        public string workType { set; get; }
        public string date { get; }
        public ObservableCollection<PurchaseSet> parts { set; get; }
        public ObservableCollection<PurchaseSet> labor { set; get; }
        

        public ServiceRecord(int recordID, int custID, string poolID, int day, int month, int year, string description, string workType, string parts, string labor) {
            this.recordID = recordID;
            this.custID = custID;
            this.poolID = poolID;
            this.day = day;
            this.month = month;
            this.year = year;
            this.description = description;
            this.workType = workType;

            this.date = month.ToString() + "-" + day.ToString() + "-" + year.ToString();

            // TODO handle parse parts and labor strings -> ObservableCollection<PurchaseSet>
            // String should be exported as {part, quant}{part2, quant2}

            // Handle parse parts
            string[] splitParts = parts.Split('}');
            foreach (string pair in splitParts) {
                
            }
        }


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