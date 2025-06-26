using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Ghost.Extensions;
using Ghost.Utils;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public partial class RoleComplete : MonoBehaviour 
	{
		public long GUID;
		public int clickPriority = 0;
		public RolePart[] parts;
		public int[] partColorIndexes;
		public Transform tempOwner;
		public Transform rendererNode;
		public SpriteRenderer shadow;
		public BoxCollider shadowCollider;
		public AudioSource audioSource;

		public RolePartBody body{get;private set;}
		public RolePartMount mount{get;private set;}
		public RolePart hair{get{return parts[1];}}
		public RolePart eye{get{return parts[8];}}
		public RolePart leftWeapon{get{return parts[2];}}
		public RolePart rightWeapon{get{return parts[3];}}

		public void SetPartCount(int count)
		{
			if (null == parts || parts.Length != count)
			{
				parts = new RolePart[count];
			}
		}

		public void DressPart(int i, RolePart part)
		{
			part.SetActionSpeed(actionSpeed);
			part.SwitchColor(GetPartColorIndex(i));
			
			if (0 == i)
			{
				// body
				part.gameObject.SetActive(true);
				for (int j = 1; j < parts.Length-1; ++j)
				{
					// others
					var otherPart = parts[j];
					if (null != otherPart)
					{
						otherPart.transform.ResetParent(part.GetCP(j));
						if (2 == j || 3 == j)
						{
							// weapon
							otherPart.gameObject.SetActive(weaponEnable);
						}
						else
						{
							otherPart.gameObject.SetActive(true);
						}
					}
				}

				body = part as RolePartBody;
				
				if (null != mount && mountEnable)
				{
					body.transform.ResetParent(mount.GetCP(0));
				}
				else
				{
					body.transform.ResetParent(rendererNode);
				}

				if (null != body.collider)
				{
					body.collider.enabled = colliderEnable;
				}

				body.owner = this;

				MoveEffect();

				UpdateShadow();
			}
			else 
			{
				// others
				if (parts.Length-1 == i)
				{
					// mount
					mount = part as RolePartMount;
					mount.transform.ResetParent(rendererNode);
					if (null != body && mountEnable)
					{
						mount.gameObject.SetActive(true);
						body.transform.ResetParent(mount.GetCP(0));
					}
					else
					{
						mount.gameObject.SetActive(false);
					}
				}
				else
				{
					if (null != body)
					{
						part.transform.ResetParent(body.GetCP(i));
						if (2 == i || 3 == i)
						{
							// weapon
							part.gameObject.SetActive(weaponEnable);
						}
						else
						{
							part.gameObject.SetActive(true);
						}
					}
					else
					{
						part.transform.ResetParent(rendererNode);
						part.gameObject.SetActive(false);
					}
				}

				if (null != body && null != body.mainAnimator && part.gameObject.activeSelf)
				{
					var currentActionInfo = body.mainAnimator.GetCurrentAnimatorStateInfo(0);
					if (currentActionPlaying)
					{
						part.PlayAction(
							currentActionInfo.shortNameHash, 
							currentActionInfo.shortNameHash, 
							currentActionInfo.speed, 
							currentActionInfo.normalizedTime);
					}
					else
					{
						part.PlayAction(
							currentActionNameHash, 
							currentDefaultActionNameHash, 
							actionSpeed, 
							0);
					}
				}
			}
		}

		public void UndressPart(int i, RolePart part)
		{
			if (0 == i)
			{
				// body
				if (null != mount)
				{
					part.transform.ResetParent(rendererNode);
				}
				
				for (int j = 1; j < parts.Length-1; ++j)
				{
					// others
					var otherPart = parts[j];
					if (null != otherPart)
					{
						otherPart.transform.ResetParent(rendererNode);
						otherPart.gameObject.SetActive(false);
					}
				}
			}
			else if (parts.Length-1 == i)
			{
				// mount
				if (null != body)
				{
					body.transform.ResetParent(rendererNode);
				}
			}
			else
			{
				// others
				part.transform.ResetParent(rendererNode);
			}
		}

		public void SetPart(int i, RolePart part, bool dress)
		{
			var oldPart = parts[i];
			if (oldPart == part)
			{
				return;
			}
			parts[i] = part;

			if (0 == i)
			{
				// body
				ActionCallback();
			}

			if (null != oldPart)
			{
				UninitPart(oldPart);
				if (0 == i)
				{
					// body
					var oldBody = oldPart as RolePartBody;
					oldBody.owner = null;
					body = null;
				}
				else
				{
					if (parts.Length-1 == i)
					{
						// mount
						mount = null;
					}
				}
			}

			if (null != part)
			{
				InitPart(part);
			}

			if (dress)
			{
				if (null != part)
				{
					DressPart(i, part);
				}
				else if (null != oldPart)
				{
					UndressPart(i, oldPart);
				}
			}
		}

		public void InitPart(RolePart part)
		{
			part.layer = layer;
			InitPart_Render(part);
		}

		public void UninitPart(RolePart part)
		{
			part.layer = 0;
			UninitPart_Render(part);
		}

		public void ApplyLayer()
		{
			var newLayer = layer;
			for (int i = 0; i < parts.Length; ++i)
			{
				var part = parts[i];
				if (null != part)
				{
					part.layer = newLayer;
				}
			}
		}

		#region part interface
		private int _layer = 0;
		public int layer
		{
			get
			{
				return invisible ? Config.Layer.INVISIBLE.Value : _layer;
			}
			set
			{
				var oldLayer = layer;
				_layer = value;
				var newLayer = layer;
				if (oldLayer != newLayer)
				{
					ApplyLayer();
				}
			}
		}

		private bool _invisible = false;
		public bool invisible
		{
			get
			{
				return _invisible;
			}
			set
			{
				if (invisible == value)
				{
					return;
				}
				_invisible = value;
				ApplyLayer();
			}
		}

		private bool _shadowInvisible = false;
		public bool shadowInvisible
		{
			get
			{
				return _shadowInvisible;
			}
			set
			{
				if (shadowInvisible == value)
				{
					return;
				}
				_shadowInvisible = value;
				if (shadowInvisible)
				{
					shadow.gameObject.layer = Config.Layer.INVISIBLE.Value;;
				}
				else
				{
					shadow.gameObject.layer = Config.Layer.ACCESSABLE.Value;
				}
			}
		}

		public bool shadowEnable
		{
			set
			{
				shadow.enabled = value;
			}
		}

		private bool _colliderEnable = false;
		public bool colliderEnable
		{
			get
			{
				return _colliderEnable;
			}
			set
			{
				_colliderEnable = value;
				if (colliderEnable)
				{
					if (null != body && null != body.collider)
					{
						body.collider.enabled = true;
					}
					shadowCollider.enabled = true;
				}
				else
				{
					if (null != body && null != body.collider)
					{
						body.collider.enabled = false;
					}
					shadowCollider.enabled = false;
				}
			}
		}

		private bool _weaponEnable = false;
		public bool weaponEnable
		{
			get
			{
				return _weaponEnable;
			}
			set
			{
				_weaponEnable = value;
				if (null != leftWeapon)
				{
					leftWeapon.gameObject.SetActive(weaponEnable);
				}
				if (null != rightWeapon)
				{
					rightWeapon.gameObject.SetActive(weaponEnable);
				}
			}
		}

		private bool _mountEnable = false;
		public bool mountEnable
		{
			get
			{
				return _mountEnable;
			}
			set
			{
				_mountEnable = value;
				if (mountEnable)
				{
					if (null != mount)
					{
						mount.gameObject.SetActive(true);
						if (null != body)
						{
							body.transform.ResetParent(mount.GetCP(0));
						}
					}
				}
				else
				{
					if (null != mount)
					{
						if (null != body)
						{
							body.transform.ResetParent(rendererNode);
						}
						mount.gameObject.SetActive(false);
					}
				}
			}
		}

		public bool SetPartColorIndex(int part, int i)
		{
			if (null == partColorIndexes || !partColorIndexes.CheckIndex(part))
			{
				return false;
			}
			partColorIndexes[part] = i;
			return true;
		}
		public int GetPartColorIndex(int part)
		{
			if (null == partColorIndexes || !partColorIndexes.CheckIndex(part))
			{
				return -1;
			}
			return partColorIndexes[part];
		}

		public int bodyColorIndex
		{
			get
			{
				return GetPartColorIndex(0);
			}
			set
			{
				if (SetPartColorIndex(0, value) && null != body)
				{
					body.SwitchColor(value);
				}
			}
		}

		public int hairColorIndex
		{
			get
			{
				return GetPartColorIndex(1);
			}
			set
			{
				if (SetPartColorIndex(1, value) && null != hair)
				{
					hair.SwitchColor(value);
				}
			}
		}

		public int eyeColorIndex
		{
			get
			{
				return GetPartColorIndex(8);
			}
			set
			{
				if (SetPartColorIndex(8, value) && null != eye)
				{
					eye.SwitchColor(value);
				}
			}
		}

		private float _actionSpeed = 1f;
		public float actionSpeed
		{
			get
			{
				return _actionSpeed;
			}
			set
			{
				_actionSpeed = value;
				for (int i = 0; i < parts.Length; ++i)
				{
					var part = parts[i];
					if (null != part)
					{
						part.SetActionSpeed(actionSpeed);
					}
				}
			}
		}

		public bool actionLoop{get;private set;}
		public System.Action<long, object> actionCallback{get;private set;}
		public object actionCallbackArg{get;private set;}
		private bool actionCallbackValid = true;
		private int currentActionNameHash = 0;
		private int currentDefaultActionNameHash = 0;
		private bool currentActionPlaying = false;

		public bool HasAction(int nameHash)
		{
			return null != body && body.HasAction(nameHash);
		}
		
		public void PlayAction(
			int nameHash, 
			int defaultNameHash, 
			float speed, 
			float normalizedTime, 
			bool loop, 
			System.Action<long, object> callback,
			object callbackArg)
		{
			if (null == body)
			{
				return;
			}
			if (actionSpeed != speed)
			{
				actionSpeed = speed;
			}
			actionLoop = loop;
			actionCallback = callback;
			actionCallbackArg = callbackArg;
			if (null != actionCallback)
			{
				SetActionCallbackValid();
			}

			currentActionNameHash = nameHash;
			if (!HasAction(nameHash))
			{
				nameHash = defaultNameHash;
				if (null != LuaLuancher.Me && !HasAction(nameHash))
				{
					currentActionNameHash = LuaLuancher.Me.defaultActionNameHash;
				}
				else
				{
					currentActionNameHash = nameHash;
				}
			}

			currentDefaultActionNameHash = defaultNameHash;
			currentActionPlaying = false;
			for (int i = 0; i < parts.Length; ++i)
			{
				var part = parts[i];
				if (null != part)
				{
					part.PlayAction(nameHash, defaultNameHash, speed, normalizedTime);
				}
			}
		}

		public Transform GetEP(int i)
		{
			return null != body ? body.GetEP(i) : null;
		}

		public Transform GetCP(int i)
		{
			return null != body ? body.GetCP(i) : null;
		}
		
		public void MoveEffect()
		{
			var effectList = EffectHandle.tempEffectList;

			var fromEP = tempOwner;
			var childCount = fromEP.childCount;
			for (int j = 0; j < childCount; ++j)
			{
				var child = fromEP.GetChild(j);
				var effect = child.GetComponent<EffectHandle>();
				if (null != effect && 0 < effect.epID)
				{
					effectList.Add(effect);
				}
			}
			
			for (int i = 0; i < effectList.Count; ++i)
			{
				var effect = effectList[i];
				var toEP = GetEP(effect.epID);
				if (null != toEP)
				{
					effect.transform.SetParent(toEP, false);
				}
			}
			
			effectList.Clear();
		}

		public void AdjustProjector(Projector projector, float projectorHeight, float sizeScale)
		{
			if (null != body)
			{
				body.AdjustProjector(projector, projectorHeight, sizeScale);
			}
			else
			{
				projector.fieldOfView = 30;
			}
			projector.farClipPlane = projector.transform.localPosition.y * transform.lossyScale.y * 2 + 1;
		}
		#endregion part interface

		private Coroutine actionCallbackValidCoroutine = null;
		public void SetActionCallbackValid()
		{
			if (null != actionCallbackValidCoroutine)
			{
				return;
			}
			if (gameObject.activeInHierarchy)
			{
				actionCallbackValid = false;
				actionCallbackValidCoroutine = StartCoroutine(DoSetActionCallbackValid());
			}
		}

		IEnumerator DoSetActionCallbackValid()
		{
			yield return new WaitForEndOfFrame();
			actionCallbackValid = true;
			actionCallbackValidCoroutine = null;
		}
		public void ActionCallback()
		{
			if (null != actionCallback)
			{
				actionCallback(GUID, actionCallbackArg);
				actionCallback = null;
			}
			actionCallbackValid = false;
		}

		private Coroutine updateShadowCoroutine = null;
		public void UpdateShadow()
		{
			if (null != updateShadowCoroutine)
			{
				return;
			}
			if (gameObject.activeInHierarchy)
			{
				updateShadowCoroutine = StartCoroutine(DoUpdateShadow());
			}
		}

		IEnumerator DoUpdateShadow()
		{
			if (null != body)
			{
				body.UpdateCollider();
			}
			yield return new WaitForEndOfFrame();
			if (null != body)
			{
				body.AdjustShadow(shadow);
			}
			else
			{
				shadow.transform.localScale = Vector3.one;
			}
			updateShadowCoroutine = null;
		}

		#region behaviour
		void Update()
		{
			Update_Render();
			if (null != body && null != body.mainAnimator)
			{
				if (!currentActionPlaying)
				{
					var currentActionInfo = body.mainAnimator.GetCurrentAnimatorStateInfo(0);
					if (currentActionInfo.shortNameHash == currentActionNameHash)
					{
						currentActionPlaying = true;
					}
				}

				if (null != actionCallback || actionLoop)
				{
					var nextActionInfo = body.mainAnimator.GetNextAnimatorStateInfo(0);
					if (!nextActionInfo.IsValid())
					{
						var currentActionInfo = body.mainAnimator.GetCurrentAnimatorStateInfo(0);
						if (currentActionNameHash == currentActionInfo.shortNameHash && 1 < currentActionInfo.normalizedTime)
						{
							if (actionCallbackValid)
							{
								ActionCallback();
							}
							if (actionLoop && !currentActionInfo.loop)
							{
								PlayAction(
									currentActionInfo.shortNameHash, 
									currentDefaultActionNameHash, 
									actionSpeed, 
									0, 
									true,
									null, 
									null);
							}
						}
					}
				}
			}
			else
			{
				ActionCallback();
				currentActionPlaying = false;
			}
		}

		void OnDestroy()
		{
			ActionCallback();
			if (null != updateShadowCoroutine)
			{
				StopCoroutine(updateShadowCoroutine);
				updateShadowCoroutine = null;
			}
		}

#if DEBUG_DRAW
		[SLua.DoNotToLuaAttribute]
		public Vector2 dd_RectOffset = Vector2.zero;
		[SLua.DoNotToLuaAttribute]
		public Vector2 dd_RectSize = Vector2.one;
		[SLua.DoNotToLuaAttribute]
		public float dd_Range = 0f;
		private void DebugDraw(Color color)
		{
			Gizmos.color = color;

			var rect = new Rect();
			rect.size = dd_RectSize;
			rect.center = dd_RectOffset;

			var p1 = transform.TransformPoint(new Vector3(rect.xMin, 0, rect.yMin));
			var p2 = transform.TransformPoint(new Vector3(rect.xMax, 0, rect.yMin));
			var p3 = transform.TransformPoint(new Vector3(rect.xMax, 0, rect.yMax));
			var p4 = transform.TransformPoint(new Vector3(rect.xMin, 0, rect.yMax));
			Gizmos.DrawLine(p1, p2);
			Gizmos.DrawLine(p2, p3);
			Gizmos.DrawLine(p3, p4);
			Gizmos.DrawLine(p4, p1);

			if (0 < dd_Range)
			{
				DebugUtils.DrawCircle(transform.position, new Quaternion(), dd_Range, 50, color);
			}
		}
		
		void OnDrawGizmos()
		{
			DebugDraw(Color.green);
		}
		
		void OnDrawGizmosSelected()
		{
			DebugDraw(Color.red);
		}
#endif // DEBUG_DRAW
		#endregion behaviour

		#region action event
		[SLua.DoNotToLuaAttribute]
		public void ActionEventFire()
		{
			if (0 != GUID && null != LuaLuancher.Me)
			{
				LuaLuancher.Me.Call("Creature_Fire", GUID);
			}
		}
        [SLua.DoNotToLuaAttribute]
        public void ActionEventInterrupt()
        {
            if (0 != GUID && null != LuaLuancher.Me)
            {
                LuaLuancher.Me.Call("Creature_Interrupt", GUID);
            }
        }

        [SLua.DoNotToLuaAttribute]
		public void ActionEventDead()
		{
			if (0 != GUID && null != LuaLuancher.Me)
			{
				LuaLuancher.Me.Call("Creature_Dead", GUID);
			}
		}
		#endregion action event
	}
} // namespace RO
