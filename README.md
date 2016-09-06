# CRoaring.Net v1.0.0-alpha

A .Net wrapper for [CRoaring](https://github.com/RoaringBitmap/CRoaring) - a C implementation of [RoaringBitmap](https://github.com/RoaringBitmap/RoaringBitmap).

## Usage
```cs
using (var rb1 = new RoaringBitmap())
using (var rb2 = new RoaringBitmap())
{
	rb1.Add(1, 2, 3, 4, 5, 100, 100);
	rb1.Optimize();
	
	rb2.Add(3, 4, 5, 7, 50);
	rb2.Optimize();

	using (var result = rb1.And(rb2))
	{
		Console.WriteLine(result.Contains(2));
		Console.WriteLine(result.Contains(4));
		Console.WriteLine(result.Contains(5));
	}
}
```

## Compiling
### Using CLI (Linux)

Requirements:
- [MinGW-w64](https://mingw-w64.org)
- [.Net Core 1.0 SDK](https://www.microsoft.com/net/core)

Run the included `build.sh` script.

## Using Visual Studio (Windows)

Requirements:
- [VS2015 Update 3](https://www.visualstudio.com/downloads/)
- [.Net Core 1.0 SDK](https://www.microsoft.com/net/core)

Microsoft CodeGen currently doesn't support the intrinsics required for building for x86_64. 
Instead, it is recommended you build using the Linux route above with a virtual machine or [Ubuntu on Windows](https://msdn.microsoft.com/en-us/commandline/wsl/about).