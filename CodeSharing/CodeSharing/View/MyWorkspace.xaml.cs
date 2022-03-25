using System;
using System.Windows.Input;
using CodeSharing.Controller;
using CodeSharing.Model;

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
            StackLayout objCode = (StackLayout)((Button)sender).Parent.Parent.LogicalChildren[1];

            objCode.IsVisible = !objCode.IsVisible;
        }

        private void SaveChanges(object sender, EventArgs e)
        {
            string blockName = ((MyBlock)((Editor)sender).BindingContext).BlockName;
            string code = ((Editor)sender).Text;

            JsonDb db = new JsonDb();

            var response = db.CodeChanges(blockName, code);

            if (!response.Item1) DisplayAlert("Code Not Changed", response.Item2, "OK");

            ((Editor)sender).IsVisible = false;
            ((Button)((Editor)sender).Parent.LogicalChildren[0]).IsVisible = false;
            ((Label)((Editor)sender).Parent.LogicalChildren[2]).IsVisible = true;
        }

        private void OpenCodeEditor(object sender, EventArgs e)
        {
            ((Button)sender).IsVisible = false;
            ((Editor)((Button)sender).Parent.LogicalChildren[1]).IsVisible = true;
            ((Label)((Button)sender).Parent.LogicalChildren[2]).IsVisible = false;
        }
    }
}