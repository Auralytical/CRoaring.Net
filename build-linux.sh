#!/bin/bash
x86_64-linux-gnu-gcc -march=native -O3 -std=c11 -shared -o src/CRoaring/libCRoaring.so -fPIC src/CRoaring/roaring.c
cp src/CRoaring/libCRoaring.so src/CRoaring.Net/

dotnet restore
dotnet build src/CRoaring.Net
