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
                MyCode code = new MyCode(block);
                code.CreateBlock();

                DisplayAlert("Block Created", "New block created succesafully", "OK");
            }
        }

        private void CreateJsonSave()
        {
            List<MyBlock> mb = new List<MyBlock>();

            string[] codes =
            {
                "Console.WriteLine(\"Hello World!\")",
                "var test = Console.ReadLine()"
            };

            string[] codes2 =
            {
                "Console.WriteLine(\"Hello World!\")",
                "var test = Console.ReadLine()"
            };

            string[] codes3 =
            {
                "Console.WriteLine(\"Hello World!\")",
                "var test = Console.ReadLine()"
            };

            List<MyBlock> blocks = new List<MyBlock>();
            blocks.Add(new MyBlock { BlockName = "MongoDB", CodeList = codes });
            blocks.Add(new MyBlock { BlockName = "Test2", CodeList = codes2 });
            blocks.Add(new MyBlock { BlockName = "Test3", CodeList = codes3 });

            mb = blocks;

            string json = JsonConvert.SerializeObject(blocks);

            string fileName = "Coding.json";
            string path = Path.GetTempPath() + fileName;

            if (!File.Exists(path))
            {
                var file = File.Create(path);
                file.Close();
            }

            File.WriteAllText(path, json);
        }
    }
}