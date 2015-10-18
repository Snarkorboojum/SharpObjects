using System;
using System.Collections.Generic;

namespace SharpObjects.Model
{
	public class DataObject : Dictionary<String, DataObjectValue>
	{
		public DataObjectValue GetPropertyValue(String propertyName)
		{
			if (propertyName == null)
				throw new ArgumentNullException(nameof(propertyName));

			DataObjectValue result;
			return TryGetValue(propertyName, out result)
				? result
				: default(DataObjectValue);
		}

		public void SetPropertyValue(String propertyName, DataObjectValue propertyValue)
		{
			if (propertyName == null)
				throw new ArgumentNullException(nameof(propertyName));

			this[propertyValue] = propertyValue;
		}
	}
}