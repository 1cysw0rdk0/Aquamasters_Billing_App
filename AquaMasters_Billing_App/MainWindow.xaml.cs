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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using System.IO;

namespace AquaMasters_Billing_App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public Dictionary<string, Part> PriceSheet;
        public string savePath;

        public MainWindow()
        {
            InitializeComponent();
            InitializeBackendData();
            InitializeFrontendData();

        }

        private void InitializeBackendData() {

            //Generate save path for this computer
            this.savePath = Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
            this.savePath += "\\Aquamasters\\test.ini";

            //Initialize file to read text
            StreamReader file = File.OpenText(this.savePath);

            //Initialize price sheet with new empty dictionary
            this.PriceSheet = new Dictionary<string, Part>();

            //Read every line from the file, desearializing into parts and adding to dictonary
            while (!file.EndOfStream)
            {
                Part part = (Part)JsonConvert.DeserializeObject(file.ReadLine(), typeof(Part));
                this.PriceSheet.Add(part.name, part);
            }


            

        }


        private void InitializeFrontendData() {

            dg.Columns.

        }


        private void UpdateTotals() {

            this.SubtotalCostTB.Text = (Decimal.Parse(this.PartCostTB.Text) + Decimal.Parse(this.LaborCostTB.Text)).ToString();
            string cost = (Decimal.Parse(this.SubtotalCostTB.Text) * 1.0635m).ToString();
            this.TotalCostTB.Text = cost.Substring(0, cost.IndexOf(".") + 3);

        }









        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Write an object to a json formatted string in a file
            Part json = new Part();

            json.setCost(5.00);
            json.setName("Shock");
            json.setType("Chem");

            string strPath = Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
            strPath = strPath + "\\Aquamasters\\test.ini";

            string jsonData = JsonConvert.SerializeObject(json, Formatting.None);
            System.IO.File.WriteAllText(strPath, jsonData);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            UpdateTotals();

        }

        private void dg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }

    /**
     * Part: the basic framework for a part or chem
     *      has a name, type and cost.
     *      fields are public to allow JsonConvert
     *      to serialize the object.
     */
    public class Part {

        public string name;
        public string type;
        public decimal cost;

        public decimal getCost() => this.cost;
        public string  getName() => this.name;
        public string  getType() => this.type;

        public void setCost(double cost) => this.cost = Decimal.Parse(cost.ToString());
        public void setName(string  name) => this.name = name;
        public void setType(string  type) => this.type = type;


        public Part(double cost, string name, string type)
        {
            this.cost = Decimal.Parse(cost.ToString());
            this.name = name;
            this.type = type;
        }

        public Part() { }
    }
}
