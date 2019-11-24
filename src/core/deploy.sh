#!/bin/bash

export PATH=$PATH:/usr/share/dotnet-sdk/
export DOTNET_ROOT=/usr/share/dotnet-sdk/ 

dotnet build
dotnet publish

if [ sudo systemctl -q is-enabled turtlebay.service ] 
then
	sudo systemctl disable turtlebay.service 
fi

sudo mkdir -p /opt/turtlebay
sudo chmod +x /opt/turtlebay
#sudo rm -Rf /opt/turtlebay/*


cp -Rf TurtleBay/bin/Debug/netcoreapp3.0/publish/* /opt/turtlebay
cp turtlebay.sh /opt/turtlebay
sudo chmod +x /opt/turtlebay/turtlebay.sh

sudo cp turtlebay.service /etc/systemd/system
sudo systemctl enable turtlebay.service
