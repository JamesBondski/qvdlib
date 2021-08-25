# qvdlib

## About the project
This is a library for dealing with QlikView Data Files (short: QVD) from .NET. They are the only easy way to write data from Qlik Script other than CSV which has its own issues. It is a proprietary binary format that (as far as I know) is not publicly documented. However, there are implementations in C (https://github.com/devinsmith/qvdreader) and Python (https://github.com/korolmi/qvdfile).

It is more of a personal project for fun, but done in the hope, that something useful will come out of it. For now, it is only able to read the XML header which can easily be accessed in each file.

## Getting started
You can download this from nuget under the name Bondski.QvdLib (https://www.nuget.org/packages/Bondski.QvdLib/1.0.0).

To read a qvd file, simply do something like this:
```cs
QvdReader qvdReader = new QvdReader("/path/to/file");
while(qvdReader.NextRow())
{
  Console.WriteLine(qvdReader["ColumnName"].ToString());
}
```

When using the library, it is important to understand the Qlik type system, as it has implications for the values you will be getting. Especially, there is no way of verifying that all values in a given field are of the same type. 

There are the followng types in Qlik:
|Type|String|Int|Double|
|---|---|---|---|
|String|X|-|-|
|Integer|-|X|-|
|Double|-|-|X|
|Dual Integer|X|X|-|
|Dual Double|X|-|X|
|Null|-|-|-|

For performance reasons, Integer and Double values are always stored as primitives, so they are not nullable. So if you want to use the properties for Int and Double, you have to check the type of the value yourself to handle null values correctly.

The recommended way is to use the corresponding toX-method as this will handle null values for you. It will use the type of value closest to what you are expecting (i.e. if you use a ToInt() on a Dual Double, it will convert the double value into an integer, **if you use it on a String, it will try to convert that into an integer**). It is using Convert.ToX for all conversions.

For the future, I am planning to implement a way to better customize which of these conversions should be done automatically and how errors are supposed to be handled. 

## Roadmap
For now, the library does what it is supposed to, it reads qvd files. Right now, I am mostly looking to:
- test the library on as many qvds as possible and fix any issues that come up. If you have a file that is not read correctly, please open an issue.
- Improving the unit test coverage (especially for value reading there are quite a few holes)
- Better handling of different types of values (see "Getting started")
- A few optimizations

Maybe, at some point in the future, I will implement writing qvd files.

## License
Licensed under the Apache License 2.0.
