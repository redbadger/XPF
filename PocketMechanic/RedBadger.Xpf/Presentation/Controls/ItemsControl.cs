namespace RedBadger.Xpf.Presentation.Controls
{
    using System;
    using System.Collections;
    using System.Windows;

    public class ItemsControl : Control
    {
        public static readonly XpfDependencyProperty ItemTemplateProperty =
            XpfDependencyProperty.Register(
                "ItemTemplate", typeof(Func<IElement>), typeof(ItemsControl), new PropertyMetadata(null));

        public static readonly XpfDependencyProperty ItemsPanelProperty = XpfDependencyProperty.Register(
            "ItemsPanel", typeof(Panel), typeof(ItemsControl), new PropertyMetadata(null));

        public static readonly XpfDependencyProperty ItemsSourceProperty = XpfDependencyProperty.Register(
            "ItemsSource", typeof(IEnumerable), typeof(ItemsControl), new PropertyMetadata(null));

        public ItemsControl()
        {
            this.ItemsPanel = new StackPanel();
        }

        public Func<IElement> ItemTemplate
        {
            get
            {
                return (Func<IElement>)this.GetValue(ItemTemplateProperty.Value);
            }

            set
            {
                this.SetValue(ItemTemplateProperty.Value, value);
            }
        }

        public Panel ItemsPanel
        {
            get
            {
                return (Panel)this.GetValue(ItemsPanelProperty.Value);
            }

            set
            {
                this.SetValue(ItemsPanelProperty.Value, value);
            }
        }

        public IEnumerable ItemsSource
        {
            get
            {
                return (IEnumerable)this.GetValue(ItemsSourceProperty.Value);
            }

            set
            {
                this.SetValue(ItemsSourceProperty.Value, value);
            }
        }

        public override void OnApplyTemplate()
        {
            if (this.ItemTemplate == null || this.ItemsSource == null)
            {
                return;
            }

            foreach (var item in this.ItemsSource)
            {
                var element = this.ItemTemplate();
                element.DataContext = item;
                this.ItemsPanel.Children.Add(element);
            }
        }
    }
}