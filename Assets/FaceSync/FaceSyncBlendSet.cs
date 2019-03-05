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

		// --------------------------------------------------------------------

		public float GetDuration()
		{
			float duration = 0;
			for(int i =0; i < BlendShapes.Count; ++i)
			{
				float entryDuration = BlendShapes[i].Curve.keys[BlendShapes[i].Curve.keys.Length - 1].time;
				if (entryDuration > duration)
					duration = entryDuration;
			}

			return duration;
		}
	}

}