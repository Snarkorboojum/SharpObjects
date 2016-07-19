using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Kappa.Core.System
{
	[StructLayout(LayoutKind.Explicit, Pack = 1)]
	[DebuggerDisplay("{_type} {BoxedValue}")]
	public partial struct DataObjectValue
	{
		#region Constants

		public static DataObjectValue Nothing;

		public static DataObjectValue Zero = new DataObjectValue(0);

		#endregion

		#region Fields

		[FieldOffset(0)] // size 1 byte
		private readonly DataObjectValueType _type;

		// skip 3 bytes to align reference type field

		[FieldOffset(4)]
		private readonly Boolean _booleanValue; // size 1 byte

		[FieldOffset(4)]
		private readonly Int32 _intValue; // size 4 bytes

		[FieldOffset(4)]
		private readonly Single _singleValue; // size 4 bytes

		[FieldOffset(4)]
		private readonly Double _doubleValue; // size 8 bytes

		[FieldOffset(16)] // should be aligned to 4 and be the last one to work on Any CPU
		private readonly Object _referenceTypeValue; // size 4 or 8 bytes

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

		public DataObjectValue(Double value)
			: this()
		{
			_doubleValue = value;
			_type = DataObjectValueType.Double;
		}


		public DataObjectValue(String value)
			: this()
		{
			_referenceTypeValue = value;
			_type = DataObjectValueType.String;
			if (value == null)
				return;

			if (Boolean.TrueString.Equals(value, StringComparison.OrdinalIgnoreCase))
			{
				_booleanValue = true;
				_type |= DataObjectValueType.Boolean;
				return;
			}

			if (Boolean.FalseString.Equals(value, StringComparison.OrdinalIgnoreCase))
			{
				_booleanValue = false;
				_type |= DataObjectValueType.Boolean;
				return;
			}

			Int32 parsedIntegerValue;
			if (Int32.TryParse(value, out parsedIntegerValue))
			{
				_intValue = parsedIntegerValue;
				_type |= DataObjectValueType.Integer;
				return;
			}

			Double parsedDoubleValue;
			if (Double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out parsedDoubleValue))
			{
				_doubleValue = parsedDoubleValue;
				_type |= DataObjectValueType.Double;
				return;
			}


			Single parsedSingleValue;
			if (Single.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out parsedSingleValue))
			{
				_singleValue = parsedSingleValue;
				_type |= DataObjectValueType.Float;
				return;
			}

			_intValue = value.Length; // store length
		}

		public DataObjectValue(Object value)
			: this()
		{
			if (value==null)
			{
				_type = DataObjectValueType.None;
				_referenceTypeValue = null;

				return;
			}

			var objectType = value.GetType();

			if (objectType == typeof(Boolean))
			{
				_type = DataObjectValueType.None;
				_booleanValue = (Boolean)value;
			}

			else if (objectType == typeof(Int32))
			{
				_type = DataObjectValueType.Integer;
				_intValue = (Int32)value;
			}

			else if (objectType == typeof(Single))
			{
				_type = DataObjectValueType.Float;
				_singleValue = (Single)value;
			}

			else if (objectType == typeof(Double))
			{
				_type = DataObjectValueType.Double;
				_doubleValue = (Double)value;

			}

			else if (objectType == typeof(String))
			{
				#region Except from DataObjectValue(String) constructor for futher notice.
				{
					var stringValue = (String)value;

					_referenceTypeValue = value;
					_type = DataObjectValueType.String;
					if (value == null)
						return;

					if (Boolean.TrueString.Equals(stringValue, StringComparison.OrdinalIgnoreCase))
					{
						_booleanValue = true;
						_type |= DataObjectValueType.Boolean;
						return;
					}

					if (Boolean.FalseString.Equals(stringValue, StringComparison.OrdinalIgnoreCase))
					{
						_booleanValue = false;
						_type |= DataObjectValueType.Boolean;
						return;
					}

					Int32 parsedIntegerValue;
					if (Int32.TryParse(stringValue, out parsedIntegerValue))
					{
						_intValue = parsedIntegerValue;
						_type |= DataObjectValueType.Integer;
						return;
					}

					Double parsedDoubleValue;
					if (Double.TryParse(stringValue, NumberStyles.Float, CultureInfo.InvariantCulture, out parsedDoubleValue))
					{
						_doubleValue = parsedDoubleValue;
						_type |= DataObjectValueType.Double;
						return;
					}


					Single parsedSingleValue;
					if (Single.TryParse(stringValue, NumberStyles.Float, CultureInfo.InvariantCulture, out parsedSingleValue))
					{
						_singleValue = parsedSingleValue;
						_type |= DataObjectValueType.Float;
						return;
					}

					_intValue = stringValue.Length; // store length
				}

				#endregion
			}

			else
			{
				_referenceTypeValue = value;
				_type = DataObjectValueType.Object;
			}
		}

		#endregion

		public Boolean HasValue
		{
			get
			{
				if (_type == DataObjectValueType.None)
					return false;

				if (_type.HasFlagFast(DataObjectValueType.ValueType))
					return true;

				if (_type.HasFlagFast(DataObjectValueType.Object))
					return _referenceTypeValue != null;

				throw new InvalidOperationException("Can not determine the existence of value. Unknown value type");
			}
		}

		public Object BoxedValue
		{
			get
			{
				switch (_type)
				{
					case DataObjectValueType.None:
						return null;

					case DataObjectValueType.Boolean:
						return _booleanValue;

					case DataObjectValueType.Integer:
						return _intValue;

					case DataObjectValueType.Float:
						return _singleValue;

					case DataObjectValueType.Double:
						return _doubleValue;

					case DataObjectValueType.String:
					case DataObjectValueType.BooleanString:
					case DataObjectValueType.IntegerString:
					case DataObjectValueType.FloatString:
					case DataObjectValueType.DoubleString:
						return _referenceTypeValue;

					case DataObjectValueType.Object:
						return _referenceTypeValue;

					default:
						throw new InvalidOperationException("Cannot get boxed value. Unknown value type");
				}
			}
		}

		public Boolean IsNumeric => _type.HasAnyOfFlags(DataObjectValueType.Numeric);

		public override String ToString()
		{
			switch (_type)
			{
				case DataObjectValueType.None:
					return "Unknown";

				case DataObjectValueType.Boolean:
					return _booleanValue ? Boolean.TrueString : Boolean.FalseString;

				case DataObjectValueType.Integer:
					return _intValue.ToString();

				case DataObjectValueType.Float:
					return _singleValue.ToString(CultureInfo.InvariantCulture);

				case DataObjectValueType.Double:
					return _doubleValue.ToString(CultureInfo.InvariantCulture);

				case DataObjectValueType.String:
					var stringValue = (String)_referenceTypeValue;
					if (stringValue == null)
						return "null";

					if (stringValue == String.Empty)
						return "Empty";

					return stringValue;

				case DataObjectValueType.BooleanString:
				case DataObjectValueType.IntegerString:
				case DataObjectValueType.FloatString:
				case DataObjectValueType.DoubleString:
					return (String)_referenceTypeValue;

				case DataObjectValueType.Object:
					return _referenceTypeValue?.ToString() ?? "null";

				default:
					throw new InvalidOperationException("Cannot perform 'To String' operation. Unknown value type");
			}
		}

		[Flags]
		internal enum DataObjectValueType : UInt32
		{
			[DebuggerDisplay("None")]
			None = 0,

			#region Value Types

			[Browsable(false)]
			ValueType = 1 << 0,

			[DebuggerDisplay("[Boolean]")]
			Boolean = 1 << 1 | ValueType,

			[DebuggerDisplay("[Int32]")]
			Integer = 1 << 2 | ValueType,

			[DebuggerDisplay("[Single]")]
			Float = 1 << 3 | ValueType,

			[DebuggerDisplay("[Double]")]
			Double = 1 << 4 | ValueType,

			#endregion

			#region Reference Types

			[DebuggerDisplay("[Object]")]
			Object = 1 << 30,

			[DebuggerDisplay("[String]")]
			String = 1u << 31 | Object,

			[DebuggerDisplay("[Boolean from String]")]
			BooleanString = String | Boolean,

			[DebuggerDisplay("[Int32 from String]")]
			IntegerString = String | Integer,

			[DebuggerDisplay("[Single from String]")]
			FloatString = String | Float,

			[DebuggerDisplay("[Double from String]")]
			DoubleString = String | Double,

			#endregion

			#region Combination Values

			[DebuggerDisplay("[Numeric]")]
			Numeric = Integer | Float | Double,

			#endregion
		}
	}
}