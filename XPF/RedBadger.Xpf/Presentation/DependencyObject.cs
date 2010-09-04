namespace RedBadger.Xpf.Presentation
{
    using System;

    public class DependencyObject : IDependencyObject
    {
        public void ClearValue(DependencyProperty dependencyProperty)
        {
            throw new NotImplementedException();
        }

        public void ClearBinding(DependencyProperty dependencyProperty)
        {
            throw new NotImplementedException();
        }

        public object GetValue(DependencyProperty dependencyProperty)
        {
            throw new NotImplementedException();
        }

        public BindingExpression SetBinding(DependencyProperty dependencyProperty, Binding binding)
        {
            throw new NotImplementedException();
        }

        public void SetValue(DependencyProperty dependencyProperty, object value)
        {
            throw new NotImplementedException();
        }
    }
}