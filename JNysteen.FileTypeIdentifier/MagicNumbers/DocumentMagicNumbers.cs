namespace JNysteen.FileTypeIdentifier.MagicNumbers
{
    /// <summary>
    ///     Magic numbers for a selection of document file types
    /// </summary>
    public static class DocumentMagicNumbers
    {
        /// <summary>
        ///     A PDF file (https://en.wikipedia.org/wiki/PDF)
        /// </summary>
        public const string PDF = "PDF";

        /// <summary>
        ///     Magic numbers for the <see cref="PDF"/> file type
        /// </summary>
        public static byte?[][] PDFMagicNumbers => new[]
        {
            new byte?[] {0x25, 0x50, 0x44, 0x46, 0x2d}
        };
    }
}