using System;
using System.Collections.Generic;
using System.Linq;

namespace JNysteen.FileTypeIdentifier
{
    internal class Trie<TKeyElement, TValue>
    {
        internal readonly TrieNode RootNode;
        private int _longestContainedPrefix;
        
        public Trie()
        {
            RootNode = new TrieNode();
        }
        
        public void Add(TValue entity, IEnumerable<TKeyElement> prefix)
        {
            var enumeratedElements = prefix.ToArray();
            _longestContainedPrefix = Math.Max(enumeratedElements.Length, _longestContainedPrefix);
            RootNode.Add(entity, enumeratedElements);
        }

        public TValue GetLongestMatch(IEnumerable<TKeyElement> keySequence)
        {
            if (keySequence == null)
                return default;
            var sequence = keySequence.Take(_longestContainedPrefix).ToArray();
            if (sequence.Length == 0)
                return default;
            return RootNode.GetLongestMatch(sequence, 0).value;
        }

        /// <summary>
        ///     Returns the count of all unique nodes included in the trie
        /// </summary>
        public int GetNodeCount()
        {
            // We subtract one as we do not include the root node in the count
            return RootNode.GetNodeCount() - 1;
        }

        internal class TrieNode
        {
            public TKeyElement WildCard { get; }
            internal TKeyElement Key { get; set; }
            internal TValue Value { get; set; }
            internal Dictionary<NullWrapper<TKeyElement>, TrieNode> Children { get; set; } = new Dictionary<NullWrapper<TKeyElement>, TrieNode>();

            public TrieNode(TKeyElement wildCard = default)
            {
                WildCard = wildCard;
            }

            public static List<TrieNode> ConvertSequenceToTrieNodeChain(TValue entity, TKeyElement[] keySequence)
            {
                var chain = new List<TrieNode>();
                for (var i = 0; i < keySequence.Length; i++)
                {
                    var node = new TrieNode
                    {
                        Key = keySequence[i]
                    };

                    var isLastElement = i == keySequence.Length - 1;
                    if (isLastElement)
                        node.Value = entity;
                    
                    chain.Add(node);
                }

                return chain;
            }

            public void Add2(List<TrieNode> trieNodeChain)
            {
                // TODO implement proper insertion of the nodes into the trie
            }
            
            public void Add(TValue entity, TKeyElement[] keySequence, int currentIndex = -1)
            {
                if (currentIndex < 0)
                {
                    var startKey = new NullWrapper<TKeyElement>(keySequence[0]);
                    TrieNode nextNode;
                    var hasMatchingChildForPrefix = Children.TryGetValue(startKey, out nextNode);
                    if (!hasMatchingChildForPrefix)
                    {
                        nextNode = new TrieNode()
                        {
                            Key = startKey.Value
                        };
                        Children.Add(startKey, nextNode);
                    }   

                    nextNode.Add(entity, keySequence, 1);
                }
                else
                {
                    var currentKey = keySequence[currentIndex];
                    Key = currentKey;
                
                    // If this is the last part of the keys, place the entity at this node
                    if (currentIndex == keySequence.Length - 1)
                    {
                        Value = entity;
                        return;
                    }

                    var nextKey = new NullWrapper<TKeyElement>(keySequence[currentIndex + 1]);
                
                    TrieNode nextNode;
                    var hasMatchingChildForPrefix = Children.TryGetValue(nextKey, out nextNode);

                    if (!hasMatchingChildForPrefix)
                    {
                        nextNode = new TrieNode()
                        {
                            Key = nextKey.Value
                        };
                        Children.Add(nextKey, nextNode);
                    }   

                    nextNode.Add(entity, keySequence, currentIndex+1);
                }
            }
            
            public (TValue value, int foundAtIndex) GetLongestMatch(TKeyElement[] remainingPrefixElements, int currentIndex = -1)
            {
                // Special logic for the root node
                if (currentIndex < 0)
                {
                    return Children
                            .Select(t => t.Value.GetLongestMatch(remainingPrefixElements, 0))
                            .Where(m => m.value != null)
                            .OrderByDescending(m => m.foundAtIndex)
                            .FirstOrDefault()
                        ;
                }
                
                var isLastPrefixElement = currentIndex == remainingPrefixElements.Length - 1;
                var currentKey = remainingPrefixElements[currentIndex];
                var currentKeyMatchesNodeKey = currentKey.Equals(Key) || currentKey.Equals(WildCard) || Key.Equals(WildCard);
                
                // If this is the last element of the prefix, we'll return whatever value is found here - if the keys match
                if (isLastPrefixElement)
                    return currentKeyMatchesNodeKey ? (Value, currentIndex) : default;

                // If the current prefix does not match the key of the current node, and the node is not a wildcard, it is not a match
                if (!currentKeyMatchesNodeKey)
                    return default;

                // Otherwise, we'll look further into the trie for the longest match we can find
                var nextKey = remainingPrefixElements[currentIndex + 1];
                
                // A list of all the tries that should be matched against
                var triesToMatch = new List<TrieNode>();

                // Add any node that matches the next key
                Children.TryGetValue(new NullWrapper<TKeyElement>(nextKey), out var nextTrieWithMatchingKey);
                if(nextTrieWithMatchingKey != null)
                    triesToMatch.Add(nextTrieWithMatchingKey);
                
                // Add any wild card that is an immediate descendant of this node
                Children.TryGetValue(new NullWrapper<TKeyElement>(WildCard), out var nextWildcardTrie);
                if(nextWildcardTrie != null)
                    triesToMatch.Add(nextWildcardTrie);
                
                // If the next key is a wild card, add all immediate descendants of this node
                if(nextKey.Equals(WildCard))
                    triesToMatch.AddRange(Children.Values);
                    
                var longestMatch = triesToMatch
                    .Select(t => t.GetLongestMatch(remainingPrefixElements, currentIndex + 1))
                    .Where(m => m.value != null)
                    .OrderByDescending(m => m.foundAtIndex)
                    .FirstOrDefault()
                    ;

                // Return the longest match found beyond this node, if any
                // Otherwise, this node is the best match - return this
                return longestMatch.value != null ? longestMatch : (Value, currentIndex);
            }
            
            /// <summary>
            ///     Returns the count of all unique nodes included in the tree where this node is the root.
            ///     The count also includes this node
            /// </summary>
            public int GetNodeCount()
            {
                var uniqueNodes = GetUniqueNodes();
                return uniqueNodes?.Count ?? 0;
            }

            private HashSet<TrieNode> GetUniqueNodes(HashSet<TrieNode> acc = null)
            {
                if (acc?.Contains(this) == true)
                    return acc;
                
                acc ??= new HashSet<TrieNode>();
                acc.Add(this);

                return Children.Aggregate(acc, (current, child) => child.Value.GetUniqueNodes(current));
            }

            /// <summary>
            ///     A Dictionary cannot have null-keys, but we need to mimic null, as we use them for wild cards.
            ///     This class is just a wrapper for null values, enabling us to put them in a Dictionary anyways.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            internal class NullWrapper<T>
            {
                public T Value { get; }

                public NullWrapper(T value)
                {
                    Value = value;
                }
                
                protected bool Equals(NullWrapper<T> other)
                {
                    return EqualityComparer<T>.Default.Equals(Value, other.Value);
                }

                public override bool Equals(object obj)
                {
                    if (ReferenceEquals(null, obj)) return false;
                    if (ReferenceEquals(this, obj)) return true;
                    if (obj.GetType() != this.GetType()) return false;
                    return Equals((NullWrapper<T>) obj);
                }

                public override int GetHashCode()
                {
                    return EqualityComparer<T>.Default.GetHashCode(Value);
                }
            }
        }
    }
}