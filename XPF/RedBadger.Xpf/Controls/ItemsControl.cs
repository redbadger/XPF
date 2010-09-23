namespace RedBadger.Xpf.Controls
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
        public static readonly ReactiveProperty<Func<IElement>> ItemTemplateProperty =
            ReactiveProperty<Func<IElement>>.Register("ItemTemplate", typeof(ItemsControl));

        public static readonly ReactiveProperty<Panel> ItemsPanelProperty =
            ReactiveProperty<Panel>.Register("ItemsPanel", typeof(ItemsControl), ItemsPanelChanged);

        public static readonly ReactiveProperty<IEnumerable> ItemsSourceProperty =
            ReactiveProperty<IEnumerable>.Register("ItemsSource", typeof(ItemsControl), ItemsSourceChanged);

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
                return this.GetValue(ItemTemplateProperty);
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
                return this.GetValue(ItemsPanelProperty);
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
                return this.GetValue(ItemsSourceProperty);
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

        private static void ItemsPanelChanged(IReactiveObject source, ReactivePropertyChangeEventArgs<Panel> change)
        {
            var itemsControl = (ItemsControl)source;
            Panel panel = change.NewValue;
            if (!(panel.Children is ITemplatedList<IElement>))
            {
                throw new NotSupportedException(
                    "ItemsControl requires a panel whose Children collection implements ITemplatedList<IElement>");
            }

            itemsControl.InvalidateMeasure();
            itemsControl.scrollViewer.Content = panel;
        }

        private static void ItemsSourceChanged(
            IReactiveObject source, ReactivePropertyChangeEventArgs<IEnumerable> change)
        {
            var itemsControl = (ItemsControl)source;
            if (change.OldValue is INotifyCollectionChanged)
            {
                itemsControl.changingItems.Dispose();
            }

            var observableCollection = change.NewValue as INotifyCollectionChanged;
            if (observableCollection != null)
            {
                itemsControl.changingItems =
                    Observable.FromEvent<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>(
                        handler => new NotifyCollectionChangedEventHandler(handler), 
                        handler => observableCollection.CollectionChanged += handler, 
                        handler => observableCollection.CollectionChanged -= handler).Subscribe(
                            itemsControl.OnNextItemChange);
            }

            itemsControl.isItemsSourceNew = true;
            itemsControl.InvalidateMeasure();
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