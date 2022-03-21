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
    public partial class CodeWorkspace : ContentPage
    {
        public CodeWorkspace()
        {
            InitializeComponent();
        }
    }
}