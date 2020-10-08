using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JNysteen.FileTypeIdentifier.Interfaces;

namespace JNysteen.FileTypeIdentifier
{
    /// <inheritdoc />
    public class FileTypeIdentifier : IFileTypeIdentifier
    {
        private readonly List<FileMagicNumberDefinition> _fileMagicNumberDefinitions;
        private int _longestMagicNumber;

        internal FileTypeIdentifier(List<FileMagicNumberDefinition> magicNumberDefinitions)
        {
            _fileMagicNumberDefinitions = magicNumberDefinitions ?? throw new ArgumentNullException(nameof(magicNumberDefinitions));
            _longestMagicNumber = magicNumberDefinitions.Any() ? magicNumberDefinitions.Max(m => m.LongestMagicNumber) : 0;
        }

        /// <summary>
        ///     Creates an instance of FileTypeIdentifier
        /// </summary>
        public FileTypeIdentifier() : this(new List<FileMagicNumberDefinition>())
        {
        }

        /// <inheritdoc />
        public FileMagicNumberDefinition GetFileType(Stream stream)
        {
            var longestMagicNumber = _longestMagicNumber;
            var magicNumberBytes = new byte[longestMagicNumber];
            var numberOfReadBytes = stream.Read(magicNumberBytes, 0, magicNumberBytes.Length);

            return GetFileType(magicNumberBytes);
        }

        /// <inheritdoc />
        public FileMagicNumberDefinition GetFileType(byte[] fileContents)
        {
            return MatchFileType(fileContents, _fileMagicNumberDefinitions);
        }

        /// <inheritdoc />
        public FileMagicNumberDefinition GetFileType(IEnumerable<byte> fileContents)
        {
            var longestMagicNumber = _longestMagicNumber;
            var fileHeader = fileContents.Take(longestMagicNumber).ToArray();
            
            return GetFileType(fileHeader);
        }

        /// <inheritdoc />
        public void AddMagicNumberDefinition(FileMagicNumberDefinition magicNumberDefinition)
        {
            if (magicNumberDefinition == null) 
                throw new ArgumentNullException(nameof(magicNumberDefinition));
            
            _fileMagicNumberDefinitions.Add(magicNumberDefinition);
            if (magicNumberDefinition.LongestMagicNumber > _longestMagicNumber)
                _longestMagicNumber = magicNumberDefinition.LongestMagicNumber;
        }

        internal FileMagicNumberDefinition MatchFileType(byte[] fileContentsContainingHeader)
        {
            return MatchFileType(fileContentsContainingHeader, _fileMagicNumberDefinitions);
        }
        
        internal static FileMagicNumberDefinition MatchFileType(byte[] fileContentsContainingHeader, List<FileMagicNumberDefinition> fileMagicNumberDefinitions)
        {
            if (fileContentsContainingHeader == null || fileContentsContainingHeader.Length == 0)
                return null;

            foreach (var fileMagicNumberDefinition in fileMagicNumberDefinitions)
            {
                foreach (var magicNumber in fileMagicNumberDefinition.MagicNumbers)
                {
                    // If the input is shorter than the magic number header, it is impossible to match the input against the magic number - skip it
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
                        return fileMagicNumberDefinition;
                }
            }

            return null;
        }
    }
}