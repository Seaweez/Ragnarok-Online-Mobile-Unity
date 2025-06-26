using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class BusManager : SingleTonGO<BusManager>
	{
		public static BusManager Instance
		{
			get
			{
				return Me;
			}
		}
		public GameObject monoGameObject
		{
			get
			{
				return gameObject;
			}
		}

		private Dictionary<long, Bus> buses_ = new Dictionary<long, Bus>();
		
		public bool Add(Bus bus)
		{
			if (buses_.ContainsKey(bus.ID))
			{
				return false;
			}
			buses_.Add(bus.ID, bus);
			return true;
		}
		
		public void Remove(Bus bus)
		{
			if (!buses_.ContainsKey(bus.ID))
			{
				return;
			}
			buses_.Remove(bus.ID);
		}
		
		public Bus GetBus(long ID)
		{
			Bus bus;
			if (buses_.TryGetValue(ID, out bus))
			{
				return bus;
			}
			return null;
		}

		public Dictionary<long, Bus> GetBuses()
		{
			return buses_;
		}

		public Bus CloneBus(long id)
		{
			Bus bus = GetBus (id);
			if (null != bus)
				return bus.Copy ();
			return null;
		}
	}
} // namespace RO
