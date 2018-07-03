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

        public List<String> parts;
        public List<decimal> quants;
        public List<tempPart> tempParts;

        public class tempPart {
            public decimal quantity { get; set; }
            public String name { get; set; }
        }

        public addInitial() {
            InitializeComponent();
            InitializeFrontEndData();
        }


        private void InitializeFrontEndData() {


            this.parts = new List<string>();
            this.quants = new List<decimal>();
            this.tempParts = new List<tempPart>();

            this.ShockList.ItemsSource = tempParts;

            this.ShockList.Columns.Add(new DataGridTextColumn { Header = "Quantity", Binding = new Binding("quantity"), Width = 56 });
            this.ShockList.Columns.Add(new DataGridTextColumn { Header = "Name", Binding = new Binding("name"), Width = 150, IsReadOnly=true });



        }

        private void Accept_Click(object sender, RoutedEventArgs e) {
            this.DialogResult = true;
            this.Close();

        }

        private void Accept_Copy_Click(object sender, RoutedEventArgs e) {
            this.DialogResult = false;
            this.Close();
        }

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
