namespace JNysteen.FileTypeIdentifier.MagicNumbers
{
    /// <summary>
    ///     Magic number definitions for a selection of document file types
    /// </summary>
    public static class DocumentMagicNumbers
    {
        /// <summary>
        ///     A PDF file (https://en.wikipedia.org/wiki/PDF)
        /// </summary>
        public static FileMagicNumberDefinition PDF => new FileMagicNumberDefinition(".pdf", new[]
        {
            new byte?[] {0x25, 0x50, 0x44, 0x46, 0x2d}
        });
    }
}