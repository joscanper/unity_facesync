using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace FaceSync
{
	[CustomEditor(typeof(FaceSyncData))]
	public class FaceSyncDataEditor : Editor
	{
		private int mSelectedKeyframe;

		public FaceSyncDataEditor()
		{
			mSelectedKeyframe = -1;
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			serializedObject.Update();

			FaceSyncData syncData = target as FaceSyncData;
			float dataDuration = syncData.GetDuration();

			if (GUILayout.Button("Play"))
				PlayClip(syncData.Sound);


			float border = 10;
			float initY = 150;
			float width = EditorGUIUtility.currentViewWidth;
			Rect barRect = new Rect(border, initY, width - (border * 2), 5);
			if (GUI.Button(barRect,""))
			{
				// TODO - calculate time by mousePosition;
				syncData.Keyframes.Add(new FaceSyncKeyframe(0.5f));
			}

			for (int i = 0; i < syncData.Keyframes.Count; ++i)
			{
				FaceSyncKeyframe keyframe = syncData.Keyframes[i];
				float x = (width - (border * 2)) * (keyframe.Time / dataDuration);
				string label = keyframe.BlendSet ? keyframe.BlendSet.Label : "!";
				if (GUI.Button(new Rect(border + x - 10, initY - 20, 20, 18), label))
				{
					mSelectedKeyframe = i;
				}
				GUI.Box(new Rect(border + x, initY, 1, 5), "");
			}


			if (syncData.Sound != null)
			{
				float audioPercentage = syncData.Sound.length / dataDuration;
				GUI.backgroundColor = Color.cyan;
				GUI.Box(new Rect(border, initY, (width - (border * 2)) * audioPercentage, 5), "");
				GUI.backgroundColor = Color.white;

			}

			if (mSelectedKeyframe >= 0)
			{
				GUILayout.BeginArea(new Rect(border, initY + 30, width - (border * 2), 200));
				
				ShowKeyframeData(syncData.Keyframes[mSelectedKeyframe], syncData.Sound.length);
				
				GUILayout.EndArea();
			}

			serializedObject.ApplyModifiedProperties();
		}

		public void ShowKeyframeData(FaceSyncKeyframe keyframe, float maxTime)
		{
			keyframe.BlendSet = EditorGUILayout.ObjectField(keyframe.BlendSet, typeof(FaceSyncBlendSet), false, null) as FaceSyncBlendSet;
			keyframe.Time = EditorGUILayout.Slider("Time", keyframe.Time, 0, maxTime);	
		}

		public static void PlayClip(AudioClip clip)
		{
			Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
			Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
			MethodInfo method = audioUtilClass.GetMethod(
				"PlayClip",
				BindingFlags.Static | BindingFlags.Public,
				null,
				new System.Type[] {
		 typeof(AudioClip)
			},
			null
			);
			method.Invoke(
				null,
				new object[] {
		 clip
			}
			);
		}
	}
}