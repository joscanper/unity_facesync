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
				for (int i = 0; i < BlendSet.BlendShapes.Count; ++i)
				{
					FaceSyncBlendSet.BlendSetEntry entry = BlendSet.BlendShapes[i];
					if (entry != null)
						mApplier.ApplyBlendShape(entry.BlendShape, entry.Value);
				}
			}
		}

	}
}
