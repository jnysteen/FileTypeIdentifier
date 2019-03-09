using System.Collections.Generic;

namespace JNysteen.FileTypeIdentifier.Interfaces
{
    /// <summary>
    ///     Represents the mapping between some file signatures and their file types.
    /// </summary>
    public interface IFileSignatureMapping
    {
        /// <summary>
        ///     Retrieves the file signatures configured in this mapping.
        /// </summary>
        IEnumerable<(byte?[] fileSignature, string fileType)> FileSignatures { get; }

        /// <summary>
        ///     Adds a file signature for the given file type to the mapping table. `null`-values in the signature byte array are
        ///     wildcards that allow for
        ///     any byte values at the given indices.
        /// </summary>
        void AddFileSignature(byte?[] signature, string fileType);

        /// <summary>
        ///     Adds file signatures for the given file type to the mapping table. `null`-values in the signature byte array are
        ///     wildcards that allow for
        ///     any byte values at the given indices.
        /// </summary>
        void AddFileSignatures(IEnumerable<byte?[]> signatures, string fileType);

        /// <summary>
        ///     Retrieves the length of the longest signature found in the mapping table.
        /// </summary>
        int GetLongestSignature();
    }
}