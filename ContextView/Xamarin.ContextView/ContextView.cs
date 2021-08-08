using System;
using Xamarin.Forms;
using Xamarin.CommunityToolkit.Effects;
using System.Collections.Generic;
using System.Linq;
using Xamarin.CommunityToolkit.ObjectModel;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Xamarin.ContextView
{
    public class ContextView : ContentView
    {
        private readonly IAsyncCommand _openContextMenuCommand;
        private readonly TapGestureRecognizer _openContextMenuTapGesture;

        public ContextView()
        {
            this._openContextMenuCommand = CommandFactory.Create(OpenContextPopupAsync);
            this._openContextMenuTapGesture = new TapGestureRecognizer { Command = this._openContextMenuCommand };
            this.GestureRecognizers.Add(this._openContextMenuTapGesture);
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            SetMenuItemsBinding();
        }

        private ObservableCollection<MenuItem> _menuItems;
        public ObservableCollection<MenuItem> MenuItems
        {
            get
            {
                if (this._menuItems == null)
                {
                    this._menuItems = new ObservableCollection<MenuItem>();
                    this._menuItems.CollectionChanged += OnMenuItemsCollectionChanged;
                }

                return this._menuItems;
            }
        }

        private void OnMenuItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            SetMenuItemsBinding();
        }

        private void SetMenuItemsBinding()
        {
            foreach (var menuItem in this._menuItems)
                menuItem.BindingContext = this.BindingContext;
        }

        private async Task OpenContextPopupAsync()
        {
            if (this.IsEnabled is true)
            {
                var contextPopup = new ContextPopup(this.MenuItems)
                {
                    Anchor = this
                };
                await this.Navigation.ShowPopupAsync(contextPopup);
            }
        }
    }
}
