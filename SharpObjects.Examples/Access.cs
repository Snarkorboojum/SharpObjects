using Kappa.Core.System;
using System;

namespace SharpObjects.Examples
{
	public class Access
	{
		public void Desired()
		{
			/*
			var data = GetData();

			DataObjectValue id = data.Id;
			if (id > 0)
			{
				Console.WriteLine($"Width: {data.Width}, Height: {data.Height}");
			}

			DataObjectValue counter = data.Counter;
			data.Timestamp = counter++;
			*/
		}

		public void Real()
		{
			DataObject data = GetData();

			DataObjectValue id = data.GetPropertyValue("Id");
			if (id > 0)
			{
				Console.WriteLine($"Width: {data.GetPropertyValue("Width")}, Height: {data.GetPropertyValue("Height")}");
			}

			DataObjectValue counter = data.GetPropertyValue("Counter");
			data.SetPropertyValue("Counter", counter++);
		}

		public DataObject GetData()
		{
			return new DataObjectX
			{
				Id = "256",
				Width = 15,
				Height = 16.4f
			};
		}
	}
}