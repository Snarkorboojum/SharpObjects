using System;
using System.Globalization;

namespace Kappa.Core.System
{
	public partial struct DataObjectValue
	{
		#region Constants

		private const Int32 MinTrueInt = 1;

		private const Int32 DefaultFalseInt = 0;

		private const Single MinTrueSingle = 1.0f;

		private const Single DefaultFalseSingle = 0.0f;

		private const Double MinTrueDouble = 1.0d;

		private const Double DefaultFalseDouble = 0.0d;

		#endregion

		#region Conversion operators to DataObjectValue

		public static implicit operator DataObjectValue(Boolean value)
		{
			return new DataObjectValue(value);
		}

		public static implicit operator DataObjectValue(Int32 value)
		{
			return new DataObjectValue(value);
		}

		public static implicit operator DataObjectValue(Single value)
		{
			return new DataObjectValue(value);
		}

		public static implicit operator DataObjectValue(Double value)
		{
			return new DataObjectValue(value);
		}

		public static implicit operator DataObjectValue(String value)
		{
			return new DataObjectValue(value);
		}

		#endregion

		#region Conversion operators from DataObjectValue 

		public static implicit operator Boolean(DataObjectValue dataObjectValue)
		{
			if (dataObjectValue._type.HasFlagFast(DataObjectValueType.Boolean))
				return dataObjectValue._booleanValue;

			if (dataObjectValue._type.HasFlagFast(DataObjectValueType.Integer))
				return dataObjectValue._intValue >= MinTrueInt;

			if (dataObjectValue._type.HasFlagFast(DataObjectValueType.Float))
				return dataObjectValue._singleValue >= MinTrueSingle;

			if (dataObjectValue._type.HasFlagFast(DataObjectValueType.Double))
				return dataObjectValue._singleValue >= MinTrueDouble;

			if (dataObjectValue._type.HasFlagFast(DataObjectValueType.String))
				return !String.IsNullOrEmpty((String)dataObjectValue._referenceTypeValue);

			if (dataObjectValue._type.HasFlagFast(DataObjectValueType.Object))
				return dataObjectValue._referenceTypeValue != null;

			if (dataObjectValue._type == DataObjectValueType.None)
				return default(Boolean);

			throw new InvalidCastException($"Cannot cast '{nameof(DataObjectValue)}' with '{dataObjectValue.ToString()}' value to '{nameof(Boolean)}'");
		}

		public static implicit operator Int32(DataObjectValue dataObjectValue)
		{
			var castedValue = CastToInteger(dataObjectValue);
			if (castedValue.HasValue)
				return castedValue.Value;

			throw new InvalidCastException($"Cannot cast '{nameof(DataObjectValue)}' with '{dataObjectValue.ToString()}' value to '{nameof(Int32)}'");
		}

		public static implicit operator Single(DataObjectValue dataObjectValue)
		{
			var castedValue = CastToFloat(dataObjectValue);
			if (castedValue.HasValue)
				return castedValue.Value;

			throw new InvalidCastException($"Cannot cast '{nameof(DataObjectValue)}' with '{dataObjectValue.ToString()}' value to '{nameof(Single)}'");
		}

		public static implicit operator Double(DataObjectValue dataObjectValue)
		{
			var castedValue = CastToDouble(dataObjectValue);
			if (castedValue.HasValue)
				return castedValue.Value;

			throw new InvalidCastException($"Cannot cast '{nameof(DataObjectValue)}' with '{dataObjectValue.ToString()}' value to '{nameof(Double)}'");
		}

		public static implicit operator String(DataObjectValue dataObjectValue)
		{
			if (dataObjectValue._type.HasFlagFast(DataObjectValueType.String))
				return (String)dataObjectValue._referenceTypeValue;

			if (dataObjectValue._type.HasFlagFast(DataObjectValueType.Boolean))
				return dataObjectValue._booleanValue ? Boolean.TrueString : Boolean.FalseString;

			if (dataObjectValue._type.HasFlagFast(DataObjectValueType.Integer))
				return dataObjectValue._intValue.ToString();

			if (dataObjectValue._type.HasFlagFast(DataObjectValueType.Float))
				return dataObjectValue._singleValue.ToString(CultureInfo.InvariantCulture);

			if (dataObjectValue._type == DataObjectValueType.None)
				return default(String);

			throw new InvalidCastException($"Cannot cast '{nameof(DataObjectValue)}' with '{dataObjectValue.ToString()}' value to '{nameof(String)}'");
		}

		#endregion

		private static Int32? CastToInteger(DataObjectValue dataObjectValue)
		{
			if (dataObjectValue._type.HasFlagFast(DataObjectValueType.Integer))
				return dataObjectValue._intValue;

			if (dataObjectValue._type.HasFlagFast(DataObjectValueType.Boolean))
				return dataObjectValue._booleanValue ? MinTrueInt : DefaultFalseInt;

			if (dataObjectValue._type.HasFlagFast(DataObjectValueType.Float))
				return (Int32)dataObjectValue._singleValue;

			if (dataObjectValue._type.HasFlagFast(DataObjectValueType.Double))
				return (Int32)dataObjectValue._doubleValue;

			if (dataObjectValue._type.HasFlagFast(DataObjectValueType.String))
			{
				var stringValue = (String)dataObjectValue._referenceTypeValue;
				return String.IsNullOrEmpty(stringValue)
					? default(Int32)
					: stringValue.Length;
			}

			if (dataObjectValue._type.HasFlagFast(DataObjectValueType.Object) && dataObjectValue._referenceTypeValue == null)
				return default(Int32);

			if (dataObjectValue._type == DataObjectValueType.None)
				return default(Int32);

			return null;
		}

		private static Single? CastToFloat(DataObjectValue dataObjectValue)
		{
			if (dataObjectValue._type.HasFlagFast(DataObjectValueType.Float))
				return dataObjectValue._singleValue;

			if (dataObjectValue._type.HasFlagFast(DataObjectValueType.Double))
				return (Single)dataObjectValue._doubleValue;

			if (dataObjectValue._type.HasFlagFast(DataObjectValueType.Boolean))
				return dataObjectValue._booleanValue ? MinTrueSingle : DefaultFalseSingle;

			if (dataObjectValue._type.HasFlagFast(DataObjectValueType.Integer))
				return dataObjectValue._intValue;

			if (dataObjectValue._type.HasFlagFast(DataObjectValueType.String))
			{
				var stringValue = (String)dataObjectValue._referenceTypeValue;
				return String.IsNullOrEmpty(stringValue)
					? default(Single)
					: stringValue.Length;
			}

			if (dataObjectValue._type.HasFlagFast(DataObjectValueType.Object) && dataObjectValue._referenceTypeValue == null)
				return default(Single);

			if (dataObjectValue._type == DataObjectValueType.None)
				return default(Single);

			return null;
		}

		private static Double? CastToDouble(DataObjectValue dataObjectValue)
		{
			if (dataObjectValue._type.HasFlagFast(DataObjectValueType.Float))
				return dataObjectValue._singleValue;

			if (dataObjectValue._type.HasFlagFast(DataObjectValueType.Double))
				return dataObjectValue._doubleValue;

			if (dataObjectValue._type.HasFlagFast(DataObjectValueType.Boolean))
				return dataObjectValue._booleanValue ? MinTrueDouble : DefaultFalseDouble;

			if (dataObjectValue._type.HasFlagFast(DataObjectValueType.Integer))
				return dataObjectValue._intValue;

			if (dataObjectValue._type.HasFlagFast(DataObjectValueType.String))
			{
				var stringValue = (String)dataObjectValue._referenceTypeValue;
				return String.IsNullOrEmpty(stringValue)
					? default(Double)
					: stringValue.Length;
			}

			if (dataObjectValue._type.HasFlagFast(DataObjectValueType.Object) && dataObjectValue._referenceTypeValue == null)
				return default(Double);

			if (dataObjectValue._type == DataObjectValueType.None)
				return default(Double);

			return null;
		}
	}
}