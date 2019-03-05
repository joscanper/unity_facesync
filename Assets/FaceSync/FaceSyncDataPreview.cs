using UnityEngine;

namespace FaceSync
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(FaceSyncApplier))]
	public class FaceSyncDataPreview : MonoBehaviour
	{
		public FaceSyncData Data;
		[Range(0f, 1f)]
		public float Progress;

		private FaceSyncApplier mApplier;

		// --------------------------------------------------------------------

		private void Update()
		{
			if (!mApplier)
				mApplier = GetComponent<FaceSyncApplier>();

			if (Data)
			{
				mApplier.ApplyBlendData(Data, Progress * Data.GetDuration());
			}
		}

	}
}
