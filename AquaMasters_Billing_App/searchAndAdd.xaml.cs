using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace AquaMasters_Billing_App {
    /// <summary>
    /// Interaction logic for searchAndAdd.xaml
    /// </summary>
    public partial class searchAndAdd : Window {

        /// <summary>
        /// partOrder - A partOrder object contains a part and its quantity
        /// </summary>
        public struct partOrder {
            public Part part { get; set; }
            public Decimal quantity { get; set; }
        }

        // Lists for parts to be passed back to main window
        // And a reference to the main price list
        public List<Part> priceSheetList;
        public List<partOrder> cart;
        public Dictionary<string, Part> masterPriceList;

        /// <summary>
        /// 
        /// searchAndAdd - Constructs a new searchAndAdd window
        ///     - Copies items from the master dictionary to masterPriceList
        ///     - Initializes both dataGrids with columns
        ///     - Sets the itemsources for both dataGrids
        ///     
        /// </summary>
        /// 
        /// <param name="PriceSheet">Used to gain copies of the master price list.</param>
        public searchAndAdd(Dictionary<string, Part> PriceSheet) {
            InitializeComponent();

            // Copy items from dictionary to a list so as to avoid having to deal with MultiBinding cause fuck that
            priceSheetList = new List<Part>();
            cart = new List<partOrder>();
            masterPriceList = PriceSheet;
            foreach (Part part in PriceSheet.Values) { priceSheetList.Add(part); };

            // Initialize the Data grid for priceList items
            priceListDG.Columns.Add(new DataGridTextColumn { Header = "Part - Labor - Service", Binding = new Binding("name"), Width=264, IsReadOnly=true });
            priceListDG.Columns.Add(new DataGridTextColumn { Header = "Cost", Binding = new Binding("cost"), MinWidth=45, IsReadOnly=false });
            priceListDG.Columns.Add(new DataGridTextColumn { Header = "Type", Binding = new Binding("type"), MinWidth=47, MaxWidth=47, IsReadOnly=true });

            // Initialize the Data grid for cart items
            cartDg.Columns.Add(new DataGridTextColumn { Header = "Part - Labor - Service", Binding = new Binding("part.name"), Width = 264, IsReadOnly=true });
            cartDg.Columns.Add(new DataGridTextColumn { Header = "Cost", Binding = new Binding("part.cost"), MinWidth = 45, IsReadOnly = true });
            cartDg.Columns.Add(new DataGridTextColumn { Header = "Quantity", Binding = new Binding("quantity"), MinWidth=47, IsReadOnly=false });

            // Bind the data grids to the proper lists
            priceListDG.ItemsSource = priceSheetList;
            cartDg.ItemsSource = cart;
        }

        /// <summary>
        /// 
        /// priceListDG_MouseDoubleClick - Adds an item to the cart when it is double clicked on the left
        ///     - Checks to make sure the name column was clicked
        ///     - Adds 1 of the part to the cart
        ///     
        /// </summary>
        /// 
        /// <param name="sender">Unused.</param>
        /// <param name="e">Unused.</param>
        private void priceListDG_MouseDoubleClick(object sender, MouseButtonEventArgs e) {

            // Add the double clicked item, if the item was clicked on the name label.
            // Refresh the cart so it will update
            if (priceListDG.CurrentColumn == priceListDG.Columns[0]) {
                cart.Add(new partOrder { part = (Part)priceListDG.SelectedItem, quantity = 1m });
                cartDg.Items.Refresh();
            }
            
        }

        /// <summary>
        /// 
        /// updatePriceList - Changing any of the checkboxes calls this
        ///     - CheckTypes is called to update the display list
        ///     
        /// </summary>
        /// 
        /// <param name="sender">Unused.</param>
        /// <param name="e">Unused.</param>
        private void updatePriceList(object sender, RoutedEventArgs e) {
            checkTypes();
        }

        /// <summary>
        /// 
        /// FilterBox_KeyUp - Pressing a key in the search box calls this
        ///     - CheckTypes is called to update the display list
        ///     
        /// </summary>
        /// 
        /// <param name="sender">Unused.</param>
        /// <param name="e">Unused.</param>
        private void FilterBox_KeyUp(object sender, KeyEventArgs e) {
            checkTypes();
        }

        /// <summary>
        /// 
        /// checkTypes - Called to update the display list
        ///     - Initializes the display list
        ///     - If Chems is checked, loop through all items and add chems to the display 
        ///     - If Labor is checked, loop through all items and add labor to the display
        ///     - If Services is checked, loop through all items and add Services to the display
        ///     - If Parts is checked, loop through all items and add parts to the display
        ///     - Initialize a temporary list
        ///     - Sort out any entries which does not contain the search text
        ///     - Reset the itemsource an refresh
        ///     
        /// </summary>
        private void checkTypes() {
            priceSheetList = new List<Part>();

            if (ShowChems.IsChecked.Value) {
                foreach (Part part in masterPriceList.Values) { if (part.type == "Chem") { priceSheetList.Add(part); } }
            }

            if (ShowLabor.IsChecked.Value) {
                foreach (Part part in masterPriceList.Values) { if (part.type == "Labor") { priceSheetList.Add(part); } }
            }

            if (ShowServices.IsChecked.Value) {
                foreach (Part part in masterPriceList.Values) { if (part.type == "Service") { priceSheetList.Add(part); } }
            }

            if (ShowParts.IsChecked.Value) {
                foreach (Part part in masterPriceList.Values) { if (part.type == "Part") { priceSheetList.Add(part); } }
            }

            List<Part> tempPriceSheet = new List<Part>();

            if (FilterBox.Text != null) {
                foreach (Part part in priceSheetList) {
                    if ((part.name.ToUpper().Contains(FilterBox.Text.ToUpper()))) {
                        tempPriceSheet.Add(part);
                    }
                }
            }

            priceSheetList = tempPriceSheet;

            priceListDG.ItemsSource = priceSheetList;
            priceListDG.Items.Refresh();

        }

        /// <summary>
        /// 
        /// Cancel_Click - Handles the event in which cancel is clicked
        ///     - Prevents the main window from collecting data
        ///     - Closes the window
        ///     
        /// </summary>
        /// <param name="sender">Unused.</param>
        /// <param name="e">Unused.</param>
        private void Cancel_Click(object sender, RoutedEventArgs e) {
            DialogResult = false;
            this.Close();
        }

        /// <summary>
        /// 
        /// Checkout_Click - Handles the event in which accept is clicked
        ///     - Allows the main window to collect data
        ///     - Closes the window
        ///     
        /// </summary>
        /// <param name="sender">Unused.</param>
        /// <param name="e">Unused.</param>
        private void Checkout_Click(object sender, RoutedEventArgs e) {
            DialogResult = true;
            this.Close();

        }

        /// <summary>
        /// 
        /// Checkout_Copy_Click - Add a new part to the master list
        ///     - Adds a new part to the master list
        ///     - Updates all displays
        ///     
        /// </summary>
        /// <param name="sender">Unused.</param>
        /// <param name="e">Unused.</param>
        private void Checkout_Copy_Click(object sender, RoutedEventArgs e) {
            masterPriceList.Add(FilterBox_Copy.Text, new Part(0.00m, FilterBox_Copy.Text, "Chem"));
            checkTypes();
        }

        private void Checkout_Copy1_Click(object sender, RoutedEventArgs e) {
            this.priceSheetList.Remove((Part)this.priceListDG.SelectedItem);
            this.masterPriceList.Remove(((Part)this.priceListDG.SelectedItem).name);
            this.priceListDG.Items.Refresh();
        }
    }
}
