namespace JNysteen.FileTypeIdentifier.MagicNumbers
{
    public static class DocumentMagicNumbers
    {
        public const string PDF = "PDF";

        public static byte?[][] PDFMagicNumbers => new[]
        {
            new byte?[] {0x25, 0x50, 0x44, 0x46, 0x2d}
        };
    }
}