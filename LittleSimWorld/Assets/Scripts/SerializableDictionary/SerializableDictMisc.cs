using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerializableDictMisc : MonoBehaviour
{
    [Serializable]
    public abstract class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        protected abstract List<SerializableKeyValuePair<TKey, TValue>> _keyValuePairs { get; set; }

        // save the dictionary to lists
        public void OnBeforeSerialize()
        {
            _keyValuePairs.Clear();
            foreach (KeyValuePair<TKey, TValue> pair in this)
            {
                _keyValuePairs.Add(new SerializableKeyValuePair<TKey, TValue>(pair.Key, pair.Value));
            }
        }

        // load dictionary from lists
        public void OnAfterDeserialize()
        {
            this.Clear();

            for (int i = 0; i < _keyValuePairs.Count; i++)
            {
                this[_keyValuePairs[i].Key] = _keyValuePairs[i].Value;
            }
        }
    }

    [Serializable]
    public class SerializableKeyValuePair<TKey, TValue> : IEquatable<SerializableKeyValuePair<TKey, TValue>>
    {
        [SerializeField]
        TKey _key;
        public TKey Key { get { return _key; } }

        [SerializeField]
        TValue _value;
        public TValue Value { get { return _value; } }

        public SerializableKeyValuePair()
        {

        }

        public SerializableKeyValuePair(TKey key, TValue value)
        {
            this._key = key;
            this._value = value;
        }

        public bool Equals(SerializableKeyValuePair<TKey, TValue> other)
        {
            var comparer1 = EqualityComparer<TKey>.Default;
            var comparer2 = EqualityComparer<TValue>.Default;

            return comparer1.Equals(_key, other._key) &&
                comparer2.Equals(_value, other._value);
        }

        public override int GetHashCode()
        {
            var comparer1 = EqualityComparer<TKey>.Default;
            var comparer2 = EqualityComparer<TValue>.Default;

            int h0;
            h0 = comparer1.GetHashCode(_key);
            h0 = (h0 << 5) + h0 ^ comparer2.GetHashCode(_value);
            return h0;
        }

        public override string ToString()
        {
            return String.Format("(Key: {0}, Value: {1})", _key, _value);
        }
    }
    
    [Serializable]
    public class StringIntTuple : SerializableKeyValuePair<string, int>
    {
        public StringIntTuple(string item1, int item2) : base(item1, item2) { }
    }

    [Serializable]
    public class StringIntDictionary : SerializableDictionary<string, int>
    {
        [SerializeField] private List<StringIntTuple> _pairs = new List<StringIntTuple>();

        protected override List<SerializableKeyValuePair<string, int>> _keyValuePairs
        {
            get
            {
                var list = new List<SerializableKeyValuePair<string, int>>();
                foreach (var pair in _pairs)
                {
                    list.Add(new SerializableKeyValuePair<string, int>(pair.Key, pair.Value));
                }
                return list;
            }

            set
            {
                _pairs.Clear();
                foreach (var kvp in value)
                {
                    _pairs.Add(new StringIntTuple(kvp.Key, kvp.Value));
                }
            }
        }
    }
    
    [Serializable]
    public class StringFloatTuple : SerializableKeyValuePair<string, float>
    {
        public StringFloatTuple(string item1, float item2) : base(item1, item2) { }
    }

    [Serializable]
    public class StringFloatDictionary : SerializableDictionary<string, float>
    {
        [SerializeField] private List<StringFloatTuple> _pairs = new List<StringFloatTuple>();

        protected override List<SerializableKeyValuePair<string, float>> _keyValuePairs
        {
            get
            {
                var list = new List<SerializableKeyValuePair<string, float>>();
                foreach (var pair in _pairs)
                {
                    list.Add(new SerializableKeyValuePair<string, float>(pair.Key, pair.Value));
                }
                return list;
            }

            set
            {
                _pairs.Clear();
                foreach (var kvp in value)
                {
                    _pairs.Add(new StringFloatTuple(kvp.Key, kvp.Value));
                }
            }
        }
    }
}
