@echo off
:: CRoaring
if not exist "lib/CRoaring/build" mkdir "lib/CRoaring/build"
cd lib/CRoaring/build
cmake -DCMAKE_GENERATOR_PLATFORM=x64 ..
cmake --build . --config Release --target roaring
cd src/Release
copy roaring.dll "../../../../../src/CRoaring.Net/" /Y
copy roaring.dll "../../../../../test/CRoaring.Net.Test/" /Y
cd ../../../../../

:: CRoaring.Net
cd src/CRoaring.Net
dotnet restore
dotnet build
cd ../../

:: CRoaring.Net.Test
cd test/CRoaring.Net.Test
dotnet restore
dotnet test
cd ../../