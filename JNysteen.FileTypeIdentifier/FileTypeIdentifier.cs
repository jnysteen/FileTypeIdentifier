using System.IO;
using JNysteen.FileTypeIdentifier.Interfaces;

namespace JNysteen.FileTypeIdentifier
{
    /// <inheritdoc />
    public class FileTypeIdentifier : IFileTypeIdentifier
    {
        private readonly IFileSignatureMatcher _fileSignatureMatcher;

        /// <summary>
        ///     Creates a FileTypeIdentifier that will match file signatures using the provided file signature matcher.
        /// </summary>
        public FileTypeIdentifier(IFileSignatureMatcher fileSignatureMatcher)
        {
            _fileSignatureMatcher = fileSignatureMatcher;
        }

        /// <inheritdoc />
        public string GetFileType(Stream fileStream)
        {
            var longestFileSignature = _fileSignatureMatcher.GetLongestSignature();
            var signatureBytes = new byte[longestFileSignature];
            var numberOfReadBytes = fileStream.Read(signatureBytes, 0, signatureBytes.Length);

            return GetFileType(signatureBytes);
        }

        /// <inheritdoc />
        public string GetFileType(byte[] fileContents)
        {
            return _fileSignatureMatcher.MatchFileType(fileContents);
        }
    }
}