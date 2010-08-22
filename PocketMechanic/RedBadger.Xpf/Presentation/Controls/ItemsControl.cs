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
                "ItemTemplate", typeof(Func<object, IElement>), typeof(ItemsControl), new PropertyMetadata(null));

        public static readonly XpfDependencyProperty ItemsPanelProperty = XpfDependencyProperty.Register(
            "ItemsPanel", typeof(Panel), typeof(ItemsControl), new PropertyMetadata(null, ItemsPanelChanged));

        public static readonly XpfDependencyProperty ItemsSourceProperty = XpfDependencyProperty.Register(
            "ItemsSource", typeof(IEnumerable), typeof(ItemsControl), new PropertyMetadata(null, ItemsSourceChanged));

        private readonly ScrollViewer scrollViewer;

        private IDisposable changingItems;

        private bool isItemsSourceNew;

        public ItemsControl()
        {
            this.scrollViewer = new ScrollViewer { VisualParent = this };
            this.ItemsPanel = new StackPanel();
            this.scrollViewer.Content = this.ItemsPanel;
        }

        public Func<object, IElement> ItemTemplate
        {
            get
            {
                return (Func<object, IElement>)this.GetValue(ItemTemplateProperty.Value);
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

        public override IEnumerable<IElement> GetVisualChildren()
        {
            var child = this.scrollViewer;
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
            var child = this.scrollViewer;
            if (child != null)
            {
                child.Arrange(new Rect(new Point(), finalSize));
            }

            return finalSize;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var child = this.scrollViewer;
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
                itemsControl.ItemsPanelChanged(args.OldValue, args.NewValue);
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
            switch (eventData.EventArgs.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var newItem in eventData.EventArgs.NewItems)
                    {
                        this.ItemsPanel.AddChild(newItem, this.ItemTemplate);
                    }

                    break;
                case NotifyCollectionChangedAction.Remove:
                    {
                        int startingIndex = eventData.EventArgs.OldStartingIndex;
                        for (int index = startingIndex;
                             index < startingIndex + eventData.EventArgs.OldItems.Count;
                             index++)
                        {
                            this.ItemsPanel.RemoveChildAt(index);
                        }

                        break;
                    }

                case NotifyCollectionChangedAction.Replace:
                    {
                        int startingIndex = eventData.EventArgs.NewStartingIndex;

                        foreach (var newItem in eventData.EventArgs.NewItems)
                        {
                            this.ItemsPanel.RemoveChildAt(startingIndex);
                            this.ItemsPanel.InsertChildAt(startingIndex, newItem, this.ItemTemplate);
                            startingIndex++;
                        }

                        break;
                    }

#if !WINDOWS_PHONE
                case NotifyCollectionChangedAction.Move:
                    this.ItemsPanel.MoveChild(
                        eventData.EventArgs.OldStartingIndex, eventData.EventArgs.NewStartingIndex);
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
            this.ItemsPanel.ClearChildren();

            if (this.ItemsSource != null)
            {
                foreach (var item in this.ItemsSource)
                {
                    this.ItemsPanel.AddChild(item, this.ItemTemplate);
                }
            }
        }
    }
}