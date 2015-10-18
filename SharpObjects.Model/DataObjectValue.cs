using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace SharpObjects.Model
{
	[StructLayout(LayoutKind.Explicit, Pack = 1)]
	public struct DataObjectValue : IEquatable<DataObjectValue>
	{
		#region Fields
		[FieldOffset(0)] // size 1 byte
		private readonly DataObjectValueType _type;

		//skip 3 bytes to align reference type field

		[FieldOffset(4)] // size 1 byte
		private readonly Boolean _booleanValue;

		[FieldOffset(4)] // size 4 bytes
		private readonly Int32 _intValue;

		[FieldOffset(4)] // size 4 bytes
		private readonly Single _singleValue;

		[FieldOffset(8)] // should be aligned by 4 and be last one to work on Any CPU
		private readonly Object _referenceTypeValue;

		#endregion

		#region Constructors

		public DataObjectValue(Boolean value)
			: this()
		{
			_booleanValue = value;
			_type = DataObjectValueType.Boolean;
		}

		public DataObjectValue(Int32 value)
			: this()
		{
			_intValue = value;
			_type = DataObjectValueType.Integer;
		}

		public DataObjectValue(Single value)
			: this()
		{
			_singleValue = value;
			_type = DataObjectValueType.Float;
		}

		public DataObjectValue(String value)
			: this()
		{
			_referenceTypeValue = value;
			_type = DataObjectValueType.String;
		}

		public DataObjectValue(Object value)
			: this()
		{
			_referenceTypeValue = value;
			_type = DataObjectValueType.Object;
		}

		#endregion

		public Boolean HasValue
		{
			get
			{
				switch (_type)
				{
					case DataObjectValueType.None:
						return false;

					case DataObjectValueType.Boolean:
					case DataObjectValueType.Integer:
					case DataObjectValueType.Float:
						return true;

					case DataObjectValueType.String:
					case DataObjectValueType.Object:
						return _referenceTypeValue != null;

					default:
						throw new InvalidOperationException("Can not determine the existence of value. Unknown value type");
				}
			}
		}

		#region Conversion From

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

		#region Conversion To

		public static implicit operator Boolean(DataObjectValue dataObjectValue)
		{
			if (dataObjectValue._type == DataObjectValueType.Boolean)
				return dataObjectValue._booleanValue;

			throw new InvalidCastException($"Cannot cast '{nameof(DataObjectValue)}' to '{nameof(Boolean)}'");
		}

		public static implicit operator Int32(DataObjectValue dataObjectValue)
		{
			if (dataObjectValue._type == DataObjectValueType.Integer)
				return dataObjectValue._intValue;

			throw new InvalidCastException($"Cannot cast '{nameof(DataObjectValue)}' to '{nameof(Int32)}'");
		}

		public static implicit operator Single(DataObjectValue dataObjectValue)
		{
			if (dataObjectValue._type == DataObjectValueType.Float)
				return dataObjectValue._singleValue;

			throw new InvalidCastException($"Cannot cast '{nameof(DataObjectValue)}' to '{nameof(Single)}'");
		}

		public static implicit operator String(DataObjectValue dataObjectValue)
		{
			if (dataObjectValue._type == DataObjectValueType.String)
				return (String)dataObjectValue._referenceTypeValue;

			throw new InvalidCastException($"Cannot cast '{nameof(DataObjectValue)}' to '{nameof(String)}'");
		}

		#endregion

		#region Equality Methods

		public Boolean Equals(DataObjectValue other)
		{
			return Equals(other, typeConsistencyCheck: true);
		}

		[SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
		public Boolean Equals(DataObjectValue other, Boolean typeConsistencyCheck)
		{
			if (other._type == DataObjectValueType.None)
				return _type == DataObjectValueType.None;

			switch (_type)
			{
				case DataObjectValueType.None:
					return other._type == DataObjectValueType.None;

				case DataObjectValueType.Boolean:
					{
						if (other._type == DataObjectValueType.Boolean)
							return _booleanValue == other._booleanValue;

						if (typeConsistencyCheck)
							throw new InvalidOperationException("Cannot compare values with different types");

						if (other._type == DataObjectValueType.String)
							return Equals((String)other._referenceTypeValue, _booleanValue);

						return _booleanValue && other.HasValue;
					}

				case DataObjectValueType.Integer:
					{
						if (other._type == DataObjectValueType.Integer)
							return _intValue == other._intValue;

						if (typeConsistencyCheck)
							throw new InvalidOperationException("Cannot compare values with different types");

						switch (other._type)
						{
							case DataObjectValueType.Boolean:
								return other._booleanValue;

							case DataObjectValueType.Float:
								// ReSharper disable once RedundantCast
								return ((Single)_intValue) == other._singleValue;

							case DataObjectValueType.String:
								return Equals((String)other._referenceTypeValue, _intValue);

							default:
								return false;
						}
					}

				case DataObjectValueType.Float:
					{
						if (other._type == DataObjectValueType.Float)
							return _singleValue == other._singleValue;

						if (typeConsistencyCheck)
							throw new InvalidOperationException("Cannot compare values with different types");

						switch (other._type)
						{
							case DataObjectValueType.Boolean:
								return other._booleanValue;

							case DataObjectValueType.Integer:
								// ReSharper disable once RedundantCast
								return _singleValue == (Single)other._intValue;

							case DataObjectValueType.String:
								return Equals((String)other._referenceTypeValue, _singleValue);

							default:
								return false;
						}
					}

				case DataObjectValueType.String:
					{
						if (other._type == DataObjectValueType.String)
							return String.Equals((String)_referenceTypeValue, (String)other._referenceTypeValue, StringComparison.Ordinal);

						if (typeConsistencyCheck)
							throw new InvalidOperationException("Cannot compare values with different types");

						switch (other._type)
						{
							case DataObjectValueType.Boolean:
								return Equals((String)_referenceTypeValue, other._booleanValue);

							case DataObjectValueType.Integer:
								return Equals((String)_referenceTypeValue, other._intValue);

							case DataObjectValueType.Float:
								return Equals((String)_referenceTypeValue, other._singleValue);

							case DataObjectValueType.Object:
								return other._referenceTypeValue == _referenceTypeValue; // reference equals

							default:
								return false;
						}
					}

				case DataObjectValueType.Object:
					{
						if (other._type == DataObjectValueType.Object)
							return _referenceTypeValue == other._referenceTypeValue;

						if (typeConsistencyCheck)
							throw new InvalidOperationException("Cannot compare values with different types");

						if (other._type == DataObjectValueType.Boolean)
							return _referenceTypeValue != null == other._booleanValue;


						if (other._type == DataObjectValueType.String)
							return _referenceTypeValue == other._referenceTypeValue; // reference equals

						return false;
					}

				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		#endregion

		#region String Comparison

		private static Boolean Equals(String stringValue, Boolean booleanValue)
		{
			if (stringValue == null)
				return false;

			if (Boolean.TrueString.Equals(stringValue, StringComparison.OrdinalIgnoreCase))
				return booleanValue;

			if (Boolean.FalseString.Equals(stringValue, StringComparison.OrdinalIgnoreCase))
				return !booleanValue;

			return false;
		}

		private static Boolean Equals(String stringValue, Int32 integerValue)
		{
			if (stringValue == null)
				return false;

			Int32 parsedValue;
			return Int32.TryParse(stringValue, out parsedValue) && integerValue == parsedValue;
		}

		private static Boolean Equals(String stringValue, Single singleValue)
		{
			if (stringValue == null)
				return false;

			Single parsedValue;
			// ReSharper disable once CompareOfFloatsByEqualityOperator
			return Single.TryParse(stringValue, out parsedValue) && singleValue == parsedValue;
		}

		#endregion

		[Flags]
		internal enum DataObjectValueType : byte
		{
			None = 0,
			Boolean = 1 << 0,
			Integer = 1 << 1,
			Float = 1 << 2,
			String = 1 << 3,
			Object = 1 << 4
		}
	}
}