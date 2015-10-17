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

		[Flags]
		internal enum DataObjectValueType : byte
		{
			Integer = 1 << 0,
			Float = 1 << 1,
			String = 1 << 2,
			Object = 1 << 3
		}
	}
}