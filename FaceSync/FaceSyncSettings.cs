using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR

using UnityEditor;

#endif // UNITY_EDITOR

using System;

namespace FaceSync
{
	[Serializable]
	[CreateAssetMenu(fileName = "Settings", menuName = "FaceSync/Settings")]
	public class FaceSyncSettings : ScriptableObject
	{
#if UNITY_EDITOR
		public FaceSyncAutodetectRules[] AutodetectRules;

		private static FaceSyncSettings mSettings;

		public static FaceSyncSettings GetSettings()
		{
			if (mSettings)
			{
				return mSettings;
			}

			string path = "Assets/FaceSync/FaceSync/Data/Settings.asset";

			mSettings = AssetDatabase.LoadAssetAtPath(path, typeof(FaceSyncSettings)) as FaceSyncSettings;

			if (mSettings == null)
			{
				mSettings = CreateInstance<FaceSyncSettings>();
				FaceSyncUtils.CreateAssetWithPath(mSettings, path);
			}

			return mSettings;
		}

#endif // UNITY_EDITOR

		public Dictionary<string, FaceSyncBlendSet> GetHashedRules()
		{
			Dictionary<string, FaceSyncBlendSet> ruleDict = new Dictionary<string, FaceSyncBlendSet>();
			foreach (FaceSyncAutodetectRules rules in AutodetectRules)
			{
				foreach (FaceSyncAutodetectRules.RuleEntry entry in rules.Rules)
				{
					ruleDict.Add(entry.Identifier, entry.Set);
				}
			}
			return ruleDict;
		}
	}
}