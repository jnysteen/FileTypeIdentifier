using System;
using System.Collections.Generic;
using System.Linq;

namespace JNysteen.FileTypeIdentifier
{
    internal class FileMagicNumberDefinition
    {
        public string FileType { get; }
        public List<byte?[]> MagicNumbers { get; }
        public int LongestMagicNumber => MagicNumbers.OrderByDescending(m => m.Length).First().Length;
        
        public FileMagicNumberDefinition(IEnumerable<byte?[]> magicNumbers, string fileType)
        {
            if (magicNumbers == null)
                throw new ArgumentNullException(nameof(magicNumbers));
            
            if (fileType == null)
                throw new ArgumentNullException(nameof(fileType));

            if (string.IsNullOrWhiteSpace(fileType))
                throw new ArgumentException("File type did not contain any characters!", nameof(fileType));

            var magicNumbersEnumerated = magicNumbers.ToList();
            
            foreach (var magicNumber in magicNumbersEnumerated)
            {
                if (magicNumber == null)
                    throw new ArgumentNullException(nameof(magicNumber));

                if (magicNumber.Length == 0)
                    throw new ArgumentException("Magic number did not contain any bytes!", nameof(magicNumber));
            }

            MagicNumbers = magicNumbersEnumerated;
            FileType = fileType;
        }
        
        public FileMagicNumberDefinition(byte?[] magicNumber, string fileType) : this(new List<byte?[]>() {magicNumber}, fileType)
        { }
    }
}