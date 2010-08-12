namespace RedBadger.Xpf.Presentation
{
    using System;
    using System.Windows;
    using System.Windows.Data;

    public class BindingExpression
    {
        private readonly XpfDependencyProperty dependencyProperty;

        private readonly FrameworkElement frameworkElement;

        private PropertyChangedNotifier propertyChangedNotifier;

        private readonly DependencyObject uiElement;

        public BindingExpression(DependencyObject uiElement, XpfDependencyProperty dependencyProperty)
        {
            if (uiElement == null)
            {
                throw new ArgumentNullException("uiElement");
            }

            if (dependencyProperty == null)
            {
                throw new ArgumentNullException("dependencyProperty");
            }

            this.uiElement = uiElement;
            this.dependencyProperty = dependencyProperty;

            this.frameworkElement = new BindingFrameworkElement(uiElement, dependencyProperty.Value);
        }

        public void ClearBinding()
        {
            this.frameworkElement.ClearValue(BindingFrameworkElement.DependencyProperty);

            if (this.propertyChangedNotifier != null)
            {
                this.propertyChangedNotifier.ValueChanged -= this.PropertyChangedNotifierOnValueChanged;
                this.propertyChangedNotifier = null;
            }
        }

        public void SetBinding(Binding binding)
        {
            this.frameworkElement.SetBinding(BindingFrameworkElement.DependencyProperty, binding);

            if (this.propertyChangedNotifier != null)
            {
                this.propertyChangedNotifier.ValueChanged -= this.PropertyChangedNotifierOnValueChanged;
                this.propertyChangedNotifier = null;
            }

            if (binding.Mode == BindingMode.TwoWay)
            {
                this.propertyChangedNotifier = new PropertyChangedNotifier(this.uiElement, this.dependencyProperty);
                this.propertyChangedNotifier.ValueChanged += this.PropertyChangedNotifierOnValueChanged;
            }
        }

        private void PropertyChangedNotifierOnValueChanged(
            object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            this.frameworkElement.SetValue(
                BindingFrameworkElement.DependencyProperty, propertyChangedEventArgs.NewValue);
        }

        public class BindingFrameworkElement : FrameworkElement
        {
            public static readonly DependencyProperty DependencyProperty;

            private static readonly Guid defaultValue = Guid.NewGuid();

            private readonly DependencyProperty dependencyProperty;

            private readonly DependencyObject uiElement;

            static BindingFrameworkElement()
            {
                DependencyProperty = DependencyProperty.Register(
                    "Dependency", 
                    typeof(object), 
                    typeof(BindingFrameworkElement), 
                    new PropertyMetadata(defaultValue, DependencyPropertyChangedCallback));
            }

            public BindingFrameworkElement(DependencyObject uiElement, DependencyProperty dependencyProperty)
            {
                this.uiElement = uiElement;
                this.dependencyProperty = dependencyProperty;
            }

            private static void DependencyPropertyChangedCallback(
                DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
            {
                var bindingFrameworkElement = (BindingFrameworkElement)dependencyObject;
                if (args.NewValue is Guid && (Guid)args.NewValue == defaultValue)
                {
                    bindingFrameworkElement.uiElement.ClearValue(bindingFrameworkElement.dependencyProperty);
                }
                else
                {
                    bindingFrameworkElement.uiElement.SetValue(
                        bindingFrameworkElement.dependencyProperty, args.NewValue);
                }
            }
        }
    }
}