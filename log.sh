#!/bin/sh

rm sk.log
adb shell logcat -c
adb shell monkey -p technobaboo.lynx_sk_test 1
adb logcat -b main &> sk.log