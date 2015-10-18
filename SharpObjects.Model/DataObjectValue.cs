using System;
using System.Runtime.InteropServices;

namespace SharpObjects.Model
{
	[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 9)]
	public struct DataObjectValue
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

		#region Conversion

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

		[Flags] 
		internal enum DataObjectValueType : byte
		{
			Boolean = 1 << 0,
			Integer = 1 << 1,
			Float = 1 << 2,
			String = 1 << 3,
			Object = 1 << 4
		}
	}
}