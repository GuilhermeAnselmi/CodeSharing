using System;
using System.Collections.Generic;
using System.Linq;
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

            ListBlocks();
        }

        private void ListBlocks()
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

                ListBlocks();
            }
        }

        private void Expand(object sender, EventArgs e)
        {
            ImageButton btn = ((ImageButton)sender);
            StackLayout objCode = (StackLayout)btn.Parent.Parent.LogicalChildren[1];

            objCode.IsVisible = !objCode.IsVisible;
            if (objCode.IsVisible) { btn.Source = "up.png"; } else { btn.Source = "down.png"; }
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

        private void RenameBlock(object sender, EventArgs e)
        {
            Rename(((Button)sender).CommandParameter.ToString());
        }

        private async void Rename(string blockName)
        {
            string newName = await DisplayPromptAsync("Rename Block", "New Name", "Save", "Cancel",
                "MyBlocking", -1, Keyboard.Text, "");

            if (newName != null && newName != "")
            {
                JsonDb db = new JsonDb();
                MyBlock[] blocks = db.ListAllBlocks();

                ((MyBlock)blocks.Where(x => x.BlockName == blockName).ToList()[0]).BlockName = newName;

                Tuple<bool, string> response = db.RewriteBlock(blocks, newName);

                if (response.Item1)
                {
                    ListBlocks();
                }
                else
                {
                    DisplayAlert("Block Not Renamed", response.Item2, "OK");
                }
            }
        }

        private void RemoveBlock(object sender, EventArgs e)
        {
            Remove(((Button)sender).CommandParameter.ToString());
        }

        private async void Remove(string blockName)
        {
            bool remove = await DisplayAlert("Remove Block", "You where removed block " + blockName + "?", "Remove", "Cancel");

            JsonDb db = new JsonDb();

            if (remove)
            {
                Tuple<bool, string> response = db.RemoveBlock(blockName);

                if (response.Item1)
                {
                    ListBlocks();
                }
                else
                {
                    DisplayAlert("Block Not Removed", response.Item2, "OK");
                }
            }
        }

        private void Dev(object sender, EventArgs e)
        {
            DisplayAlert("Development", "System in Development", "OK");
        }
    }
}