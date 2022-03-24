using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeSharing.Controller;
using CodeSharing.Model;
using Newtonsoft.Json;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CodeSharing.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyWorkspace : ContentPage
    {
        public MyWorkspace()
        {
            InitializeComponent();

            JsonDb db = new JsonDb();

            MyBlock[] block = db.ListAllBlocks();

            lstBlocks.ItemsSource = block;
        }

        private void NewBlock(object sender, EventArgs e)
        {
            CreateBlock();
        }

        private async void CreateBlock()
        {
            var block = await DisplayPromptAsync("New Block", "Block Name", "Create", "Cancel",
                "Block of My Code", -1, Keyboard.Text, "");

            if (block != "" && block != null)
            {
                Tuple<bool, string> response = null;

                JsonDb db = new JsonDb();
                response = db.CreateBlock(block);

                DisplayAlert(response.Item1 ? "Block Created" : "Block Not Created", response.Item2, "OK");
            }
        }

        private void Expand(object sender, EventArgs e)
        {
            ((Button)sender).CommandParameter.ToString();

            StackLayout obj = (StackLayout)FindByName("Test");
        }
    }
}