using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace FaceSync
{
	[CustomEditor(typeof(FaceSyncData))]
	public class FaceSyncDataEditor : Editor
	{
		private readonly float sBorder = 20f;
		private readonly float sInitY = 160f;

		private int mSelectedKeyframe;
		private float mClipStartTime;
		private float mWidth;

		// --------------------------------------------------------------------

		public FaceSyncDataEditor()
		{
			mSelectedKeyframe = -1;
		}

		// --------------------------------------------------------------------

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			FaceSyncData syncData = target as FaceSyncData;

			mWidth = EditorGUIUtility.currentViewWidth;
			float clipDuration = syncData && syncData.Sound ? syncData.Sound.length : 1f; ;

			Rect barRect = new Rect(sBorder, sInitY, mWidth - (sBorder * 2f), 5f);
			if (GUI.Button(barRect, ""))
			{
				float t = ((Event.current.mousePosition.x - barRect.x) / barRect.width) * clipDuration;
				syncData.Keyframes.Add(new FaceSyncKeyframe(t));
				mSelectedKeyframe = syncData.Keyframes.Count - 1;
				EditorUtility.SetDirty(target);
			}

			EditorGUILayout.Separator();

			ShowSoundUI(barRect);

			EditorGUILayout.Separator();

			ShowKeyframesUI();

			if (mSelectedKeyframe >= 0)
			{
				GUILayout.BeginArea(new Rect(sBorder, sInitY + 30, mWidth - (sBorder * 2), 200));

				ShowKeyframeDataUI(syncData.Keyframes[mSelectedKeyframe], clipDuration);

				GUILayout.EndArea();
			}

			serializedObject.ApplyModifiedProperties();
		}

		// --------------------------------------------------------------------

		private void ShowKeyframesUI()
		{
			FaceSyncData syncData = target as FaceSyncData;

			EditorGUI.BeginChangeCheck();

			syncData.ReferenceText = EditorGUILayout.TextArea(syncData.ReferenceText);

			if (EditorGUI.EndChangeCheck())
				EditorUtility.SetDirty(target);

			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("Autodetect Keyframes"))
				AutodetectWithRules();

			if (GUILayout.Button("Clear Keyframes"))
			{
				syncData.Keyframes.Clear();
				mSelectedKeyframe = -1;
			}
			EditorGUILayout.EndHorizontal();

			float totalDuration = syncData.GetDuration();
			for (int i = 0; i < syncData.Keyframes.Count; ++i)
			{
				FaceSyncKeyframe keyframe = syncData.Keyframes[i];
				float x = (mWidth - (sBorder * 2)) * (keyframe.Time / totalDuration);
				string label = keyframe.BlendSet ? keyframe.BlendSet.Label : "!";
				GUI.contentColor = keyframe.BlendSet ? keyframe.BlendSet.Color : Color.red;
				GUI.backgroundColor = i == mSelectedKeyframe ? Color.cyan : Color.grey;
				if (GUI.Button(new Rect(sBorder + x - 10, sInitY - 20, 20, 18), label))
				{
					mSelectedKeyframe = i;
				}
				GUI.Box(new Rect(sBorder + x, sInitY, 1, 5), "");
			}
			GUI.contentColor = Color.white;
			GUI.backgroundColor = Color.white;
		}

		// --------------------------------------------------------------------

		private void ShowSoundUI(Rect barRect)
		{
			FaceSyncData syncData = target as FaceSyncData;
			float totalDuration = syncData.GetDuration();

			EditorGUILayout.BeginHorizontal();

			syncData.Sound = EditorGUILayout.ObjectField(syncData.Sound, typeof(AudioClip), false, null) as AudioClip;
			
			if (syncData.Sound != null)
			{
				float clipDuration = syncData.Sound.length;
				float clipPreviewT = Time.realtimeSinceStartup - mClipStartTime;

				if (clipPreviewT >= clipDuration)
				{
					if (GUILayout.Button("Play"))
					{
						PlayClip(syncData.Sound);
						mClipStartTime = Time.realtimeSinceStartup;
					}
				}
				else
				{
					GUILayout.Label(clipPreviewT + "s");
				}

				EditorGUILayout.EndHorizontal();

				float audioPercentage = syncData.Sound.length / totalDuration;
				GUI.backgroundColor = Color.cyan;
				GUI.Box(new Rect(sBorder, sInitY, (mWidth - (sBorder * 2)) * audioPercentage, 5), "");
				GUI.backgroundColor = Color.white;

				if (clipPreviewT < clipDuration)
				{
					Rect clipT = new Rect(barRect);
					clipT.x = clipT.x + (clipPreviewT / clipDuration) * clipT.width;
					clipT.y -= 2f;
					clipT.width = 1f;
					clipT.height = 10f;
					GUI.Box(clipT, "");
					EditorUtility.SetDirty(target);
				}
			}
			else
			{
				EditorGUILayout.EndHorizontal();
			}
		}

		// --------------------------------------------------------------------

		public void ShowKeyframeDataUI(FaceSyncKeyframe keyframe, float maxTime)
		{
			EditorGUI.BeginChangeCheck();
			keyframe.BlendSet = EditorGUILayout.ObjectField(keyframe.BlendSet, typeof(FaceSyncBlendSet), false, null) as FaceSyncBlendSet;
			keyframe.Time = EditorGUILayout.Slider("Time", keyframe.Time, 0, maxTime);

			if (GUILayout.Button("Delete"))
			{
				(target as FaceSyncData).Keyframes.Remove(keyframe);
				mSelectedKeyframe = -1;
			}

			if (EditorGUI.EndChangeCheck())
				EditorUtility.SetDirty(target);
		}

		// --------------------------------------------------------------------

		private void AutodetectWithRules() // TODO - Move this to an autodetection thing
		{
			FaceSyncData syncData = target as FaceSyncData;
			Dictionary<string, FaceSyncBlendSet> rules = FaceSyncSettings.GetSettings().GetHashedRules();
			float totalDuration = syncData.GetDuration();
			string lowerCaseText = syncData.ReferenceText.ToLower();
			for (int i = 0; i < syncData.ReferenceText.Length; ++i)
			{
				foreach (var rule in rules)
				{
					if (lowerCaseText.Substring(i).StartsWith(rule.Key.ToLower()))
					{
						FaceSyncKeyframe kf = new FaceSyncKeyframe(((float)i / syncData.ReferenceText.Length) * totalDuration);
						kf.BlendSet = rule.Value;
						syncData.Keyframes.Add(kf);
					}
				}
			}
		}

		// --------------------------------------------------------------------

		public static void PlayClip(AudioClip clip) // TODO - Move this to a utils class
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