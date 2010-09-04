namespace RedBadger.Xpf.Presentation
{
    public interface IDependencyObject
    {
        void ClearBinding(IDependencyProperty dependencyProperty);

        T GetValue<T>(IDependencyProperty dependencyProperty);

        BindingExpression SetBinding(IDependencyProperty dependencyProperty, Binding binding);

        void SetValue<T>(IDependencyProperty dependencyProperty, T value);
    }
}