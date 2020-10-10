using System;
using System.Collections.Generic;
using System.Linq;

namespace JNysteen.FileTypeIdentifier
{
    internal class Trie<TKeyElement, TValue>
    {
        private readonly TrieNode<TKeyElement, TValue> _rootNode;
        private int _longestContainedPrefix;
        
        public Trie()
        {
            _rootNode = new TrieNode<TKeyElement, TValue>();
        }
        
        public void Add(TValue entity, IEnumerable<TKeyElement> prefix)
        {
            var enumeratedElements = prefix.ToArray();
            _longestContainedPrefix = Math.Max(enumeratedElements.Length, _longestContainedPrefix);
            _rootNode.Add(entity, enumeratedElements, 0);
        }

        public TValue GetLongestMatch(IEnumerable<TKeyElement> keySequence)
        {
            if (keySequence == null)
                return default;
            var sequence = keySequence.Take(_longestContainedPrefix).ToArray();
            if (sequence.Length == 0)
                return default;
            return _rootNode.GetLongestMatch(sequence, 0).value;
        }

        internal class TrieNode<TKey, TValue>
        {
            public TKey WildCard { get; }
            internal TKey Key { get; set; }
            internal TValue Value { get; set; }
            internal Dictionary<NullWrapper<TKey>, TrieNode<TKey, TValue>> Children { get; set; } = new Dictionary<NullWrapper<TKey>, TrieNode<TKey, TValue>>();

            public TrieNode(TKey wildCard = default)
            {
                WildCard = wildCard;
            }
            
            public void Add(TValue entity, TKey[] keySequence, int currentIndex)
            {
                var currentKey = keySequence[currentIndex];
                Key = currentKey;
                
                // If this is the last part of the keys, place the entity at this node
                if (currentIndex == keySequence.Length - 1)
                {
                    Value = entity;
                    return;
                }

                var nextKey = new NullWrapper<TKey>(keySequence[currentIndex + 1]);
                
                TrieNode<TKey, TValue> nextNode;
                var hasMatchingChildForPrefix = Children.TryGetValue(nextKey, out nextNode);

                if (!hasMatchingChildForPrefix)
                {
                    nextNode = new TrieNode<TKey, TValue>()
                    {
                        Key = nextKey.Value
                    };
                    Children.Add(nextKey, nextNode);
                }   

                nextNode.Add(entity, keySequence, currentIndex+1);
            }
            
            public (TValue value, int foundAtIndex) GetLongestMatch(TKey[] remainingPrefixElements, int currentIndex)
            {
                var isLastPrefixElement = currentIndex == remainingPrefixElements.Length - 1;
                var currentKey = remainingPrefixElements[currentIndex];
                var currentKeyMatchesNodeKey = currentKey.Equals(Key) || currentKey.Equals(WildCard) || Key.Equals(WildCard);
                
                // If this is the last element of the prefix, we'll return whatever value is found here - if the keys match
                if (isLastPrefixElement)
                    return currentKeyMatchesNodeKey ? (Value, currentIndex) : default;

                // Otherwise, we'll look further into the trie for the longest match we can find
                var nextKey = remainingPrefixElements[currentIndex + 1];
                
                // A list of all the tries that should be matched against
                var triesToMatch = new List<TrieNode<TKey, TValue>>();

                // Add any node that matches the next key
                Children.TryGetValue(new NullWrapper<TKey>(nextKey), out var nextTrieWithMatchingKey);
                if(nextTrieWithMatchingKey != null)
                    triesToMatch.Add(nextTrieWithMatchingKey);
                
                // Add any wild card that is an immediate descendant of this node
                Children.TryGetValue(new NullWrapper<TKey>(WildCard), out var nextWildcardTrie);
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