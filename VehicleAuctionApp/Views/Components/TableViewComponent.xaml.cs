using System.Collections;
using VehicleAuctionApp.Models.Components;
namespace VehicleAuctionApp.Views.Components;

public partial class TableViewComponent : ContentView
{
    public TableViewComponent()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty ItemsProperty =
           BindableProperty.Create(
               nameof(Items),
               typeof(IEnumerable),
               typeof(TableViewComponent),
               propertyChanged: OnItemsChanged);

    public IEnumerable Items
    {
        get => (IEnumerable)GetValue(ItemsProperty);
        set => SetValue(ItemsProperty, value);
    }

    public static readonly BindableProperty ColumnDefinitionsProperty =
            BindableProperty.Create(
                nameof(ColumnDefinitions),
                typeof(IEnumerable<CustomColumnDefinition>),
                typeof(TableViewComponent),
                propertyChanged: OnColumnDefinitionsChanged);


    public IEnumerable<CustomColumnDefinition> ColumnDefinitions
    {
        get => (IEnumerable<CustomColumnDefinition>)GetValue(ColumnDefinitionsProperty);
        set => SetValue(ColumnDefinitionsProperty, value);
    }

    public static readonly BindableProperty SelectedItemProperty =
              BindableProperty.Create(
                  nameof(SelectedItem),
                  typeof(object),
                  typeof(TableViewComponent),
                  default(object),
                  BindingMode.TwoWay);

    public object SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    private void BuildHeaders()
    {
        HeaderStack.Children.Clear();
        if (ColumnDefinitions != null)
        {
            foreach (var column in ColumnDefinitions)
            {
                var label = new Label
                {
                    Text = column.Header,
                    FontAttributes = column.IsBold ? FontAttributes.Bold : FontAttributes.None,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Start,
                    Margin = new Thickness(5, 0)
                };

                if (column.Width.HasValue)
                {
                    label.WidthRequest = column.Width.Value;
                }

                HeaderStack.Children.Add(label);
            }
        }
    }

    private void BuildItemTemplate()
    {
        if (ColumnDefinitions == null)
            return;

        var dataTemplate = new DataTemplate(() =>
        {
            var grid = new Grid
            {
                Padding = 10,
                ColumnSpacing = 10
            };

            foreach (var column in ColumnDefinitions)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition
                {
                    Width = column.Width.HasValue ? new GridLength(column.Width.Value) : GridLength.Auto
                });
            }

            int columnIndex = 0;
            foreach (var column in ColumnDefinitions)
            {
                var label = new Label
                {
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Start,
                    Margin = new Thickness(5, 0)
                };

                if (!string.IsNullOrEmpty(column.FormatString))
                {
                    label.SetBinding(Label.TextProperty, new Binding(column.BindingProperty, stringFormat: column.FormatString));
                }
                else
                {
                    label.SetBinding(Label.TextProperty, column.BindingProperty);
                }

                if (column.IsBold)
                {
                    label.FontAttributes = FontAttributes.Bold;
                }

                grid.SetColumn(label, columnIndex);
                grid.Children.Add(label);
                columnIndex++;
            }

            return grid;
        });

        TableCollectionView.ItemTemplate = dataTemplate;
    }

    private static void OnColumnDefinitionsChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (TableViewComponent)bindable;
        control.BuildHeaders();
        control.BuildItemTemplate();
    }

    private static void OnItemsChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (TableViewComponent)bindable;
    }
}