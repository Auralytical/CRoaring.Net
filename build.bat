@echo off
REM CRoaring
if not exist lib/CRoaring/build mkdir lib/CRoaring/build
cd lib/CRoaring/build
cmake -DCMAKE_GENERATOR_PLATFORM=x64 ..
MSBuild RoaringBitmap.sln /p:Configuration=ALL_BUILD
copy roaring.dll ../../../src/CRoaring.Net/
cd ../../../

REM CRoaring.Net
cd src/CRoaring.Net
dotnet restore
dotnet build
cd ../../