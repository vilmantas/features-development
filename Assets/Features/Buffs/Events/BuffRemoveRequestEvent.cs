using System;
using UnityEngine.Events;
using Features.Buffs;

namespace Features.Buffs.Events
{
	[Serializable]
	public class BuffRemoveRequestEvent : UnityEvent<BuffBase>
	{
	}
}
