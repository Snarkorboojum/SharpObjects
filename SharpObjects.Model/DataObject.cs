using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SharpObjects.Model
{
	public class DataObject : Dictionary<String, Object>
	{
	}

	[StructLayout(LayoutKind.Explicit, Pack = 1)]
	public struct DataObjectValue
	{
		[FieldOffset(0)]
		private DataObjectValueType _type;

		[FieldOffset(sizeof(DataObjectValueType))]
		private Boolean _booleanValue;

		[FieldOffset(sizeof(DataObjectValueType))]
		private Int32 _intValue;

		[FieldOffset(sizeof(DataObjectValueType) + sizeof(Int32))]
		private Object _referenceTypeValue;
	}

	internal enum DataObjectValueType : byte
	{
		Integer,
		Float,
		String,
		Object
	}
}