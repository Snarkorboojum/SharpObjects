using System;
using System.Diagnostics.CodeAnalysis;

namespace SharpObjects.Model
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
					var singleRounded = (Int32)_singleValue;
					return _singleValue - singleRounded == 0
						? singleRounded.GetHashCode()
						: _singleValue.GetHashCode();
				}

				if (_type == DataObjectValueType.String || _type == DataObjectValueType.Object)
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
				return _booleanValue == other._singleValue > 0;

			if (other._type == DataObjectValueType.String)
				return false;

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

			if (other._type == DataObjectValueType.Boolean)
				return _singleValue > 0 == other._booleanValue;

			if (other._type.HasFlagFast(DataObjectValueType.Integer))
				// ReSharper disable once RedundantCast
				return _singleValue == (Single)other._intValue;

			return false;
		}

		private Boolean StringEqualsTo(DataObjectValue other, Boolean typeConsistencyCheck)
		{
			{
				if (other._type == DataObjectValueType.String)
					return String.Equals((String)_referenceTypeValue, (String)other._referenceTypeValue, StringComparison.Ordinal);

				if (typeConsistencyCheck)
					throw new InvalidOperationException("Cannot compare values with different types");

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