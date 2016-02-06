using System;
using System.Diagnostics.CodeAnalysis;

namespace Kappa.Core.System
{
	public partial struct DataObjectValue : IEquatable<DataObjectValue>
	{
		#region Equality Methods

		[SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
		public override Int32 GetHashCode()
		{
			unchecked
			{
				var hashCode = (Int32)_type;
				if (_type == DataObjectValueType.None)
					return hashCode;

				if (_type.HasFlagFast(DataObjectValueType.Boolean))
					return _booleanValue.GetHashCode();

				if (_type.HasFlagFast(DataObjectValueType.Integer))
					return _intValue.GetHashCode();

				if (_type.HasFlagFast(DataObjectValueType.Float))
				{
					var floorValue = (Int32)_singleValue;
					return _singleValue - floorValue == 0
						? floorValue.GetHashCode()
						: _singleValue.GetHashCode();
				}

				if (_type.HasFlagFast(DataObjectValueType.Double))
				{
					var floorValue = (Int32)_doubleValue;
					return _doubleValue - floorValue == 0
						? floorValue.GetHashCode()
						: _doubleValue.GetHashCode();
				}

				if (_type == DataObjectValueType.String)
					return String.IsNullOrEmpty((String)_referenceTypeValue)
						? 0
						: _referenceTypeValue.GetHashCode();

				if (_type == DataObjectValueType.Object)
					return _referenceTypeValue?.GetHashCode() ?? 0;

				throw new InvalidOperationException("Cannot perform 'Get Hash Code' operation. Unknown value type");
			}
		}

		public override Boolean Equals(Object other)
		{
			if (other is DataObjectValue)
				return Equals((DataObjectValue)other, typeConsistencyCheck: false);

			if (other is Boolean)
				return Equals(new DataObjectValue((Boolean)other), typeConsistencyCheck: false);

			if (other is Int32)
				return Equals(new DataObjectValue((Int32)other), typeConsistencyCheck: false);

			if (other is Single)
				return Equals(new DataObjectValue((Single)other), typeConsistencyCheck: false);

			if (other is Double)
				return Equals(new DataObjectValue((Single)other), typeConsistencyCheck: false);

			var stringObject = other as String;
			return Equals(stringObject != null ? new DataObjectValue(stringObject) : new DataObjectValue(other), typeConsistencyCheck: false);
		}

		public Boolean Equals(DataObjectValue other)
		{
			return Equals(other, typeConsistencyCheck: false);
		}

		public Boolean Equals(DataObjectValue other, Boolean typeConsistencyCheck)
		{
			if (other._type == DataObjectValueType.None)
				return _type == DataObjectValueType.None;

			switch (_type)
			{
				case DataObjectValueType.None:
					return other._type == DataObjectValueType.None;

				case DataObjectValueType.Boolean:
				case DataObjectValueType.BooleanString:
					return BooleanEqualsTo(other, typeConsistencyCheck);

				case DataObjectValueType.Integer:
				case DataObjectValueType.IntegerString:
					return IntegerEqualsTo(other, typeConsistencyCheck);

				case DataObjectValueType.Float:
				case DataObjectValueType.FloatString:
					return FloatEqualsTo(other, typeConsistencyCheck);

				case DataObjectValueType.Double:
				case DataObjectValueType.DoubleString:
					return DoubleEqualsTo(other, typeConsistencyCheck);

				case DataObjectValueType.String:
					return StringEqualsTo(other, typeConsistencyCheck);

				case DataObjectValueType.Object:
					return ObjectEqualsTo(other, typeConsistencyCheck);

				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		#region Strongly-typed equality methods

		[SuppressMessage("ReSharper", "UnusedParameter.Local")]
		private Boolean BooleanEqualsTo(DataObjectValue other, Boolean typeConsistencyCheck)
		{
			if (other._type == DataObjectValueType.Boolean)
				return _booleanValue == other._booleanValue;

			if (typeConsistencyCheck)
				throw new InvalidOperationException("Cannot compare values with different types");

			if (other._type.HasFlagFast(DataObjectValueType.Boolean))
				return _booleanValue == other._booleanValue;

			if (other._type.HasFlagFast(DataObjectValueType.Integer))
				return _booleanValue == other._intValue > 0;

			if (other._type.HasFlagFast(DataObjectValueType.Float))
				return _booleanValue == other._singleValue >= MinTrueSingle;

			if (other._type.HasFlagFast(DataObjectValueType.Double))
				return _booleanValue == other._doubleValue >= MinTrueDouble;

			if (other._type.HasFlagFast(DataObjectValueType.String))
				return !_booleanValue == String.IsNullOrEmpty((String)other._referenceTypeValue);

			return _booleanValue && other.HasValue;
		}

		[SuppressMessage("ReSharper", "UnusedParameter.Local")]
		private Boolean IntegerEqualsTo(DataObjectValue other, Boolean typeConsistencyCheck)
		{
			if (other._type == DataObjectValueType.Integer)
				return _intValue == other._intValue;

			if (typeConsistencyCheck)
				throw new InvalidOperationException("Cannot compare values with different types");

			if (other._type.HasFlagFast(DataObjectValueType.Integer))
				return _intValue == other._intValue;

			if (other._type.HasFlagFast(DataObjectValueType.Boolean))
				return _intValue > 0 == other._booleanValue;

			if (other._type.HasFlagFast(DataObjectValueType.Float))
			{
				// ReSharper disable once RedundantCast
				// ReSharper disable once CompareOfFloatsByEqualityOperator
				return ((Single)_intValue) == other._singleValue;
			}

			if (other._type.HasFlagFast(DataObjectValueType.Double))
			{
				// ReSharper disable once RedundantCast
				// ReSharper disable once CompareOfFloatsByEqualityOperator
				return ((Double)_intValue) == other._doubleValue;
			}

			if (other._type.HasFlagFast(DataObjectValueType.String))
			{
				var stringValue = (String)other._referenceTypeValue;
				if (String.IsNullOrEmpty(stringValue))
					return _intValue < MinTrueInt;

				return _intValue == stringValue.Length;
			}

			if (other._type.HasFlagFast(DataObjectValueType.Object) && other._referenceTypeValue == null)
				return _intValue == DefaultFalseInt;

			return false;
		}

		[SuppressMessage("ReSharper", "UnusedParameter.Local")]
		[SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
		private Boolean FloatEqualsTo(DataObjectValue other, Boolean typeConsistencyCheck)
		{
			if (other._type == DataObjectValueType.Float)
				return _singleValue == other._singleValue;

			if (typeConsistencyCheck)
				throw new InvalidOperationException("Cannot compare values with different types");

			if (other._type.HasFlagFast(DataObjectValueType.Float))
				return _singleValue == other._singleValue;

			if (other._type.HasFlagFast(DataObjectValueType.Double))
				return _singleValue == other._doubleValue;

			if (other._type.HasFlagFast(DataObjectValueType.Boolean))
				return _singleValue >= MinTrueSingle == other._booleanValue;

			if (other._type.HasFlagFast(DataObjectValueType.Integer))
				// ReSharper disable once RedundantCast
				return _singleValue == (Single)other._intValue;

			if (other._type.HasFlagFast(DataObjectValueType.String))
			{
				var stringValue = (String)other._referenceTypeValue;
				if (String.IsNullOrEmpty(stringValue))
					return _singleValue < MinTrueSingle;

				return _singleValue == stringValue.Length;
			}

			if (other._type.HasFlagFast(DataObjectValueType.Object) && other._referenceTypeValue == null)
				return _singleValue == DefaultFalseSingle;

			return false;
		}

		[SuppressMessage("ReSharper", "UnusedParameter.Local")]
		[SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
		private Boolean DoubleEqualsTo(DataObjectValue other, Boolean typeConsistencyCheck)
		{
			if (other._type == DataObjectValueType.Double)
				return _doubleValue == other._doubleValue;

			if (typeConsistencyCheck)
				throw new InvalidOperationException("Cannot compare values with different types");

			if (other._type.HasFlagFast(DataObjectValueType.Float))
				return _doubleValue == other._singleValue;

			if (other._type.HasFlagFast(DataObjectValueType.Double))
				return _doubleValue == other._doubleValue;

			if (other._type.HasFlagFast(DataObjectValueType.Boolean))
				return _doubleValue >= MinTrueDouble == other._booleanValue;

			if (other._type.HasFlagFast(DataObjectValueType.Integer))
				// ReSharper disable once RedundantCast
				return _doubleValue == (Double)other._intValue;

			if (other._type.HasFlagFast(DataObjectValueType.String))
			{
				var stringValue = (String)other._referenceTypeValue;
				if (String.IsNullOrEmpty(stringValue))
					return _doubleValue < MinTrueDouble;

				return _doubleValue == stringValue.Length;
			}

			if (other._type.HasFlagFast(DataObjectValueType.Object) && other._referenceTypeValue == null)
				return _doubleValue == DefaultFalseDouble;

			return false;
		}

		[SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
		private Boolean StringEqualsTo(DataObjectValue other, Boolean typeConsistencyCheck)
		{
			{
				if (other._type == DataObjectValueType.String)
					return String.Equals((String)_referenceTypeValue, (String)other._referenceTypeValue, StringComparison.Ordinal);

				if (typeConsistencyCheck)
					throw new InvalidOperationException("Cannot compare values with different types");

				if (other._type.HasFlagFast(DataObjectValueType.ValueType))
				{
					if (other._type.HasFlagFast(DataObjectValueType.Boolean))
						return other._booleanValue == ComparisonIndex >= MinTrueInt;

					if (other._type.HasFlagFast(DataObjectValueType.Integer))
						return other._intValue == ComparisonIndex;

					if (other._type.HasFlagFast(DataObjectValueType.Float))
						return other._singleValue == ComparisonIndex;

					if (other._type.HasFlagFast(DataObjectValueType.Double))
						return other._doubleValue == ComparisonIndex;
				}

				if (other._type == DataObjectValueType.Object)
					return other._referenceTypeValue == _referenceTypeValue; // reference equals

				return false;
			}
		}

		private Boolean ObjectEqualsTo(DataObjectValue other, Boolean typeConsistencyCheck)
		{
			{
				if (other._type == DataObjectValueType.Object)
					return _referenceTypeValue == other._referenceTypeValue;

				if (typeConsistencyCheck)
					throw new InvalidOperationException("Cannot compare values with different types");

				if (other._type == DataObjectValueType.Boolean)
					return (_referenceTypeValue != null) == other._booleanValue;

				if (other._type == DataObjectValueType.String)
					return _referenceTypeValue == other._referenceTypeValue; // reference equals

				return false;
			}
		}

		#endregion

		#endregion

		#region Equality Operators

		public static Boolean operator ==(DataObjectValue left, DataObjectValue right)
		{
			return left.Equals(right);
		}

		public static Boolean operator !=(DataObjectValue left, DataObjectValue right)
		{
			return !left.Equals(right);
		}

		#endregion
	}
}