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
        }

        

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


    }
}
