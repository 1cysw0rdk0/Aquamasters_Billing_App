using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AquaMasters_Billing_App {
	/// <summary>
	/// Interaction logic for ServerInfo.xaml
	/// </summary>
	public partial class serverInfo : Window {

		private String defAddress, defUser;

		public serverInfo(string address = "localhost", string user = "root") {
			InitializeComponent();
			defAddress = address;
			defUser = user;

			this.addressTB.Text = defAddress;
			this.usernameTB.Text = defUser;

		}


		// UXBS
		private void addressTB_GotFocus(object sender, RoutedEventArgs e) {
			if (addressTB.Text.Equals(defAddress)) {
				addressTB.Text = "";
			}
		}

		private void addressTB_LostFocus(object sender, RoutedEventArgs e) {
			if (addressTB.Text.Trim().Equals("")) {
				addressTB.Text = defAddress;
			}
		}

		private void usernameTB_GotFocus(object sender, RoutedEventArgs e) {
			if (usernameTB.Text.Equals(defUser)) {
				usernameTB.Text = "";
			}
		}

		private void usernameTB_LostFocus(object sender, RoutedEventArgs e) {
			if (usernameTB.Text.Trim().Equals("")) {
				usernameTB.Text = defUser;
			}
		}

		private void Window_KeyDown(object sender, KeyEventArgs e) {
			if(e.Key.Equals(Key.Enter)) {
				AcceptButton_Click(sender, e);
			}
		}

		private void AcceptButton_Click(object sender, RoutedEventArgs e) {

			string connString = @"server=" + addressTB.Text.Trim() + ";userid=" + usernameTB.Text + ";password=" + passwordPB.Password + ";database=aquamastersservice";

			// Test Database Credentials
			try {
				using(MySqlConnection testConnection = new MySqlConnection(connString)) {
					testConnection.Open();
					testConnection.Dispose();
					DialogResult = true;
					this.Close();
				}
			} catch {
				MessageBox.Show("Failed to authenticate with database. Check Credentials and Database.", "Error connecting to database", MessageBoxButton.OK);
			}
			

		}
	}
}
