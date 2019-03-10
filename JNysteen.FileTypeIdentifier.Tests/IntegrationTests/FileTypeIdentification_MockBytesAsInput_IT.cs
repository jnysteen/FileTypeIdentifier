using Xunit;

namespace JNysteen.FileTypeIdentifier.Tests.IntegrationTests
{
    public class FileTypeIdentification_MockBytesAsInput_IT
    {
        
        [Fact]
        public void CanGetFileType_FileContentsAsLongAsMagicNumber_Positive()
        {
            var fileMagicNumberMapping = new MagicNumberMapping();
            var magicNumberMatcher = new MagicNumberMatcher(fileMagicNumberMapping);
            var fileTypeIdentifier = new FileTypeIdentifier(magicNumberMatcher);

            var magicNumber = new byte?[] {1, 2, 3, 4, 5};
            var fileType = "TEST";
            fileMagicNumberMapping.AddMagicNumber(magicNumber, fileType);

            var testFileContents = new byte[] {1, 2, 3, 4, 5};

            var identifiedFileType = fileTypeIdentifier.GetFileType(testFileContents);
            Assert.NotNull(identifiedFileType);
            Assert.Equal(fileType, identifiedFileType);
        }

        [Fact]
        public void CanGetFileType_FileContentsLongerThanMagicNumber_Positive()
        {
            var fileMagicNumberMapping = new MagicNumberMapping();
            var magicNumberMatcher = new MagicNumberMatcher(fileMagicNumberMapping);
            var fileTypeIdentifier = new FileTypeIdentifier(magicNumberMatcher);

            var magicNumber = new byte?[] {1, 2, 3};
            var fileType = "TEST";
            fileMagicNumberMapping.AddMagicNumber(magicNumber, fileType);

            var testFileContents = new byte[] {1, 2, 3, 4, 5};

            var identifiedFileType = fileTypeIdentifier.GetFileType(testFileContents);
            Assert.NotNull(identifiedFileType);
            Assert.Equal(fileType, identifiedFileType);
        }

        [Fact]
        public void CanGetFileType_MagicNumberLongerThanInput_Positive()
        {
            var fileMagicNumberMapping = new MagicNumberMapping();
            var magicNumberMatcher = new MagicNumberMatcher(fileMagicNumberMapping);
            var fileTypeIdentifier = new FileTypeIdentifier(magicNumberMatcher);

            var magicNumber = new byte?[] {1, 2, 3, 4, 5};
            var fileType = "TEST";
            fileMagicNumberMapping.AddMagicNumber(magicNumber, fileType);

            var testFileContents = new byte[] {1, 2, 3};

            var identifiedFileType = fileTypeIdentifier.GetFileType(testFileContents);
            Assert.Null(identifiedFileType);
        }

        [Fact]
        public void CanGetFileType_UnknownMagicNumber_Positive()
        {
            var fileMagicNumberMapping = new MagicNumberMapping();
            var magicNumberMatcher = new MagicNumberMatcher(fileMagicNumberMapping);
            var fileTypeIdentifier = new FileTypeIdentifier(magicNumberMatcher);

            var magicNumber = new byte?[] {1, 2, 3};
            var fileType = "TEST";
            fileMagicNumberMapping.AddMagicNumber(magicNumber, fileType);

            var testFileContents = new byte[] {2, 3, 4, 5};

            var identifiedFileType = fileTypeIdentifier.GetFileType(testFileContents);
            Assert.Null(identifiedFileType);
        }

        [Fact]
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
            Assert.Null(identifiedFileType);
        }
    }
}