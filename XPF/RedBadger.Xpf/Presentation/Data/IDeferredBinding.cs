namespace RedBadger.Xpf.Presentation.Data
{
    public interface IDeferredBinding 
    {
        void Resolve(object dataContext);
    }
}