@echo off
:: CRoaring
if not exist "lib/CRoaring/build" mkdir "lib/CRoaring/build"
cd lib/CRoaring/build
cmake -DCMAKE_GENERATOR_PLATFORM=x64 ..
cmake --build . --config Release --target roaring
cd src/Release
copy roaring.dll "../../../../../src/CRoaring.Net/" /Y
cd ../../../../../

:: CRoaring.Net
cd src/CRoaring.Net
dotnet restore
dotnet build
cd ../../