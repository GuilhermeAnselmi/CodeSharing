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

            ListAllBlocks();
        }

        private void ListAllBlocks()
        {
            JsonDb db = new JsonDb();

            MyBlock[] block = db.ListAllBlocks();

            lstBlocks.ItemsSource = block;

            if (block != null) background.IsVisible = false;
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

                ListAllBlocks();
            }
        }

        private void Expand(object sender, EventArgs e)
        {
            StackLayout objCode = (StackLayout)((Button)sender).Parent.Parent.LogicalChildren[1];

            objCode.IsVisible = !objCode.IsVisible;
        }

        private void SaveChanges(object sender, EventArgs e)
        {
            Button btnSave = ((Button)sender);
            Button btnEdit = ((Button)btnSave.Parent.LogicalChildren[0]);
            Editor editor = ((Editor)btnSave.Parent.LogicalChildren[2]);

            string blockName = ((Button)sender).CommandParameter.ToString();
            string code = editor.Text;

            JsonDb db = new JsonDb();

            var response = db.CodeChanges(blockName, code);

            if (!response.Item1) DisplayAlert("Code Not Changed", response.Item2, "OK");

            btnSave.IsVisible = false;
            btnEdit.IsVisible = true;

            editor.IsReadOnly = true;
            editor.BackgroundColor = Color.Transparent;
        }

        private void OpenCodeEditor(object sender, EventArgs e)
        {
            Button btnEdit = ((Button)sender);
            Button btnSave = ((Button)btnEdit.Parent.LogicalChildren[1]);
            Editor editor = ((Editor)btnEdit.Parent.LogicalChildren[2]);

            btnEdit.IsVisible = false;
            btnSave.IsVisible = true;

            editor.IsReadOnly = false;
            editor.BackgroundColor = Color.Black;
        }
    }
}