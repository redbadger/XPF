namespace RedBadger.Xpf.Presentation
{
    using System;
    using System.Windows;
    using System.Windows.Data;

    public class BindingService
    {
        private readonly FrameworkElement frameworkElement;

        public BindingService(UIElement uiElement, DependencyProperty dependencyProperty)
        {
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