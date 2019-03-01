using UnityEngine;
using System.Collections.Generic;
using System;

namespace FaceSync
{
	[CreateAssetMenu(fileName = "BlendSet", menuName = "FaceSync/BlendSet")]
	public class FaceSyncBlendSet : ScriptableObject
	{
		[Serializable]
		public class BlendSetEntry
		{
			public FaceSyncBlendShapeID BlendShape;
			public AnimationCurve Curve;
		}

		public string Label;
		public Color Color;
		public List<BlendSetEntry> BlendShapes = new List<BlendSetEntry>();
	}

}