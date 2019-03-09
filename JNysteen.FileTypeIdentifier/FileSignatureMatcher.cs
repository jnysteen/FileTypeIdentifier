using System;
using JNysteen.FileTypeIdentifier.Interfaces;

namespace JNysteen.FileTypeIdentifier
{
    /// <inheritdoc />
    internal class FileSignatureMatcher : IFileSignatureMatcher
    {
        private readonly IFileSignatureMapping _fileSignatureMapping;

        /// <summary>
        ///     Creates a new FileSignatureMatcher, that are able to recognize the file signatures found in the provided mapping.
        /// </summary>
        public FileSignatureMatcher(IFileSignatureMapping fileSignatureMapping)
        {
            _fileSignatureMapping = fileSignatureMapping;
        }

        /// <inheritdoc />
        public int GetLongestSignature()
        {
            return _fileSignatureMapping.GetLongestSignature();
        }

        /// <inheritdoc />
        public string MatchFileType(byte[] fileContentsContainingHeader)
        {
            if (fileContentsContainingHeader == null || fileContentsContainingHeader.Length == 0)
                return null;

            foreach (var fileSignatureAndType in _fileSignatureMapping.FileSignatures)
            {
                var (signature, fileType) = fileSignatureAndType;

                // If the input is shorter than the signature header, it is impossible to match the input against the signature - continue
                if (fileContentsContainingHeader.Length < signature.Length)
                    continue;

                var shortestSequence = Math.Min(fileContentsContainingHeader.Length, signature.Length);

                var failed = false;

                for (var i = 0; i < shortestSequence; i++)
                {
                    var signatureByte = signature[i];

                    // Null bytes in the signatures are wildcards and should be ignored
                    if (signatureByte == null)
                        continue;

                    if (signatureByte != fileContentsContainingHeader[i])
                    {
                        failed = true;
                        break;
                    }
                }

                if (!failed)
                    return fileType;
            }

            return null;
        }
    }
}