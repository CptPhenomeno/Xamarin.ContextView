using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace ContextView.Sample.ViewModels
{
    internal class AnyViewsViewModel : ObservableObject
    {
        private Color _boxColor;
        public Color BoxColor
        {
            get => this._boxColor;
            set => SetProperty(ref this._boxColor, value);
        }

        public ICommand LabelCommand { get; private set; }
        public ICommand BoxCommand { get; private set; }

        public AnyViewsViewModel()
        {
            this.BoxColor = Color.Red;
            
            this.LabelCommand = CommandFactory.Create(() => App.Current.MainPage.DisplayAlert("Hey!", "This is a label", "Cancel"));
            this.BoxCommand = CommandFactory.Create<Color>(color => this.BoxColor = color);
        }
    }
}
