namespace RedBadger.Xpf.Presentation.Controls
{
    using Microsoft.Xna.Framework;

    using RedBadger.Xpf.Internal;
    using RedBadger.Xpf.Presentation.Media;

    public class Viewbox
    {
        internal static Vector2 ComputeScaleFactor(
            Size availableSize, Size contentSize, Stretch stretch, StretchDirection stretchDirection)
        {
            float scaleX = 1.0f;
            float scaleY = 1.0f;
            bool isWidthContrained = !float.IsPositiveInfinity(availableSize.Width);
            bool isHeightConstrained = !float.IsPositiveInfinity(availableSize.Height);
            if (stretch == Stretch.None || (!isWidthContrained && !isHeightConstrained))
            {
                return new Vector2(scaleX, scaleY);
            }

            scaleX = contentSize.Width.IsCloseTo(0f) ? 0f : (availableSize.Width / contentSize.Width);
            scaleY = contentSize.Height.IsCloseTo(0f) ? 0f : (availableSize.Height / contentSize.Height);
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
                    if (scaleX < 1.0f)
                    {
                        scaleX = 1.0f;
                    }

                    if (scaleY < 1.0f)
                    {
                        scaleY = 1.0f;
                    }

                    break;

                case StretchDirection.DownOnly:
                    if (scaleX > 1.0f)
                    {
                        scaleX = 1.0f;
                    }

                    if (scaleY > 1.0f)
                    {
                        scaleY = 1.0f;
                    }

                    break;
            }

            return new Vector2(scaleX, scaleY);
        }
    }
}