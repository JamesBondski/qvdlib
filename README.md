# qvdlib

## About the project
This is a library for dealing with QlikView Data Files (short: QVD) from .NET. They are the only easy way to write data from Qlik Script other than CSV which has its own issues. It is a proprietary binary format that (as far as I know) is not publicly documented. However, there are implementations in C (https://github.com/devinsmith/qvdreader) and Python (https://github.com/korolmi/qvdfile).

It is more of a personal project for fun, but done in the hope, that something useful will come out of it. For now, it is only able to read the XML header which can easily be accessed in each file.

## Getting started
As soon as the first release is ready, I will put this on Nuget and you can simply use the package.

```cs
//Will add this later
```

## Roadmap
The next (big) step will be adding reading of first values and then rows from the file. After that, I will probably start looking into optimizing things for various use cases (reading all data from a file will be rather different from trying to read specific rows). Maybe, at some point, I'd like to implement writing files.
