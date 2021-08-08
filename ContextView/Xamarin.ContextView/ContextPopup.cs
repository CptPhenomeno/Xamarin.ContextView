using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace Xamarin.ContextView
{
    internal class ContextPopup : Popup
    {
        private readonly IList<MenuItem> _menuItems;

        public ContextPopup(IList<MenuItem> menuItems)
        {
            this.Size = new Size(300, 300);
            this._menuItems = menuItems;
            var stackLayout = new StackLayout();
            var scrollView = new ScrollView();

            foreach (var menuItem in this._menuItems)
            {
                menuItem._internalContextPopup = this;
                stackLayout.Children.Add(menuItem);
            }

            scrollView.Content = stackLayout;

            this.Content = scrollView;
        }

    }
}
