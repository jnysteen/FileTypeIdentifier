using System.Linq;
using FluentAssertions;
using JNysteen.FileTypeIdentifier.MagicNumbers;
using NUnit.Framework;

namespace JNysteen.FileTypeIdentifier.Tests.UnitTests
{
    public class TrieTests
    {

        [Test]
        public void CanInsertIntoTrie()
        {
            // Arrange
            var onlyMagicNumber = new byte?[] {1, 2, null, 4};
            var definition = new FileMagicNumberDefinition(".test", onlyMagicNumber);
            var trie = new Trie<byte?, FileMagicNumberDefinition>();
            
            // Act
            trie.Add(definition, onlyMagicNumber);
            
            // Assert
            Assert.Fail("assert something here");
        }
    }
}