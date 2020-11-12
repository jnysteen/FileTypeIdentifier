using System.Linq;
using FluentAssertions;
using FluentAssertions.Execution;
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
            trie.GetNodeCount().Should().Be(onlyMagicNumber.Length, $"{onlyMagicNumber.Length} nodes should have been inserted into the trie");
        }
        
        [Test]
        public void CanInsertIntoTrie2()
        {
            // Arrange
            var onlyMagicNumber = new byte?[] {1, null, 3};
            var definition = new FileMagicNumberDefinition(".test", onlyMagicNumber);
            var trie = new Trie<byte?, FileMagicNumberDefinition>();

            trie.RootNode.Should().NotBeNull("root node should be initialized");
            trie.RootNode.Value.Should().BeNull("root node's value should be null");
            trie.RootNode.Key.Should().BeNull("root node's key should be null");
            trie.RootNode.Children.Should().BeEmpty("root node should not have any children yet");
            
            // Act
            trie.Add(definition, onlyMagicNumber);
            
            // Assert
            using (new AssertionScope())
            {
                trie.RootNode.Children.Should().HaveCount(1, "a single child should have been added");
            
                var chainElement1 = trie.RootNode.Children.First();
                chainElement1.Key.Value.Should().Be(1);
                chainElement1.Value.Children.Should().HaveCount(1, "a single child should have been added");
                chainElement1.Value.Value.Should().BeNull("no magic number definition should have been stored on this node");
            
                var chainElement2 = chainElement1.Value.Children.First();
                chainElement2.Key.Value.Should().Be(null);
                chainElement2.Value.Children.Should().HaveCount(1, "a single child should have been added");
                chainElement2.Value.Value.Should().BeNull("no magic number definition should have been stored on this node");
            
                var chainElement3 = chainElement2.Value.Children.First();
                chainElement3.Key.Value.Should().Be(3);
                chainElement3.Value.Children.Should().BeEmpty( "this should be the last element of the chain");
                chainElement3.Value.Value.Should().Be(definition, "the magic number definition should have been stored here");
            }
        }
    }
}