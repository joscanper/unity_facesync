using UnityEngine;
using System.Collections.Generic;

namespace FaceSync
{ 

	[ExecuteInEditMode]
	[RequireComponent(typeof(FaceSyncApplier))]
	[RequireComponent(typeof(AudioSource))]
	public class FaceSyncController : MonoBehaviour
	{
		private static readonly int sDefaultDataSize = 5;

		private struct DataEntry
		{
			public float Time;
			public FaceSyncData Data;
			public float Duration;

			public bool HasFinished { get { return Time > Duration; }  }
		}

		private AudioSource mSource;
		private FaceSyncApplier mApplier;
		private List<DataEntry> mDataToApply = new List<DataEntry>(sDefaultDataSize);
		private Stack<DataEntry> mDataPool = new Stack<DataEntry>(sDefaultDataSize);

		// --------------------------------------------------------------------

		private void Awake()
		{
			CacheComponents();

			for (int i = 0; i < sDefaultDataSize; ++i)
			{
				mDataPool.Push(new DataEntry());
			}
		}

		// --------------------------------------------------------------------

		private void CacheComponents()
		{
			if (!mSource) mSource = GetComponent<AudioSource>();
			if (!mApplier) mApplier = GetComponent<FaceSyncApplier>();
		}

		// --------------------------------------------------------------------

		public void PlayData(FaceSyncData data)
		{
			CacheComponents();

			DataEntry entry = GetEntry(data);
			mDataToApply.Add(entry);

			if (data.Sound)
				mSource.PlayOneShot(data.Sound);
		}

		// --------------------------------------------------------------------

		private void Update()
		{
			CacheComponents();

			for (int i = 0; i < mDataToApply.Count; ++i)
			{
				DataEntry entry = mDataToApply[i];
				entry.Time += Time.deltaTime;
				mApplier.ApplyBlendData(entry.Data, entry.Time);
				
				if (entry.HasFinished)
				{
					RemoveEntry(i);
					--i;
				}
				else
				{
					mDataToApply[i] = entry;
				}
			}
				
		}

		// --------------------------------------------------------------------

		private DataEntry GetEntry(FaceSyncData data)
		{
			DataEntry entry = mDataPool.Count > 0 ? mDataPool.Pop() : new DataEntry();
			entry.Time = 0f;
			entry.Data = data;
			entry.Duration = data.GetDuration();
			return entry;
		}

		// --------------------------------------------------------------------

		private void RemoveEntry(int index)
		{
			DataEntry entry = mDataToApply[index];
			mDataPool.Push(entry);
			mDataToApply.RemoveAt(index);
		}
	}

}