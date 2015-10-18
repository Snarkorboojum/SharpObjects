using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SharpObjects.Model
{
	[StructLayout(LayoutKind.Explicit, Pack = 1)]
	[DebuggerDisplay("{_type} {BoxedValue}")]
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

		[SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
		public override Int32 GetHashCode()
		{
			unchecked
			{
				var hashCode = (Int32)_type;
				if (_type == DataObjectValueType.None)
					return hashCode;

				if (_type.HasFlag(DataObjectValueType.Boolean))
					return _booleanValue.GetHashCode();

				if (_type.HasFlag(DataObjectValueType.Integer))
					return _intValue.GetHashCode();

				if (_type.HasFlag(DataObjectValueType.Float))
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

			if (other._type.HasFlag(DataObjectValueType.Boolean))
				return _booleanValue == other._booleanValue;

			if (other._type.HasFlag(DataObjectValueType.Integer))
				return _booleanValue == other._intValue > 0;

			if (other._type.HasFlag(DataObjectValueType.Float))
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


			if (other._type.HasFlag(DataObjectValueType.Integer))
				return _intValue == other._intValue;

			if (other._type.HasFlag(DataObjectValueType.Boolean))
				return _intValue > 0 == other._booleanValue;

			if (other._type.HasFlag(DataObjectValueType.Float))
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

			if (other._type.HasFlag(DataObjectValueType.Float))
				return _singleValue == other._singleValue;

			if (other._type == DataObjectValueType.Boolean)
				return _singleValue > 0 == other._booleanValue;

			if (other._type.HasFlag(DataObjectValueType.Integer))
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

			[DebuggerDisplay("[Boolean]")]
			Boolean = 1 << 0,

			[DebuggerDisplay("[Int32]")]
			Integer = 1 << 1,

			[DebuggerDisplay("[Single]")]
			Float = 1 << 2,

			[DebuggerDisplay("[String]")]
			String = 1 << 3,

			[DebuggerDisplay("[Boolean from String]")]
			BooleanString = String | Boolean,

			[DebuggerDisplay("[Int32 from String]")]
			IntegerString = String | Integer,

			[DebuggerDisplay("[Single from String]")]
			FloatString = String | Float,

			[DebuggerDisplay("[Object]")]
			Object = 1 << 4
		}
	}

	internal static class DataObjectValueTypeExtensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Boolean HasFlag(this DataObjectValue.DataObjectValueType source, DataObjectValue.DataObjectValueType flag)
		{
			var sourceValue = (Int32)source;
			var flagValue = (Int32)flag;

			return (sourceValue & flagValue) == flagValue;
		}
	}
}