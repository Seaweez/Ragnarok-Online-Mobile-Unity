using UnityEngine;
using System;
using System.Collections.Generic;
using Ghost.Extensions;
using SLua;

namespace RO
{
	public class GameObjectMapHelper : LuaObject
	{
		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
		public static int GetChangedCullingStates(IntPtr l)
		{
			var self = GameObjectMap.Me;
			if (null != self)
			{
				var changedInfos = self.GetChangedInfo();
				if (0 < changedInfos.Count)
				{
					for (int i = 0; i < changedInfos.Count; ++i)
					{
						var info = changedInfos[i];
						int n = i*4;
						pushValue(l, info.type);
						LuaDLL.lua_rawseti(l, -2, n+1);
						pushValue(l, info.key);
						LuaDLL.lua_rawseti(l, -2, n+2);
						if (info.wasVisible != info.isVisible)
						{
							pushValue(l, (info.isVisible ? 1 : 0));
						}
						else
						{
							LuaDLL.lua_pushnil(l);
						}
						LuaDLL.lua_rawseti(l, -2, n+3);
						if (info.previousDistance != info.currentDistance)
						{
							pushValue(l, info.currentDistance);
						}
						else
						{
							LuaDLL.lua_pushnil(l);
						}
						LuaDLL.lua_rawseti(l, -2, n+4);

						info.wasVisible = info.isVisible;
						info.previousDistance = info.currentDistance;
					}
					changedInfos.Clear();
				}
			}
			pushValue(l,true);
			return 1;
		}

		[System.Flags]
		public enum SyncTransformFlag{
			LocalPosition = 1 << 0,
			LocalEulerAngles = 1 << 1,
			LocalScale = 1 << 2,
			LocalEulerAngleY = 1 << 3,
			LocalScaleAll = 1 << 4,
			Position = 1 << 5,
			Rotation = 1 << 6,
		}

		public static bool CheckSynctransformFlag(int flag, SyncTransformFlag f)
		{
			return (int)f == (flag & (int)f);
		}

		[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
		public static int SyncTransform(IntPtr l)
		{
			var self = GameObjectMap.Me;
			if (null != self)
			{
				if (LuaDLL.lua_type(l, 2) == LuaTypes.LUA_TTABLE)
				{
					int n = LuaDLL.lua_rawlen(l, 2);
					int i = 1;
					while (i <= n)
					{
						var type = GetElementInTable<int>(l, 2, ++i);
						var key = GetElementInTable<long>(l, 2, ++i);
						var flag = GetElementInTable<int>(l, 2, ++i);
						if (CheckSynctransformFlag(flag, SyncTransformFlag.LocalPosition))
						{
							GameObjectMap.SetLocalPosition(
								type, key, 
								GetElementInTable<float>(l, 2, ++i), 
								GetElementInTable<float>(l, 2, ++i),
								GetElementInTable<float>(l, 2, ++i));
						}
						if (CheckSynctransformFlag(flag, SyncTransformFlag.LocalEulerAngles))
						{
							GameObjectMap.SetLocalEulerAngles(
								type, key, 
								GetElementInTable<float>(l, 2, ++i),
								GetElementInTable<float>(l, 2, ++i),
								GetElementInTable<float>(l, 2, ++i));
						}
						if (CheckSynctransformFlag(flag, SyncTransformFlag.LocalScale))
						{
							GameObjectMap.SetLocalScale(
								type, key, 
								GetElementInTable<float>(l, 2, ++i),
								GetElementInTable<float>(l, 2, ++i),
								GetElementInTable<float>(l, 2, ++i));
						}
						if (CheckSynctransformFlag(flag, SyncTransformFlag.LocalEulerAngleY))
						{
							GameObjectMap.SetLocalEulerAngleY(
								type, key, 
								GetElementInTable<float>(l, 2, ++i));
						}
						if (CheckSynctransformFlag(flag, SyncTransformFlag.LocalScaleAll))
						{
							var s = GetElementInTable<float>(l, 2, ++i);
							GameObjectMap.SetLocalScale(
								type, key, 
								s,s,s);
						}
						if (CheckSynctransformFlag(flag, SyncTransformFlag.Position))
						{
							GameObjectMap.SetPosition(
								type, key, 
								GetElementInTable<float>(l, 2, ++i), 
								GetElementInTable<float>(l, 2, ++i),
								GetElementInTable<float>(l, 2, ++i));
						}
						if (CheckSynctransformFlag(flag, SyncTransformFlag.Rotation))
						{
							GameObjectMap.SetEulerAngles(
								type, key, 
								GetElementInTable<float>(l, 2, ++i),
								GetElementInTable<float>(l, 2, ++i),
								GetElementInTable<float>(l, 2, ++i));
						}
					}
				}
			}
			pushValue(l,true);
			return 1;
		}

		static T GetElementInTable<T>(IntPtr l, int p, int i)
		{
			LuaDLL.lua_rawgeti(l, 2, i);
			var v = (T)Convert.ChangeType(checkVar(l, -1), typeof(T));
			LuaDLL.lua_pop(l, 1);
			return v;
		}
		
		public static void reg(IntPtr l)
		{
			getTypeTable(l, "RO.GameObjectMapHelper");
			addMember(l, GetChangedCullingStates, false);
			addMember(l, SyncTransform, false);
			createTypeMetatable(l, null, typeof(GameObjectMapHelper));
		}
	}

	public partial class GameObjectMap
	{
		public static Transform GetTransform(int type, long key)
		{
			var self = GameObjectMap.Me;
			if (null == self)
			{
				return null;
			}
			return self.Get_Transform(type, key);
		}

		public static bool TransformIsNull (int type, long key)
		{
			return null == GetTransform(type, key);
		}
		
		public static void DestroyGameObject (int type, long key)
		{
			var t = GetTransform(type, key);
			if (null == t) 
			{
				return;
			}
			GameObject.Destroy (t.gameObject);
		}

		public static void SetGameObjectActive (int type, long key, int active)
		{
			var t = GetTransform(type, key);
			if (null == t) 
			{
				return;
			}
			t.gameObject.SetActive(0 != active);
		}
		
		public static void RestoreBehaviours (int type, long key)
		{
			var t = GetTransform(type, key);
			if (null == t) 
			{
				return;
			}
			LuaGameObject.RestoreBehaviours(t.gameObject);
		}
		
		public static void SetRenderQueue (int type, long key, int renderQueue)
		{
			var t = GetTransform(type, key);
			if (null == t) 
			{
				return;
			}
			LuaGameObject.SetRenderQueue(t.gameObject, renderQueue);
		}
		
		public static void SetUIDepth (int type, long key, int depth)
		{
			var t = GetTransform(type, key);
			if (null == t) 
			{
				return;
			}
			LuaGameObject.SetUIDepth(t.gameObject, depth);
		}
		
		public static void FollowTransform (int typeSrc, long keySrc, int typeTarget, long keyTarget, Vector3 offset)
		{
			var tSrc = GetTransform(typeSrc, keySrc);
			if (null == tSrc) 
			{
				return;
			}
			var tTarget = GetTransform(typeTarget, keyTarget);
			if (null == tTarget) 
			{
				return;
			}
			LuaGameObject.FollowTransform(tSrc, tTarget, offset);
		}
		
		public static void FollowTransformWithRotation (int typeSrc, long keySrc, int typeTarget, long keyTarget, Vector3 offset)
		{
			var tSrc = GetTransform(typeSrc, keySrc);
			if (null == tSrc) 
			{
				return;
			}
			var tTarget = GetTransform(typeTarget, keyTarget);
			if (null == tTarget) 
			{
				return;
			}
			LuaGameObject.FollowTransformWithRotation(tSrc, tTarget, offset);
		}
		
		public static void GetForwardPosition (int type, long key, float distance, out float x, out float y, out float z)
		{
			var t = GetTransform(type, key);
			if (null == t) 
			{
				x = y = z = 0;
				return;
			}
			LuaGameObject.GetForwardPosition(t, distance, out x, out y, out z);
		}
		
		public static void GetPosition (int type, long key, out float x, out float y, out float z)
		{
			var t = GetTransform(type, key);
			if (null == t) 
			{
				x = y = z = 0;
				return;
			}
			LuaGameObject.GetPosition(t, out x, out y, out z);
		}
		
		public static void GetLocalPosition (int type, long key, out float x, out float y, out float z)
		{
			var t = GetTransform(type, key);
			if (null == t) 
			{
				x = y = z = 0;
				return;
			}
			LuaGameObject.GetLocalPosition(t, out x, out y, out z);
		}
		
		public static void GetRotation (int type, long key, out float x, out float y, out float z, out float w)
		{
			var t = GetTransform(type, key);
			if (null == t) 
			{
				x = y = z = w = 0;
				return;
			}
			LuaGameObject.GetRotation(t, out x, out y, out z, out w);
		}
		
		public static void GetLocalRotation (int type, long key, out float x, out float y, out float z, out float w)
		{
			var t = GetTransform(type, key);
			if (null == t) 
			{
				x = y = z = w = 0;
				return;
			}
			LuaGameObject.GetLocalRotation(t, out x, out y, out z, out w);
		}
		
		public static void GetEulerAngles (int type, long key, out float x, out float y, out float z)
		{
			var t = GetTransform(type, key);
			if (null == t) 
			{
				x = y = z = 0;
				return;
			}
			LuaGameObject.GetEulerAngles(t, out x, out y, out z);
		}
		
		public static void GetLocalEulerAngles (int type, long key, out float x, out float y, out float z)
		{
			var t = GetTransform(type, key);
			if (null == t) 
			{
				x = y = z = 0;
				return;
			}
			LuaGameObject.GetLocalEulerAngles(t, out x, out y, out z);
		}
		
		public static void GetScale (int type, long key, out float x, out float y, out float z)
		{
			var t = GetTransform(type, key);
			if (null == t) 
			{
				x = y = z = 0;
				return;
			}
			LuaGameObject.GetScale(t, out x, out y, out z);
		}
		
		public static void GetLocalScale (int type, long key, out float x, out float y, out float z)
		{
			var t = GetTransform(type, key);
			if (null == t) 
			{
				x = y = z = 0;
				return;
			}
			LuaGameObject.GetLocalScale(t, out x, out y, out z);
		}

		public static void InverseTransformPointByVector3 (int type, long key, float px, float py, float pz, out float x, out float y, out float z)
		{
			var t = GetTransform(type, key);
			if (null == t) 
			{
				x = y = z = 0;
				return;
			}
			LuaGameObject.InverseTransformPointByVector3(t, new Vector3(px,py,pz), out x, out y, out z);
		}
		
		public static void InverseTransformPointByTransform (int type, long key, int typeP, long keyP, Space s, out float x, out float y, out float z)
		{
			var t = GetTransform(type, key);
			if (null == t) 
			{
				x = y = z = 0;
				return;
			}
			var tP = GetTransform(typeP, keyP);
			if (null == t) 
			{
				x = y = z = 0;
				return;
			}
			LuaGameObject.InverseTransformPointByTransform(t, tP, s, out x, out y, out z);
		}
		
		public static void InverseTransformVector (int type, long key, float px, float py, float pz, out float x, out float y, out float z)
		{
			var t = GetTransform(type, key);
			if (null == t) 
			{
				x = y = z = 0;
				return;
			}
			LuaGameObject.InverseTransformVector(t, new Vector3(px,py,pz), out x, out y, out z);
		}
		
		public static void TransformPoint (int type, long key, float px, float py, float pz,out float x, out float y, out float z)
		{
			var t = GetTransform(type, key);
			if (null == t) 
			{
				x = y = z = 0;
				return;
			}
			LuaGameObject.TransformPoint(t, new Vector3(px,py,pz), out x, out y, out z);
		}
		
		public static void SetLocalEulerAngleY (int type, long key, float angleY)
		{
			var t = GetTransform(type, key);
			if (null == t) 
			{
				return;
			}
			LuaGameObject.SetLocalEulerAngleY(t, angleY);
		}
		
		public static void LocalRotateToByAxisY (int type, long key, float x, float y, float z)
		{
			var t = GetTransform(type, key);
			if (null == t) 
			{
				return;
			}
			LuaGameObject.LocalRotateToByAxisY(t, new Vector3(x, y, z));
		}
		
		public static void LocalRotateDeltaByAxisY (int type, long key, float delta)
		{
			var t = GetTransform(type, key);
			if (null == t) 
			{
				return;
			}
			LuaGameObject.LocalRotateDeltaByAxisY(t, delta);
		}

		public static void SetLocalPosition(int type, long key, float x, float y, float z)
		{
			var t = GetTransform(type, key);
			if (null == t) 
			{
				return;
			}
			t.localPosition = new Vector3(x, y, z);
		}
		public static void SetLocalRotation(int type, long key, float x, float y, float z, float w)
		{
			var t = GetTransform(type, key);
			if (null == t) 
			{
				return;
			}
			t.localRotation = new Quaternion(x, y, z, w);
		}
		public static void SetLocalEulerAngles(int type, long key, float x, float y, float z)
		{
			var t = GetTransform(type, key);
			if (null == t) 
			{
				return;
			}
			t.localEulerAngles = new Vector3(x, y, z);
		}
		public static void SetLocalScale(int type, long key, float x, float y, float z)
		{
			var t = GetTransform(type, key);
			if (null == t) 
			{
				return;
			}
			t.localScale = new Vector3(x, y, z);
		}
		public static void SetPosition(int type, long key, float x, float y, float z)
		{
			var t = GetTransform(type, key);
			if (null == t) 
			{
				return;
			}
			t.position = new Vector3(x, y, z);
		}
		public static void SetRotation(int type, long key, float x, float y, float z, float w)
		{
			var t = GetTransform(type, key);
			if (null == t) 
			{
				return;
			}
			t.rotation = new Quaternion(x, y, z, w);
		}
		public static void SetEulerAngles(int type, long key, float x, float y, float z)
		{
			var t = GetTransform(type, key);
			if (null == t) 
			{
				return;
			}
			t.eulerAngles = new Vector3(x, y, z);
		}
	
	}
} // namespace RO
