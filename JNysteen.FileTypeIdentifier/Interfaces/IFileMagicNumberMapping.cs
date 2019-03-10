using System.Collections.Generic;

namespace JNysteen.FileTypeIdentifier.Interfaces
{
    /// <summary>
    ///     Represents the mapping between some magic numbers and their file types.
    /// </summary>
    public interface IFileMagicNumberMapping
    {
        /// <summary>
        ///     Retrieves the magic numbers configured in this mapping.
        /// </summary>
        IEnumerable<(byte?[] magicNumber, string fileType)> FileMagicNumbers { get; }

        /// <summary>
        ///     Adds a magic numbers for the given file type to the mapping table. `null`-values in the magic number byte array are
        ///     wildcards that allow for any byte values at the given indices.
        /// </summary>
        void AddMagicNumber(byte?[] magicNumber, string fileType);

        /// <summary>
        ///     Adds magic numbers for the given file type to the mapping table. `null`-values in a magic number byte array are
        ///     wildcards that allow for any byte values at the given indices.
        /// </summary>
        void AddMagicNumbers(IEnumerable<byte?[]> magicNumbers, string fileType);

        /// <summary>
        ///     Retrieves the length of the longest magic number found in the mapping table.
        /// </summary>
        int GetLongestMagicNumber();
    }
}