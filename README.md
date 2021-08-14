# Xamarin.ContextView
With Xamarin.ContextView you can add a context menu to any Xamarin.Forms views.

## Getting started
First of all you must reference the Xamarin.ContextView in your xaml file

    xmlns:context="clr-namespace:Xamarin.ContextView;assembly=Xamarin.ContextView"
    
then you can start using context view.

The ContextView is basically a content view, so you can wrap any view with a ContextView.
The menu options are defined using the MenuItems property

    <ListView
        x:Name="Elements"
        CachingStrategy="RecycleElementAndDataTemplate"
        IsPullToRefreshEnabled="False"
        ItemsSource="{Binding Restourants}"
        SelectionMode="None">
        <ListView.ItemTemplate>
            <DataTemplate>
                <ViewCell>
                    <context:ContextView>
                        <context:ContextView.MenuItems>
                            <context:MenuItem Command="{Binding BindingContext.OpenWebSiteCommand, Source={x:Reference ListViewAllItemContext}}" CommandParameter="{Binding .}">
                                <Label
                                    Padding="16"
                                    BackgroundColor="Lime"
                                    FontAttributes="Bold"
                                    FontSize="14"
                                    Text="Open WebSite"
                                    TextColor="Red" />
                            </context:MenuItem>
                            <context:MenuItem Command="{Binding BindingContext.OpenPositionCommand, Source={x:Reference ListViewAllItemContext}}" CommandParameter="{Binding .}">
                                <Label
                                    Padding="16"
                                    BackgroundColor="Pink"
                                    FontAttributes="Bold"
                                    FontSize="14"
                                    Text="Open Position"
                                    TextColor="Blue" />
                            </context:MenuItem>
                        </context:ContextView.MenuItems>

                        <StackLayout Margin="4" BackgroundColor="Wheat">
                            <Label FontSize="18" Text="{Binding Name}" />
                        </StackLayout>
                    </context:ContextView>

                </ViewCell>
            </DataTemplate>
        </ListView.ItemTemplate>
    </ListView>
    
MenuItem is another content view and also in this case you can wrap any existing xamarin forms view in it.


## Interaction mode
The ContextView menu can be open with a simple tap gesture or with long press gesture and is possibile to set this option using the Mode property.
The Mode property has two possible choice

- **Tap**: the default, the menu is open with a simple tap. You can set the number of taps required with the **NumberOfTapsRequired** property (default 1)
- **LongPress**: the menu is open with a long press. You can set the pression lenght before the menu should be opened using the **LongPressDuration** property (default 100 ms)


## Size
The ContextView try to set the menu size automatically using a best effort approach.
If you prefer it is possible to set the context menu size manually using the **Size** property.


