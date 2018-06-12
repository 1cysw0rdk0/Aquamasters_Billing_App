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
        public MainWindow()
        {
            InitializeComponent();

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
            string strPath = Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
            strPath = strPath + "\\Aquamasters\\test.ini";

            StreamReader file = File.OpenText(strPath);
            Dictionary<string, Part> PriceSheet = new Dictionary<string, Part>();


            while (!file.EndOfStream) {
                Part part = (Part)JsonConvert.DeserializeObject(file.ReadLine(), typeof(Part));
                PriceSheet.Add(part.name, part);
            }

            Console.Out.Write("");

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
