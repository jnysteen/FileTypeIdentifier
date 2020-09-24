using FluentAssertions;
using NUnit.Framework;

namespace JNysteen.FileTypeIdentifier.Tests.UnitTests
{
    public class MagicNumberMatcherUT
    {
        [Theory]
        public void CanMatchFileType_FileContentsAsLongAsMagicNumber_Positive()
        {
            var fileMagicNumberMapping = new MagicNumberMapping();
            var magicNumberMatcher = new MagicNumberMatcher(fileMagicNumberMapping);

            var magicNumber = new byte?[] {1, 2, 3, 4, 5};
            var fileType = "TEST";
            fileMagicNumberMapping.AddMagicNumber(magicNumber, fileType);

            var testFileContents = new byte[] {1, 2, 3, 4, 5};

            var identifiedFileType = magicNumberMatcher.MatchFileType(testFileContents);
            identifiedFileType.Should().NotBeNull("a file type should have been identified");
            identifiedFileType.Should().Be(fileType);
        }

        [Theory]
        public void CanMatchFileType_FileContentsLongerThanMagicNumber_Positive()
        {
            var fileMagicNumberMapping = new MagicNumberMapping();
            var magicNumberMatcher = new MagicNumberMatcher(fileMagicNumberMapping);

            var magicNumber = new byte?[] {1, 2, 3};
            var fileType = "TEST";
            fileMagicNumberMapping.AddMagicNumber(magicNumber, fileType);

            var testFileContents = new byte[] {1, 2, 3, 4, 5};

            var identifiedFileType = magicNumberMatcher.MatchFileType(testFileContents);
            identifiedFileType.Should().NotBeNull("a file type should have been identified");
            identifiedFileType.Should().Be(fileType);
        }
        
        [Theory]
        public void CanMatchFileType_ReturnsLongestMatch_Positive()
        {
            var fileMagicNumberMapping = new MagicNumberMapping();
            var magicNumberMatcher = new MagicNumberMatcher(fileMagicNumberMapping);

            var magicNumber1 = new byte?[] {1, 2, 3};
            var fileType1 = "TEST";
            fileMagicNumberMapping.AddMagicNumber(magicNumber1, fileType1);
            
            var magicNumber2 = new byte?[] {1, 2, 3, 4, 5};
            var fileType2 = fileType1 + "-other-file-type";
            fileMagicNumberMapping.AddMagicNumber(magicNumber2, fileType2);
            
            var magicNumberNotMatching = new byte?[] {1, 2, 3, 4, 5, 8};
            var fileTypeNotMatching = fileType2 + "-completely-other-type";
            fileMagicNumberMapping.AddMagicNumber(magicNumberNotMatching, fileTypeNotMatching);

            var expectedMatchedFileType = fileType2;

            var testFileContents = new byte[] {1, 2, 3, 4, 5, 6, 7};

            var identifiedFileType = magicNumberMatcher.MatchFileType(testFileContents);
            identifiedFileType.Should().NotBeNull("a file type should have been identified");
            identifiedFileType.Should().Be(expectedMatchedFileType);
        }
        
        [Theory]
        public void CanMatchFileType_MagicNumberWithWildcards_Positive()
        {
            var fileMagicNumberMapping = new MagicNumberMapping();
            var magicNumberMatcher = new MagicNumberMatcher(fileMagicNumberMapping);

            var magicNumber = new byte?[] {1, 2, null, null, 5};
            var fileType = "TEST";
            fileMagicNumberMapping.AddMagicNumber(magicNumber, fileType);

            var testFileContents = new byte[] {1, 2, 3, 4, 5};

            var identifiedFileType = magicNumberMatcher.MatchFileType(testFileContents);
            identifiedFileType.Should().NotBeNull("a file type should have been identified");
            identifiedFileType.Should().Be(fileType);
        }

        [Theory]
        public void CanMatchFileType_MagicNumberLongerThanInput_Positive()
        {
            var fileMagicNumberMapping = new MagicNumberMapping();
            var magicNumberMatcher = new MagicNumberMatcher(fileMagicNumberMapping);

            var magicNumber = new byte?[] {1, 2, 3, 4, 5};
            var fileType = "TEST";
            fileMagicNumberMapping.AddMagicNumber(magicNumber, fileType);

            var testFileContents = new byte[] {1, 2, 3};

            var identifiedFileType = magicNumberMatcher.MatchFileType(testFileContents);
            identifiedFileType.Should().BeNull("the file's contents was shorter than any configured magic number");
        }

        [Theory]
        public void CanMatchFileType_UnknownMagicNumber_Positive()
        {
            var fileMagicNumberMapping = new MagicNumberMapping();
            var magicNumberMatcher = new MagicNumberMatcher(fileMagicNumberMapping);

            var magicNumber = new byte?[] {1, 2, 3};
            var fileType = "TEST";
            fileMagicNumberMapping.AddMagicNumber(magicNumber, fileType);

            var testFileContents = new byte[] {2, 3, 4, 5};

            var identifiedFileType = magicNumberMatcher.MatchFileType(testFileContents);
            identifiedFileType.Should().BeNull("no configured magic number matches file's header");
        }

        [Theory]
        public void InputHeaderIsNull_Negative()
        {
            var fileMagicNumberMapping = new MagicNumberMapping();
            var magicNumberMatcher = new MagicNumberMatcher(fileMagicNumberMapping);

            var magicNumber = new byte?[] {1, 2, 3};
            var fileType = "TEST";
            fileMagicNumberMapping.AddMagicNumber(magicNumber, fileType);

            byte[] testFileContents = null;

            var identifiedFileType = magicNumberMatcher.MatchFileType(testFileContents);
            identifiedFileType.Should().BeNull("the input file's header is null");
        }
    }
}