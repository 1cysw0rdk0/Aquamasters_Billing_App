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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class newCustomer : Window {
        public newCustomer() => InitializeComponent();

        private void updateGotFocus(string defaultText, object textbox) {
            if (((TextBox)textbox).Text.Equals(defaultText)) {
                ((TextBox)textbox).Text = "";
            }
        }

        private void updateLostFocus(string defaultText, object textbox) {
            if (string.IsNullOrWhiteSpace(((TextBox)textbox).Text)) {
                ((TextBox)textbox).Text = defaultText;
            }
        }

        private void updateStartPhone(string defaultText, object textbox) {
            if (((TextBox)textbox).Text.Equals(defaultText) || ((TextBox)textbox).Text.Equals("ERROR")) {
                ((TextBox)textbox).Text = "";
            }
        }
        
        private void updateEndPhone(string defaultText, object textbox) {
            TextBox tb = (TextBox)textbox;

            string temp = tb.Text;

            temp = temp.Replace("-", "");
            temp = temp.Replace("(", "");
            temp = temp.Replace(")", "");
            temp = temp.Replace(" ", "");

            tb.Text = temp;

            updateLostFocus(defaultText, textbox);
            if (tb.Text.Equals(defaultText)) { return; }

            if (tb.Text.Length != 10) {
                tb.Text = "ERROR";
            } else {
                temp = tb.Text;
                // this line is gross, but its late and it works so \_(-_-)_/
                tb.Text = "(" + temp[0] + temp[1] + temp[2] + ") " + temp[3] + temp[4] + temp[5] + " - " + temp[6] + temp[7] + temp[8] + temp[9];
            }
        }

        // First Name Text Box
        private void FirstNameTB_GotFocus(object sender, RoutedEventArgs e) => updateGotFocus("First Name", (TextBox)e.Source);
        private void FirstNameTB_LostFocus(object sender, RoutedEventArgs e) => updateLostFocus("First Name", (TextBox)e.Source);

        // Last Name Text Box
        private void LastNameTB_GotFocus(object sender, RoutedEventArgs e) => updateGotFocus("Last Name", (TextBox)e.Source);
        private void LastNameTB_LostFocus(object sender, RoutedEventArgs e) => updateLostFocus("Last Name", (TextBox)e.Source);

        // Address Text Box
        private void AddressTB_GotFocus(object sender, RoutedEventArgs e) => updateGotFocus("Address", e.Source);
        private void AddressTB_LostFocus(object sender, RoutedEventArgs e) => updateLostFocus("Address", e.Source);

        // Town Text Box
        private void TownTB_GotFocus(object sender, RoutedEventArgs e) => updateGotFocus("Town", e.Source);
        private void TownTB_LostFocus(object sender, RoutedEventArgs e) => updateLostFocus("Town", e.Source);

        // Zip Text Box
        private void ZipTB_GotFocus(object sender, RoutedEventArgs e) {
            updateGotFocus("Zip", e.Source);
            updateGotFocus("ERROR", e.Source);
        }

        private void ZipTB_LostFocus(object sender, RoutedEventArgs e) {
            updateLostFocus("Zip", e.Source);
            if (ZipTB.Text.Trim().Length != 5 || !int.TryParse(ZipTB.Text.Trim(), out int result)) {
                if (!ZipTB.Text.Equals("Zip")) {
                    ZipTB.Text = "ERROR";
                }
            } else {
                updateLostFocus("Zip", e.Source);
            }
        }

        // Phones
        private void PrimaryPhoneTB_GotFocus(object sender, RoutedEventArgs e) => updateStartPhone("Primary Phone", e.Source);
        private void PrimaryPhoneTB_LostFocus(object sender, RoutedEventArgs e) => updateEndPhone("Primary Phone", e.Source);

        private void AltPhone1TB_LostFocus(object sender, RoutedEventArgs e) => updateEndPhone("Alternate Phone", e.Source);
        private void AltPhone1TB_GotFocus(object sender, RoutedEventArgs e) => updateStartPhone("Alternate Phone", e.Source);

        private void AltPhone2TB_GotFocus(object sender, RoutedEventArgs e) => updateStartPhone("Alternate Phone", e.Source);
        private void AltPhone2TB_LostFocus(object sender, RoutedEventArgs e) => updateEndPhone("Alternate Phone", e.Source);

        // Right Side TB's 
        // This is called by all boxes
        private void SkimmerTB_GotFocus(object sender, RoutedEventArgs e) => updateGotFocus("0", e.Source);
        private void SkimmerTB_LostFocus(object sender, RoutedEventArgs e) => updateLostFocus("0", e.Source);

        private void SizeTB_GotFocus(object sender, RoutedEventArgs e) => updateGotFocus("18x36", e.Source);
        private void SizeTB_LostFocus(object sender, RoutedEventArgs e) => updateLostFocus("18x36", e.Source);

        private void CancelButton_Click(object sender, RoutedEventArgs e) => this.Close();

        private void AcceptButton_Click(object sender, RoutedEventArgs e) {
            // TODO 
            /**
             * Handle Accept Click
             *   X Parse All Boxes 
             *   Construct SQL Query to check for duplicate
             *     X Display warning iff duplicate suspected
             *   Construct SQL Query to add Customer
             *   Open DB Connection
             *   Execute Query
             *     Verify success
             *   Close DB Connection
             *   this.Close()
             */

            void errorMessage(string message) {
                string caption = "Error detected in input";
                MessageBox.Show(message, caption, MessageBoxButton.OK);
            }

            // Parse all input
            #region Parse

            // Parse CustomerInfo
            #region CustomerInfo
            // Parse First Name
            string firstName = FirstNameTB.Text.Trim();
            if (firstName.Equals("First Name")) {
                errorMessage("Please enter a first name");
                return;
            }

            // Parse Last Name
            string lastName = LastNameTB.Text.Trim();
            if (lastName.Equals("Last Name")) {
                errorMessage("Please enter a last name");
                return;
            }

            // Parse Address
            string address = AddressTB.Text.Trim();
            if (address.Equals("Address")) {
                errorMessage("Please enter an address");
                return;
            }

            // Parse Town
            string town = TownTB.Text.Trim();
            if (town.Equals("Town")) {
                errorMessage("Please enter a town");
                return;
            }
            #endregion CustomerInfo

            // Parse Zip Code
            #region ZipCode
            if (int.TryParse(ZipTB.Text.Trim(), out int zip)) {
                zip = int.Parse(ZipTB.Text.Trim());
            } else {
                // ERROR in ZIP box
                string message = "Zip code is invalid";
                string caption = "Error detected in input";
                MessageBoxButton button = MessageBoxButton.OK;

                MessageBox.Show(message, caption, button);
                return;
            }
            #endregion ZipCode

            // Parse Phone Numbers
            #region PhoneNumbers
            string primaryPhone = "", altPhone = "", alt2Phone = "";
            Boolean tryParsePhone(TextBox tb) {
                if (tb.Text.Equals("Primary Phone") || tb.Text.Equals("Alternate Phone")) { return false; }

                string temp = tb.Text.Trim().Replace(")","").Replace("(","").Replace("-","").Replace(" ","");
                if (int.TryParse(temp, out _)) {
                    return true;
                } else {
                    // ERROR in Phone Box (tb)
                    return false;
                }
            }

            if (tryParsePhone(PrimaryPhoneTB)) {
                primaryPhone = PrimaryPhoneTB.Text.Trim();
            } else {
                primaryPhone = "";
            }

            if (tryParsePhone(AltPhone1TB)) {
                altPhone = AltPhone1TB.Text.Trim();
            } else {
                altPhone = "";
            }

            if (tryParsePhone(AltPhone2TB)) {
                alt2Phone = AltPhone2TB.Text.Trim();
            } else {
                alt2Phone = "";
            }
            #endregion PhoneNumbers

            // Parse Right Side
            #region RightSide
            string size = SizeTB.Text.Trim();
            string skimmers = SkimmerTB.Text.Trim();
            string drains = MainDrainTB.Text.Trim();
            string returns = ReturnsTB.Text.Trim();
            string pump = PumpTB.Text.Trim();
            #endregion RightSide

            // Parse Drop Downs
            // Parse Style
            #region Style
            string construction = "";
            if (ConstructionDD.SelectedItem != null) {
                construction = (string)((ComboBoxItem)ConstructionDD.SelectedItem).Content;
            } else {
                // NO CONSTRUCTION SELECTED, CREATE ERROR MESSAGE
                string message = "No construction style selected. Please select a style.";
                string caption = "Error detected in input";
                MessageBoxButton button = MessageBoxButton.OK;

                MessageBox.Show(message, caption, button);
                return;
            }
            #endregion Style

            // Parse Cover
            #region Cover
            string cover = "";
            if (CoverDD.SelectedItem != null) {
                cover = ((string)((ComboBoxItem)CoverDD.SelectedItem).Content).Split(' ')[0];
            } else {
                // NO COVER SELECTED, DEFAULTS TO NONE?
                // ?TODO? verify no cover before defaulting
                cover = "None";
            }
            #endregion Cover

            // Parse Spa + Heater
            #region Spa and Heater
            Boolean spa = false;
            if (SpaDD.SelectedItem != null) {
                if (((ComboBoxItem)SpaDD.SelectedItem).Content.ToString().Equals("Yes")) {
                    spa = true;
                }
            }

            // Heater
            Boolean heater = false;
            if (HeaterDD.SelectedItem != null) {
                if (((ComboBoxItem)HeaterDD.SelectedItem).Content.ToString().Equals("Yes")) {
                    heater = true;
                }
            }
            #endregion Spa and Heater

            // Parse Fiter Media
            #region Filter
            string filter = "";
            if (FilterMediaDD.SelectedItem != null) {
                filter = (string)((ComboBoxItem)FilterMediaDD.SelectedItem).Content;
            } else {
                // NO FILTER MEDIA SELECTED, GENERATE ERROR MESSAGE
                string message = "No filter style selected. Please select a style.";
                string caption = "Error detected in input";
                MessageBoxButton button = MessageBoxButton.OK;

                MessageBox.Show(message, caption, button);
                return;
            }
            #endregion Filter
            #endregion Parse

            // Check for duplicate customer information in the database
            if (checkDuplicate(new CustomerInfo(firstName, lastName, zip, primaryPhone, altPhone, alt2Phone, address), out CustomerInfo[] duplicates)) {

                foreach (CustomerInfo duplicate in duplicates) {
                    string caption = "Duplicate suspected";
                    string message = "Review the customer found in the database, and ensure the proposed customer is not a duplicate." +
                        "Existing Customer Information for" +
                        "Name: " + duplicate.FirstName + " " + duplicate.LastName +
                        "Address: " + duplicate.Address + ", " + duplicate.Zip.ToString() +
                        "Phone 1: " + duplicate.Phone1 +
                        "Phone 2: " + duplicate.Phone2 +
                        "Phone 3: " + duplicate.Phone3;
                    MessageBoxButton buttons = MessageBoxButton.OKCancel;
                    MessageBoxResult result = MessageBox.Show(message, caption, buttons);

                    if(result.Equals(MessageBoxResult.Cancel)) {
                        // This customer is a duplicate, close the window.
                        this.Close();
                    }
                }
            }

            // Customer is not a duplicate, add them to the database



            // Close windwo when done
            this.Close();

        }


        private bool checkDuplicate(CustomerInfo customer, out CustomerInfo[] duplicate) {
            // Create SQL Query to check for Duplicates
            // Use first name, last name, zip, phone numbers, address
            // Fuzzy logic, if >3 match, add match to output array

            duplicate = null;
            return false;
        }

        class CustomerInfo {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Phone1 { get; set; }
            public string Phone2 { get; set; }
            public string Phone3 { get; set; }
            public string Address { get; set; }
            public int Zip { get; set; }

            public CustomerInfo(string firstName, string lastName, int zip, string primaryPhone, string altPhone, string alt2Phone, string address) {
                FirstName = firstName;
                LastName = lastName;
                Zip = zip;
                Phone1 = primaryPhone;
                Phone2 = altPhone;
                Phone3 = alt2Phone;
                Address = address;
            }

        }
    }
}
