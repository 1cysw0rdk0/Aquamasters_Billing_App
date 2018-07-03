﻿using System;
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
    /// Interaction logic for searchAndAdd.xaml
    /// </summary>
    public partial class searchAndAdd : Window {

        public struct partOrder {
            public Part part { get; set; }
            public Decimal quantity { get; set; }
        }

        public List<Part> priceSheetList;
        public List<partOrder> cart;

        public searchAndAdd(Dictionary<string, Part> PriceSheet) {
            InitializeComponent();


            // Copy items from dictionary to a list so as to avoid having to deal with MultiBinding cause fuck that
            priceSheetList = new List<Part>();
            cart = new List<partOrder>();
            foreach (Part part in PriceSheet.Values) { priceSheetList.Add(part); };

            
            // Initialize the Data grid for priceList items
            priceListDG.Columns.Add(new DataGridTextColumn { Header = "Part - Labor - Service", Binding = new Binding("name"), Width=264, IsReadOnly=true });
            priceListDG.Columns.Add(new DataGridTextColumn { Header = "Cost", Binding = new Binding("cost"), MinWidth=45, IsReadOnly=false });
            priceListDG.Columns.Add(new DataGridTextColumn { Header = "Type", Binding = new Binding("type"), MinWidth=47, MaxWidth=47, IsReadOnly=true });

            // Initialize the Data grid for cart items
            cartDg.Columns.Add(new DataGridTextColumn { Header = "Part - Labor - Service", Binding = new Binding("part.name"), Width = 264, IsReadOnly=true });
            cartDg.Columns.Add(new DataGridTextColumn { Header = "Cost", Binding = new Binding("part.cost"), MinWidth = 45, IsReadOnly = true });
            cartDg.Columns.Add(new DataGridTextColumn { Header = "Quantity", Binding = new Binding("quantity"), MinWidth=47, IsReadOnly=false });

            // Bind the data grids to the proper lists
            priceListDG.ItemsSource = priceSheetList;
            cartDg.ItemsSource = cart;
        }

        private void priceListDG_MouseDoubleClick(object sender, MouseButtonEventArgs e) {

            // Add the double clicked item, if the item was clicked on the name label.
            // Refresh the cart so it will update
            if (priceListDG.CurrentColumn == priceListDG.Columns[0]) {
                cart.Add(new partOrder { part = (Part)priceListDG.SelectedItem, quantity = 0 });
                cartDg.Items.Refresh();
            }
            
        }
    }
}