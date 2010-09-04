namespace RedBadger.Xpf.Presentation.Controls
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;

#if WINDOWS_PHONE
    using Microsoft.Phone.Reactive;
#endif

    public class ItemsControl : Control
    {
        public static readonly IDependencyProperty ItemTemplateProperty =
            DependencyProperty<Func<IElement>, ItemsControl>.Register("ItemTemplate", new PropertyMetadata(null));

        public static readonly IDependencyProperty ItemsPanelProperty =
            DependencyProperty<Panel, ItemsControl>.Register(
                "ItemsPanel", new PropertyMetadata(null, ItemsPanelChanged));

        public static readonly IDependencyProperty ItemsSourceProperty =
            DependencyProperty<IEnumerable, ItemsControl>.Register(
                "ItemsSource", new PropertyMetadata(null, ItemsSourceChanged));

        private readonly ScrollViewer scrollViewer;

        private IDisposable changingItems;

        private bool isItemsSourceNew;

        public ItemsControl()
        {
            this.scrollViewer = new ScrollViewer { VisualParent = this };
            this.ItemsPanel = new StackPanel();
        }

        public Func<IElement> ItemTemplate
        {
            get
            {
                return this.GetValue<Func<IElement>>(ItemTemplateProperty);
            }

            set
            {
                this.SetValue(ItemTemplateProperty, value);
            }
        }

        public Panel ItemsPanel
        {
            get
            {
                return this.GetValue<Panel>(ItemsPanelProperty);
            }

            set
            {
                this.SetValue(ItemsPanelProperty, value);
            }
        }

        public IEnumerable ItemsSource
        {
            get
            {
                return this.GetValue<IEnumerable>(ItemsSourceProperty);
            }

            set
            {
                this.SetValue(ItemsSourceProperty, value);
            }
        }

        public override IEnumerable<IElement> GetVisualChildren()
        {
            ScrollViewer child = this.scrollViewer;
            if (child != null)
            {
                yield return child;
            }

            yield break;
        }

        public override void OnApplyTemplate()
        {
            if (this.isItemsSourceNew)
            {
                this.PopulatePanelFromItemsSource();
                this.isItemsSourceNew = false;
            }
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            ScrollViewer child = this.scrollViewer;
            if (child != null)
            {
                child.Arrange(new Rect(new Point(), finalSize));
            }

            return finalSize;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            ScrollViewer child = this.scrollViewer;
            if (child == null)
            {
                return Size.Empty;
            }

            child.Measure(availableSize);
            return child.DesiredSize;
        }

        private static void ItemsPanelChanged(
            DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var itemsControl = dependencyObject as ItemsControl;
            if (itemsControl != null)
            {
                itemsControl.ItemsPanelChanged(args.NewValue);
            }
        }

        private static void ItemsSourceChanged(
            DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var itemsControl = dependencyObject as ItemsControl;
            if (itemsControl != null)
            {
                itemsControl.ItemsSourceChanged(args.OldValue, args.NewValue);
            }
        }

        private void ItemsPanelChanged(object newValue)
        {
            if (!(((Panel)newValue).Children is ITemplatedList<IElement>))
            {
                throw new NotSupportedException(
                    "ItemsControl requires a panel whose Children collection implements ITemplatedList<IElement>");
            }

            this.InvalidateMeasure();

            var newPanel = newValue as IElement;
            if (newPanel != null)
            {
                this.scrollViewer.Content = newPanel;
            }
        }

        private void ItemsSourceChanged(object oldValue, object newValue)
        {
            if (oldValue is INotifyCollectionChanged)
            {
                this.changingItems.Dispose();
            }

            var observableCollection = newValue as INotifyCollectionChanged;
            if (observableCollection != null)
            {
                this.changingItems =
                    Observable.FromEvent<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>(
                        handler => new NotifyCollectionChangedEventHandler(handler), 
                        handler => observableCollection.CollectionChanged += handler, 
                        handler => observableCollection.CollectionChanged -= handler).Subscribe(this.OnNextItemChange);
            }

            this.isItemsSourceNew = true;
            this.InvalidateMeasure();
        }

        private void OnNextItemChange(IEvent<NotifyCollectionChangedEventArgs> eventData)
        {
            var children = (ITemplatedList<IElement>)this.ItemsPanel.Children;
            switch (eventData.EventArgs.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (object newItem in eventData.EventArgs.NewItems)
                    {
                        children.Add(newItem, this.ItemTemplate);
                    }

                    break;
                case NotifyCollectionChangedAction.Remove:
                    {
                        int startingIndex = eventData.EventArgs.OldStartingIndex;
                        for (int index = startingIndex;
                             index < startingIndex + eventData.EventArgs.OldItems.Count;
                             index++)
                        {
                            children.RemoveAt(index);
                        }

                        break;
                    }

                case NotifyCollectionChangedAction.Replace:
                    {
                        int startingIndex = eventData.EventArgs.NewStartingIndex;

                        foreach (object newItem in eventData.EventArgs.NewItems)
                        {
                            this.ItemsPanel.Children.RemoveAt(startingIndex);
                            children.Insert(startingIndex, newItem, this.ItemTemplate);
                            startingIndex++;
                        }

                        break;
                    }

#if !WINDOWS_PHONE
                case NotifyCollectionChangedAction.Move:
                    children.Move(eventData.EventArgs.OldStartingIndex, eventData.EventArgs.NewStartingIndex);

                    break;
#endif
                case NotifyCollectionChangedAction.Reset:
                    this.PopulatePanelFromItemsSource();
                    break;
            }
        }

        private void PopulatePanelFromItemsSource()
        {
            var children = (ITemplatedList<IElement>)this.ItemsPanel.Children;
            children.Clear();

            if (this.ItemsSource != null)
            {
                foreach (object item in this.ItemsSource)
                {
                    children.Add(item, this.ItemTemplate);
                }
            }
        }
    }
}