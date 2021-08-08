using ContextView.Sample.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ContextView.Sample
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void GoToListViewAllItemContextPage_Clicked(object sender, EventArgs e)
        {
            this.Navigation.PushAsync(new ListViewAllItemContextPage());
        }
    }
}
