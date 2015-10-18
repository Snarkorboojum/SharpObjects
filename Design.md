# Sharp Objects
## Initializers

Sharp object allows to initialize storngly typed data objects in code without type declaration. Just declare and set properties to initialize any object.

Example:
```
var x = { Id = "256", Width = 15, Height = 16.4f };
x.Width = 14;
Console.WrileLine("Width={0}, Height={1}", x.Width, x.Height);

```
To make that code working SharoObjects generates dedicated type declaration and expands short-form initializer to typical C# initializer.

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

## Type Declaration
## Serialization