namespace JNysteen.FileTypeIdentifier.Interfaces
{
    /// <summary>
    ///     Contains the logic for matching file contents with a collection of known magic numbers
    /// </summary>
    public interface IFileMagicNumberMatcher
    {
        /// <summary>
        ///     Returns the file type of the first magic number, that matches the provided file header. If no matches are found, null
        ///     is returned.
        /// </summary>
        string MatchFileType(byte[] fileContentsContainingHeader);

        /// <summary>
        ///     Retrieves the longest magic number found in the matcher's magic number mapping .
        /// </summary>
        int GetLongestMagicNumber();
    }
}