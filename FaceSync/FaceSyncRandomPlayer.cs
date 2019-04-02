using System.Collections.Generic;
using UnityEngine;

namespace FaceSync
{
    
    [RequireComponent(typeof(FaceSyncApplier))]
	public class FaceSyncRandomPlayer : MonoBehaviour
	{
		[System.Serializable]
		public class RandomPlayerEntry
		{
			public FaceSyncBlendSet BlendSet;
			public float MinTime;
			public float MaxTime;

			private float mNextPlay;

			// --------------------------------------------------------------------

			public void PrepareNextPlay()
			{
				mNextPlay = Time.realtimeSinceStartup + Random.Range(MinTime, MaxTime);
			}

			// --------------------------------------------------------------------

			public void Update(FaceSyncApplier applier)
			{
				float t = (Time.realtimeSinceStartup - mNextPlay) / BlendSet.Duration;
				if (t > 0f)
				{
					if (t > 1f)
						PrepareNextPlay();
					else
						applier.ApplyBlendSet(BlendSet, t);
				}
			}
		}

		public List<RandomPlayerEntry> Entries;
		
		private FaceSyncApplier mApplier;

		// --------------------------------------------------------------------

		private void Awake()
		{
			mApplier = GetComponent<FaceSyncApplier>();
			for (int i = 0; i < Entries.Count; ++i)
				Entries[i].PrepareNextPlay();
		}

		// --------------------------------------------------------------------

		private void Update()
		{
			for (int i = 0; i < Entries.Count; ++i)
				Entries[i].Update(mApplier);
		}
	}
}