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
             *   Parse All Boxes
             *   Construct SQL Query to check for duplicate
             *     Display warning iff duplicate suspected
             *   Construct SQL Query to add Customer
             *   Open DB Connection
             *   Execute Query
             *     Verify success
             *   Close DB Connection
             *   this.Close()
             */
        }
    }
}
