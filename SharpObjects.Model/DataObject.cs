using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SharpObjects.Model
{
	public class DataObject : Dictionary<String, Object>
	{
	}

	[StructLayout(LayoutKind.Explicit, Pack = 1, Size = 9)]
	public struct DataObjectValue
	{
		[FieldOffset(0)]
		private Boolean _booleanValue;

		[FieldOffset(0)]
		private Int32 _intValue;

		[FieldOffset(sizeof(Int32))]
		private Object _referenceTypeValue;

		[FieldOffset(sizeof(Int32) + 4)]
		private DataObjectValueType _type;
	}

	internal enum DataObjectValueType : byte
	{
		Integer,
		Float,
		String,
		Object
	}
}