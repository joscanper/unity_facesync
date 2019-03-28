using UnityEngine;
using System.Collections;

namespace FaceSync {

	public class FaceSyncRandomPlayer : MonoBehaviour
	{
		public FaceSyncBlendSet BlendSet;
		public float MinTime;
		public float MaxTime;

		private float mNextBlink;
		private FaceSyncApplier mApplier;

		// --------------------------------------------------------------------

		private void Awake()
		{
			mApplier = this.GetComponent<FaceSyncApplier>();
			PrepareNextBlink();
		}

		// --------------------------------------------------------------------

		private void PrepareNextBlink()
		{
			mNextBlink = Time.realtimeSinceStartup + Random.Range(MinTime, MaxTime);
		}

		// --------------------------------------------------------------------

		void Update()
		{
			float t = (Time.realtimeSinceStartup - mNextBlink) / BlendSet.GetDuration();
			if (t > 0f)
			{
				if (t > 1f)
					PrepareNextBlink();
				else
					mApplier.ApplyBlendSet(BlendSet, t);
			}
		}
	}

}