namespace RedBadger.Xpf.Internal.Controls
{
    using System.Windows;
    using System.Windows.Media;

    using RedBadger.Xpf.Presentation.Controls;

    using Vector = RedBadger.Xpf.Presentation.Vector;

    internal class Viewbox
    {
        internal static Vector ComputeScaleFactor(
            Size availableSize, Size contentSize, Stretch stretch, StretchDirection stretchDirection)
        {
            double scaleX = 1.0;
            double scaleY = 1.0;
            bool isWidthContrained = !double.IsPositiveInfinity(availableSize.Width);
            bool isHeightConstrained = !double.IsPositiveInfinity(availableSize.Height);
            if (stretch == Stretch.None || (!isWidthContrained && !isHeightConstrained))
            {
                return new Vector(scaleX, scaleY);
            }

            scaleX = contentSize.Width.IsCloseTo(0) ? 0 : (availableSize.Width / contentSize.Width);
            scaleY = contentSize.Height.IsCloseTo(0) ? 0 : (availableSize.Height / contentSize.Height);
            if (!isWidthContrained)
            {
                scaleX = scaleY;
            }
            else if (!isHeightConstrained)
            {
                scaleY = scaleX;
            }
            else
            {
                switch (stretch)
                {
                    case Stretch.Uniform:
                        scaleX = scaleY = (scaleX < scaleY) ? scaleX : scaleY;
                        break;

                    case Stretch.UniformToFill:
                        scaleX = scaleY = (scaleX > scaleY) ? scaleX : scaleY;
                        break;
                }
            }

            switch (stretchDirection)
            {
                case StretchDirection.UpOnly:
                    if (scaleX < 1.0)
                    {
                        scaleX = 1.0;
                    }

                    if (scaleY < 1.0)
                    {
                        scaleY = 1.0;
                    }

                    break;

                case StretchDirection.DownOnly:
                    if (scaleX > 1.0)
                    {
                        scaleX = 1.0;
                    }

                    if (scaleY > 1.0)
                    {
                        scaleY = 1.0;
                    }

                    break;
            }

            return new Vector(scaleX, scaleY);
        }
    }
}