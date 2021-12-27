using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HashTable
{
    public class CustomDictionary<TKey, TValue> : IDictionary<TKey, TValue>

    {
        private LinkedList<KeyValuePair<TKey, TValue>>[] _hashTable;
        public ICollection<TKey> Keys { get; }

        public ICollection<TValue> Values { get; }

        public bool IsReadOnly => false;

        public int Count { get; private set; } = 0;

        private int _capacity = 8;


        public CustomDictionary()
        {
            _hashTable = new LinkedList<KeyValuePair<TKey, TValue>>[_capacity];

            Keys = new List<TKey>();
            Values = new List<TValue>();

            for (var i = 0; i < _hashTable.Length; ++i)
            {
                _hashTable[i] = new LinkedList<KeyValuePair<TKey, TValue>>();
            }
        }

        public CustomDictionary(int size)
        {
            _hashTable = new LinkedList<KeyValuePair<TKey, TValue>>[size];

            Keys = new List<TKey>();
            Values = new List<TValue>();

            for (var i = 0; i < _hashTable.Length; ++i)
            {
                _hashTable[i] = new LinkedList<KeyValuePair<TKey, TValue>>();
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                var hash = Math.Abs(key.GetHashCode() % _hashTable.Length);

                if (ContainsKey(key))
                {
                    var result = default(TValue);

                    foreach (var item in _hashTable[hash].Where(item => key.Equals(item.Key)))
                    {
                        result = item.Value;
                    }

                    return result;
                }
                else
                {
                    throw new Exception("No item");
                }
            }

            set
            {
                var hash = Math.Abs(key.GetHashCode() % _hashTable.Length);

                if (ContainsKey(key))
                {
                    for (var cur = _hashTable[hash].First; cur != null && !cur.Equals(_hashTable[hash].Last); cur = cur.Next)
                    {
                        if (!key.Equals(cur.Value.Key)) continue;
                        var newNode = new LinkedListNode<KeyValuePair<TKey, TValue>>(
                            new KeyValuePair<TKey, TValue>(key, value));
                        cur = newNode;
                    }
                }
                else
                {
                    Add(key, value);
                }
            }
        }
        private void IncreaseHashTable()
        {
            var newHashTable =
                new LinkedList<KeyValuePair<TKey, TValue>>[_hashTable.Length * 2];

            for (var i = 0; i < _hashTable.Length; ++i)
            {
                newHashTable[i] = _hashTable[i];
            }

            _hashTable = newHashTable;
        }

        public void Add(TKey key, TValue value)
        {
            var hash = Math.Abs(key.GetHashCode() % _hashTable.Length);

            if (!ContainsKey(key))
            {
                Count++;

                if (_hashTable.Any(x => x?.Count > _hashTable.Length / 3))
                {
                    IncreaseHashTable();
                }

                var pair = new KeyValuePair<TKey, TValue>(key, value);
                _hashTable[hash].AddLast(pair);

                Keys.Add(key);
                Values.Add(value);
            }
            else
            {
                throw new Exception("This item already exists");
            }
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public bool ContainsKey(TKey key)
        {
            var hash = Math.Abs(key.GetHashCode() % _hashTable.Length);

            return _hashTable[hash].Select(item => item.Key).Any(x => x.Equals(key));
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            var hash = Math.Abs(item.Key.GetHashCode() % _hashTable.Length);
            return _hashTable[hash].Contains(item);
        }

        public bool Remove(TKey key)
        {
            var hash = Math.Abs(key.GetHashCode() % _hashTable.Length);

            if (ContainsKey(key))
            {
                Count--;

                foreach (var item in _hashTable[hash].Where(item => key.Equals(item.Key)))
                {
                    Keys.Remove(item.Key);
                    Values.Remove(item.Value);
                    _hashTable[hash].Remove(item);
                    break;
                }

                return true;
            }
            else
            {
                Console.WriteLine("Key not found!");
                return false;
            }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return Remove(item.Key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            value = default;
            var hash = Math.Abs(key.GetHashCode() % _hashTable.Length);

            if (!ContainsKey(key)) return false;
            var item = _hashTable[hash]?.FirstOrDefault(x => x.Key.Equals(key));
            if (!item.HasValue)
            {
                return false;
            }
            value = item.Value.Value;
            return true;
        }

        public void Clear()
        {
            for (var i = 0; i < _hashTable.Length; ++i)
            {
                _hashTable[i] = new LinkedList<KeyValuePair<TKey, TValue>>();
            }

            Count = 0;
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {

            var allPairs = new List<KeyValuePair<TKey, TValue>>();

            foreach (var t in _hashTable)
            {
                allPairs.AddRange(t);
            }

            var curIndex = 0;
            for (var i = arrayIndex; i < array.Length; ++i)
            {
                array[i] = allPairs[curIndex];
                curIndex++;
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _hashTable.SelectMany(t => t).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}