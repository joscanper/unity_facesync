using UnityEngine;

namespace FaceSync
{
	[ExecuteInEditMode]
	
	public class FaceSyncBlendSetPreview : MonoBehaviour
	{
		public FaceSyncBlendSet BlendSet;
		[Range(0f,1f)]
		public float Progress;

		private SkinnedMeshRenderer mMesh;

		private void Update()
		{
			if (!mMesh)
				mMesh = this.GetComponent<SkinnedMeshRenderer>();

			if (BlendSet)
			{
				foreach (FaceSyncBlendSet.BlendSetEntry entry in BlendSet.BlendShapes)
				{
					if (entry != null)
						ApplyBlendShape(entry.BlendShape, entry.Curve.Evaluate(Progress));
				}
			}
		}

		private void ApplyBlendShape(FaceSyncBlendShapeID id, float value)
		{
			// TODO - Hash the blendshapes
			int index = mMesh.sharedMesh.GetBlendShapeIndex(id.Identifier);
			mMesh.SetBlendShapeWeight(index, value);
			Debug.Log("Setting : " + id.Identifier + ", index : " + index + " to : " + value);
		}
	}
}
