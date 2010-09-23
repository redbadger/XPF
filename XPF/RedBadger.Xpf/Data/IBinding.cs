namespace RedBadger.Xpf.Data
{
    internal interface IBinding
    {
        BindingResolutionMode ResolutionMode { get; }

        void Resolve(object dataContext);
    }
}