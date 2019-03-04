using System;

namespace FaceSync
{
	[Serializable]
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

}
