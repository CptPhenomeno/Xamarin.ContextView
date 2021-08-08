using ContextView.Sample.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ContextView.Sample.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListViewAllItemContextPage : ContentPage
    {
        public ListViewAllItemContextPage()
        {
            InitializeComponent();
            BindingContext = new ListViewAllItemContextViewModel();
        }
    }
}