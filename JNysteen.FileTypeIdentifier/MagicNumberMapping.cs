using System;
using System.Collections.Generic;
using System.Linq;
using JNysteen.FileTypeIdentifier.Exceptions;
using JNysteen.FileTypeIdentifier.Interfaces;

namespace JNysteen.FileTypeIdentifier
{
    /// <inheritdoc />
    public class MagicNumberMapping : IFileMagicNumberMapping
    {
        internal readonly List<(byte?[], string)> FileMagicNumberMappingTable;

        /// <summary>
        ///     Creates a new, empty FileMagicNumberMapping
        /// </summary>
        public MagicNumberMapping()
        {
            FileMagicNumberMappingTable = new List<(byte?[], string)>();
        }

        /// <inheritdoc />
        IEnumerable<(byte?[] magicNumber, string fileType)> IFileMagicNumberMapping.FileMagicNumbers =>
            FileMagicNumberMappingTable;

        /// <inheritdoc />
        public void AddMagicNumber(byte?[] magicNumber, string fileType)
        {
            if (magicNumber == null)
                throw new ArgumentNullException(nameof(magicNumber));

            if (magicNumber.Length == 0)
                throw new ArgumentException("Magic number did not contain any bytes!", nameof(magicNumber));

            if (fileType == null)
                throw new ArgumentNullException(nameof(fileType));

            if (string.IsNullOrWhiteSpace(fileType))
                throw new ArgumentException("File type did not contain any characters!", nameof(fileType));

            FileMagicNumberMappingTable.Add((magicNumber, fileType));
        }

        /// <inheritdoc />
        public void AddMagicNumbers(IEnumerable<byte?[]> magicNumbers, string fileType)
        {
            foreach (var magicNumber in magicNumbers)
                AddMagicNumber(magicNumber, fileType);
        }

        /// <inheritdoc />
        public int GetLongestMagicNumber()
        {
            if (!FileMagicNumberMappingTable.Any())
                throw new InvalidConfigurationException("The mapping does not contain any magic numbers!");

            return FileMagicNumberMappingTable.Max(t => t.Item1.Length);
        }
    }
}