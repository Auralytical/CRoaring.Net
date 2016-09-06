#!/bin/bash
x86_64-w64-mingw32-gcc -march=native -O3 -std=c11 -shared -o src/CRoaring/CRoaring.dll -fPIC src/CRoaring/roaring.c
