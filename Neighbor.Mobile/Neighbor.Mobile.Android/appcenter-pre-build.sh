#!/usr/bin/env bash

# Example: Change bundle name of an iOS app for non-production
if [ "$APPCENTER_BRANCH" != "main" ];
then
    MANIFEST_FILE="$APPCENTER_SOURCE_DIRECTORY/Neighbor.Mobile/Neighbor.Mobile.Android/Properties/AndroidManifest.xml"
    VERSIONCODE=`grep versionCode $MANIFEST_FILE | sed 's/.*versionCode="//;s/".*//'`
    VERSIONNAME=`grep versionName $MANIFEST_FILE | sed 's/.*versionName="//;s/\.[0-9]*".*//'`
    NEWCODE=$APPCENTER_BUILD_ID
    NEWNAME=$VERSIONNAME.$NEWCODE-$APPCENTER_BRANCH
    echo "Updating Android build information. New version code: $NEWCODE - New version name: $NEWNAME";
    sed -i '' -e 's/versionCode *= *"'$VERSIONCODE'"/versionCode="'$NEWCODE'"/; s/versionName *= *"[^"]*"/versionName="'$NEWNAME'"/' $MANIFEST_FILE
fi