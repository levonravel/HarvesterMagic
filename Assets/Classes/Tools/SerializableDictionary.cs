using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
/*
 * Code was provided from https://wiki.unity3d.com/index.php/SerializableDictionary
*/
namespace ShinyBoxInteractive
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : SerializableDictionary, ISerializationCallbackReceiver, IDictionary<TKey, TValue>
    {
        [SerializeField]
        public List<SerializableKeyValuePair> list = new List<SerializableKeyValuePair>();

        [Serializable]
        public struct SerializableKeyValuePair
        {
            public TKey Key;
            public TValue Value;

            public SerializableKeyValuePair(TKey key, TValue value)
            {
                Key = key;
                Value = value;
            }

            public void SetValue(TValue value)
            {
                Value = value;
            }
        }
        private Dictionary<TKey, uint> KeyPositions => _keyPositions.Value;
        private Lazy<Dictionary<TKey, uint>> _keyPositions;

        public SerializableDictionary()
        {
            _keyPositions = new Lazy<Dictionary<TKey, uint>>(MakeKeyPositions);
        }

        private Dictionary<TKey, uint> MakeKeyPositions()
        {
            var numEntries = list.Count;
            var result = new Dictionary<TKey, uint>(numEntries);
            for (int i = 0; i < numEntries; i++)
                result[list[i].Key] = (uint)i;
            return result;
        }

        public void OnBeforeSerialize() { }
        public void OnAfterDeserialize()
        {
            _keyPositions = new Lazy<Dictionary<TKey, uint>>(MakeKeyPositions);
        }
        public TValue this[TKey key]
        {
            get => list[(int)KeyPositions[key]].Value;
            set
            {
                if (KeyPositions.TryGetValue(key, out uint index))
                    list[(int)index].SetValue(value);
                else
                {
                    KeyPositions[key] = (uint)list.Count;
                    list.Add(new SerializableKeyValuePair(key, value));
                }
            }
        }
        public ICollection<TKey> Keys => list.Select(tuple => tuple.Key).ToArray();
        public ICollection<TValue> Values => list.Select(tuple => tuple.Value).ToArray();
        public void Add(TKey key, TValue value)
        {
            if (KeyPositions.ContainsKey(key))
                throw new ArgumentException("An element with the same key already exists in the dictionary.");
            else
            {
                KeyPositions[key] = (uint)list.Count;
                list.Add(new SerializableKeyValuePair(key, value));
            }
        }
        public bool ContainsKey(TKey key) => KeyPositions.ContainsKey(key);
        public bool Remove(TKey key)
        {
            if (KeyPositions.TryGetValue(key, out uint index))
            {
                var kp = KeyPositions;
                kp.Remove(key);

                var numEntries = list.Count;

                list.RemoveAt((int)index);
                for (uint i = index; i < numEntries; i++)
                    kp[list[(int)i].Key] = i;

                return true;
            }
            else
                return false;
        }
        /// <summary>
        /// Created a Key indexer Levon Ravel
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public TKey GetKeyAtIndex(int index)
        {
            return list[index].Key;
        }
        /// <summary>
        /// Created a Value indexer Levon Ravel
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public TValue GetValueAtIndex(int index)
        {
            return list[index].Value;
        }
        public bool TryGetValue(TKey key, out TValue value)
        {
            if (KeyPositions.TryGetValue(key, out uint index))
            {
                value = list[(int)index].Value;
                return true;
            }
            else
            {
                value = default;
                return false;
            }
        }
        public int Count => list.Count;
        public bool IsReadOnly => false;
        public void Add(KeyValuePair<TKey, TValue> kvp) => Add(kvp.Key, kvp.Value);
        public void Clear() => list.Clear();
        public bool Contains(KeyValuePair<TKey, TValue> kvp) => KeyPositions.ContainsKey(kvp.Key);
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            var numKeys = list.Count;
            if (array.Length - arrayIndex < numKeys)
                throw new ArgumentException("arrayIndex");
            for (int i = 0; i < numKeys; i++, arrayIndex++)
            {
                var entry = list[i];
                array[arrayIndex] = new KeyValuePair<TKey, TValue>(entry.Key, entry.Value);
            }
        }
        public bool Remove(KeyValuePair<TKey, TValue> kvp) => Remove(kvp.Key);
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return list.Select(ToKeyValuePair).GetEnumerator();
            KeyValuePair<TKey, TValue> ToKeyValuePair(SerializableKeyValuePair skvp)
            {
                return new KeyValuePair<TKey, TValue>(skvp.Key, skvp.Value);
            }
        }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    public class SerializableDictionary { }
}