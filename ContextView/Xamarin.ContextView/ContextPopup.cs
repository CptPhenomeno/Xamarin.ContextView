using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace Xamarin.ContextView
{
    internal class ContextPopupLayout : StackLayout
    {
        private readonly ContextPopup _contextPopup;
        private readonly bool _shouldAutosize;
        private double _maxWidth = double.MinValue;
        private double _maxHeight = double.MinValue;

        public ContextPopupLayout(ContextPopup contextPopup, bool shouldAutosize)
        {
            this._contextPopup = contextPopup;
            this._shouldAutosize = shouldAutosize;

            this.HorizontalOptions = LayoutOptions.CenterAndExpand;
            this.VerticalOptions = LayoutOptions.CenterAndExpand;
        }

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            var measure = base.OnMeasure(widthConstraint, heightConstraint);
            if (this._shouldAutosize is true)
            {
                this._maxWidth = Math.Max(this._maxWidth, measure.Request.Width);
                this._maxHeight = Math.Max(this._maxHeight, measure.Request.Height);
                this._contextPopup.Size = new Size(this._maxWidth + 4 * this.Margin.HorizontalThickness, this._maxHeight + 4 * this.Margin.VerticalThickness); 
            }
            return measure;
        }
    }

    internal class ContextPopupBuilder
    {
        private ContextPopup _popup;
        private IList<MenuItem> _menuItems;
        private Size _size;
        private Color _backgroundColor;
        private View _anchor;

        public ContextPopupBuilder()
        {
            this.Reset();
        }

        public ContextPopupBuilder WithMenuItems(IList<MenuItem> menuItems)
        {
            this._menuItems = menuItems;
            return this;
        }

        public ContextPopupBuilder WithSize(Size size)
        {
            this._size = size;
            return this;
        }

        public ContextPopupBuilder WithBackgroundColor(Color backgroundColor)
        {
            this._backgroundColor = backgroundColor;
            return this;
        }
        public ContextPopupBuilder WithAnchor(View anchor)
        {
            this._anchor = anchor;
            return this;
        }


        public ContextPopup Build()
        {
            if (this._menuItems == null || this._menuItems.Any() is false)
                throw new InvalidOperationException("You must specify a MenuItems collection");

            this._popup = new ContextPopup(
                this._menuItems,
                this._size,
                this._backgroundColor,
                this._anchor);

            return this._popup;
        }

        private void Reset()
        {
            this._menuItems = null;
            this._size = Size.Zero;
            this._backgroundColor = Color.White;
            this._anchor = null;
        }
    }

    internal class ContextPopup : Popup
    {
        private readonly IList<MenuItem> _menuItems;
        private StackLayout _stackLayout;
        private ScrollView _scrollView;

        internal ContextPopup() { }

        internal ContextPopup(IList<MenuItem> menuItems, Size size, Color backgroundColor, View anchor)
        {
            this._menuItems = menuItems;
            this.Size = size;
            this.BackgroundColor = backgroundColor;
            this.Anchor = anchor;
            this._stackLayout = new ContextPopupLayout(this, size == Size.Zero) { Margin = 2, Padding = 2, BackgroundColor = this.BackgroundColor };
            this._scrollView = new ScrollView { Margin = 0, Padding = 0, BackgroundColor = this.BackgroundColor };

            foreach (var menuItem in this._menuItems)
            {
                menuItem._internalContextPopup = this;
                this._stackLayout.Children.Add(menuItem);
            }

            this._scrollView.Content = this._stackLayout;

            this.Content = this._scrollView;
        }
    }
}
