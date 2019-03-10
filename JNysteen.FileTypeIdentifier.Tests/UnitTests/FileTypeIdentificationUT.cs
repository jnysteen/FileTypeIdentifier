using JNysteen.FileTypeIdentifier.Interfaces;
using Moq;
using Xunit;

namespace JNysteen.FileTypeIdentifier.Tests.UnitTests
{
    public class FileTypeIdentificationUT
    {
        [Fact]
        public void CanGetFileType_MatcherFindsMatch_Positive()
        {
            var fileType = "TEST";

            var magicNumberMappingMock = new Mock<IFileMagicNumberMatcher>();
            magicNumberMappingMock.Setup(matcher => matcher.MatchFileType(It.IsAny<byte[]>())).Returns(fileType);

            var fileTypeIdentifier = new FileTypeIdentifier(magicNumberMappingMock.Object);

            var identifiedFileType = fileTypeIdentifier.GetFileType(new byte[0]);
            Assert.NotNull(identifiedFileType);
            Assert.Equal(fileType, identifiedFileType);
        }

        [Fact]
        public void CanGetFileType_MatcherFindsNoMatch_Negative()
        {
            var magicNumberMappingMock = new Mock<IFileMagicNumberMatcher>();
            magicNumberMappingMock.Setup(matcher => matcher.MatchFileType(It.IsAny<byte[]>())).Returns<string>(null);

            var fileTypeIdentifier = new FileTypeIdentifier(magicNumberMappingMock.Object);

            var identifiedFileType = fileTypeIdentifier.GetFileType(new byte[0]);
            Assert.Null(identifiedFileType);
        }
    }
}