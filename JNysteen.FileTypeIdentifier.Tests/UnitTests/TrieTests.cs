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

            trie.GetNodeCount().Should().Be(0, "no nodes should exist in the trie");
            
            // Act
            trie.Add(definition, onlyMagicNumber);
            
            // Assert
            trie.GetNodeCount().Should().Be(onlyMagicNumber.Length, "nodes should have been inserted into the trie");
        }
    }
}