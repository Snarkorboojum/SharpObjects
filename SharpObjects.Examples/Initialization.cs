using System;
using SharpObjects.Model;

namespace SharpObjects.Examples
{
	public class Initialization
	{
		public void Desired()
		{
			/*
			var x = { Id = "256", Width = 15, Height = 16.4f };

			Console.WriteLine(x);
			*/
		}

		public void Real()
		{
			var x = new DataObjectX()
			{
				Id = "256",
				Width = 15,
				Height = 16.4f
			};

			Console.WriteLine(x);
		}
	}

	public class DataObjectX : DataObject
	{
		public DataObjectValue Id
		{
			get
			{
				return GetPropertyValue(nameof(Id));
			}
			set
			{
				SetPropertyValue(nameof(Id), value);
			}
		}

		public DataObjectValue Width
		{
			get
			{
				return GetPropertyValue(nameof(Width));
			}
			set
			{
				SetPropertyValue(nameof(Width), value);
			}
		}

		public DataObjectValue Height
		{
			get
			{
				return GetPropertyValue(nameof(Height));
			}
			set
			{
				SetPropertyValue(nameof(Height), value);
			}
		}
	}
}