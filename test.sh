#!/bin/bash
./build-linux.sh

cp src/CRoaring/libCRoaring.so test/CRoaring.Net.Test/

dotnet restore
dotnet test test/CRoaring.Net.Test
