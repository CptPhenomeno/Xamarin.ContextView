using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace Xamarin.ContextView
{
    public class MenuItem : ContentView
    {
        private readonly ICommand _internalCommand;
        internal ContextPopup _internalContextPopup;

        public MenuItem()
        {
            this._internalCommand = CommandFactory.Create(InternalMenuAction);
            this.GestureRecognizers.Add(new TapGestureRecognizer { Command = this._internalCommand });
        }

        #region Command
        public static readonly BindableProperty CommandProperty = BindableProperty.Create(
            nameof(Command),
            typeof(ICommand),
            typeof(MenuItem));

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }
        #endregion

        #region CommandParameter
        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(
            nameof(Command),
            typeof(object),
            typeof(MenuItem));

        public object CommandParameter
        {
            get => (object)GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        } 
        #endregion

        private void InternalMenuAction()
        {
            if (this.Command != null && this.Command.CanExecute(this.CommandParameter))
            {
                this.Command.Execute(this.CommandParameter);

                this._internalContextPopup?.Dismiss(null);
            }
        }
    }
}
