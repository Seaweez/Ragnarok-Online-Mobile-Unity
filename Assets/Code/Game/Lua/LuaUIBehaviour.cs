using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SLua;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class LuaUIBehaviour : LuaBehaviour
	{
		public UILayer layer;
		[SerializeField]
		public TextAsset
			lua;
//		public IList<string> ListUINotifications ()
//		{
//			object o = CallLuaMethod ("ListUINotifications", "");
//
//			List<string> res = new List<string> ();
//			object[] oo = (object[])o;
//			if (o != null && oo != null) {
//				for (int i=0; i<oo.Length; i++)
//					res.Add (oo [i].ToString ());
//			}
//			return res;
//		}
//
//		public void HandleUINotifications (INotification notification)
//		{
//			Notification notif = notification as Notification;
//			CallLuaMethod ("HandleUINotifications", notif);
//		}
	}
}