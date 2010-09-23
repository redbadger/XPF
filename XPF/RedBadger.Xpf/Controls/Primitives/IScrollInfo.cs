namespace RedBadger.Xpf.Controls.Primitives
{
    public interface IScrollInfo
    {
        bool CanHorizontallyScroll { get; set; }

        bool CanVerticallyScroll { get; set; }

        Size Extent { get; }

        Vector Offset { get; }

        Size Viewport { get; }

        void SetHorizontalOffset(double offset);

        void SetVerticalOffset(double offset);
    }
}