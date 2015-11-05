using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace SharpObjects.Model
{
	[StructLayout(LayoutKind.Explicit, Pack = 1)]
	[DebuggerDisplay("{_type} {BoxedValue}")]
	public partial struct DataObjectValue
	{
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

		[FieldOffset(8)] // should be aligned to 4 and be the last one to work on Any CPU
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

		[PublicAPI]
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

		[PublicAPI]
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