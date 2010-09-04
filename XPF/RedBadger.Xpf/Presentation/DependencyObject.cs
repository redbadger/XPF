namespace RedBadger.Xpf.Presentation
{
    using System;
    using System.Collections.Generic;

    public class DependencyObject : IDependencyObject
    {
        private readonly Dictionary<IDependencyProperty, object> propertyValues =
            new Dictionary<IDependencyProperty, object>();

        public void ClearValue(IDependencyProperty dependencyProperty)
        {
            if (dependencyProperty == null)
            {
                throw new ArgumentNullException("dependencyProperty");
            }

            this.propertyValues.Remove(dependencyProperty);
        }

        public void ClearBinding(IDependencyProperty dependencyProperty)
        {
            throw new NotImplementedException();
        }

        public T GetValue<T>(IDependencyProperty dependencyProperty)
        {
            if (dependencyProperty == null)
            {
                throw new ArgumentNullException("dependencyProperty");
            }

            if (!typeof(T).IsAssignableFrom(dependencyProperty.PropertyType))
            {
                throw new ArgumentException("Incorrect Type for this DependencyProperty");
            }

            object value;
            if (this.propertyValues.TryGetValue(dependencyProperty, out value))
            {
                return (T)value;
            }

            return (T)dependencyProperty.DefaultValue;
        }

        public BindingExpression SetBinding(IDependencyProperty dependencyProperty, Binding binding)
        {
            throw new NotImplementedException();
        }

        public void SetValue<T>(IDependencyProperty dependencyProperty, T value)
        {
            if (dependencyProperty == null)
            {
                throw new ArgumentNullException("dependencyProperty");
            }

            if (dependencyProperty.PropertyType != typeof(T))
            {
                throw new ArgumentException("value is not of the correct Type for this DependencyProperty");
            }

            if (Equals(value, DependencyProperty<object, object>.UnsetValue))
            {
                this.ClearValue(dependencyProperty);
            }
            else
            {
                var oldValue = this.GetValue<object>(dependencyProperty);
                if (!ArePropertyValuesEqual(dependencyProperty, oldValue, value))
                {
                    this.propertyValues[dependencyProperty] = value;
                    if (dependencyProperty.PropertyChangedCallback != null)
                    {
                        dependencyProperty.PropertyChangedCallback(
                            this, new DependencyPropertyChangedEventArgs(dependencyProperty, oldValue, value));
                    }
                }
            }
        }

        private static bool ArePropertyValuesEqual(IDependencyProperty dependencyProperty, object value1, object value2)
        {
            if (!dependencyProperty.PropertyType.IsValueType && dependencyProperty.PropertyType != typeof(string))
            {
                return ReferenceEquals(value1, value2);
            }

            return Equals(value1, value2);
        }
    }
}