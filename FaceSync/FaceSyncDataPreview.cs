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

		private FaceSyncController mController;

		public void Play()
		{
			if (!mController)
				mController = GetComponent<FaceSyncController>();

			mController.PlayData(Data);
		}
	}
}