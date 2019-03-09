namespace JNysteen.FileTypeIdentifier.FileSignatures
{
    public static class DocumentSignatures
    {
        public const string PDF = "PDF";
        public static byte?[][] PDFSignatures => new[] {new byte?[] {0x25, 0x50, 0x44, 0x46, 0x2d}};
    }
}