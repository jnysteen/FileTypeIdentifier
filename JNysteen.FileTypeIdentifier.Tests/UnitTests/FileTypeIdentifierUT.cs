using System.Collections;
using FluentAssertions;
using JNysteen.FileTypeIdentifier.Interfaces;
using NUnit.Framework;

namespace JNysteen.FileTypeIdentifier.Tests.UnitTests
{
    internal class FileTypeIdentifierUT
    {
        [TestCaseSource(nameof(MagicNumberMatchingShouldMatchTestCases))]
        public void CanMatchFileType(MagicNumberMapping fileMagicNumberMapping, byte[] testFileContents, string actualFileType)
        {
            var magicNumberMatcher = new FileTypeIdentifier(fileMagicNumberMapping);
            var identifiedFileType = magicNumberMatcher.MatchFileType(testFileContents);
            identifiedFileType.Should().NotBeNull("a file type should have been identified");
            identifiedFileType.Should().Be(actualFileType);
        }
        
        [TestCaseSource(nameof(MagicNumberMatchingShouldNotMatchingTestCases))]
        public void CanAvoidMatchingFileType(MagicNumberMapping fileMagicNumberMapping, byte[] testFileContents)
        {
            var magicNumberMatcher = new FileTypeIdentifier(fileMagicNumberMapping);
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
                
                var magicNumberDef1 = new FileMagicNumberDefinition(new byte?[] {1, 2, 3}, "TEST");
                multipleMappings.AddMagicNumberDefinition(magicNumberDef1);
            
                var magicNumberDef2 = new FileMagicNumberDefinition(new byte?[] {1, 2, 3, 4, 5}, magicNumberDef1.FileType + "-other-file-type");
                multipleMappings.AddMagicNumberDefinition(magicNumberDef2);
            
                var magicNumberNotMatchedDef = new FileMagicNumberDefinition(new byte?[] {1, 2, 3, 4, 5, 8}, magicNumberDef2.FileType + "-completely-other-type");
                multipleMappings.AddMagicNumberDefinition(magicNumberNotMatchedDef);
                
                yield return new TestCaseData(
                    new object[]
                    {
                        multipleMappings,
                        new byte[] {1, 2, 3, 4, 5, 6, 7},
                        magicNumberDef2.FileType
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
            var magicNumberDef = new FileMagicNumberDefinition(magicNumber, fileType);
            fileMagicNumberMapping.AddMagicNumberDefinition(magicNumberDef);
            return fileMagicNumberMapping;
        }
    }
}