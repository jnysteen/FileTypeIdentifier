using FluentAssertions;
using NUnit.Framework;

namespace JNysteen.FileTypeIdentifier.Tests.IntegrationTests
{
    public class FileTypeIdentification_ByteArrayAsInput_IT
    {
        [Theory]
        public void InputHeaderIsNull_Negative()
        {
            var fileMagicNumberMapping = new MagicNumberMapping();
            var magicNumberMatcher = new MagicNumberMatcher(fileMagicNumberMapping);
            var fileTypeIdentifier = new FileTypeIdentifier(magicNumberMatcher);

            var magicNumber = new byte?[] {1, 2, 3};
            var fileType = "TEST";
            fileMagicNumberMapping.AddMagicNumber(magicNumber, fileType);

            byte[] testFileContents = null;

            var identifiedFileType = fileTypeIdentifier.GetFileType(testFileContents);
            identifiedFileType.Should().BeNull("no file type should have been identified");
        }
    }
}