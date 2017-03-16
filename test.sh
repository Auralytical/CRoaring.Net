#!/bin/bash
# CRoaring
mkdir -p lib/CRoaring/build
cd lib/CRoaring/build
cmake ..
make
cp libroaring.so ../../../src/CRoaring.Net/
cp libroaring.so ../../../test/CRoaring.Net.Test/
cd ../../../

# CRoaring.Net
cd src/CRoaring.Net
dotnet restore
dotnet build
cd ../../

# CRoaring.Net.Test
cd test/CRoaring.Net.Test
dotnet restore
dotnet test
cd ../../