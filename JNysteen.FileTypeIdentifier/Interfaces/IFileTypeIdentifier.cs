using System.Collections.Generic;
using System.IO;

namespace JNysteen.FileTypeIdentifier.Interfaces
{
    /// <summary>
    ///     Exposes methods for identifying file types.
    /// </summary>
    public interface IFileTypeIdentifier
    {
        /// <summary>
        ///     Attempts to identify the file type using the contents of the file byte stream. If the file type is recognized, the
        ///     name of it is returned - otherwise, null is returned.
        /// </summary>
        FileMagicNumberDefinition GetFileType(Stream stream);

        /// <summary>
        ///     Attempts to identify the file type using the file bytes. If the file type is recognized, the name of it is returned
        ///     - otherwise, null is returned.
        /// </summary>
        FileMagicNumberDefinition GetFileType(byte[] fileContents);
        
        /// <summary>
        ///     Attempts to identify the file type using the file bytes. If the file type is recognized, the name of it is returned
        ///     - otherwise, null is returned.
        /// </summary>
        FileMagicNumberDefinition GetFileType(IEnumerable<byte> fileContents);

        /// <summary>
        ///     Configure the file type identifier with the magic number for the given file type
        /// </summary>
        /// <param name="magicNumberDefinition">A magic number for the file type</param>
        void AddMagicNumberDefinition(FileMagicNumberDefinition magicNumberDefinition);
    }
}