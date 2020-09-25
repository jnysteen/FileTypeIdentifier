namespace JNysteen.FileTypeIdentifier.MagicNumbers
{
    /// <summary>
    ///     Magic numbers for a selection of image file types
    /// </summary>
    public static class ImageMagicNumbers
    {
        /// <summary>
        ///     A bitmap image file (https://en.wikipedia.org/wiki/BMP_file_format)
        /// </summary>
        public const string BMP = "BMP";

        /// <summary>
        ///     A GIF file (https://en.wikipedia.org/wiki/GIF)
        /// </summary>
        public const string GIF = "GIF";

        /// <summary>
        ///     A JPEG (JPG) file (https://en.wikipedia.org/wiki/JPEG)
        /// </summary>
        public const string JPEG = "JPEG";

        /// <summary>
        ///     A PNG file (https://en.wikipedia.org/wiki/Portable_Network_Graphics)
        /// </summary>
        public const string PNG = "PNG";

        /// <summary>
        ///     Magic numbers for the <see cref="BMP"/> file type
        /// </summary>
        public static byte?[][] BMPMagicNumbers => new[]
        {
            new byte?[] {0x42, 0x4D}
        };

        /// <summary>
        ///     Magic numbers for the <see cref="GIF"/> file type
        /// </summary>
        public static byte?[][] GIFMagicNumbers => new[]
        {
            new byte?[] {0x47, 0x49, 0x46, 0x38, 0x37, 0x61},
            new byte?[] {0x47, 0x49, 0x46, 0x38, 0x39, 0x61}
        };

        /// <summary>
        ///     Magic numbers for the <see cref="JPEG"/> file type
        /// </summary>
        public static byte?[][] JPEGMagicNumbers => new[]
        {
            new byte?[] {0xFF, 0xD8, 0xFF, 0xDB},
            new byte?[] {0xFF, 0xD8, 0xFF, 0xE0, 0x00, 0x10, 0x4A, 0x46, 0x49, 0x46, 0x00, 0x01},
            new byte?[] {0xFF, 0xD8, 0xFF, 0xEE, 0xFF, 0xD8, 0xFF, 0xE1, null, null, 0x45, 0x78, 0x69, 0x66, 0x00, 0x00}
        };

        /// <summary>
        ///     Magic numbers for the <see cref="PNG"/> file type
        /// </summary>
        public static byte?[][] PNGMagicNumbers => new[]
        {
            new byte?[] {0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A}
        };
    }
}