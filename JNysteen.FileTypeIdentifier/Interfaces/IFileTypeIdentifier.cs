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
        string GetFileType(Stream fileStream);

        /// <summary>
        ///     Attempts to identify the file type using the file bytes. If the file type is recognized, the name of it is returned
        ///     - otherwise, null is returned.
        /// </summary>
        string GetFileType(byte[] fileContents);
        
        /// <summary>
        ///     Attempts to identify the file type using the file bytes. If the file type is recognized, the name of it is returned
        ///     - otherwise, null is returned.
        /// </summary>
        string GetFileType(IEnumerable<byte> fileContents);
    }
}