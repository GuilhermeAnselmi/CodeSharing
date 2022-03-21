using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CodeSharing.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Menu : ContentPage
    {
        class Buttons
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Parameter { get; set; }
        }

        public Menu()
        {
            InitializeComponent();

            List<Buttons> buttons = new List<Buttons>();

            buttons.Add(new Buttons { Id = "btnMyWorkspace", Name = "My Workspace", Parameter = "My" });
            buttons.Add(new Buttons { Id = "btnNewWorkspace", Name = "All Workspaces", Parameter = "All" });

            menuList.ItemsSource = buttons;

        }

        public void NavigationMenu(object sender, EventArgs e)
        {
            switch (((Button)sender).CommandParameter.ToString())
            {
                case "My":
                    ((FlyoutPage)App.Current.MainPage).Detail = new NavigationPage(new MyWorkspace());
                    break;

                case "All":
                    ((FlyoutPage)App.Current.MainPage).Detail = new NavigationPage(new AllWorkspaces());
                    break;
            }

            ((FlyoutPage)App.Current.MainPage).IsPresented = false;
        }
    }
}