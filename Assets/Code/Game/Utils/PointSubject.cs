using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Ghost.Extensions;
using Ghost.Utils;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class PointSubject : MonoBehaviour 
	{
		private const string EFFECT_POINT_PREFIX = "EP_";
		private const string CONNECT_POINT_PREFIX = "CP_";
		private const string EFFECT_CONNECT_POINT_PREFIX = "ECP_";
		
		public static GameObject[] GetPoints(GameObject obj, string prefix)
		{
			int maxIndex = 0;
			var indexes = new List<int>();
			
			var pattern = StringUtils.ConnectToString(prefix, @"\d+");
			var objs = obj.FindGameObjectsInChildren(delegate(GameObject testObj){
				var match = Regex.Match(testObj.name, pattern, RegexOptions.IgnoreCase);
				if (match.Success)
				{
					match = Regex.Match(match.Value, @"\d+", RegexOptions.RightToLeft);
					if (match.Success)
					{
						var index = int.Parse(match.Value);
						if (0 <= index && !indexes.Contains(index))
						{
							maxIndex = System.Math.Max(maxIndex, index);
							indexes.Add(index);
							return true;
						}
					}
				}
				return false;
			});
			
			if (0 < objs.Length)
			{
				var sortedObjs = new GameObject[maxIndex+1];
				for (int i = 0; i < objs.Length; ++i)
				{
					sortedObjs[indexes[i]] = objs[i];
				}
				return sortedObjs;
			}
			
			return objs;
		}

		#region EP
		public static GameObject[] GetEffectPoints(GameObject obj)
		{
			return GetPoints(obj, EFFECT_POINT_PREFIX);
		}
		
		[SerializeField]
		private GameObject[] effectPoints_ = null;
		public GameObject[] effectPoints
		{
			get
			{
				return effectPoints_;
			}
		}
		public GameObject GetEffectPoint(int index)
		{
			if (!effectPoints_.CheckIndex(index))
			{
				return null;
			}
			return effectPoints_[index];
		}
		public void RefreshEffectPoints()
		{
			effectPoints_ = GetEffectPoints(gameObject);
		}

		[SerializeField]
		private Vector3[] effectFixedPoints_ = null;
		public Vector3[] effectFixedPoints
		{
			get
			{
				return effectFixedPoints_;
			}
		}
		public Vector3 GetEffectFixedPoint(int index)
		{
			if (!effectFixedPoints_.CheckIndex(index))
			{
				return Vector3.zero;
			}
			return effectFixedPoints_[index];
		}
		
		public void RefreshEffectFixedPoints()
		{
			if (!effectPoints_.IsNullOrEmpty())
			{
				if (null == effectFixedPoints_ || effectFixedPoints_.Length != effectPoints_.Length)
				{
					effectFixedPoints_ = new Vector3[effectPoints_.Length];
				}
				for (int i = 0; i < effectPoints_.Length; ++i)
				{
					var ep = effectPoints_[i];
					effectFixedPoints_[i] = (null != ep) ? (transform.InverseTransformPoint(ep.transform.position)) : Vector3.zero;
				}
			}
			else
			{
				effectFixedPoints_ = new Vector3[0];
			}
		}
		#endregion EP

		#region CP
		public static GameObject[] GetConnectPoints(GameObject obj)
		{
			return GetPoints(obj, CONNECT_POINT_PREFIX);
		}
		
		[SerializeField]
		private GameObject[] connectPoints_ = null;
		public GameObject[] connectPoints
		{
			get
			{
				return connectPoints_;
			}
		}
		public GameObject GetConnectPoint(int index)
		{
			if (!connectPoints_.CheckIndex(index))
			{
				return null;
			}
			return connectPoints_[index];
		}
		public GameObject GetFirstValidConnectPoint()
		{
			if (connectPoints_.IsNullOrEmpty())
			{
				return null;
			}
			foreach (var cp in connectPoints_)
			{
				if (null != cp)
				{
					return cp;
				}
			}
			return null;
		}
		public void RefreshConnnectPoints()
		{
			connectPoints_ = GetConnectPoints(gameObject);
		}
		#endregion CP

		#region ECP
		[System.Serializable]
		public struct EffectConnectInfo
		{
			public int ID;
			public GameObject ep;

			public EffectConnectInfo(int i, GameObject obj)
			{
				ID = i;
				ep = obj;
			}
		}
		public static EffectConnectInfo[] GetEffectConnectPoints(GameObject obj)
		{
			var infos = new List<EffectConnectInfo>();
			var pattern = StringUtils.ConnectToString(EFFECT_CONNECT_POINT_PREFIX, @"\d+");
			obj.FindGameObjectsInChildren(delegate(GameObject testObj){
				var match = Regex.Match(testObj.name, pattern, RegexOptions.IgnoreCase);
				if (match.Success)
				{
					match = Regex.Match(match.Value, @"\d+", RegexOptions.RightToLeft);
					if (match.Success)
					{
						var effectID = int.Parse(match.Value);
						infos.Add(new EffectConnectInfo(effectID, testObj));
					}
				}
				return false;
			});
			return infos.ToArray();
		}
		[SerializeField]
		private EffectConnectInfo[] effectConnectPoints_ = null;
		public EffectConnectInfo[] effectConnectPoints
		{
			get
			{
				return effectConnectPoints_;
			}
		}
		public void RefreshEffectConnnectPoints()
		{
			effectConnectPoints_ = GetEffectConnectPoints(gameObject);
		}
		#endregion ECP

		public void ScanPoints()
		{
			RefreshEffectPoints();
			RefreshConnnectPoints();
			RefreshEffectConnnectPoints();
		}

		public void MoveEffects(PointSubject other)
		{
			if (this == other)
			{
				return;
			}
			for (int i = 0; i < effectPoints_.Length; ++i)
			{
				var ep = GetEffectPoint(i);
				if (null != ep && 0 < ep.transform.childCount)
				{
					var effectCount = ep.transform.childCount;
					var otherEP = other.GetEffectPoint(i);
					if (null != otherEP)
					{
						var children = new List<Transform>();
						for (int j = 0; j < effectCount; ++j)
						{
							children.Add(ep.transform.GetChild(j));
						}
						foreach (var child in children)
						{
							child.ResetParent(otherEP.transform);
						}
					}
				}
			}
		}

		public void ScanObstacles()
		{
			var objs = gameObject.FindGameObjectsInChildren(delegate(GameObject obj){
				return string.Equals("Obstacle", obj.name);
			});
			if (!objs.IsNullOrEmpty())
			{
				foreach (var obj in objs)
				{
					var obstacle = obj.GetComponent<UnityEngine.AI.NavMeshObstacle>();
					if (null == obstacle)
					{
						obstacle = obj.AddComponent<UnityEngine.AI.NavMeshObstacle>();
					}
					obstacle.carving = true;
					var meshFilters = obj.FindComponentsInChildren<MeshFilter>();
					if (!meshFilters.IsNullOrEmpty())
					{
						foreach (var component in meshFilters)
						{
							Component.DestroyImmediate(component, true);
						}
					}
					var meshRenderers = obj.FindComponentsInChildren<MeshRenderer>();
					if (!meshRenderers.IsNullOrEmpty())
					{
						foreach (var component in meshRenderers)
						{
							Component.DestroyImmediate(component, true);
						}
					}
				}
			}
		}

		public void ActionEventPlayEffectAt(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return;
			}
			string path = name;
			Vector3 epPosition;
			
			GameObject ep = null;
			var i1 = name.IndexOf('[');
			if (0 <= i1)
			{
				path = name.Substring(0, i1);
				var i2 = name.IndexOf(']', i1);
				var start = i1+1;
				var len = i2-start;
				if (0 < len)
				{
					var n = int.Parse(name.Substring(start, len));
					ep = GetEffectPoint(n);
				}
			}
			if (null == ep)
			{
				epPosition = transform.position;
			}
			else
			{
				epPosition = ep.transform.position;
			}
			if (null != LuaLuancher.Me)
			{
				LuaLuancher.Me.Call("PlayEffect_OneShotAt", path, epPosition.x, epPosition.y, epPosition.z);
			}
		}

		protected virtual void Reset()
		{
			ScanPoints();
			ScanObstacles();
		}

		protected virtual void Start()
		{
			if (!effectConnectPoints.IsNullOrEmpty())
			{
				var dictionary = EffectDictionary.global;
				if (null != dictionary)
				{
					var loader = ResourceManager.Loader;
					foreach (var ecp in effectConnectPoints)
					{
						var effectInfo = dictionary.GetEffectInfo(ecp.ID);
						if (null != effectInfo)
						{
							var prefab = loader.SLoad<GameObject>(ResourcePathHelper.IDEffect(effectInfo.path));
							if (null != prefab)
							{
								var effect = GameObject.Instantiate(prefab) as GameObject;
								effect.name = prefab.name;
								effect.transform.ResetParent(ecp.ep.transform);
								GameObjectUtil.Instance.ChangeLayersRecursively(effect, ecp.ep.gameObject.layer);
							}
						}
					}
				}
			}
		}

		protected virtual void OnDestroy()
		{
//			effectPoints_ = null;
//			connectPoints_ = null;
//			effectConnectPoints_ = null;
		}
	}
} // namespace RO
