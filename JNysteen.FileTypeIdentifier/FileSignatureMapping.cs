using System;
using System.Collections.Generic;
using System.Linq;
using JNysteen.FileTypeIdentifier.Exceptions;
using JNysteen.FileTypeIdentifier.Interfaces;

namespace JNysteen.FileTypeIdentifier
{
    /// <inheritdoc />
    public class FileSignatureMapping : IFileSignatureMapping
    {
        internal readonly List<(byte?[], string)> FileSignaturesMappingTable;

        /// <summary>
        ///     Creates a new, empty FileSignatureMapping
        /// </summary>
        public FileSignatureMapping()
        {
            FileSignaturesMappingTable = new List<(byte?[], string)>();
        }

        /// <inheritdoc />
        IEnumerable<(byte?[] fileSignature, string fileType)> IFileSignatureMapping.FileSignatures =>
            FileSignaturesMappingTable;

        /// <inheritdoc />
        public void AddFileSignature(byte?[] signature, string fileType)
        {
            if (signature == null)
                throw new ArgumentNullException(nameof(signature));

            if (signature.Length == 0)
                throw new ArgumentException("Signature did not contain any bytes!", nameof(signature));

            if (fileType == null)
                throw new ArgumentNullException(nameof(fileType));

            if (string.IsNullOrWhiteSpace(fileType))
                throw new ArgumentException("File type did not contain any characters!", nameof(fileType));

            FileSignaturesMappingTable.Add((signature, fileType));
        }

        /// <inheritdoc />
        public void AddFileSignatures(IEnumerable<byte?[]> signatures, string fileType)
        {
            foreach (var signature in signatures)
                AddFileSignature(signature, fileType);
        }

        /// <inheritdoc />
        public int GetLongestSignature()
        {
            if (!FileSignaturesMappingTable.Any())
                throw new InvalidConfigurationException("The mapping does not contain any file signatures!");

            return FileSignaturesMappingTable.Max(t => t.Item1.Length);
        }
    }
}