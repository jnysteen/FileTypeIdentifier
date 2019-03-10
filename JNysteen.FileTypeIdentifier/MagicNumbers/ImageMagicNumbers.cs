namespace JNysteen.FileTypeIdentifier.MagicNumbers
{
    public static class ImageMagicNumbers
    {
        public const string BMP = "BMP";

        public const string GIF = "GIF";

        public const string JPEG = "JPEG";

        public const string PNG = "PNG";

        public static byte?[][] BMPMagicNumbers => new[]
        {
            new byte?[] {0x42, 0x4D}
        };

        public static byte?[][] GIFMagicNumbers => new[]
        {
            new byte?[] {0x47, 0x49, 0x46, 0x38, 0x37, 0x61},
            new byte?[] {0x47, 0x49, 0x46, 0x38, 0x39, 0x61}
        };

        public static byte?[][] JPEGMagicNumbers => new[]
        {
            new byte?[] {0xFF, 0xD8, 0xFF, 0xDB},
            new byte?[] {0xFF, 0xD8, 0xFF, 0xE0, 0x00, 0x10, 0x4A, 0x46, 0x49, 0x46, 0x00, 0x01},
            new byte?[] {0xFF, 0xD8, 0xFF, 0xEE, 0xFF, 0xD8, 0xFF, 0xE1, null, null, 0x45, 0x78, 0x69, 0x66, 0x00, 0x00}
        };

        public static byte?[][] PNGMagicNumbers => new[]
        {
            new byte?[] {0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A}
        };
    }
}