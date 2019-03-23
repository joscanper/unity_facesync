using System;
using System.Collections.Generic;
using UnityEngine;

namespace FaceSync
{
	[CreateAssetMenu(fileName = "Settings", menuName = "FaceSync/Autodetect Rules")]
	public class FaceSyncAutodetectRules : ScriptableObject
	{
		[Serializable]
		public class RuleEntry
		{
			public string Identifier;
			public FaceSyncBlendSet Set;
		}

		public List<RuleEntry> Rules;
	}
}
