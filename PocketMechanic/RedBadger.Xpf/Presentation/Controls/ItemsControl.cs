namespace RedBadger.Xpf.Presentation.Controls
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Windows;

#if WINDOWS_PHONE
    using Microsoft.Phone.Reactive;
#endif

    public class ItemsControl : Control
    {
        public static readonly XpfDependencyProperty ItemTemplateProperty =
            XpfDependencyProperty.Register(
                "ItemTemplate", typeof(Func<IElement>), typeof(ItemsControl), new PropertyMetadata(null));

        public static readonly XpfDependencyProperty ItemsPanelProperty = XpfDependencyProperty.Register(
            "ItemsPanel", typeof(Panel), typeof(ItemsControl), new PropertyMetadata(null, ItemsPanelChanged));

        public static readonly XpfDependencyProperty ItemsSourceProperty = XpfDependencyProperty.Register(
            "ItemsSource", typeof(IEnumerable), typeof(ItemsControl), new PropertyMetadata(null, ItemsSourceChanged));

        private IDisposable itemsChanged;

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

        public override IEnumerable<IElement> GetChildren()
        {
            var content = this.ItemsPanel;
            if (content != null)
            {
                yield return content;
            }

            yield break;
        }

        public override void OnApplyTemplate()
        {
            if (this.ItemTemplate == null || this.ItemsSource == null)
            {
                return;
            }

            this.PopulatePanelFromItemsSource();
        }

        private static void ItemsPanelChanged(
            DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var itemsControl = dependencyObject as ItemsControl;
            if (itemsControl != null)
            {
                itemsControl.ItemsPanelChanged(args.OldValue, args.NewValue);
            }
        }

        private static void ItemsSourceChanged(
            DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var itemsControl = dependencyObject as ItemsControl;
            if (itemsControl != null)
            {
                itemsControl.ItemsSourceChanged(args.NewValue);
            }
        }

        private void ItemsPanelChanged(object oldValue, object newValue)
        {
            this.InvalidateMeasure();

            var oldPanel = oldValue as IElement;
            var newPanel = newValue as IElement;

            if (oldPanel != null)
            {
                oldPanel.VisualParent = null;
            }

            if (newPanel != null)
            {
                newPanel.VisualParent = this;
            }
        }

        private void ItemsSourceChanged(object newValue)
        {
            if (newValue == null)
            {
                this.ItemsPanel.Children.Clear();
                this.itemsChanged.Dispose();
                return;
            }

            var observableCollection = newValue as INotifyCollectionChanged;
            if (observableCollection != null)
            {
                this.itemsChanged =
                    Observable.FromEvent<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>(
                        handler => new NotifyCollectionChangedEventHandler(handler), 
                        handler => observableCollection.CollectionChanged += handler, 
                        handler => observableCollection.CollectionChanged -= handler).Subscribe(this.OnNextItemChange);
            }
        }

        private IElement NewItem(object item)
        {
            var element = this.ItemTemplate();
            element.DataContext = item;
            return element;
        }

        private void OnNextItemChange(IEvent<NotifyCollectionChangedEventArgs> eventData)
        {
            switch (eventData.EventArgs.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var newItem in eventData.EventArgs.NewItems)
                    {
                        this.ItemsPanel.Children.Add(this.NewItem(newItem));
                    }

                    break;
                case NotifyCollectionChangedAction.Remove:
                    {
                        var startingIndex = eventData.EventArgs.OldStartingIndex;
                        for (int index = startingIndex;
                             index < startingIndex + eventData.EventArgs.OldItems.Count;
                             index++)
                        {
                            this.ItemsPanel.Children.RemoveAt(index);
                        }

                        break;
                    }

                case NotifyCollectionChangedAction.Replace:
                    {
                        var startingIndex = eventData.EventArgs.NewStartingIndex;

                        foreach (var newItem in eventData.EventArgs.NewItems)
                        {
                            this.ItemsPanel.Children.RemoveAt(startingIndex);
                            this.ItemsPanel.Children.Insert(startingIndex, this.NewItem(newItem));
                            startingIndex++;
                        }

                        break;
                    }

#if !WINDOWS_PHONE
                case NotifyCollectionChangedAction.Move:
                    var elementToMove = this.ItemsPanel.Children[eventData.EventArgs.OldStartingIndex];
                    this.ItemsPanel.Children.RemoveAt(eventData.EventArgs.OldStartingIndex);
                    this.ItemsPanel.Children.Insert(eventData.EventArgs.NewStartingIndex, elementToMove);
                    break;
#endif
                case NotifyCollectionChangedAction.Reset:
                    this.PopulatePanelFromItemsSource();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void PopulatePanelFromItemsSource()
        {
            this.ItemsPanel.Children.Clear();

            foreach (var item in this.ItemsSource)
            {
                this.ItemsPanel.Children.Add(this.NewItem(item));
            }
        }
    }
}