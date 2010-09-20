namespace XpfSamples.S02ApplicationBar
{
    using RedBadger.Xpf.Presentation.Media;

    public class ApplicationBarIconButton
    {
        private readonly ImageSource iconImageSource;

        private readonly string text;

        public ApplicationBarIconButton(string text, ImageSource iconImageSource)
        {
            this.text = text;
            this.iconImageSource = iconImageSource;
        }

        public ImageSource IconImageSource
        {
            get
            {
                return this.iconImageSource;
            }
        }

        public string Text
        {
            get
            {
                return this.text;
            }
        }
    }
}