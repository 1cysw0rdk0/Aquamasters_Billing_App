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

namespace AquaMasters_Billing_App
{
    /// <summary>
    /// Interaction logic for addOpening.xaml
    /// </summary>
    public partial class addOpening : Window
    {
        public addOpening()
        {
            
            InitializeComponent();
            this.SmallRB.IsChecked = false;
        }

        public List<String> parts;
        public List<decimal> quants;

        private void AssembleOnlyCB_Click(object sender, RoutedEventArgs e) {
            if (!this.AssembleOnlyCB.IsChecked.Value) {
                CleanCoverCB.IsChecked = true;
                AddChemsCB.IsChecked = true;
                _5GalCB.IsChecked = true;
                Poly60.IsChecked = true;
            }
            UpdateVisibility();
        }

        private void Solid_Click(object sender, RoutedEventArgs e) => UpdateVisibility();

        private void AddChemsCB_Click(object sender, RoutedEventArgs e) {
            if (this.AddChemsCB.IsChecked.Value) {
                _5GalCB.IsChecked = true;
                Poly60.IsChecked = true;
            }
            UpdateVisibility();
        }

        private void UpdateVisibility() {

            // Logic for Filter Assembly Only
            if (this.AssembleOnlyCB.IsChecked.Value) {
                CleanCoverCB.IsChecked = false;
                AddChemsCB.IsChecked = false;

                this.CleanCoverCB.Opacity = 0;
                this.AddChemsCB.Opacity = 0;

            } else {
                this.CleanCoverCB.Opacity = 100;
                this.AddChemsCB.Opacity = 100;
            }

            // Logic for Solid Covers
            if (!this.SolidRB.IsChecked.Value) {
                PumpCoverCB.IsChecked = false;
                PumpCoverCB.Opacity = 0;
            } else {
                PumpCoverCB.Opacity = 100;
            }

            // Logic for Cleaning Cover
            if (!this.CleanCoverCB.IsChecked.Value || !this.SolidRB.IsChecked.Value) {
                PumpCoverCB.IsChecked = false;
                PumpCoverCB.Opacity = 0;
            }
            else {
                PumpCoverCB.Opacity = 100;
            }

            // Logic for Chems
            if (!this.AddChemsCB.IsChecked.Value) {
                _5GalCB.IsChecked = false;
                CaseCB.IsChecked = false;
                Poly60.IsChecked = false;

                _5GalCB.Opacity = 0;
                CaseCB.Opacity = 0;
                Poly60.Opacity = 0;
            } else {
                _5GalCB.Opacity = 100;
                CaseCB.Opacity = 100;
                Poly60.Opacity = 100;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e) { this.DialogResult = false; this.Close(); }

        private void Accept_Click(object sender, RoutedEventArgs e) {

            parts = new List<String>();
            quants = new List<decimal>();

            if (this.AssembleOnlyCB.IsChecked.Value) {
                parts.Add("Assemble and Connect System Only");
                quants.Add(1m);
                this.DialogResult = true;
                this.Close();
                return;
            }

            // Pool size
            if (this.SmallRB.IsChecked.Value) {
                parts.Add("15x30 - 16x32 Opening");
                quants.Add(1m);
            } else {
                parts.Add("18x36 - 20x40 Opening");
                quants.Add(1m);
            }

            // Labor
            if (this.FullRB.IsChecked.Value) {
                parts.Add("2 Men - Opening");
            } else if (this.HalfRB.IsChecked.Value) {
                parts.Add("1 Man - Opening");
            }

            quants.Add(Decimal.Parse(HoursTB.Text));

            // 
            if (!this.CleanCoverCB.IsChecked.Value) {
                parts.Add("No Cover");
                quants.Add(1m);
            }

            if (this.PumpCoverCB.IsChecked.Value) {
                parts.Add("Pump off cover");
                quants.Add(1m);
            }

            if (this.AddChemsCB.IsChecked.Value) {
                if (this._5GalCB.IsChecked.Value) {
                    parts.Add("5 Gallon Shock (Carboys)");
                    quants.Add(1m);
                }
                if (this.CaseCB.IsChecked.Value) {
                    parts.Add("Case of Shock");
                    quants.Add(1m);
                }
                if (this.Poly60.IsChecked.Value) {
                    parts.Add("Poly 60 Algaecide");
                    quants.Add(1m);
                }
            }

            if (this.NoPower.IsChecked.Value) {
                parts.Add("No Power, Return later");
                quants.Add(1m);
            }

            if (this.BigL.IsChecked.Value) {
                parts.Add("Big L");
                quants.Add(1m);
            }

            if (this.Turbo.IsChecked.Value) {
                parts.Add("Turbo System");
                quants.Add(1m);
            }

            if (this.Spa.IsChecked.Value) {
                parts.Add("Spa");
                quants.Add(1m);
            }

            if (this.Soomanyreturns.IsChecked.Value) {
                parts.Add("More than 4 Returns");
                quants.Add(1m);
            }

            if (this.WaterFeed.IsChecked.Value) {
                parts.Add("Water Feed");
                quants.Add(1m);
            }

            if (this.ExtraPump.IsChecked.Value) {
                parts.Add("Extra Pump");
                quants.Add(1m);
            }

            if (this.Controller.IsChecked.Value) {
                parts.Add("Controller");
                quants.Add(1m);
            }

            if (this.Waterfall.IsChecked.Value) {
                parts.Add("Waterfall / Pipe");
                quants.Add(1m);
            }

            if (this.LeafCatch.IsChecked.Value) {
                parts.Add("Leaf Catcher");
                quants.Add(1m);
            }

            if (this.BuddaJet.IsChecked.Value) {
                parts.Add("Budda Jet");
                quants.Add(1m);
            }

            if (this.CoverCables.IsChecked.Value) {
                parts.Add("Cover Cable");
                quants.Add(decimal.Parse(CoverCablesNum.Text));
            }

            if (this.Heater.IsChecked.Value) {
                parts.Add("Heater");
                quants.Add(1m);
            }

            if (this.SaltGen.IsChecked.Value) {
                parts.Add("Salt Generator");
                quants.Add(1m);
            }



            this.DialogResult = true;
            this.Close();
        }

        
    }
}
