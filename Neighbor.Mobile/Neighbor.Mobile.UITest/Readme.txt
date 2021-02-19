# Softwares
## Install adb
- sudo apt install android-tools-adb android-tools-fastboot

## Install android SDK
- sudo apt install android-sdk

## Install Mono
- sudo apt install gnupg ca-certificates
- sudo apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF
- echo "deb https://download.mono-project.com/repo/ubuntu stable-focal main" | sudo tee /etc/apt/sources.list.d/mono-official-stable.list
- sudo apt install mono-devel


# Command
mono nunit/nunit-console/nunit3-console.exe neighbor/test/Neighbor.Mobile.UITest.dll --test="Neighbor.Mobile.UITest.Tests(Android)" --testparam="apkPath=neighbor/apks/com.companyname.neighbor.mobile.apk"