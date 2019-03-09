namespace JNysteen.FileTypeIdentifier.Interfaces
{
    /// <summary>
    ///     Contains the logic for matching file contents with a collection of known file signatures
    /// </summary>
    public interface IFileSignatureMatcher
    {
        /// <summary>
        ///     Returns the file type of the first signature, that matches the provided file header. If no matches are found, null
        ///     is returned.
        /// </summary>
        string MatchFileType(byte[] fileContentsContainingHeader);

        /// <summary>
        ///     Retrieves the longest file signature found in the matcher's file signature mapping .
        /// </summary>
        int GetLongestSignature();
    }
}