using System.IO;
using JNysteen.FileTypeIdentifier.Interfaces;

namespace JNysteen.FileTypeIdentifier
{
    /// <inheritdoc />
    public class FileTypeIdentifier : IFileTypeIdentifier
    {
        private readonly IFileMagicNumberMatcher _fileMagicNumberMatcher;

        /// <summary>
        ///     Creates a FileTypeIdentifier that will match file magic numbers using the provided magic number matcher.
        /// </summary>
        internal FileTypeIdentifier(IFileMagicNumberMatcher fileMagicNumberMatcher)
        {
            _fileMagicNumberMatcher = fileMagicNumberMatcher;
        }

        /// <summary>
        ///     Creates a FileTypeIdentifier that will match file magic numbers using the provided magic number mapping and the
        ///     default magic number matcher.
        /// </summary>
        public FileTypeIdentifier(IFileMagicNumberMapping fileMagicNumberMapping)
        {
            _fileMagicNumberMatcher = new MagicNumberMatcher(fileMagicNumberMapping);
        }

        /// <inheritdoc />
        public string GetFileType(Stream fileStream)
        {
            var longestMagicNumber = _fileMagicNumberMatcher.GetLongestMagicNumber();
            var magicNumberBytes = new byte[longestMagicNumber];
            var numberOfReadBytes = fileStream.Read(magicNumberBytes, 0, magicNumberBytes.Length);

            return GetFileType(magicNumberBytes);
        }

        /// <inheritdoc />
        public string GetFileType(byte[] fileContents)
        {
            return _fileMagicNumberMatcher.MatchFileType(fileContents);
        }
    }
}