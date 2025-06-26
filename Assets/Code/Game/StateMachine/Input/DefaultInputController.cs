using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Ghost.Extensions;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	sealed public class DefaultInputController : InputController 
	{
//		private static List<RoleComplete> tempRoleList = new List<RoleComplete>();

		public bool disableMove = false;
		public bool disableZoom = false;
		public bool disableLock = false;
		public float clickRoleRange = 1f;

#if OBSOLETE
		private Transform flag{get;set;}
		private ResourceID flagResourceID_ = null;
		public ResourceID flagResourceID
		{
			get
			{
				return flagResourceID_;
			}
			set
			{
				if (value == flagResourceID)
				{
					return;
				}
				if (null != flagResourceID && null != value && string.Equals(flagResourceID.IDStr, value.IDStr))
				{
					return;
				}
				flagResourceID_ = value;
				if (null != flagResourceID)
				{
					if (null != flag)
					{
						GameObject.Destroy(flag);
						flag = null;
					}
					var prefab = ResourceManager.Me.Load<GameObject>(flagResourceID);
					if (null != prefab)
					{
						var go = GameObject.Instantiate<GameObject>(prefab);
						go.name = prefab.name;
						go.SetActive(false);
						flag = go.transform;
						if (null != roleController)
						{
							roleController.flag = flag;
						}
					}
				}
			}
		}

		private RoleBaseController roleController{get;set;}
#endif

		private bool valid{get;set;}

		public DefaultInputController(InputHelper[] ihs)
			: base(ihs)
		{

		}

#if OBSOLETE
		public void ClickTerrain(Vector3 position)
		{
			var role = Player.Me.role;
			if (null == role) {
				return;
			}
			if (null == roleController)
			{
				roleController = new RoleBaseController(role);
			}
			roleController.flag = flag;
			roleController.destination = position;
			role.SetController(roleController);
			
			// notify ManualControlled
			role.OnManualControlled();
		}

		private void TryClickTerrain(Vector3 position)
		{
			if (0 < clickRoleRange)
			{
//				position = NavMeshAdjustY.SamplePosition(position);
				var role = Player.Me.role;
				var minDistance = float.PositiveInfinity;
				var target = RolePicker.PickRole(delegate(RoleAgent x, RoleAgent y){
					if (role == y || !y.accessable)
					{
						return -1;
					}
					var distance = Vector3.Distance(position, y.position);
					if (clickRoleRange < distance)
					{
						return -1;
					}
					if (minDistance > distance)
					{
						minDistance = distance;
						return 1;
					}
					return -1;
				});
				if (null != target)
				{
					ClickAccessableObject(target.accessableInfo);
					return;
				}
			}
			ClickTerrain(position);
		}

		private void ClickAccessableObject(Accessable accessableObj)
		{
			if (!disableLock && null != accessableObj)
			{
				var role = Player.Me.role;
				if (null == role) 
				{
					return;
				}

				// notify ManualControlled
				role.OnManualControlled();
				
				// lock
				role.lockTarget = accessableObj;
				
				if (!disableMove)
				{
					// access
					var objData = accessableObj.data; 
					if (null != objData && RoleInfo.Camp.ENEMY == objData.camp)
					{
						// attack
						role.AttackTarget(accessableObj);
					}
					else if (null != objData && RoleInfo.Camp.FRIEND == objData.camp)
					{
						// do nothing
					}
					else
					{
						role.Access(accessableObj);
					}
				}
			}
		}

		private bool TryHitAccessableObject(RoleAgent role, Ray ray)
		{
			var hits = Physics.RaycastAll(ray, float.PositiveInfinity, LayerMask.GetMask(Config.Layer.ACCESSABLE.Key));
			if (!hits.IsNullOrEmpty())
			{
				RoleAgent hitedEnemy = null;
				RoleAgent hitedNeutral = null;
				RoleAgent hitedFriend = null;
				int foundAll = 0;
				for (int i = 0; i < hits.Length && 3 > foundAll; ++i)
				{
					var hit = hits[i];
					var hitedRole = hit.collider.gameObject.GetComponent<RoleAgent>();
					if (null != hitedRole && null != hitedRole.data)
					{
						if (null == hitedEnemy)
						{
							if (RoleInfo.Camp.ENEMY == hitedRole.data.camp)
							{
								hitedEnemy = hitedRole;
								++foundAll;
								continue;
							}
						}
						
						if (null == hitedNeutral)
						{
							if (RoleInfo.Camp.NEUTRAL == hitedRole.data.camp)
							{
								hitedNeutral = hitedRole;
								++foundAll;
								continue;
							}
						}
						
						if (null == hitedFriend)
						{
							if (RoleInfo.Camp.FRIEND == hitedRole.data.camp)
							{
								hitedFriend = hitedRole;
								++foundAll;
								continue;
							}
						}
					}
				}
				if (null != hitedEnemy)
				{
					ClickAccessableObject(hitedEnemy.accessableInfo);
				}
				else if (null != hitedNeutral)
				{
					ClickAccessableObject(hitedNeutral.accessableInfo);
				}
				else if (null != hitedFriend)
				{
					ClickAccessableObject(hitedFriend.accessableInfo);
				}
				else
				{
					ClickAccessableObject(hits[0].collider.gameObject.GetComponent<Accessable>());
				}
				return true;
			}
			return false;
		}

		private void TryHitTerrain(RoleAgent role, Ray ray)
		{
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, float.PositiveInfinity, LayerMask.GetMask(Config.Layer.TERRAIN.Key)))
			{
				TryClickTerrain(hit.point);
			}
			else
			{
				var terrainPlane = new Plane(Vector3.up, role.position);
				float enter;
				if (terrainPlane.Raycast(ray, out enter))
				{
					TryClickTerrain(ray.GetPoint(enter));
				}
			}
		}
#endif
	
		protected override bool DoAllowInterruptedBy(State other)
		{
			if (null != (other as JoystickInputController)
			    || null != (other as ZoomInputController)
			    || null != (other as CameraInputController)
			    || null != (other as CameraFieldOfViewInputController)
			    || null != (other as CameraGyroInputController))
			{
				return true;
			}
			return base.DoAllowInterruptedBy(other);
		}

		protected override void OnTouchBegin()
		{
			if (isOverUI)
			{
				return;
			}

			if (0 != pointerID)
			{
				if (1 == pointerID && !disableZoom)
				{
					var state0 = GetState(0);
					if (null != state0 && state0.touching && !state0.beginOnUI)
					{
						DelayExit();
						InputManager.Me.SwitchToZoom();
					}
				}
			}
			else
			{
				valid = true;
			}
		}

		protected override void OnTouchMoved()
		{
			if (!valid)
			{
				return;
			}

			if (isOverUI)
			{
				return;
			}

			if (0 != pointerID)
			{
				if (1 == pointerID && !disableZoom)
				{
					var state0 = GetState(0);
					if (null != state0 && state0.touching && !state0.beginOnUI
					    && !beginOnUI)
					{
						DelayExit();
						InputManager.Me.SwitchToZoom();
					}
				}
			}
			else
			{
				if (!beginOnUI/* && !disableMove*/)
				{
					DelayExit();
					InputManager.Me.SwitchToJoystick(touchBeginPoint);
				}
			}
		}

		protected override void OnTouchEnd()
		{
			if (!valid)
			{
				return;
			}

			if (0 != pointerID)
			{
				return;
			}

			if (isOverUI || beginOnUI)
			{
				return;
			}

			if (null != InputManager.Me && InputManager.Model.PHOTOGRAPH == InputManager.Me.model)
			{
				return;
			}

			if (null != LuaLuancher.Me)
			{
				Ray ray = Camera.main.ScreenPointToRay(touchPoint);

				var hits = Physics.RaycastAll(ray, float.PositiveInfinity, LayerMask.GetMask(Config.Layer.ACCESSABLE.Key));
				if (!hits.IsNullOrEmpty())
				{
					int clickPriority = 99999999;
					LuaGameObjectClickable hitedObj = null;
					RoleComplete hitedRole = null;
					for (int i = 0; i < hits.Length; ++i)
					{
						var obj = hits[i].collider.gameObject.GetComponent<LuaGameObjectClickable>();
						if (null != obj)
						{
							if (obj.clickPriority < clickPriority)
							{
								clickPriority = obj.clickPriority;
								hitedObj = obj;
								hitedRole = null;
								if (0 >= clickPriority)
								{
									break;
								}
							}
						}
						else
						{
							var role = hits[i].collider.gameObject.GetComponentInParent<RoleComplete>();
							if (null != role)
							{
								if (role.clickPriority < clickPriority)
								{
									clickPriority = role.clickPriority;
									hitedRole = role;
									hitedObj = null;
									if (0 >= clickPriority)
									{
										break;
									}
								}
							}
						}
					}

					if (null != hitedRole)
					{
						LuaLuancher.Me.Call("Input_ClickRole", hitedRole.GUID);
					}
					else if (null != hitedObj)
					{
						LuaLuancher.Me.Call("Input_ClickObject", hitedObj);
					}
				}
				else if (!disableMove)
				{
					RaycastHit hit;
					if (Physics.Raycast(ray, out hit, float.PositiveInfinity, LayerMask.GetMask(Config.Layer.TERRAIN.Key)))
					{
						LuaLuancher.Me.Call("Input_ClickTerrain", hit.point.x, hit.point.y, hit.point.z);
					}
					else
					{
						Transform terrainTransform = null;
						if (null != LuaLuancher.Me && null != LuaLuancher.Me.myself)
						{
							terrainTransform = LuaLuancher.Me.myself.transform;
						}
						else if (null != Map2DManager.Me)
						{
							var map2D = Map2DManager.Me.GetMap2D();
							if (null != map2D)
							{
								terrainTransform = map2D.transform;
							}
						}
						if (null != terrainTransform)
						{
							var terrainPlane = new Plane(Vector3.up, terrainTransform.position);
							float enter;
							if (terrainPlane.Raycast(ray, out enter))
							{
								var hitPosition = ray.GetPoint(enter);
								LuaLuancher.Me.Call("Input_ClickTerrain", hitPosition.x, hitPosition.y, hitPosition.z);
							}
						}
					}
				}
			}
		}
	
	}
} // namespace RO
