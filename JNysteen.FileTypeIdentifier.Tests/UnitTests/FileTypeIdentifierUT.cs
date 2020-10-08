using System.Collections;
using System.Collections.Generic;
using FluentAssertions;
using JNysteen.FileTypeIdentifier.Interfaces;
using NUnit.Framework;

namespace JNysteen.FileTypeIdentifier.Tests.UnitTests
{
    internal class FileTypeIdentifierUT
    {
        [TestCaseSource(nameof(MagicNumberMatchingShouldMatchTestCases))]
        public void CanMatchFileType(List<FileMagicNumberDefinition> definitions, byte[] testFileContents, string actualFileType)
        {
            var magicNumberMatcher = new FileTypeIdentifier(definitions);
            var identifiedFileType = magicNumberMatcher.MatchFileType(testFileContents);
            identifiedFileType.Should().NotBeNull("a file type should have been identified");
            identifiedFileType.PrimaryFileExtension.Should().Be(actualFileType);
        }
        
        [TestCaseSource(nameof(MagicNumberMatchingShouldNotMatchingTestCases))]
        public void CanAvoidMatchingFileType(List<FileMagicNumberDefinition> definitions, byte[] testFileContents)
        {
            var magicNumberMatcher = new FileTypeIdentifier(definitions);
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
                        new List<FileMagicNumberDefinition>(){new FileMagicNumberDefinition("TEST", new byte?[] {1, 2, 3, 4, 5})},
                        new byte[] {1, 2, 3, 4, 5},
                        "TEST"
                    }
                    ).SetName("File contents is as long as magic number");
                
                yield return new TestCaseData(
                    new object[]
                    {
                        new List<FileMagicNumberDefinition>(){new FileMagicNumberDefinition("TEST", new byte?[] {1, 2, 3})},
                        new byte[] {1, 2, 3, 4, 5},
                        "TEST"
                    }
                ).SetName("File contents is longer than the magic number");

                yield return new TestCaseData(
                    new object[]
                    {
                        new List<FileMagicNumberDefinition>(){new FileMagicNumberDefinition("TEST", new byte?[] {1, 2, null, null, 5})},
                        new byte[] {1, 2, 3, 4, 5},
                        "TEST"
                    }
                ).SetName("Magic number with wild cards");

                var multipleMappings = new List<FileMagicNumberDefinition>();
                
                var magicNumberDef1 = new FileMagicNumberDefinition("TEST", new byte?[] {1, 2, 3});
                multipleMappings.Add(magicNumberDef1);
            
                var magicNumberDef2 = new FileMagicNumberDefinition(magicNumberDef1.PrimaryFileExtension + "-other-file-type", new byte?[] {1, 2, 3, 4, 5});
                multipleMappings.Add(magicNumberDef2);
            
                var magicNumberNotMatchedDef = new FileMagicNumberDefinition(magicNumberDef2.PrimaryFileExtension + "-completely-other-type", new byte?[] {1, 2, 3, 4, 5, 8});
                multipleMappings.Add(magicNumberNotMatchedDef);
                
                yield return new TestCaseData(
                    new object[]
                    {
                        multipleMappings,
                        new byte[] {1, 2, 3, 4, 5, 6, 7},
                        magicNumberDef2.PrimaryFileExtension
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
                        new List<FileMagicNumberDefinition>(){new FileMagicNumberDefinition("TEST", new byte?[] {1, 2, 3, 4, 5})},
                        new byte[] {1, 2, 3}
                    }
                ).SetName("File contents is shorter than any configured magic number");
                
                yield return new TestCaseData(
                    new object[]
                    {
                new List<FileMagicNumberDefinition>(){new FileMagicNumberDefinition("TEST", new byte?[] {1, 2, 3})},
                        new byte[] {2, 3, 4, 5}
                    }
                ).SetName("No configured magic number matches file's header");
                
                yield return new TestCaseData(
                    new object[]
                    {
                new List<FileMagicNumberDefinition>(){new FileMagicNumberDefinition("TEST", new byte?[] {1, 2, 3})},
                        null
                    }
                ).SetName("The input file contents is null");
            }
        }
    }
}