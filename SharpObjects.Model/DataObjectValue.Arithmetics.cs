using System;

namespace SharpObjects.Model
{
	public partial struct DataObjectValue
	{
		public DataObjectValue Negate()
		{
			switch (_type)
			{
				case DataObjectValueType.Boolean:
				case DataObjectValueType.BooleanString:
					return new DataObjectValue(!_booleanValue);

				case DataObjectValueType.Integer:
				case DataObjectValueType.IntegerString:
					return new DataObjectValue(-_intValue);

				case DataObjectValueType.Float:
				case DataObjectValueType.FloatString:
					return new DataObjectValue(-_singleValue);

				case DataObjectValueType.Double:
				case DataObjectValueType.DoubleString:
					return new DataObjectValue(-_doubleValue);

				default:
					return this;
			}
		}

		public DataObjectValue Add(DataObjectValue other)
		{
			if (_type == DataObjectValueType.None)
				return other;

			if (other._type == DataObjectValueType.None)
				return this;

			if (_type.HasFlagFast(DataObjectValueType.Boolean))
				return other._type.HasFlagFast(DataObjectValueType.Boolean)
					? new DataObjectValue(_booleanValue | other._booleanValue)
					: other;

			if (other._type.HasFlagFast(DataObjectValueType.Boolean))
				return this;

			if (_type == DataObjectValueType.String)
				return new DataObjectValue((String)_referenceTypeValue + (String)other);

			if (other._type == DataObjectValueType.String)
				return new DataObjectValue((String)this + (String)other._referenceTypeValue);

			switch (_type)
			{
				case DataObjectValueType.Integer:
				case DataObjectValueType.IntegerString:
					{
						if (other._type.HasFlagFast(DataObjectValueType.Integer))
							return new DataObjectValue(_intValue + other._intValue);

						if (other._type.HasFlagFast(DataObjectValueType.Float))
							return new DataObjectValue(_intValue + other._singleValue);

						if (other._type.HasFlagFast(DataObjectValueType.Double))
							return new DataObjectValue(_intValue + other._doubleValue);
					}
					break;

				case DataObjectValueType.Float:
				case DataObjectValueType.FloatString:
					{
						if (other._type.HasFlagFast(DataObjectValueType.Float))
							return new DataObjectValue(_singleValue + other._singleValue);

						if (other._type.HasFlagFast(DataObjectValueType.Double))
							return new DataObjectValue(_singleValue + other._doubleValue);

						if (other._type.HasFlagFast(DataObjectValueType.Integer))
							return new DataObjectValue(_singleValue + other._intValue);
					}
					break;

				case DataObjectValueType.Double:
				case DataObjectValueType.DoubleString:
					{
						if (other._type.HasFlagFast(DataObjectValueType.Double))
							return new DataObjectValue(_doubleValue + other._doubleValue);

						if (other._type.HasFlagFast(DataObjectValueType.Float))
							return new DataObjectValue(_doubleValue + other._singleValue);

						if (other._type.HasFlagFast(DataObjectValueType.Integer))
							return new DataObjectValue(_doubleValue + other._intValue);
					}
					break;

			}

			return this;
		}

		public DataObjectValue Substact(DataObjectValue other)
		{
			if (_type == DataObjectValueType.None)
				return Nothing;

			if (other._type == DataObjectValueType.None)
				return this;

			if (_type.HasFlagFast(DataObjectValueType.Boolean))
			{                                                                               // false - false	=	false
				return other._type.HasFlagFast(DataObjectValueType.Boolean)                 // false - true		=	false
					? new DataObjectValue(_booleanValue && !other._booleanValue)            // true  - false	=	true
					: other;                                                                // true  - true		=	false
			}

			if (other._type.HasFlagFast(DataObjectValueType.Boolean))
				return this;

			switch (_type)
			{
				case DataObjectValueType.Integer:
				case DataObjectValueType.IntegerString:
					{
						if (other._type.HasFlagFast(DataObjectValueType.Integer))
							return new DataObjectValue(_intValue - other._intValue);

						if (other._type.HasFlagFast(DataObjectValueType.Float))
							return new DataObjectValue(_intValue - other._singleValue);

						if (other._type.HasFlagFast(DataObjectValueType.Double))
							return new DataObjectValue(_intValue - other._doubleValue);
					}
					break;

				case DataObjectValueType.Float:
				case DataObjectValueType.FloatString:
					{
						if (other._type.HasFlagFast(DataObjectValueType.Float))
							return new DataObjectValue(_singleValue - other._singleValue);

						if (other._type.HasFlagFast(DataObjectValueType.Double))
							return new DataObjectValue(_singleValue - other._doubleValue);

						if (other._type.HasFlagFast(DataObjectValueType.Integer))
							return new DataObjectValue(_singleValue - other._intValue);
					}
					break;

				case DataObjectValueType.Double:
				case DataObjectValueType.DoubleString:
					{
						if (other._type.HasFlagFast(DataObjectValueType.Double))
							return new DataObjectValue(_doubleValue - other._doubleValue);

						if (other._type.HasFlagFast(DataObjectValueType.Float))
							return new DataObjectValue(_doubleValue - other._singleValue);

						if (other._type.HasFlagFast(DataObjectValueType.Integer))
							return new DataObjectValue(_doubleValue - other._intValue);
					}
					break;

			}

			return Equals(other)
				? Zero
				: this;
		}

		#region Unary operators

		public static DataObjectValue operator -(DataObjectValue dataObjectValue)
		{
			return dataObjectValue.Negate();
		}

		#endregion

		#region Binary opetators

		public static DataObjectValue operator +(DataObjectValue summand1, DataObjectValue summand2)
		{
			return summand1.Add(summand2);
		}

		public static DataObjectValue operator -(DataObjectValue minuend, DataObjectValue subtrahend)
		{
			return minuend.Substact(subtrahend);
		}

		#endregion
	}
}