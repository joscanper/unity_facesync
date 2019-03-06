using UnityEngine;
using System.Collections.Generic;

namespace FaceSync
{ 

	[ExecuteInEditMode]
	[RequireComponent(typeof(FaceSyncApplier))]
	[RequireComponent(typeof(AudioSource))]
	public class FaceSyncController : MonoBehaviour
	{

		private struct DataEntry
		{
			public float Time;
			public FaceSyncData Data;
			public float Duration;

			public bool HasFinished { get { return Time > Duration; }  }
		}

		private AudioSource mSource;
		private FaceSyncApplier mApplier;
		private List<DataEntry> mDataToApply = new List<DataEntry>(5);

		private void Awake()
		{
			CacheComponents();
		}

		private void CacheComponents()
		{
			if (!mSource) mSource = GetComponent<AudioSource>();
			if (!mApplier) mApplier = GetComponent<FaceSyncApplier>();
		}

		public void PlayData(FaceSyncData data)
		{
			CacheComponents();

			DataEntry entry = new DataEntry{
				Time = 0f,
				Data = data,
				Duration = data.GetDuration()
			};
			mDataToApply.Add(entry);

			if (data.Sound)
				mSource.PlayOneShot(data.Sound);
		}

		private void Update()
		{
			CacheComponents();

			for (int i = 0; i < mDataToApply.Count; ++i)
			{
				DataEntry entry = mDataToApply[i];
				entry.Time += Time.deltaTime;
				mApplier.ApplyBlendData(entry.Data, entry.Time);
				Debug.Log("Applying : " + entry.Data.name + " Time : " + entry.Time);
				if (entry.HasFinished)
				{
					mDataToApply.RemoveAt(i);
					--i;
				}
				else
				{
					mDataToApply[i] = entry;
				}
			}
				
		}
	}

}