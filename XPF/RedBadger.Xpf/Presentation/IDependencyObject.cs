namespace RedBadger.Xpf.Presentation
{
    public interface IDependencyObject
    {
        void ClearBinding(DependencyProperty dependencyProperty);

        object GetValue(DependencyProperty dependencyProperty);

        BindingExpression SetBinding(DependencyProperty dependencyProperty, Binding binding);

        void SetValue(DependencyProperty dependencyProperty, object value);
    }
}