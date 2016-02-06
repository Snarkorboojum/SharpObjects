using System;

namespace Kappa.Core.System
{
	public partial struct DataObjectValue : IComparable<DataObjectValue>
	{
		private Int32 ComparisonIndex
		{
			get
			{
				if (_type == DataObjectValueType.None)
					return 0;

				if (_type.HasFlagFast(DataObjectValueType.Boolean))
					return _booleanValue ? MinTrueInt : DefaultFalseInt;

				if (_type.HasFlagFast(DataObjectValueType.Integer))
					return _intValue;

				if (_type.HasFlagFast(DataObjectValueType.Float))
					return (Int32)_singleValue;

				if (_type == DataObjectValueType.String)
					return _intValue; // stores size

				return _referenceTypeValue?.GetHashCode() ?? 0;
			}
		}

		public Int32 CompareTo(DataObjectValue other)
		{
			const Int32 equals = 0;
			const Int32 greaterThan = 1;
			const Int32 lessThan = -1;

			if (_type == DataObjectValueType.None)
				return other._type == DataObjectValueType.None ? equals : lessThan;

			if (other._type == DataObjectValueType.None)
				return greaterThan;

			if (_type.HasFlagFast(DataObjectValueType.Boolean))
			{
				var otherAsBoolean = (Boolean)other;

				return _booleanValue
					? (otherAsBoolean ? equals : greaterThan)
					: (otherAsBoolean ? lessThan : equals);
			}

			if (_type.HasFlagFast(DataObjectValueType.Integer))
			{
				if (other._type.HasFlagFast(DataObjectValueType.Boolean))
					return -1 * other.CompareTo(this);

				if (other._type.HasFlagFast(DataObjectValueType.Float))
					return ((Single)_intValue).CompareTo(other._singleValue);

				if (other._type.HasFlagFast(DataObjectValueType.Float))
					return ((Double)_intValue).CompareTo(other._doubleValue);

				if (other._type == DataObjectValueType.String)
					return _intValue.CompareTo(other.ComparisonIndex);

				var otherAsInteger = CastToInteger(other);
				return !otherAsInteger.HasValue
					? greaterThan
					: _intValue.CompareTo(otherAsInteger.Value);
			}

			if (_type.HasFlagFast(DataObjectValueType.Float))
			{
				if (other._type.HasFlagFast(DataObjectValueType.Boolean))
					return -1 * other.CompareTo(this);

				if (other._type == DataObjectValueType.String)
					return _singleValue.CompareTo(other.ComparisonIndex);

				var otherAsFloat = CastToFloat(other);
				return !otherAsFloat.HasValue
					? greaterThan
					: _singleValue.CompareTo(otherAsFloat.Value);
			}

			if (_type.HasFlagFast(DataObjectValueType.Double))
			{
				if (other._type.HasFlagFast(DataObjectValueType.Boolean))
					return -1 * other.CompareTo(this);

				if (other._type == DataObjectValueType.String)
					return _singleValue.CompareTo(other.ComparisonIndex);

				var otherAsDouble = CastToDouble(other);
				return !otherAsDouble.HasValue
					? greaterThan
					: _doubleValue.CompareTo(otherAsDouble.Value);
			}

			if (_type.HasFlagFast(DataObjectValueType.String))
			{
				if (other._type.HasFlagFast(DataObjectValueType.Boolean))
				{
					if (other._booleanValue)
						return String.IsNullOrEmpty((String)_referenceTypeValue) ? lessThan : equals;
					else
						return String.IsNullOrEmpty((String)_referenceTypeValue) ? equals : greaterThan;
				}

				if (other._type.HasFlagFast(DataObjectValueType.Integer))
					return ComparisonIndex.CompareTo(other._intValue);

				if (other._type.HasFlagFast(DataObjectValueType.Float))
					return ComparisonIndex.CompareTo((Int32)other._singleValue);

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