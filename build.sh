#!/bin/bash
x86_64-linux-gnu-gcc -march=native -O3 -std=c11 -shared -o src/CRoaring/libCRoaring.so -fPIC src/CRoaring/roaring.c
x86_64-w64-mingw32-gcc -march=native -O3 -std=c11 -shared -o src/CRoaring/CRoaring.dll -fPIC src/CRoaring/roaring.c

cp src/CRoaring/CRoaring.dll src/CRoaring.Net/
cp src/CRoaring/libCRoaring.so src/CRoaring.Net/
cp src/CRoaring/CRoaring.dll test/CRoaring.Net.Test/
cp src/CRoaring/libCRoaring.so test/CRoaring.Net.Test/

dotnet restore
dotnet build
dotnet test test/CRoaring.Net.Test
