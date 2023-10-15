#!/bin/sh

rm sk.log
adb shell logcat -c
adb shell monkey -p technobaboo.stereokit_lynx_template 1
adb logcat -b main &> sk.log