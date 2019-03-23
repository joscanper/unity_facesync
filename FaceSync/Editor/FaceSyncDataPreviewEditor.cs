using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace FaceSync
{
	[CustomEditor(typeof(FaceSyncDataPreview))]
	public class FaceSyncDataPreviewEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			serializedObject.Update();

			FaceSyncDataPreview preview = target as FaceSyncDataPreview;
			if (GUILayout.Button("Play"))
			{
				preview.Play();
			}

			serializedObject.ApplyModifiedProperties();
		}
	}
}