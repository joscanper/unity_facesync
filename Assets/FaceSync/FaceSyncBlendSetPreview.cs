using UnityEngine;

namespace FaceSync
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(FaceSyncApplier))]
	public class FaceSyncBlendSetPreview : MonoBehaviour
	{
		public FaceSyncBlendSet BlendSet;
		[Range(0f,1f)]
		public float Progress;

		private FaceSyncApplier mApplier;

		// --------------------------------------------------------------------

		private void Update()
		{
			if (!mApplier)
				mApplier = GetComponent<FaceSyncApplier>();

			if (BlendSet)
			{
				foreach (FaceSyncBlendSet.BlendSetEntry entry in BlendSet.BlendShapes)
				{
					if (entry != null)
						mApplier.ApplyBlendShape(entry.BlendShape, entry.Value);
				}
			}
		}

	}
}
