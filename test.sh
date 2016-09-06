#!/bin/bash
./build-linux.sh

dotnet restore
dotnet test test/CRoaring.Net.Test
