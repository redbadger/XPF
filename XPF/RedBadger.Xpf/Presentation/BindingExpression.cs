/*namespace RedBadger.Xpf.Presentation
{
    using System;
    using System.Globalization;

    using RedBadger.Xpf.Presentation.Data;

    public class BindingExpression
    {
        private readonly IProperty dependencyProperty;

        private readonly FrameworkElement frameworkElement;

        private readonly DependencyObject uiElement;

        private PropertyChangedNotifier propertyChangedNotifier;

        public BindingExpression(DependencyObject uiElement, IProperty dependencyProperty)
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
            this.frameworkElement.ClearValue(BindingFrameworkElement.Property);

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
            this.frameworkElement.DataContext = this.GetDefaultDataContext();
            this.frameworkElement.SetBinding(BindingFrameworkElement.Property, binding);

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

        public void SetDataContext(object dataContext)
        {
            this.frameworkElement.DataContext = dataContext ?? this.GetDefaultDataContext();
        }

        private object GetDefaultDataContext()
        {
            return this.dependencyProperty.PropertyType.IsClass ? (object)null : 0;
        }

        private void PropertyChangedNotifierOnValueChanged(
            object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            BindingFrameworkElement.SetValue(
                this.frameworkElement, BindingFrameworkElement.Property, propertyChangedEventArgs.NewValue);
        }

        public class BindingFrameworkElement : FrameworkElement
        {
            public static readonly IProperty Property;

            private static readonly Guid defaultValue = Guid.NewGuid();

            private readonly DependencyObject targetDependencyObject;

            private readonly IProperty targetDependencyProperty;

            static BindingFrameworkElement()
            {
                Property = Property<object, BindingFrameworkElement>.Register(
                    "Dependency", 
                    new PropertyMetadata(defaultValue, DependencyPropertyChangedCallback));
            }

            public BindingFrameworkElement(
                DependencyObject targetDependencyObject, IProperty targetDependencyProperty)
            {
                this.targetDependencyObject = targetDependencyObject;
                this.targetDependencyProperty = targetDependencyProperty;
            }

            public static void SetValue(DependencyObject dependencyObject, IProperty property, object value)
            {
                if (value != null && !property.PropertyType.IsAssignableFrom(value.GetType()))
                {
                    if (!(value is IConvertible))
                    {
                        value = value.ToString();
                    }

                    if (property.PropertyType != null)
                    {
                        value = Convert.ChangeType(value, property.PropertyType, CultureInfo.InvariantCulture);
                    }
                }

                dependencyObject.SetValue(property, value);
            }

            private static void DependencyPropertyChangedCallback(
                DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
            {
                var bindingFrameworkElement = (BindingFrameworkElement)dependencyObject;
                if (args.NewValue is Guid && (Guid)args.NewValue == defaultValue)
                {
                    bindingFrameworkElement.targetDependencyObject.ClearValue(
                        bindingFrameworkElement.targetDependencyProperty);
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

    public class FrameworkElement : UIElement
    {
    }
}*/