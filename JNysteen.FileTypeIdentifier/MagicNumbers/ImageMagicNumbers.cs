namespace JNysteen.FileTypeIdentifier.MagicNumbers
{
    /// <summary>
    ///     Magic number definitions for a selection of image file types
    /// </summary>
    public static class ImageMagicNumbers
    {
        /// <summary>
        ///     A bitmap image file (https://en.wikipedia.org/wiki/BMP_file_format)
        /// </summary>
        public static FileMagicNumberDefinition BMP => new FileMagicNumberDefinition(".bmp", new[]
        {
            new byte?[] {0x42, 0x4D}
        });
        
        /// <summary>
        ///     A GIF file (https://en.wikipedia.org/wiki/GIF)
        /// </summary>
        public static FileMagicNumberDefinition GIF => new FileMagicNumberDefinition(".gif", new[]
        {
            new byte?[] {0x47, 0x49, 0x46, 0x38, 0x37, 0x61},
            new byte?[] {0x47, 0x49, 0x46, 0x38, 0x39, 0x61}
        });

        /// <summary>
        ///     A JPEG (JPG) file (https://en.wikipedia.org/wiki/JPEG)
        /// </summary>
        public static FileMagicNumberDefinition JPEG => new FileMagicNumberDefinition(".jpg", new[]
        {
            new byte?[] {0xFF, 0xD8, 0xFF, 0xDB},
            new byte?[] {0xFF, 0xD8, 0xFF, 0xE0, 0x00, 0x10, 0x4A, 0x46, 0x49, 0x46, 0x00, 0x01},
            new byte?[] {0xFF, 0xD8, 0xFF, 0xEE, 0xFF, 0xD8, 0xFF, 0xE1, null, null, 0x45, 0x78, 0x69, 0x66, 0x00, 0x00}
        });

        /// <summary>
        ///     A PNG file (https://en.wikipedia.org/wiki/Portable_Network_Graphics)
        /// </summary>
        public static FileMagicNumberDefinition PNG => new FileMagicNumberDefinition(".png", new[]
        {
            new byte?[] {0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A}
        });
    }
}