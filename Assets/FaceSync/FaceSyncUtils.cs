#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace FaceSync { 
	public class FaceSyncUtils
	{
		// create all intermediate directories for an asset name
		public static void CreatePath(string path)
		{
			string[] components = path.Split('/');
			string test = components[0];
			for (int i = 1; i < components.Length - 1; i++)
			{
				if (components[i].Length > 0)
				{
					string newPath = test + "/" + components[i];
					if (!AssetDatabase.IsValidFolder(newPath))
					{
						AssetDatabase.CreateFolder(test, components[i]);
					}
					test = newPath;
				}
			}
		}

		// Create intermediate folders in the asset path if they are missing
		public static void CreateAssetWithPath(Object asset, string path)
		{
			CreatePath(path);
			AssetDatabase.CreateAsset(asset, path);
		}
	}
}
#endif