namespace XpfSamples.S02ApplicationBar
{
    using RedBadger.Xpf.Presentation.Media.Imaging;

    public class ApplicationBarIconButton
    {
        private readonly XnaImage iconImageSource;

        public ApplicationBarIconButton(XnaImage iconImageSource)
        {
            this.iconImageSource = iconImageSource;
        }

        public XnaImage IconImageSource
        {
            get
            {
                return this.iconImageSource;
            }
        }
    }
}