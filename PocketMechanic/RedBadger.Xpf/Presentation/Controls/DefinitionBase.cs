namespace RedBadger.Xpf.Presentation.Controls
{
    using System;

    public abstract class DefinitionBase
    {
        internal float AvailableSize { get; set; }

        internal float MinSize { get; private set; }

        internal void UpdateMinSize(float minSize)
        {
            this.MinSize = Math.Max(this.MinSize, minSize);
        }
    }
}