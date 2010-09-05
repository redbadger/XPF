namespace RedBadger.Xpf.Presentation
{
    public interface IProperty
    {
        object DefaultValue { get; }

        string Name { get; }
    }
}