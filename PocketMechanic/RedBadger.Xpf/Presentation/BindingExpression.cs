namespace RedBadger.Xpf.Presentation
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    public class BindingExpression
    {
        private readonly XpfDependencyProperty dependencyProperty;

        private readonly FrameworkElement frameworkElement;

        private readonly DependencyObject uiElement;

        private PropertyChangedNotifier propertyChangedNotifier;

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

            this.frameworkElement = new BindingFrameworkElement(uiElement, dependencyProperty);
        }

        public void ClearBinding()
        {
            this.frameworkElement.ClearValue(BindingFrameworkElement.DependencyProperty.Value);

            if (this.propertyChangedNotifier != null)
            {
                this.propertyChangedNotifier.ValueChanged -= this.PropertyChangedNotifierOnValueChanged;
                this.propertyChangedNotifier = null;
            }
        }

        public void SetBinding(Binding binding)
        {
#if WINDOWS_PHONE
            if (binding.Mode != BindingMode.OneWay && binding.Mode != BindingMode.TwoWay)
            {
                throw new NotSupportedException("XPF only supports OneWay and TwoWay binding.");
            }
#else
            if (binding.Mode != BindingMode.Default && binding.Mode != BindingMode.OneWay &&
                binding.Mode != BindingMode.TwoWay)
            {
                throw new NotSupportedException("XPF only supports OneWay and TwoWay binding.");
            }
#endif

            this.frameworkElement.SetBinding(BindingFrameworkElement.DependencyProperty.Value, binding);

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
            BindingFrameworkElement.SetValue(
                this.frameworkElement, BindingFrameworkElement.DependencyProperty, propertyChangedEventArgs.NewValue);
        }

        public class BindingFrameworkElement : FrameworkElement
        {
            public static readonly XpfDependencyProperty DependencyProperty;

            private static readonly Guid defaultValue = Guid.NewGuid();

            private readonly DependencyObject targetDependencyObject;

            private readonly XpfDependencyProperty targetDependencyProperty;

            static BindingFrameworkElement()
            {
                DependencyProperty = XpfDependencyProperty.Register(
                    "Dependency", 
                    typeof(object), 
                    typeof(BindingFrameworkElement), 
                    new PropertyMetadata(defaultValue, DependencyPropertyChangedCallback));
            }

            public BindingFrameworkElement(
                DependencyObject targetDependencyObject, XpfDependencyProperty targetDependencyProperty)
            {
                this.targetDependencyObject = targetDependencyObject;
                this.targetDependencyProperty = targetDependencyProperty;
            }

            public static void SetValue(DependencyObject dependencyObject, XpfDependencyProperty property, object value)
            {
                if (value != null && property.PropertyType != null && !property.PropertyType.IsAssignableFrom(value.GetType()))
                {
                    value = Convert.ChangeType(value, property.PropertyType, CultureInfo.InvariantCulture);
                }

                dependencyObject.SetValue(property.Value, value);
            }

            private static void DependencyPropertyChangedCallback(
                DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
            {
                var bindingFrameworkElement = (BindingFrameworkElement)dependencyObject;
                if (args.NewValue is Guid && (Guid)args.NewValue == defaultValue)
                {
                    bindingFrameworkElement.targetDependencyObject.ClearValue(
                        bindingFrameworkElement.targetDependencyProperty.Value);
                }
                else
                {
                    SetValue(
                        bindingFrameworkElement.targetDependencyObject, 
                        bindingFrameworkElement.targetDependencyProperty, 
                        args.NewValue);
                }
            }
        }
    }
}