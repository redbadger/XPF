namespace RedBadger.Xpf.Presentation
{
    using System.Windows;
    using System.Windows.Data;

    public interface IDependencyObject
    {
        void ClearBinding(XpfDependencyProperty dependencyProperty);

        object GetValue(DependencyProperty dependencyProperty);

        BindingExpression SetBinding(XpfDependencyProperty dependencyProperty, Binding binding);

        void SetValue(DependencyProperty dependencyProperty, object value);
    }
}