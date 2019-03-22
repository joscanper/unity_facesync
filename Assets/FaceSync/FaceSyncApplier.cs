using System.Collections.Generic;
using UnityEngine;

namespace FaceSync
{
	public class FaceSyncApplier : MonoBehaviour
	{
		public float Smoothness;

		private SkinnedMeshRenderer mMesh;

		private struct BlendShapeValue
		{
			public float AccumulativeValue;
			public int Appliers;
		}

		private Dictionary<FaceSyncBlendShapeID, float> applyData = new Dictionary<FaceSyncBlendShapeID, float>();

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

			foreach (FaceSyncKeyframe keyframe in data.Keyframes)
			{
				float keyframeDuration = keyframe.BlendSet.GetDuration();
				if (time > keyframe.Time && time < (keyframe.Time + keyframeDuration))
				{
					float keyframeProgress = (time - keyframe.Time) / keyframeDuration;
					ApplyBlendSet(keyframe.BlendSet, keyframeProgress);
				}
			}

			Apply();
		}

		// --------------------------------------------------------------------

		private void Apply()
		{
			foreach (var data in applyData)
			{
				ApplyBlendShape(data.Key, data.Value);
			}
		}

		// --------------------------------------------------------------------

		public void ApplyBlendSet(FaceSyncBlendSet set, float t)
		{
			CacheMesh();

			for (int i = 0; i < set.BlendShapes.Count; ++i)
			{
				if (!applyData.ContainsKey(set.BlendShapes[i].BlendShape))
					applyData.Add(set.BlendShapes[i].BlendShape, 0f);

				applyData[set.BlendShapes[i].BlendShape] = Mathf.Lerp(applyData[set.BlendShapes[i].BlendShape], set.BlendShapes[i].Value * Mathf.Clamp01(1f - t), Smoothness * Time.deltaTime);
			}
		}

		// --------------------------------------------------------------------

		public void ApplyBlendShape(FaceSyncBlendShapeID id, float value)
		{
			CacheMesh();

			int index = mMesh.sharedMesh.GetBlendShapeIndex(id.Identifier);
			mMesh.SetBlendShapeWeight(index, value);
		}
	}
}