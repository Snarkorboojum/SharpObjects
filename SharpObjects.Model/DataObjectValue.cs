using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;

namespace SharpObjects.Model
{
	[StructLayout(LayoutKind.Explicit, Pack = 1)]
	[DebuggerDisplay("{_type} {BoxedValue}")]
	public partial struct DataObjectValue
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

		[FieldOffset(8)] // should be aligned tou 4 and be the last one to work on Any CPU
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
			}
			else
			{
				Single parsedSingleValue;
				if (!Single.TryParse(value, out parsedSingleValue))
					return;

				_singleValue = parsedSingleValue;
				_type |= DataObjectValueType.Float;
			}
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

		private Object BoxedValue
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

					case DataObjectValueType.String:
					case DataObjectValueType.BooleanString:
					case DataObjectValueType.IntegerString:
					case DataObjectValueType.FloatString:
						return _referenceTypeValue;

					case DataObjectValueType.Object:
						return _referenceTypeValue;

					default:
						throw new InvalidOperationException("Cannot get boxed value. Unknown value type");
				}
			}
		}

		public override String ToString()
		{
			switch (_type)
			{
				case DataObjectValueType.None:
					return String.Empty;

				case DataObjectValueType.Boolean:
					return _booleanValue ? Boolean.TrueString : Boolean.FalseString;

				case DataObjectValueType.Integer:
					return _intValue.ToString();

				case DataObjectValueType.Float:
					return _singleValue.ToString(CultureInfo.InvariantCulture);

				case DataObjectValueType.String:
				case DataObjectValueType.BooleanString:
				case DataObjectValueType.IntegerString:
				case DataObjectValueType.FloatString:
					return (String)_referenceTypeValue;

				case DataObjectValueType.Object:
					return _referenceTypeValue?.ToString() ?? String.Empty;

				default:
					throw new InvalidOperationException("Cannot perform 'To String' operation. Unknown value type");
			}
		}

		[Flags]
		internal enum DataObjectValueType : byte
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

			#endregion

			/// <summary>
			/// The reference type value.
			/// </summary>
			[DebuggerDisplay("[Object]")]
			Object = 1 << 4,

			[DebuggerDisplay("[String]")]
			String = 1 << 5 | Object,

			[DebuggerDisplay("[Boolean from String]")]
			BooleanString = String | Boolean,

			[DebuggerDisplay("[Int32 from String]")]
			IntegerString = String | Integer,

			[DebuggerDisplay("[Single from String]")]
			FloatString = String | Float
		}
	}
}