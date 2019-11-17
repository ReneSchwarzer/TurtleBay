#!/bin/bash

export PATH=$PATH:/usr/share/dotnet-sdk/
export DOTNET_ROOT=/usr/share/dotnet-sdk/ 
export DOTNET_CLI_TELEMETRY_OPTOUT=1

cd /opt/turtlebay
./TurtleBay

cd ~/turtlebay