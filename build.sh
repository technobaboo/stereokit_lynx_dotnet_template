#!/bin/sh

dotnet clean Projects/Android/stereokit_lynx_template_Android.csproj
dotnet publish -c Release Projects/Android/stereokit_lynx_template_Android.csproj
adb install Projects/Android/bin/Release/net7.0-android/technobaboo.stereokit_lynx_template-Signed.apk