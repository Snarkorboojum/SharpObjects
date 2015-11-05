using System;
using System.Globalization;

namespace SharpObjects.Model
{
	public partial struct DataObjectValue
	{
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
				return dataObjectValue._intValue > 0;

			if (dataObjectValue._type.HasFlagFast(DataObjectValueType.Float))
				return dataObjectValue._singleValue > 0;

			if (dataObjectValue._type.HasFlagFast(DataObjectValueType.Object))
				return !String.IsNullOrEmpty((String)dataObjectValue._referenceTypeValue);

			if (dataObjectValue._type.HasFlagFast(DataObjectValueType.Object))
				return dataObjectValue._referenceTypeValue != null;

			if (dataObjectValue._type == DataObjectValueType.None)
				return default(Boolean);

			throw new InvalidCastException($"Cannot cast '{nameof(DataObjectValue)}' to '{nameof(Boolean)}'");
		}

		public static implicit operator Int32(DataObjectValue dataObjectValue)
		{
			var castedValue = CastToInteger(dataObjectValue);
			if (castedValue.HasValue)
				return castedValue.Value;

			throw new InvalidCastException($"Cannot cast '{nameof(DataObjectValue)}' to '{nameof(Int32)}'");
		}

		public static implicit operator Single(DataObjectValue dataObjectValue)
		{
			var castedValue = CastToFloat(dataObjectValue);
			if (castedValue.HasValue)
				return castedValue.Value;

			throw new InvalidCastException($"Cannot cast '{nameof(DataObjectValue)}' to '{nameof(Single)}'");
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

			throw new InvalidCastException($"Cannot cast '{nameof(DataObjectValue)}' to '{nameof(String)}'");
		}

		#endregion

		private static Int32? CastToInteger(DataObjectValue dataObjectValue)
		{
			if (dataObjectValue._type.HasFlagFast(DataObjectValueType.Integer))
				return dataObjectValue._intValue;

			if (dataObjectValue._type.HasFlagFast(DataObjectValueType.Boolean))
				return dataObjectValue._booleanValue ? 1 : 0;

			if (dataObjectValue._type.HasFlagFast(DataObjectValueType.Float))
				return (Int32)dataObjectValue._singleValue;

			if (dataObjectValue._type == DataObjectValueType.None)
				return default(Int32);

			return null;
		}

		private static Single? CastToFloat(DataObjectValue dataObjectValue)
		{
			if (dataObjectValue._type.HasFlagFast(DataObjectValueType.Float))
				return dataObjectValue._singleValue;

			if (dataObjectValue._type.HasFlagFast(DataObjectValueType.Boolean))
				return dataObjectValue._booleanValue ? 1.0f : 0.0f;

			if (dataObjectValue._type.HasFlagFast(DataObjectValueType.Integer))
				return dataObjectValue._intValue;

			if (dataObjectValue._type == DataObjectValueType.None)
				return default(Single);

			return null;
		}
	}
}