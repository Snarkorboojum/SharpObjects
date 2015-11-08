using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace SharpObjects.Model
{
	[PublicAPI]
	public class DataObject : IDictionary<String, DataObjectValue>
	{
		private readonly Dictionary<String, DataObjectValue> _valueStore;

		[PublicAPI]
		public DataObject()
		{
			_valueStore = new Dictionary<String, DataObjectValue>();
		}

		[PublicAPI]
		public DataObjectValue GetPropertyValue(String propertyName)
		{
			if (propertyName == null)
				throw new ArgumentNullException(nameof(propertyName));

			DataObjectValue result;
			return _valueStore.TryGetValue(propertyName, out result)
				? result
				: DataObjectValue.Nothing;
		}

		[PublicAPI]
		public void SetPropertyValue(String propertyName, DataObjectValue propertyValue)
		{
			if (propertyName == null)
				throw new ArgumentNullException(nameof(propertyName));

			_valueStore[propertyValue] = propertyValue;
		}

		[PublicAPI]
		public Dictionary<String, DataObjectValue>.Enumerator GetEnumerator()
		{
			return _valueStore.GetEnumerator();
		}

		#region IDictionary<String, DataObjectValue> implementation 

		public DataObjectValue this[String property]
		{
			get { return _valueStore[property]; }
			set { _valueStore[property] = value; }
		}

		ICollection<String> IDictionary<String, DataObjectValue>.Keys => _valueStore.Keys;

		ICollection<DataObjectValue> IDictionary<String, DataObjectValue>.Values => _valueStore.Values;

		Boolean IDictionary<String, DataObjectValue>.ContainsKey(String key)
		{
			return _valueStore.ContainsKey(key);
		}

		void IDictionary<String, DataObjectValue>.Add(String key, DataObjectValue value)
		{
			_valueStore.Add(key, value);
		}

		Boolean IDictionary<String, DataObjectValue>.Remove(String key)
		{
			return _valueStore.Remove(key);
		}

		Boolean IDictionary<String, DataObjectValue>.TryGetValue(String key, out DataObjectValue value)
		{
			return _valueStore.TryGetValue(key, out value);
		}

		#endregion

		#region IEnumerator<KeyValuePair<String, DataObjectValue>> implementation

		IEnumerator<KeyValuePair<String, DataObjectValue>> IEnumerable<KeyValuePair<String, DataObjectValue>>.GetEnumerator()
		{
			return _valueStore.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)_valueStore).GetEnumerator();
		}

		#endregion

		#region ICollection<KeyValuePair<String, DataObjectValue>> implementation

		Int32 ICollection<KeyValuePair<String, DataObjectValue>>.Count => _valueStore.Count;

		Boolean ICollection<KeyValuePair<String, DataObjectValue>>.IsReadOnly => ((ICollection<KeyValuePair<String, DataObjectValue>>)_valueStore).IsReadOnly;

		void ICollection<KeyValuePair<String, DataObjectValue>>.Add(KeyValuePair<String, DataObjectValue> item)
		{
			((ICollection<KeyValuePair<String, DataObjectValue>>)_valueStore).Add(item);
		}

		void ICollection<KeyValuePair<String, DataObjectValue>>.Clear()
		{
			_valueStore.Clear();
		}

		Boolean ICollection<KeyValuePair<String, DataObjectValue>>.Contains(KeyValuePair<String, DataObjectValue> item)
		{
			return ((ICollection<KeyValuePair<String, DataObjectValue>>)_valueStore).Contains(item);
		}

		void ICollection<KeyValuePair<String, DataObjectValue>>.CopyTo(KeyValuePair<String, DataObjectValue>[] array, Int32 arrayIndex)
		{
			((ICollection<KeyValuePair<String, DataObjectValue>>)_valueStore).CopyTo(array, arrayIndex);
		}

		Boolean ICollection<KeyValuePair<String, DataObjectValue>>.Remove(KeyValuePair<String, DataObjectValue> item)
		{
			return ((ICollection<KeyValuePair<String, DataObjectValue>>)_valueStore).Remove(item);
		}

		#endregion
	}
}