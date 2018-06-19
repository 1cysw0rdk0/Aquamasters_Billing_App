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

        private void AssembleOnlyCB_Checked(object sender, RoutedEventArgs e)
        {
            if (this.AssembleOnlyCB.IsChecked.Value) {

            }
        }

        private void AssembleOnlyCB_Click(object sender, RoutedEventArgs e)
        {

        }


        private void UpdateVisibility() {

            if (this.AssembleOnlyCB.IsChecked.Value) {
                CleanCoverCB.IsChecked = false;
                AddChemsCB.IsChecked = false;

                this.CleanCoverCB.Opacity = 0;
                this.AddChemsCB.Opacity = 0;

            } else {
                this.CleanCoverCB.Opacity = 100;
                this.AddChemsCB.Opacity = 100;
            }

        }
    }
}
