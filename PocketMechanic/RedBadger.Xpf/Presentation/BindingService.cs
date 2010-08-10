namespace RedBadger.Xpf.Presentation
{
    using System;
    using System.Windows;
    using System.Windows.Data;

#if WINDOWS_PHONE
    using Microsoft.Phone.Reactive;
#endif

    using RedBadger.Xpf.Internal;

    public class BindingService
    {
        private readonly DependencyProperty dependencyProperty;

        private readonly FrameworkElement frameworkElement;

        private readonly UIElement uiElement;

        private IDisposable subscription;

        public BindingService(UIElement uiElement, DependencyProperty dependencyProperty)
        {
            this.uiElement = uiElement;
            this.dependencyProperty = dependencyProperty;
            if (uiElement == null)
            {
                throw new ArgumentNullException("uiElement");
            }

            if (dependencyProperty == null)
            {
                throw new ArgumentNullException("dependencyProperty");
            }

            this.frameworkElement = new BindingFrameworkElement(uiElement, dependencyProperty);
        }

        public void Is(Binding binding)
        {
            this.frameworkElement.SetBinding(BindingFrameworkElement.DependencyProperty, binding);
            if (binding.Mode == BindingMode.TwoWay)
            {
                this.subscription =
                    this.uiElement.FromDependencyProperty<object>(this.dependencyProperty).Subscribe(
                        Observer.Create<object>(
                            o => this.frameworkElement.SetValue(BindingFrameworkElement.DependencyProperty, o)));
            }
        }

        public class BindingFrameworkElement : FrameworkElement
        {
            public static readonly DependencyProperty DependencyProperty = DependencyProperty.Register(
                "Dependency", 
                typeof(object), 
                typeof(BindingFrameworkElement), 
                new PropertyMetadata(null, DependencyPropertyChangedCallback));

            private readonly DependencyProperty dependencyProperty;

            private readonly UIElement uiElement;

            public BindingFrameworkElement(UIElement uiElement, DependencyProperty dependencyProperty)
            {
                this.uiElement = uiElement;
                this.dependencyProperty = dependencyProperty;
            }

            private static void DependencyPropertyChangedCallback(
                DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
            {
                var bindingService = (BindingFrameworkElement)dependencyObject;
                bindingService.uiElement.SetValue(bindingService.dependencyProperty, args.NewValue);
            }
        }
    }
}