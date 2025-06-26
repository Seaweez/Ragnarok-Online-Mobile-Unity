using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Ghost.Utils;
using Ghost.Extensions;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class CameraPointManager : AreaTriggerManager<CameraPointManager, CameraPoint>
	{
		public static CameraPointManager Instance
		{
			get
			{
				return Me;
			}
		}
		private Dictionary<int, CameraPoint> cameraPoints_ = new Dictionary<int, CameraPoint> ();

		interface CheckAdapter
		{
			int priority{get;}
			CameraPoint Check(Transform t);
		}
		class CameraPointCheckAdapter : CheckAdapter
		{
			public CameraPoint cp;
			public CameraPointCheckAdapter(CameraPoint c)
			{
				cp = c;
			}

			public int priority
			{
				get
				{
					return cp.priority;
				}
			}
			public CameraPoint Check(Transform t)
			{
				return cp.Check(t) ? cp : null;
			}
		}
		class CameraPointLinkCheckAdapter : CheckAdapter
		{
			public CameraPointLinkInfo link;
			public System.Predicate<Transform> checkPredicate;
			public CameraPointLinkCheckAdapter(CameraPointLinkInfo l, System.Predicate<Transform> p)
			{
				link = l;
				checkPredicate = p;
			}
			
			public int priority
			{
				get
				{
					return link.priority;
				}
			}
			public CameraPoint Check(Transform t)
			{
				return checkPredicate(t) ? link.tempCP : null;
			}
		}
		private List<CheckAdapter> checkAdapters = new List<CheckAdapter>();
		private Coroutine sortCheckAdaptersCoroutine = null;
		private void CheckAdaptersDirty()
		{
			if (null != sortCheckAdaptersCoroutine)
			{
				return;
			}
			sortCheckAdaptersCoroutine = StartCoroutine(SortCheckAdapters());
		}

		IEnumerator SortCheckAdapters()
		{
			yield return new WaitForEndOfFrame();
			checkAdapters.Sort(delegate(CheckAdapter x, CheckAdapter y) {
				return y.priority - x.priority;
			});
			sortCheckAdaptersCoroutine = null;
		}
		
		protected override bool DoAdd (CameraPoint cp)
		{
			if (!cp.IDValid)
			{
				checkAdapters.Add(new CameraPointCheckAdapter(cp));
				CheckAdaptersDirty();
				return true;
			}
			if (cameraPoints_.ContainsKey (cp.ID)) 
			{
				return false;
			}
			cameraPoints_.Add (cp.ID, cp);
			checkAdapters.Add(new CameraPointCheckAdapter(cp));
			CheckAdaptersDirty();
			return true;
		}
		
		protected override  void DoRemove (CameraPoint cp)
		{
			var adapter = checkAdapters.Find (delegate(CheckAdapter obj) {
				var cpAdapter = obj as CameraPointCheckAdapter;
				return null != cpAdapter && cpAdapter.cp == cp;
			});
			if (null != adapter)
			{
				checkAdapters.Remove(adapter);
			}
			if (!cameraPoints_.ContainsKey (cp.ID)) {
				return;
			}
			cameraPoints_.Remove (cp.ID);
		}
		
		public CameraPoint GetCameraPoint (int ID)
		{
			CameraPoint cp;
			if (cameraPoints_.TryGetValue (ID, out cp)) 
			{
				return cp;
			}
			return null;
		}

		private CameraController cameraController = null;
		public CameraController.Info originalDefaultInfo{get;private set;}

		private List<CameraPointLink> links = null;
		private CameraPointLinkInfo currentLink = null;

		private CameraPointGroup group = null;

		public void EnableGroup(int index)
		{
			SetGroupValidity(index, true);
		}

		public void DisableGroup(int index)
		{
			SetGroupValidity(index, false);
		}

		public bool SetGroup(CameraPointGroup g)
		{
			if (group == g)
			{
				return true;
			}
			var oldGroup = group;
			group = g;
			if (null != oldGroup)
			{
				GameObject.Destroy(oldGroup.gameObject);
			}
//			if (null != group)
//			{
//				group.transform.parent = transform;
//			}
			return true;
		}

		public void ClearGroup(CameraPointGroup g)
		{
			if (group != g)
			{
				return;
			}
//			if (null != group)
//			{
//				group.transform.parent = null;
//			}
			group = null;
		}

		private void SetGroupValidity(int index, bool valid)
		{
			if (null == group)
			{
				return;
			}
			group.SetValidity(index, valid);
		}

		public bool AddLink(CameraPointLink link)
		{
			if (null == links)
			{
				links = new List<CameraPointLink>();
			}
			if (links.Contains(link))
			{
				return false;
			}
			links.Add(link);
//			link.transform.parent = transform;
			if (!link.links.IsNullOrEmpty())
			{
				var linkCPParent = link.transform;
				for (int i = 0; i < link.links.Length; ++i)
				{
					var l = link.links[i];
					var linkInfo = l;
					checkAdapters.Add(new CameraPointLinkCheckAdapter(linkInfo, delegate(Transform t) {
						if (linkInfo.Check(t, linkCPParent))
						{
							var defaultInfo = originalDefaultInfo;
							if (null == defaultInfo)
							{
								if (null != cameraController)
								{
									defaultInfo = cameraController.defaultInfo;
								}
								else if (null != CameraController.Me)
								{
									defaultInfo = CameraController.Me.defaultInfo;
								}
							}
							linkInfo.UpdateTempCameraPoint(defaultInfo, t, linkCPParent);
							SetLink(linkInfo);
							return true;
						}
						return false;
					}));
				}
				CheckAdaptersDirty();
			}
			return true;
		}

		public void RemoveLink(CameraPointLink link)
		{
			if (null != links)
			{
				if (links.Remove(link))
				{
//					link.transform.parent = null;
				}
			}
			if (!link.links.IsNullOrEmpty())
			{
				for (int i = 0; i < link.links.Length; ++i)
				{
					var l = link.links[i];
					var adapter = checkAdapters.Find (delegate(CheckAdapter obj) {
						var linkAdapter = obj as CameraPointLinkCheckAdapter;
						return null != linkAdapter && linkAdapter.link == l;
					});
					if (null != adapter)
					{
						checkAdapters.Remove(adapter);
					}
				}
			}
		}

		protected override void OnTriggerChanged (CameraPoint oldCameraPoint, CameraPoint newCameraPoint)
		{
			if (null != newCameraPoint)
			{
				if (null == cameraController)
				{
					cameraController = CameraController.Me;
					if (null != cameraController)
					{
						originalDefaultInfo = cameraController.defaultInfo;
					}
				}
				
				if (null != cameraController)
				{
					CameraController.Info newInfo = newCameraPoint.info.CloneSelf();
					if (!newCameraPoint.focusViewPortValid)
					{
						newInfo.focus = originalDefaultInfo.focus;
						newInfo.focusOffset = originalDefaultInfo.focusOffset;
						newInfo.focusViewPort = originalDefaultInfo.focusViewPort;
					}
					else if (null == newInfo.focus)
					{
						newInfo.focus = originalDefaultInfo.focus;
					}
					if (!newCameraPoint.rotationValid)
					{
						newInfo.rotation = originalDefaultInfo.rotation;
					}
					cameraController.defaultInfo = newInfo;
					if (cameraController.beSingleton)
					{
						cameraController.RestoreDefault(newCameraPoint.duration);
					}
				}
			}
			else
			{
				if (null != cameraController && null != originalDefaultInfo)
				{
					cameraController.defaultInfo = originalDefaultInfo;
					if (cameraController.beSingleton)
					{
						float duration = 0;
						if (null != LuaLuancher.Me && LuaLuancher.Me.ignoreAreaTrigger)
						{
                            duration = 0.1f;
                        }
						else
						{
							duration = (null != oldCameraPoint) ? oldCameraPoint.duration : 0f;
						}
						cameraController.RestoreDefault(duration);
					}
					
					cameraController = null;
					originalDefaultInfo = null;
				}
			}
		}

		protected void OnLinkChanged(CameraPointLinkInfo oldLink, CameraPointLinkInfo newLink)
		{

		}

		protected void SetLink(CameraPointLinkInfo newLink)
		{
			var oldLink = currentLink;
			if (oldLink == newLink)
			{
				return;
			}
			
			currentLink = newLink;

			OnLinkChanged(oldLink, newLink);
		}

		protected override CameraPoint DoCheck (Transform t)
		{
			if (null == CameraController.Me)
			{
				SetLink(null);
				return null;
			}
			for (int i = 0; i < checkAdapters.Count; ++i)
			{
				var adapter = checkAdapters[i];
				var cp = adapter.Check(t);
				if (null != cp)
				{
					return cp;
				}
			}
			SetLink(null);
			return null;
//			if (null != links)
//			{
//				var defaultInfo = originalDefaultInfo;
//				if (null == defaultInfo)
//				{
//					if (null != cameraController)
//					{
//						defaultInfo = cameraController.defaultInfo;
//					}
//					else if (null != CameraController.Me)
//					{
//						defaultInfo = CameraController.Me.defaultInfo;
//					}
//				}
//				var linkCPParent = transform;
//				foreach (var link in links)
//				{
//					if (!link.links.IsNullOrEmpty())
//					{
//						foreach (var l in link.links)
//						{
//							if (l.Check(t, linkCPParent))
//							{
//								l.UpdateTempCameraPoint(defaultInfo, t, linkCPParent);
//								SetLink(l);
//								return l.tempCP;
//							}
//						}
//					}
//				}
//			}
//			SetLink(null);
//			return base.DoCheck (t);
		}

		protected override void LateUpdate ()
		{
			base.LateUpdate ();
			if (null != cameraController)
			{
				if (null != currentLink)
				{
					var newCameraPoint = currentLink.tempCP;
					CameraController.Info newInfo = newCameraPoint.info.CloneSelf();
					if (!newCameraPoint.focusViewPortValid)
					{
						newInfo.focus = originalDefaultInfo.focus;
						newInfo.focusOffset = originalDefaultInfo.focusOffset;
						newInfo.focusViewPort = originalDefaultInfo.focusViewPort;
					}
					else if (null == newInfo.focus)
					{
						newInfo.focus = originalDefaultInfo.focus;
					}
					if (!newCameraPoint.rotationValid)
					{
						newInfo.rotation = originalDefaultInfo.rotation;
					}
					cameraController.defaultInfo = newInfo;
					cameraController.SmoothTo(newInfo);
				}
				else if (null != currentTrigger && currentTrigger.surround)
				{
					var surroundTransform = (null != currentTrigger.info.focus) ? currentTrigger.info.focus : currentTrigger.transform;
					var angleY = GeometryUtils.GetAngleByAxisY(cameraController.activeCamera.transform.position, surroundTransform.position);
					var euler = currentTrigger.info.rotation;
					if (!currentTrigger.rotationValid)
					{
						euler = cameraController.targetRotationEuler;
					}
					euler.y = angleY;
					currentTrigger.info.rotation = euler;
					cameraController.RotateTo(euler);
				}
			}
		}

	}
} // namespace RO
