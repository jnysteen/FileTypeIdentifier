using System.Collections.Generic;
using System.IO;
using System.Linq;
using JNysteen.FileTypeIdentifier.Interfaces;

namespace JNysteen.FileTypeIdentifier
{
    /// <inheritdoc />
    public class FileTypeIdentifier : IFileTypeIdentifier
    {
        private readonly MagicNumberMapping _magicNumberMapping;

        internal FileTypeIdentifier(MagicNumberMapping magicNumberMapping)
        {
            _magicNumberMapping = magicNumberMapping;
        }

        /// <summary>
        ///     Creates an instance of FileTypeIdentifier
        /// </summary>
        public FileTypeIdentifier() : this(new MagicNumberMapping())
        {
        }

        /// <inheritdoc />
        public string GetFileType(Stream fileStream)
        {
            var longestMagicNumber = _magicNumberMapping.GetLongestMagicNumber();
            var magicNumberBytes = new byte[longestMagicNumber];
            var numberOfReadBytes = fileStream.Read(magicNumberBytes, 0, magicNumberBytes.Length);

            return GetFileType(magicNumberBytes);
        }

        /// <inheritdoc />
        public string GetFileType(byte[] fileContents)
        {
            return MatchFileType(fileContents, _magicNumberMapping);
        }

        /// <inheritdoc />
        public string GetFileType(IEnumerable<byte> fileContents)
        {
            var longestMagicNumber = _magicNumberMapping.GetLongestMagicNumber();
            var fileHeader = fileContents.Take(longestMagicNumber).ToArray();
            
            return GetFileType(fileHeader);
        }

        /// <inheritdoc />
        public void AddMagicNumberDefinition(FileMagicNumberDefinition magicNumberDefinition)
        {
            _magicNumberMapping.AddMagicNumberDefinition(magicNumberDefinition);
        }

        internal string MatchFileType(byte[] fileContentsContainingHeader)
        {
            return MatchFileType(fileContentsContainingHeader, _magicNumberMapping);
        }
        
        internal static string MatchFileType(byte[] fileContentsContainingHeader, MagicNumberMapping magicNumberMapping)
        {
            if (fileContentsContainingHeader == null || fileContentsContainingHeader.Length == 0)
                return null;

            foreach (var fileMagicNumberDefinition in magicNumberMapping.FileMagicNumberMappingTable)
            {
                var fileType = fileMagicNumberDefinition.PrimaryFileExtension;
                foreach (var magicNumber in fileMagicNumberDefinition.MagicNumbers)
                {
                    // If the input is shorter than the magic number header, it is impossible to match the input against the magic number - continue
                    if (fileContentsContainingHeader.Length < magicNumber.Length)
                        continue;

                    var failed = false;

                    for (var i = 0; i < magicNumber.Length; i++)
                    {
                        var magicNumberByte = magicNumber[i];

                        // Null bytes in the magic numbers are wildcards and should be ignored
                        if (magicNumberByte == null)
                            continue;

                        if (magicNumberByte == fileContentsContainingHeader[i]) 
                            continue;
                    
                        failed = true;
                        break;
                    }

                    if (!failed)
                        return fileType;
                }
 
            }

            return null;
        }
    }
}