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
    /// Interaction logic for addVac.xaml
    /// </summary>
    public partial class addVac : Window {

        /// <summary>
        /// 
        /// tempPart - A class used to store object names and quantities 
        ///     in a pair.
        ///     
        /// </summary>
        public class tempPart {
            public decimal quantity { get; set; }
            public String name { get; set; }
        }

        // Lists used to store the information to be passed back to the main window
        public List<String> parts;
        public List<decimal> quantities;
        public List<tempPart> tempParts;

        /// <summary>
        /// 
        /// addVac - The constructor for the addVac window.
        ///     - Initializes the parts lists
        ///     - Sets the itemsource for the datagrids
        ///     - Adds columns to the datagrid
        ///     
        /// </summary>
        public addVac() {
            InitializeComponent();
            parts = new List<string>();
            quantities = new List<decimal>();
            tempParts = new List<tempPart>();
            this.shockList.ItemsSource = tempParts;

            shockList.Columns.Add(new DataGridTextColumn { Header = "Quantity", Binding = new Binding("quantity"), Width=56 });
            shockList.Columns.Add(new DataGridTextColumn { Header = "Name", Binding = new Binding("name"), Width=242, IsReadOnly=true });
        }

        /// <summary>
        /// 
        /// Accept_Click - Handles the event in which the accept button is clicked
        ///     
        /// </summary>
        /// <param name="sender">Unused.</param>
        /// <param name="e">Unused.</param>
        private void Accept_Click(object sender, RoutedEventArgs e) {
            
            this.quantities.Add(1m);

            this.DialogResult = true;
            this.Close();
        }

        /// <summary>
        /// 
        /// cancel_Click - Handles the event in which the cancel button is clicked
        ///     - Prevents the main window from copying list data
        ///     - Closes the window
        /// 
        /// </summary>
        /// <param name="sender">Unused.</param>
        /// <param name="e">Unused.</param>
        private void cancel_Click(object sender, RoutedEventArgs e) {
            this.DialogResult = false;
            this.Close();
        }

        /// <summary>
        /// 
        /// shock1_Click - Handles the event in which the shock 1 button is clicked
        ///     - Adds a gallon of shock object to the parts list
        ///     - Adds a quantity of 1 to the quantities list
        ///     - Creates and adds a tempPart
        ///     - Refreshes the dataGrid
        ///     
        /// </summary>
        /// <param name="sender">Unused.</param>
        /// <param name="e">Unused.</param>
        private void shock1_Click(object sender, RoutedEventArgs e) {
            this.parts.Add("1 Gallon Shock");
            this.quantities.Add(1m);
            this.tempParts.Add(new tempPart { name = "1 Gallon Shock", quantity = 1m });
            this.shockList.Items.Refresh();
        }

        /// <summary>
        /// 
        /// shock2_Click - Handles the event in which the shock 2 button is clicked
        ///     - Adds a gallon of shock object to the parts list
        ///     - Adds a quantity of 2 to the quantities list
        ///     - Creates and adds a tempPart
        ///     - Refreshes the dataGrid
        /// 
        /// </summary>
        /// <param name="sender">Unused.</param>
        /// <param name="e">Unused.</param>
        private void shock2_Click(object sender, RoutedEventArgs e) {
            this.parts.Add("1 Gallon Shock");
            this.quantities.Add(2m);
            this.tempParts.Add(new tempPart { name = "1 Gallon Shock", quantity = 2m });
            this.shockList.Items.Refresh();
        }

        /// <summary>
        /// 
        /// shock3_Click - Handles the event in which the shock 3 button is clicked
        ///     - Adds a gallon of shock object to the parts list
        ///     - Adds a quantity of 3 to the quantities list
        ///     - Creates and adds a tempPart
        ///     - Refreshes the dataGrid
        /// 
        /// </summary>
        /// <param name="sender">Unused.</param>
        /// <param name="e">Unused.</param>
        private void shock3_Click(object sender, RoutedEventArgs e) {
            this.parts.Add("1 Gallon Shock");
            this.quantities.Add(3m);
            this.tempParts.Add(new tempPart { name = "1 Gallon Shock", quantity = 3m });
            this.shockList.Items.Refresh();
        }

        /// <summary>
        /// 
        /// shock4_Click - Handles the event in which the case button is clicked
        ///     - Adds a case of shock object to the parts list
        ///     - Adds a quantity of 1 to the quantities list
        ///     - Creates and adds a tempPart
        ///     - Refreshes the dataGrid
        /// 
        /// </summary>
        /// <param name="sender">Unused.</param>
        /// <param name="e">Unused.</param>
        private void shock4_Click(object sender, RoutedEventArgs e) {
            this.parts.Add("Case of Shock");
            this.quantities.Add(1m);
            this.tempParts.Add(new tempPart { name = "Case of Shock", quantity = 1m });
            this.shockList.Items.Refresh();
        }

        /// <summary>
        /// 
        /// alk1_Click - Handles the event in which the alk 1 button is clicked
        ///     - Adds an alk object to the parts list
        ///     - Adds a quantity of 1 to the quantities list
        ///     - Creates and adds a tempPart
        ///     - Refreshes the dataGrid
        /// 
        /// </summary>
        /// <param name="sender">Unused.</param>
        /// <param name="e">Unused.</param>
        private void alk1_Click(object sender, RoutedEventArgs e) {
            this.parts.Add("10 lbs. Alkalinity");
            this.quantities.Add(1m);
            this.tempParts.Add(new tempPart { name = "10 lbs. Alkalinity", quantity = 1m });
            this.shockList.Items.Refresh();
        }

        /// <summary>
        /// 
        /// alk2_Click - Handles the event in which the alk 2 button is clicked
        ///     - Adds an alk object to the parts list
        ///     - Adds a quantity of 2 to the quantities list
        ///     - Creates and adds a tempPart
        ///     - Refreshes the dataGrid
        /// 
        /// </summary>
        /// <param name="sender">Unused.</param>
        /// <param name="e">Unused.</param>
        private void alk2_Click(object sender, RoutedEventArgs e) {
            this.parts.Add("10 lbs. Alkalinity");
            this.quantities.Add(2m);
            this.tempParts.Add(new tempPart { name = "10 lbs. Alkalinity", quantity = 2m });
            this.shockList.Items.Refresh();
        }

        /// <summary>
        /// 
        /// alk3_Click - Handles the event in which the alk 3 button is clicked
        ///     - Adds an alk object to the parts list
        ///     - Adds a quantity of 3 to the quantities list
        ///     - Creates and adds a tempPart
        ///     - Refreshes the dataGrid
        /// 
        /// </summary>
        /// <param name="sender">Unused.</param>
        /// <param name="e">Unused.</param>
        private void alk3_Click(object sender, RoutedEventArgs e) {
            this.parts.Add("10 lbs. Alkalinity");
            this.quantities.Add(3m);
            this.tempParts.Add(new tempPart { name = "10 lbs. Alkalinity", quantity = 3m });
            this.shockList.Items.Refresh();
        }

        /// <summary>
        /// 
        /// addCheck_Click - Handles the event in which the addCheck button is clicked
        ///     - Adds a check object to the parts list
        ///     - Adds a quantity of 1 to the quantities list
        ///     - Creates and adds a tempPart
        ///     - Refreshes the dataGrid
        ///     
        /// </summary>
        /// <param name="sender">Unused.</param>
        /// <param name="e">Unused.</param>
        private void addCheck_Click(object sender, RoutedEventArgs e) {
            this.parts.Add("Vac Service - Check");
            this.quantities.Add(1m);
            this.tempParts.Add(new tempPart { name = "Vac Service - Check", quantity = 1m });
            this.shockList.Items.Refresh();
        }

        /// <summary>
        /// 
        /// addNoVac_Click - Handles the event in which the addNoVac button is clicked
        ///     - Adds a no vac object to the parts list
        ///     - Adds a quantity of 1 to the quantities list
        ///     - Creates and adds a tempPart
        ///     - Refreshes the dataGrid
        ///     
        /// </summary>
        /// <param name="sender">Unused.</param>
        /// <param name="e">Unused.</param>
        private void addNoVac_Click(object sender, RoutedEventArgs e) {
            this.parts.Add("Vac Service - Can't Vac");
            this.quantities.Add(1m);
            this.tempParts.Add(new tempPart { name = "Vac Service - Can't Vac", quantity = 1m });
            this.shockList.Items.Refresh();
        }

        /// <summary>
        /// 
        /// addVac_Click - Handles the event in which the addVac button is clicked
        ///     - Adds a vac object to the parts list
        ///     - Adds a quantity of 1 to the quantities list
        ///     - Creates and adds a tempPart
        ///     - Refreshes the dataGrid
        ///     
        /// </summary>
        /// <param name="sender">Unused.</param>
        /// <param name="e">Unused.</param>
        private void addVac_Click(object sender, RoutedEventArgs e) {
            this.parts.Add("Vac Service - Full Vac");
            this.quantities.Add(1m);
            this.tempParts.Add(new tempPart { name = "Vac Service - Full Vac", quantity = 1m });
            this.shockList.Items.Refresh();
        }
    }
}
