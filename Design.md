# Sharp Objects
## Initializers

Sharp Object allows to initialize storngly typed data objects in code without type declaration. Just declare and set properties to initialize any object.

Example:
```
var x = { Id = "256", Width = 15, Height = 16.4f };
x.Width = 14;
Console.WrileLine("Width={0}, Height={1}", x.Width, x.Height);

```
To make that code working Sharp Object generates dedicated type declaration and expands short-form initializer to typical C# initializer.

Expanded:
```
var x = new DataObject_A1 { Id = "256", Width = 15, Height = 16.4f };
x.Width = 14;
Console.WrileLine("Width={0}, Height={1}", x.Width, x.Height);

```
Compiler-generated type:
```
public sealed class DataObject_A1 : DataObject
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
```

## Accesss

Sharp Objects allows to dynamically access to any property of DataObject.
```
public void Desired()
{
	DataObject data = GetData();

	DataObjectValue id = data.Id;
	if (id > 0)
	{
		Console.WriteLine($"Width: {data.Width}, Height: {data.Height}");
	}
	
	DataObjectValue counter = data.Counter;
	data.Timestamp = counter++;
}
```

Compiler generates following code:
```
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
```

GetData method:
```
public DataObject GetData()
{
	return new DataObjectX1
	{
		Id = "256",
		Width = 15,
		Height = 16.4f
	};
}
```