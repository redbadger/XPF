namespace RedBadger.Xpf.Presentation
{
    using System;
    using System.Collections.Generic;

    public class DependencyObject : IDependencyObject
    {
        private readonly Dictionary<DependencyProperty, object> propertyValues =
            new Dictionary<DependencyProperty, object>();

        public void ClearValue(DependencyProperty dependencyProperty)
        {
            if (dependencyProperty == null)
            {
                throw new ArgumentNullException("dependencyProperty");
            }

            this.propertyValues.Remove(dependencyProperty);
        }

        public void ClearBinding(DependencyProperty dependencyProperty)
        {
            throw new NotImplementedException();
        }

        public object GetValue(DependencyProperty dependencyProperty)
        {
            if (dependencyProperty == null)
            {
                throw new ArgumentNullException("dependencyProperty");
            }

            object value;
            if (this.propertyValues.TryGetValue(dependencyProperty, out value))
            {
                return value;
            }

            return dependencyProperty.DefaultValue;
        }

        public BindingExpression SetBinding(DependencyProperty dependencyProperty, Binding binding)
        {
            throw new NotImplementedException();
        }

        public void SetValue(DependencyProperty dependencyProperty, object value)
        {
            if (dependencyProperty == null)
            {
                throw new ArgumentNullException("dependencyProperty");
            }

            if (value == DependencyProperty.UnsetValue)
            {
                this.ClearValue(dependencyProperty);
            }
            else
            {
                object oldValue = this.GetValue(dependencyProperty);
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

        private static bool ArePropertyValuesEqual(DependencyProperty dependencyProperty, object value1, object value2)
        {
            if (!dependencyProperty.PropertyType.IsValueType && dependencyProperty.PropertyType != typeof(string))
            {
                return ReferenceEquals(value1, value2);
            }

            return Equals(value1, value2);
        }
    }
}