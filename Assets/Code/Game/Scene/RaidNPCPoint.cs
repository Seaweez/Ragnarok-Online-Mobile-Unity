using UnityEngine;
using System.Collections;
using Ghost.Utils;
using Ghost.Attribute;

namespace RO
{
	public class RaidNPCPoint : NPCInfo
	{
		protected override string namePrefix
		{
			get
			{
				return "Raid";
			}
		}
	}
}