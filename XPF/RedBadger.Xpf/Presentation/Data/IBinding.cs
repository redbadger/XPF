namespace RedBadger.Xpf.Presentation.Data
{
    internal interface IBinding
    {
        BindingResolutionMode ResolutionMode { get; }

        void Resolve(object dataContext);
    }
}