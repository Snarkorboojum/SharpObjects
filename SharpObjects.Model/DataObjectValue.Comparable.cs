using System;
using System.Collections.Generic;
using System.Runtime.Remoting;

namespace SharpObjects.Model
{
	public partial struct DataObjectValue : IComparable<DataObjectValue>
	{
		public Int32 CompareTo(DataObjectValue other)
		{
			const Int32 equals = 0;
			const Int32 greaterThan = 1;
			const Int32 lessThan = -1;

			if (_type == DataObjectValueType.None)
				return other._type == DataObjectValueType.None ? equals : lessThan;

			if (other._type == DataObjectValueType.None)
				return greaterThan;

			if (_type.HasFlag(DataObjectValueType.Boolean))
			{
				var otherAsBoolean = (Boolean)other;

				return _booleanValue
					? (otherAsBoolean ? @equals : greaterThan)
					: (otherAsBoolean ? lessThan : @equals);
			}

			if (_type.HasFlag(DataObjectValueType.Integer))
			{
				if (other._type.HasFlag(DataObjectValueType.Boolean))
					return -1 * other.CompareTo(this);

				if (other._type.HasFlag(DataObjectValueType.Float))
					return ((Single)_intValue).CompareTo(other._singleValue);

				var otherAsInteger = CastToInteger(other);
				return !otherAsInteger.HasValue
					? greaterThan
					: _intValue.CompareTo(otherAsInteger.Value);
			}

			if (_type.HasFlag(DataObjectValueType.Float))
			{
				if (other._type.HasFlag(DataObjectValueType.Boolean))
					return -1 * other.CompareTo(this);

				var otherAsFloat = CastToFloat(other);
				return !otherAsFloat.HasValue
					? greaterThan
					: _singleValue.CompareTo(otherAsFloat.Value);
			}

			if (_type.HasFlag(DataObjectValueType.String))
			{
				if (other._type.HasFlag(DataObjectValueType.Boolean))
				{
					if (other._booleanValue)
						return String.IsNullOrEmpty((String)_referenceTypeValue) ? lessThan : equals;
					else
						return String.IsNullOrEmpty((String)_referenceTypeValue) ? equals : greaterThan;
				}

				var otherAsString = (String)other;
				var comparison = String.Compare(((String)_referenceTypeValue), otherAsString, StringComparison.Ordinal);

				if (comparison > 0)
					return greaterThan;

				if (comparison < 0)
					return lessThan;

				return equals;
			}

			throw new InvalidOperationException($"Cannot compare '{this}' with '{other}'");
		}

		#region Comparison Operators

		public static Boolean operator >(DataObjectValue current, DataObjectValue other)
		{
			return current.CompareTo(other) > 0;
		}

		public static Boolean operator <(DataObjectValue current, DataObjectValue other)
		{
			return current.CompareTo(other) < 0;
		}

		public static Boolean operator >=(DataObjectValue current, DataObjectValue other)
		{
			return current.CompareTo(other) >= 0;
		}

		public static Boolean operator <=(DataObjectValue current, DataObjectValue other)
		{
			return current.CompareTo(other) <= 0;
		}

		#endregion
	}
}