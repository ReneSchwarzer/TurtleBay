#!/bin/bash

export PATH=$PATH:/usr/share/dotnet-sdk/
export DOTNET_ROOT=/usr/share/dotnet-sdk/ 

dotnet build
dotnet publish

sudo mkdir -p /opt/turtlebay
sudo chmod +x /opt/turtlebay
#sudo rm -Rf /opt/turtlebay/*
cp -Rf TurtleBay/bin/Debug/netcoreapp3.0/publish/* /opt/turtlebay
cp run.sh /opt/turtlebay
sudo chmod +x /opt/turtlebay/run.sh

sudo cp service.sh /etc/init.d/turtlebay
sudo chmod +x /etc/init.d/turtlebay

update-rc.d turtlebay defaults
