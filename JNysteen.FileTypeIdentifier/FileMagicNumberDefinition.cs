using System;
using System.Collections.Generic;
using System.Linq;

namespace JNysteen.FileTypeIdentifier
{
    /// <summary>
    ///     A definition of a file type and it's magic numbers
    /// </summary>
    public class FileMagicNumberDefinition
    {
        /// <summary>
        ///     The primary extension for the file type
        /// </summary>
        public string PrimaryFileExtension { get; }
        
        /// <summary>
        ///     Other known extensions for the file (if needed)
        /// </summary>
        public List<string> OtherFileExtensions { get; set; }
        
        /// <summary>
        ///     MIME types for the file (if needed)
        /// </summary>
        public List<string> MimeTypes { get; set; }
        
        /// <summary>
        ///     Known magic numbers for the file type
        /// </summary>
        public List<byte?[]> MagicNumbers { get; }
        
        /// <summary>
        ///     The lenght of the longest configured magic number for the file type
        /// </summary>
        public int LongestMagicNumber => MagicNumbers.OrderByDescending(m => m.Length).First().Length;
        
        /// <summary>
        ///     Instantiates a new magic number definition for a file type
        /// </summary>
        /// <param name="primaryFileExtension">The primary extension for the file type</param>
        /// <param name="magicNumbers">Magic numbers identifying the file type. Any wildcard bytes can be represented as `null`</param>
        /// <exception cref="ArgumentNullException">Thrown if one or more arguments are null</exception>
        /// <exception cref="ArgumentException">Thrown if one or more arguments are invalid</exception>
        public FileMagicNumberDefinition(string primaryFileExtension, IEnumerable<byte?[]> magicNumbers)
        {
            if (magicNumbers == null)
                throw new ArgumentNullException(nameof(magicNumbers));
            
            if (primaryFileExtension == null)
                throw new ArgumentNullException(nameof(primaryFileExtension));

            if (string.IsNullOrWhiteSpace(primaryFileExtension))
                throw new ArgumentException("File type did not contain any characters!", nameof(primaryFileExtension));

            var magicNumbersEnumerated = magicNumbers.ToList();
            
            foreach (var magicNumber in magicNumbersEnumerated)
            {
                if (magicNumber == null)
                    throw new ArgumentNullException(nameof(magicNumber));

                if (magicNumber.Length == 0)
                    throw new ArgumentException("Magic number did not contain any bytes!", nameof(magicNumber));
            }

            MagicNumbers = magicNumbersEnumerated;
            PrimaryFileExtension = primaryFileExtension;
        }
        
        /// <summary>
        ///     Instantiates a new magic number definition for a file type
        /// </summary>
        /// <param name="primaryFileExtension">The primary extension for the file type</param>
        /// <param name="magicNumber">A magic number identifying the file type. Any wildcard bytes can be represented as `null`</param>
        /// <exception cref="ArgumentNullException">Thrown if one or more arguments are null</exception>
        /// <exception cref="ArgumentException">Thrown if one or more arguments are invalid</exception>
        public FileMagicNumberDefinition(string primaryFileExtension, byte?[] magicNumber) : this(primaryFileExtension, new List<byte?[]> {magicNumber})
        { }
    }
}