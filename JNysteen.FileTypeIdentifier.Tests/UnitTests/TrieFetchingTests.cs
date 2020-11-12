using System.Collections;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace JNysteen.FileTypeIdentifier.Tests.UnitTests
{
    public class TrieFetchingTests
    {
        internal Trie<byte?, FileMagicNumberDefinition> TestTrie;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            TestTrie = new Trie<byte?, FileMagicNumberDefinition>();
            
            var definition1 = new FileMagicNumberDefinition(".test", new byte?[] {1, 2, null, 4});
            TestTrie.Add(definition1, definition1.MagicNumbers.FirstOrDefault());
            
            var definition2 = new FileMagicNumberDefinition(".test-should-not-match", new byte?[] {2, 2, 255});
            TestTrie.Add(definition2, definition2.MagicNumbers.FirstOrDefault());
        }

        [Test]
        [TestCaseSource(nameof(TrieFetchingTestCases))]
        public void CanFetchFromTrie(byte?[] fileContents, string expectedFileExtensionMatch)
        {
            // Act
            FileMagicNumberDefinition foundMatch = TestTrie.GetLongestMatch(fileContents);

            // Assert
            if (expectedFileExtensionMatch != null)
            {
                foundMatch.Should().NotBeNull("a definition should have been found in the trie");
                foundMatch.PrimaryFileExtension.Should().Be(expectedFileExtensionMatch);
            }
            else
            {
                foundMatch.Should().BeNull("a definition should not have been found in the trie");
            }
        }

        public static IEnumerable TrieFetchingTestCases
        {
            get
            {
                yield return new TestCaseData(
                    new object[]
                    {
                        new byte?[] {1, 2, 3, 4}, ".test" 
                    }
                    ).SetName("File contents matches one of the configured magic number definitions exactly");
                
                yield return new TestCaseData(
                    new object[]
                    {
                        new byte?[] {1, 2, 3, 4, 5, 6, 7}, ".test" 
                    }
                ).SetName("File contents starts with one of the configured magic number definitions");
                
                yield return new TestCaseData(
                    new object[]
                    {
                        new byte?[] {1, 2, 3}, null 
                    }
                ).SetName("File contents is shorter than any of the configured magic number definitions");
                
                yield return new TestCaseData(
                    new object[]
                    {
                        new byte?[] {1, 2, 3, 100}, null 
                    }
                ).SetName("File contents does not match any of the configured magic number definitions");
                
                yield return new TestCaseData(
                    new object[]
                    {
                        new byte?[] {5, 2, 3, 4, 5, 6, 7}, null 
                    }
                ).SetName("File contents does not start with any of the configured magic number definitions");
            }
        } 
    }
}