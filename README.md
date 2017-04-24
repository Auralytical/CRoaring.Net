# CRoaring.Net

A .Net wrapper for [CRoaring](https://github.com/RoaringBitmap/CRoaring) - a C implementation of [RoaringBitmap](https://github.com/RoaringBitmap/RoaringBitmap).

## Usage
```cs
using (var rb1 = new RoaringBitmap())
using (var rb2 = new RoaringBitmap())
{
	rb1.AddMany(1, 2, 3, 4, 5, 100, 1000);
	rb1.Optimize();
	
	rb2.AddMany(3, 4, 5, 7, 50);
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
### Linux
Requirements:
- [GCC](https://gcc.gnu.org/)

Run the `build.sh` script

### Windows
Requirements:
- [VS2015 or later](https://www.visualstudio.com/downloads/)
- [CMake](https://cmake.org/download/)

Note: CMake must be available from the command line (added to PATH).

Build the CRoaring and CRoaring.Net projects.

Microsoft CodeGen currently doesn't support the intrinsics required for building for x86_64. 
Instead, it is recommended you build using the Linux route above with a virtual machine or [Ubuntu on Windows](https://msdn.microsoft.com/en-us/commandline/wsl/about).

## Testing CRoaring.Net

Run the `test.sh` or `test.bat` scripts.
