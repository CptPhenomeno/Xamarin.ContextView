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
    public enum InteractionMode
    {
        Tap,
        LongPress
    }

    public class ContextView : ContentView
    {
        private readonly IAsyncCommand _openContextMenuCommand;
        private TapGestureRecognizer _openContextMenuTapGesture;

        public ContextView()
        {
            this._openContextMenuCommand = CommandFactory.Create(OpenContextPopupAsync);
            SetupInteractionMode();
        }

        #region ContextMenuBackgroundColor
        public static readonly BindableProperty ContextMenuBackgroundColorProperty = BindableProperty.Create(
            nameof(ContextMenuBackgroundColor),
            typeof(Color),
            typeof(ContextView));

        public Color ContextMenuBackgroundColor
        {
            get => (Color)GetValue(ContextMenuBackgroundColorProperty);
            set => SetValue(ContextMenuBackgroundColorProperty, value);
        }
        #endregion

        #region Mode
        public static readonly BindableProperty ModeProperty = BindableProperty.Create(
            nameof(Mode),
            typeof(InteractionMode),
            typeof(ContextView),
            InteractionMode.Tap,
            propertyChanged: OnModePropertyChanged);

        public InteractionMode Mode
        {
            get => (InteractionMode)GetValue(ModeProperty);
            set => SetValue(ModeProperty, value);
        }

        private static void OnModePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ContextView contextView && newValue is InteractionMode interactionMode)
            {
                contextView.Mode = interactionMode;
                contextView.SetupInteractionMode();
            }
        }
        #endregion

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

        private void SetupInteractionMode()
        {
            this.GestureRecognizers.Clear();
            this.Effects.Clear();

            if (this.Mode is InteractionMode.Tap)
            {
                if (this._openContextMenuTapGesture == null)
                    this._openContextMenuTapGesture = new TapGestureRecognizer { Command = this._openContextMenuCommand };

                this.GestureRecognizers.Add(this._openContextMenuTapGesture);
            }
            else
            {
                TouchEffect.SetLongPressCommand(this, this._openContextMenuCommand);
            }
        }

        private async Task OpenContextPopupAsync()
        {
            if (this.IsEnabled is true)
            {
                var contextPopup = new ContextPopup(this.MenuItems)
                {
                    Anchor = this,
                    BackgroundColor = this.ContextMenuBackgroundColor
                };
                await this.Navigation.ShowPopupAsync(contextPopup);
            }
        }
    }
}
