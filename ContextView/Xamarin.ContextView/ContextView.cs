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

        #region TapRequired
        public static readonly BindableProperty NumberOfTapsRequiredProperty = BindableProperty.Create(
            nameof(NumberOfTapsRequired),
            typeof(int),
            typeof(ContextView),
            1,
            propertyChanged: OnNumberOfTapsRequiredPropertyChanged);

        public int NumberOfTapsRequired
        {
            get => (int)GetValue(NumberOfTapsRequiredProperty);
            set => SetValue(NumberOfTapsRequiredProperty, value);
        }

        private static void OnNumberOfTapsRequiredPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ContextView contextView && newValue is int numberOfTapsRequired)
            {
                contextView._openContextMenuTapGesture.NumberOfTapsRequired = numberOfTapsRequired;
            }
        }
        #endregion


        #region LongPressDuration
        public static readonly BindableProperty LongPressDurationProperty = BindableProperty.Create(
            nameof(LongPressDuration),
            typeof(int),
            typeof(ContextView),
            100,
            propertyChanged: OnLongPressDurationPropertyChanged);

        public int LongPressDuration
        {
            get => (int)GetValue(LongPressDurationProperty);
            set => SetValue(LongPressDurationProperty, value);
        }

        private static void OnLongPressDurationPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ContextView contextView && newValue is int longPressDuration)
            {
                TouchEffect.SetLongPressDuration(contextView, longPressDuration);
            }
        }
        #endregion

        #region Size
        public static readonly BindableProperty ContextMenuSizeProperty = BindableProperty.Create(
            nameof(ContextMenuSize),
            typeof(Size),
            typeof(ContextView),
            Size.Zero);

        public Size ContextMenuSize
        {
            get => (Size)GetValue(ContextMenuSizeProperty);
            set => SetValue(ContextMenuSizeProperty, value);
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

                TouchEffect.SetLongPressCommand(this, null);
                this.GestureRecognizers.Add(this._openContextMenuTapGesture);
            }
            else
            {
                this.GestureRecognizers.Remove(this._openContextMenuTapGesture);
                TouchEffect.SetLongPressCommand(this, this._openContextMenuCommand);
                TouchEffect.SetLongPressDuration(this, this.LongPressDuration);
            }
        }

        private async Task OpenContextPopupAsync()
        {
            if (this.IsEnabled is true)
            {
                var contextPopup = new ContextPopupBuilder()
                .WithMenuItems(this.MenuItems)
                .WithSize(this.ContextMenuSize)
                .WithBackgroundColor(this.ContextMenuBackgroundColor)
                .WithAnchor(this)
                .Build();

                await this.Navigation.ShowPopupAsync(contextPopup);
            }
        }
    }
}
