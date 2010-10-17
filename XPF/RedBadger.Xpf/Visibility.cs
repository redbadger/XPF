namespace RedBadger.Xpf
{
    /// <summary>
    /// Specifies whether an <see cref="IElement">IElement</see> is <see cref="Visible">Visible</see> or <see cref="Collapsed">Collapsed</see>.
    /// </summary>
    public enum Visibility : byte
    {
        /// <summary>
        /// The <see cref="IElement">IElement</see> is not visible and no space is reserved for it in the layout.
        /// </summary>
        Collapsed = 1, 

        /// <summary>
        /// The <see cref="IElement">IElement</see> is visible.
        /// </summary>
        Visible = 0
    }
}