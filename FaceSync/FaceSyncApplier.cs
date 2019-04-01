using System.Collections.Generic;
using UnityEngine;

namespace FaceSync
{
	public class FaceSyncApplier : MonoBehaviour
	{
		public float RecoverSmoothness;
		
		private SkinnedMeshRenderer mMesh;

		private struct BlendShapeValue
		{
			public float AccumulativeValue;
			public int Appliers;
		}

		private Dictionary<FaceSyncBlendShapeID, float> mApplyData = new Dictionary<FaceSyncBlendShapeID, float>();
		private HashSet<FaceSyncBlendShapeID> mModifiedIds = new HashSet<FaceSyncBlendShapeID>();

		// --------------------------------------------------------------------

		private void Update()
		{
			Apply();
		}

		// --------------------------------------------------------------------

		public void ApplyBlendData(FaceSyncData data, float time)
		{
			foreach (FaceSyncKeyframe keyframe in data.Keyframes)
			{
				float keyframeDuration = keyframe.BlendSet.Duration;
				if (time > keyframe.Time && time < (keyframe.Time + keyframeDuration))
				{
					float keyframeProgress = (time - keyframe.Time) / keyframeDuration;
					ApplyBlendSet(keyframe.BlendSet, keyframeProgress);
				}
			}
		}

		// --------------------------------------------------------------------

		private void Apply()
		{
			foreach (var data in mApplyData)
			{
				ApplyBlendShape(data.Key, data.Value);
			}

			ApplyBlendShapeDrag();
		}

		// --------------------------------------------------------------------

		private void ApplyBlendShapeDrag()
		{
			foreach (var id in mModifiedIds)
			{
				mApplyData[id] = Mathf.Lerp(mApplyData[id], 0, RecoverSmoothness * Time.deltaTime);
			}
		}

		// --------------------------------------------------------------------

		public void ApplyBlendSet(FaceSyncBlendSet set, float t)
		{
			for (int i = 0; i < set.BlendShapes.Count; ++i)
			{
				if (!mApplyData.ContainsKey(set.BlendShapes[i].BlendShape))
					mApplyData.Add(set.BlendShapes[i].BlendShape, 0f);

				mApplyData[set.BlendShapes[i].BlendShape] = Mathf.Lerp(mApplyData[set.BlendShapes[i].BlendShape], set.BlendShapes[i].Value, set.BlendShapes[i].BlendStrength * Time.deltaTime);
			}
		}

		// --------------------------------------------------------------------

		public void ApplyBlendShape(FaceSyncBlendShapeID id, float value)
		{
			if (!mMesh)
				mMesh = GetComponent<SkinnedMeshRenderer>();

			int index = mMesh.sharedMesh.GetBlendShapeIndex(id.Identifier);
			mMesh.SetBlendShapeWeight(index, value);

			mModifiedIds.Add(id);
		}
	}
}