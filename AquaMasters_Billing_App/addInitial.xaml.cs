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
    /// Interaction logic for addInitial.xaml
    /// </summary>
    public partial class addInitial : Window {

        // Lists used to pass data back to the main window
        public List<String> parts;
        public List<decimal> quants;
        public List<tempPart> tempParts;

        // TempPart used to store pairs of data accurately
        public class tempPart {
            public decimal quantity { get; set; }
            public String name { get; set; }
        }

        // Constructs a new addInitial window
        public addInitial() {
            InitializeComponent();
            InitializeFrontEndData();
        }

        /// <summary>
        /// 
        /// InitializeFrontEndData - Initializes data lists for the window
        ///     - Sets the item source for the dataGrid
        ///     - Adds columns to the dataGrid
        /// 
        /// </summary>
        private void InitializeFrontEndData() {

            this.parts = new List<string>();
            this.quants = new List<decimal>();
            this.tempParts = new List<tempPart>();

            this.ShockList.ItemsSource = tempParts;

            this.ShockList.Columns.Add(new DataGridTextColumn { Header = "Quantity", Binding = new Binding("quantity"), Width = 56 });
            this.ShockList.Columns.Add(new DataGridTextColumn { Header = "Name", Binding = new Binding("name"), Width = 150, IsReadOnly=true });
        }

        /// <summary>
        /// 
        /// Accept_Click - Handles the event when accept is clicked
        ///     - Allows the main window to collect data
        ///     - Closes the window
        /// 
        /// </summary>
        /// <param name="sender">Unused.</param>
        /// <param name="e">Unused.</param>
        private void Accept_Click(object sender, RoutedEventArgs e) {
            this.DialogResult = true;
            this.Close();
        }

        /// <summary>
        /// 
        /// Accept_Copy_Click - Handles the event when the cancel button is clicked
        ///     - Prevents the main window from collecting data
        ///     - Closes the window
        ///     
        /// NOTE: This method is marked to be changed, the naming is incorrect
        /// 
        /// </summary>
        /// <param name="sender">Unused.</param>
        /// <param name="e">Unused.</param>
        private void Accept_Copy_Click(object sender, RoutedEventArgs e) {
            this.DialogResult = false;
            this.Close();
        }

        /// <summary>
        /// 
        /// AddLabor_Click - Adds a new labor object to the lists 
        ///     - Adds the proper rate for the labor used.
        ///     - Adds the quantity to the proper lists
        ///     - Refreshes the dataGrid
        ///     
        /// </summary>
        /// <param name="sender">Unused.</param>
        /// <param name="e">Unused.</param>
        public void AddLabor_Click(object sender, RoutedEventArgs e) {

            tempPart part = new tempPart();

            if (this.FullRB.IsChecked.Value) {
                this.parts.Add("2 Men - Initial");
                part.name = "2 Men - Initial";
            } else { 
                this.parts.Add("1 Man - Initial");
                part.name = "1 Man - Initial";
            }

            this.quants.Add(decimal.Parse(this.Hours.Text));
            part.quantity = decimal.Parse(this.Hours.Text);
            this.tempParts.Add(part);
            this.ShockList.Items.Refresh();
        }

        private void _2Gal_Click(object sender, RoutedEventArgs e) {
            this.parts.Add("1 Gallon Shock");
            this.quants.Add(2m);
            this.tempParts.Add(new tempPart { quantity = 2m, name = "1 Gallon Shock" });
            this.ShockList.Items.Refresh();
        }

        private void _3Gal_Click(object sender, RoutedEventArgs e) {
            this.parts.Add("1 Gallon Shock");
            this.quants.Add(3m);
            this.tempParts.Add(new tempPart { quantity = 3m, name = "1 Gallon Shock" });
            this.ShockList.Items.Refresh();
        }

        private void FullCase_Click(object sender, RoutedEventArgs e) {
            this.parts.Add("Case of Shock");
            this.quants.Add(1m);
            this.tempParts.Add(new tempPart { quantity = 1m, name = "Case of Shock" });
            this.ShockList.Items.Refresh();
        }
    }
}
