using FluentAssertions;
using JNysteen.FileTypeIdentifier.Interfaces;
using Moq;
using NUnit.Framework;

namespace JNysteen.FileTypeIdentifier.Tests.UnitTests
{
    public class FileTypeIdentificationUT
    {
        [Theory]
        public void CanGetFileType_MatcherFindsMatch_Positive()
        {
            var fileType = "TEST";

            var magicNumberMappingMock = new Mock<IFileMagicNumberMatcher>();
            magicNumberMappingMock.Setup(matcher => matcher.MatchFileType(It.IsAny<byte[]>())).Returns(fileType);

            var fileTypeIdentifier = new FileTypeIdentifier(magicNumberMappingMock.Object);

            var identifiedFileType = fileTypeIdentifier.GetFileType(new byte[0]);
            identifiedFileType.Should().NotBeNull("a file type should have been identified");
            identifiedFileType.Should().Be(fileType);
        }

        [Theory]
        public void CanGetFileType_MatcherFindsNoMatch_Negative()
        {
            var magicNumberMappingMock = new Mock<IFileMagicNumberMatcher>();
            magicNumberMappingMock.Setup(matcher => matcher.MatchFileType(It.IsAny<byte[]>())).Returns<string>(null);

            var fileTypeIdentifier = new FileTypeIdentifier(magicNumberMappingMock.Object);

            var identifiedFileType = fileTypeIdentifier.GetFileType(new byte[0]);
            identifiedFileType.Should().NotBeNull();
        }
    }
}