namespace RedBadger.Xpf.Presentation.Media.Imaging
{
    using System;
    using System.IO;
    using System.Windows;

    public class BitmapImage : BitmapSource
    {
        public static readonly DependencyProperty StreamSourceProperty = DependencyProperty.Register(
            "StreamSource", typeof(Stream), typeof(BitmapImage), new PropertyMetadata(null));

        public Stream StreamSource
        {
            get
            {
                return (Stream)this.GetValue(StreamSourceProperty);
            }

            set
            {
                this.SetValue(StreamSourceProperty, value);
            }
        }

        public void BeginInit()
        {
            
        }

        public void EndInit()
        {
            
        }
    }
}