using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FaceSync
{

	public class FaceSyncApplier : MonoBehaviour
	{
		private SkinnedMeshRenderer mMesh;

		// --------------------------------------------------------------------

		private void CacheMesh()
		{
			if (!mMesh)
				mMesh = GetComponent<SkinnedMeshRenderer>();
		}

		// --------------------------------------------------------------------

		public void ApplyBlendData(FaceSyncData data, float time)
		{
			CacheMesh();

			float clipDuration = data.GetDuration();
			foreach (FaceSyncKeyframe keyframe in data.Keyframes)
			{
				float keyframeDuration = keyframe.BlendSet.GetDuration();
				if (time > keyframe.Time && time < (keyframe.Time + keyframeDuration))
				{
					float keyframeProgress = (time - keyframe.Time) / keyframeDuration;
					ApplyBlendSet(keyframe.BlendSet, keyframeProgress);
				}
			}
		}

		// --------------------------------------------------------------------

		public void ApplyBlendSet(FaceSyncBlendSet set, float t)
		{
			CacheMesh();

			for (int i = 0; i < set.BlendShapes.Count; ++i)
			{
				ApplyBlendShape(set.BlendShapes[i].BlendShape, set.BlendShapes[i].Curve.Evaluate(t));
			}
		}

		// --------------------------------------------------------------------

		public void ApplyBlendShape(FaceSyncBlendShapeID id, float value)
		{
			CacheMesh();

			// TODO - Hash the blendshapes
			int index = mMesh.sharedMesh.GetBlendShapeIndex(id.Identifier);
			mMesh.SetBlendShapeWeight(index, value);
		}
	}
}
