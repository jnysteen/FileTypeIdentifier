using System.Collections;
using FluentAssertions;
using JNysteen.FileTypeIdentifier.Interfaces;
using NUnit.Framework;

namespace JNysteen.FileTypeIdentifier.Tests.UnitTests
{
    public class MagicNumberMatcherUT
    {
        [TestCaseSource(nameof(MagicNumberMatchingShouldMatchTestCases))]
        public void CanMatchFileType(IFileMagicNumberMapping fileMagicNumberMapping, byte[] testFileContents, string actualFileType)
        {
            var magicNumberMatcher = new MagicNumberMatcher(fileMagicNumberMapping);
            var identifiedFileType = magicNumberMatcher.MatchFileType(testFileContents);
            identifiedFileType.Should().NotBeNull("a file type should have been identified");
            identifiedFileType.Should().Be(actualFileType);
        }
        
        [TestCaseSource(nameof(MagicNumberMatchingShouldNotMatchingTestCases))]
        public void CanAvoidMatchingFileType(IFileMagicNumberMapping fileMagicNumberMapping, byte[] testFileContents)
        {
            var magicNumberMatcher = new MagicNumberMatcher(fileMagicNumberMapping);
            var identifiedFileType = magicNumberMatcher.MatchFileType(testFileContents);
            identifiedFileType.Should().BeNull("no file type should have been identified");
        }

        public static IEnumerable MagicNumberMatchingShouldMatchTestCases
        {
            get
            {
                yield return new TestCaseData(
                    new object[]
                    {
                        CreateMappingWithSingleMagicNumber(new byte?[] {1, 2, 3, 4, 5}, "TEST") ,
                        new byte[] {1, 2, 3, 4, 5},
                        "TEST"
                    }
                    ).SetName("File contents is as long as magic number");
                
                yield return new TestCaseData(
                    new object[]
                    {
                        CreateMappingWithSingleMagicNumber(new byte?[] {1, 2, 3}, "TEST") ,
                        new byte[] {1, 2, 3, 4, 5},
                        "TEST"
                    }
                ).SetName("File contents is longer than the magic number");

                yield return new TestCaseData(
                    new object[]
                    {
                        CreateMappingWithSingleMagicNumber(new byte?[] {1, 2, null, null, 5}, "TEST") ,
                        new byte[] {1, 2, 3, 4, 5},
                        "TEST"
                    }
                ).SetName("Magic number with wild cards");

                var multipleMappings = new MagicNumberMapping();
                
                var magicNumber1 = new byte?[] {1, 2, 3};
                var fileType1 = "TEST";
                multipleMappings.AddMagicNumber(magicNumber1, fileType1);
            
                var magicNumber2 = new byte?[] {1, 2, 3, 4, 5};
                var fileType2 = fileType1 + "-other-file-type";
                multipleMappings.AddMagicNumber(magicNumber2, fileType2);
            
                var magicNumberNotMatching = new byte?[] {1, 2, 3, 4, 5, 8};
                var fileTypeNotMatching = fileType2 + "-completely-other-type";
                multipleMappings.AddMagicNumber(magicNumberNotMatching, fileTypeNotMatching);
                
                yield return new TestCaseData(
                    new object[]
                    {
                        multipleMappings,
                        new byte[] {1, 2, 3, 4, 5, 6, 7},
                        fileType2
                    }
                    ).SetName("Multiple matching mappings, longest match is chosen");
            }
        }
        
        public static IEnumerable MagicNumberMatchingShouldNotMatchingTestCases
        {
            get
            {
                yield return new TestCaseData(
                    new object[]
                    {
                        CreateMappingWithSingleMagicNumber(new byte?[] {1, 2, 3, 4, 5}, "TEST") ,
                        new byte[] {1, 2, 3}
                    }
                ).SetName("File contents is shorter than any configured magic number");
                
                yield return new TestCaseData(
                    new object[]
                    {
                        CreateMappingWithSingleMagicNumber(new byte?[] {1, 2, 3}, "TEST") ,
                        new byte[] {2, 3, 4, 5}
                    }
                ).SetName("No configured magic number matches file's header");
                
                yield return new TestCaseData(
                    new object[]
                    {
                        CreateMappingWithSingleMagicNumber(new byte?[] {1, 2, 3}, "TEST") ,
                        null
                    }
                ).SetName("The input file contents is null");
            }
        }

        public static MagicNumberMapping CreateMappingWithSingleMagicNumber(byte?[] magicNumber, string fileType)
        {
            var fileMagicNumberMapping = new MagicNumberMapping();
            fileMagicNumberMapping.AddMagicNumber(magicNumber, fileType);
            return fileMagicNumberMapping;
        }
    }
}