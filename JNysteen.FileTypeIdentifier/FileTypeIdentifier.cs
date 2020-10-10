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
        private readonly FileMagicNumberDefinitionTrie _fileMagicNumberDefinitions;

        internal FileTypeIdentifier(FileMagicNumberDefinitionTrie magicNumberDefinitions)
        {
            _fileMagicNumberDefinitions = magicNumberDefinitions ?? throw new ArgumentNullException(nameof(magicNumberDefinitions));
        }

        /// <summary>
        ///     Creates an instance of FileTypeIdentifier
        /// </summary>
        public FileTypeIdentifier() : this(new FileMagicNumberDefinitionTrie())
        {
        }

        /// <inheritdoc />
        public FileMagicNumberDefinition GetFileType(Stream stream)
        { 
            return GetFileType(StreamAsIEnumerable(stream));
        }
        
        private static IEnumerable<byte> StreamAsIEnumerable(Stream stream)
        {
            if (stream != null)
                for (int i = stream.ReadByte(); i != -1; i = stream.ReadByte())
                    yield return (byte)i;
        }

        /// <inheritdoc />
        public FileMagicNumberDefinition GetFileType(byte[] fileContents)
        {
            return MatchFileType(fileContents, _fileMagicNumberDefinitions);
        }

        /// <inheritdoc />
        public FileMagicNumberDefinition GetFileType(IEnumerable<byte> fileContents)
        {
            return MatchFileType(fileContents, _fileMagicNumberDefinitions);
        }

        /// <inheritdoc />
        public void AddMagicNumberDefinition(FileMagicNumberDefinition magicNumberDefinition)
        {
            if (magicNumberDefinition == null) 
                throw new ArgumentNullException(nameof(magicNumberDefinition));

            foreach (var magicNumber in magicNumberDefinition.MagicNumbers)
            {
                _fileMagicNumberDefinitions.Add(magicNumberDefinition, magicNumber);
            }
        }
        
        internal static FileMagicNumberDefinition MatchFileType(IEnumerable<byte> fileContentsContainingHeader, FileMagicNumberDefinitionTrie fileMagicNumberDefinitions)
        {
            if (fileContentsContainingHeader == null)
                return null;
            var casted = fileContentsContainingHeader.Select(f => f as byte?);
            return fileMagicNumberDefinitions.GetLongestMatch(casted);
        }
    }
}