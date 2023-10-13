#!/bin/sh

dotnet publish -c Release Projects/Android/sk_cs_test_Android.csproj
adb install Projects/Android/bin/Release/net7.0-android/technobaboo.lynx_sk_test-Signed.apk