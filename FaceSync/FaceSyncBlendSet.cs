using System;
using System.Collections.Generic;
using UnityEngine;

namespace FaceSync
{
	[CreateAssetMenu(fileName = "BlendSet", menuName = "FaceSync/BlendSet")]
	public class FaceSyncBlendSet : ScriptableObject
	{
		[Serializable]
		public class BlendSetEntry
		{
			public FaceSyncBlendShapeID BlendShape;
			public float Value;
		}

		public string Label;
		public Color Color;
		public List<BlendSetEntry> BlendShapes = new List<BlendSetEntry>();
		public float Duration = 0.25f;
	}
}