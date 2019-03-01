using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace FaceSync
{

	
	public class FaceSyncKeyframe
	{
		public float Time;
		public FaceSyncBlendSet BlendSet;
		public float Value;
		
		public FaceSyncKeyframe(float time)
		{
			Time = time;
		}
	}

	[CreateAssetMenu(fileName ="FaceSyncData", menuName = "FaceSync/Sync Data")]
	public class FaceSyncData : ScriptableObject
	{

		public AudioClip Sound;
		public List<FaceSyncKeyframe> Keyframes = new List<FaceSyncKeyframe>();

		public float GetDuration()
		{
			float duration = Sound ? Sound.length : 0f;
			foreach(FaceSyncKeyframe keyframe in Keyframes) // TODO change to regular for
			{
				duration = Mathf.Max(keyframe.Time, duration);
			}
			return duration;
		}

	}
}

