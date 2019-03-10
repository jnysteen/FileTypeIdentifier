using System;
using JNysteen.FileTypeIdentifier.Interfaces;

namespace JNysteen.FileTypeIdentifier
{
    /// <inheritdoc />
    internal class MagicNumberMatcher : IFileMagicNumberMatcher
    {
        private readonly IFileMagicNumberMapping _fileMagicNumberMapping;

        /// <summary>
        ///     Creates a new magicNumberMatcher, that are able to recognize the magic numbers found in the provided mapping.
        /// </summary>
        public MagicNumberMatcher(IFileMagicNumberMapping fileMagicNumberMapping)
        {
            _fileMagicNumberMapping = fileMagicNumberMapping;
        }

        /// <inheritdoc />
        public int GetLongestMagicNumber()
        {
            return _fileMagicNumberMapping.GetLongestMagicNumber();
        }

        /// <inheritdoc />
        public string MatchFileType(byte[] fileContentsContainingHeader)
        {
            if (fileContentsContainingHeader == null || fileContentsContainingHeader.Length == 0)
                return null;

            foreach (var fileMagicNumberAndType in _fileMagicNumberMapping.FileMagicNumbersByLength)
            {
                var (magicNumber, fileType) = fileMagicNumberAndType;

                // If the input is shorter than the magic number header, it is impossible to match the input against the magic number - continue
                if (fileContentsContainingHeader.Length < magicNumber.Length)
                    continue;

                var shortestSequence = Math.Min(fileContentsContainingHeader.Length, magicNumber.Length);

                var failed = false;

                for (var i = 0; i < shortestSequence; i++)
                {
                    var magicNumberByte = magicNumber[i];

                    // Null bytes in the magic numbers are wildcards and should be ignored
                    if (magicNumberByte == null)
                        continue;

                    if (magicNumberByte != fileContentsContainingHeader[i])
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