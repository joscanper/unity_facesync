# Unity FaceSync
Simple face blendshape control system for Unity.

![](https://github.com/joscanper/untiy_facesync/blob/master/FaceSync/Demo/facesync.gif)

## 1. Installation

Simply checkout the project or unzip on your Unity project folder. Make sure the settings path is located somewhere like:

`YourUnityProject/Assets/FaceSync/FaceSync/Settings`

## 2. Setting up BlendShapeIDs

Before starting you will need a mesh exported with blendshapes. Blendshapes will appear on the SkinnedMeshRenderer component.

For each blendshape you are planning to use from your mesh you will need a BlendShapeID.

To create a new FaceSyncBlendShapeID asset right click on your Project view : Create/FaceSync/BlendShapeID

Fill the `Identifier` field with the name of the mesh blendshape this asset refers to (It has to be one of the BlendShapes that appear in the SkinnnedMeshRenderer list).

## 3. Setting up BlendSets

We can use BlendSets in our FaceSyncData (more about this below) in order to show expresions, lipsync, blinking and whatever we want to do with our mesh blendshapes.

For example, if we want to configure our character for lip syncing we will need an alphable of BlendSets.

The following mouth alphabet would require 6 BlendSets.

![](http://image.wikifoundry.com/image/2/eX6-NiE4BkBk2Ter_j78MQ62335/GW400H349)

To create a new FaceSyncBlendSet asset right click on your Project view : Create/FaceSync/BlendSet

Once created, add the BlendShapeIDs you want to use and adjust their values.

A BlendSet will need one or more BlendShapeIDs with their respective values in order to be shown on your character.

In order to preview the BlendSet on your character, attach the component FaceSyncBlendSetPreview to the object with the SkinnedMeshRenderer and assign the data file.

Keep adding BlendSets and BlendShapeIDs until you have a complete mouth alphabet (Check the ones used in the demo).

You can use BlendSets for other things like expressions, blinking, looking around, etc.

## 4. Setting Up detection rules

Detection rules are used as a substitute to speech recognition (it might be added in a future) and will speed up your workflow since you won't need to assign all BlendSets manually for a dialog line. For example you can write "Hello, how are you?" and autodetection will search for suitable BlendSets to use for that line.

To create a new FaceSyncDetectionRules asset right click on your Project view : Create/FaceSync/Detection Rules

Now you can add multiple rules to it like : "a" for your blendset that plays that vowel or "th" for the corresponding blendset.

Place the rules that have longer identifiers first so they are detected correctly.

You can assing the same BlendSet to different identifiers, for example, in the alphabet show above "a" and "i" would use the same BlendSet.

Finally, add your rules to FaceSync Settings asset, located at `FaceSync/Data/Settings`.

## 5. Creating a character dialog

With BlendShapeIDs, BlendSets and detection rules configured you can now create a FaceSyncData asset which will contain your character dialog.

To create a new FaceSyncData asset right click on your Project view : Create/FaceSync/Sync Data

First, assign the audio clip that will be used, second, write the dialog that will be used to detect phonemes.

If you have setup the detection rules properly, click "Detect Keyframes" ant the timeline will get filled with blendsets.

![](https://github.com/joscanper/untiy_facesync/blob/master/FaceSync/Demo/example.png)

In order to preview the sync data on your character, attach the component FaceSyncDataPreview to the object with the SkinnedMeshRenderer, assign the data file, run your game and click the "Play" on the component.

## Blinking & eyes movement

-TODO-

First make sure you have setup your blendshape and detection rules properly (or simply use the provided demo scene).
