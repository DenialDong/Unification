#!/bin/bash

protoc --php_out=../protos *.proto

pbjs -t static-module -w closure -o user.js *.proto
