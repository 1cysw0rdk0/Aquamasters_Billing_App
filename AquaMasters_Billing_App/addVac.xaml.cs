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

        public class tempPart {
            public decimal quantity { get; set; }
            public String name { get; set; }
        }

        public List<String> parts;
        public List<decimal> quantities;
        public List<tempPart> tempParts;

        public addVac() {
            InitializeComponent();
            parts = new List<string>();
            quantities = new List<decimal>();
            tempParts = new List<tempPart>();
            this.shockList.ItemsSource = tempParts;

            shockList.Columns.Add(new DataGridTextColumn { Header = "Quantity", Binding = new Binding("quantity"), Width=56 });
            shockList.Columns.Add(new DataGridTextColumn { Header = "Name", Binding = new Binding("name"), Width=150 });
        }





        private void Accept_Click(object sender, RoutedEventArgs e) {

            if (twoCall.IsChecked.Value) {
                if (fiver.IsChecked.Value) {
                    this.parts.Add("2 Call Vac Service - 5 Week");
                } else {
                    this.parts.Add("2 Call Vac Service");              
                }
            } else {
                if (fiver.IsChecked.Value) {
                    this.parts.Add("1 Call Vac Service - 5 Week");
                } else {
                    this.parts.Add("1 Call Vac Service");
                }
            }
            this.quantities.Add(1m);

            this.DialogResult = true;
            this.Close();
        }

        private void cancel_Click(object sender, RoutedEventArgs e) {
            this.DialogResult = false;
            this.Close();
        }

        private void shock1_Click(object sender, RoutedEventArgs e) {
            this.parts.Add("1 Gallon Shock");
            this.quantities.Add(1m);
            this.tempParts.Add(new tempPart { name = "1 Gallon Shock", quantity = 1m });
            this.shockList.Items.Refresh();
        }

        private void shock2_Click(object sender, RoutedEventArgs e) {
            this.parts.Add("1 Gallon Shock");
            this.quantities.Add(2m);
            this.tempParts.Add(new tempPart { name = "1 Gallon Shock", quantity = 2m });
            this.shockList.Items.Refresh();
        }

        private void shock3_Click(object sender, RoutedEventArgs e) {
            this.parts.Add("1 Gallon Shock");
            this.quantities.Add(3m);
            this.tempParts.Add(new tempPart { name = "1 Gallon Shock", quantity = 3m });
            this.shockList.Items.Refresh();
        }

        private void shock4_Click(object sender, RoutedEventArgs e) {
            this.parts.Add("Case of Shock");
            this.quantities.Add(1m);
            this.tempParts.Add(new tempPart { name = "Case of Shock", quantity = 1m });
            this.shockList.Items.Refresh();
        }

        private void alk1_Click(object sender, RoutedEventArgs e) {
            this.parts.Add("10 lbs. Alkalinity");
            this.quantities.Add(1m);
            this.tempParts.Add(new tempPart { name = "10 lbs. Alkalinity", quantity = 1m });
            this.shockList.Items.Refresh();
        }

        private void alk2_Click(object sender, RoutedEventArgs e) {
            this.parts.Add("10 lbs. Alkalinity");
            this.quantities.Add(2m);
            this.tempParts.Add(new tempPart { name = "10 lbs. Alkalinity", quantity = 2m });
            this.shockList.Items.Refresh();
        }

        private void alk3_Click(object sender, RoutedEventArgs e) {
            this.parts.Add("10 lbs. Alkalinity");
            this.quantities.Add(3m);
            this.tempParts.Add(new tempPart { name = "10 lbs. Alkalinity", quantity = 3m });
            this.shockList.Items.Refresh();
        }
    }
}
