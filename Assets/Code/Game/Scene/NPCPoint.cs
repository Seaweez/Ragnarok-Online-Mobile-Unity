using UnityEngine;
using System.Collections;
using Ghost.Utils;
using Ghost.Attribute;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public abstract class NPCInfo : MonoBehaviour
	{
		#if DEBUG_DRAW
		private const float DEBUG_DRAW_RADIUS = 0.5f;
		#endif // DEBUG_DRAW

		[System.Flags]
		[SLua.CustomLuaClassAttribute]
		public enum Behaviour
		{
			NONE = 0,
			MOVEABLE = 1 << 0,
			ATTACK_BACK = 1 << 1,
			OUT_RANGE_BACK = 1 << 2,
			PICK_UP = 1 << 3,
			ASSIST_ATTACK = 1 << 4,
			SELECT_TARGET = 1 << 5,
			PASSIVITY_SELECT_TARGET = 1 << 6,
			DAMANGE_ALWAYS_1 = 1 << 7,
			CAST = 1 << 8,
			GEAR = 1 << 9,
			SKILL_NONSELECTABLE = 1 << 10,
			GHOST = 1 << 11,
			DEMON = 1 << 12,
			FLY = 1 << 13,
			STEAL_CAMERA = 1 << 14,
			NAUGHTY = 1 << 15,
			ALERT = 1 << 16,
			EXPEL = 1 << 17,
		}

		[SerializeField, SetProperty("UniqueID")]
		private long UniqueID_ = 0;
		public long UniqueID
		{
			get
			{
				return UniqueID_;
			}
			set
			{
				UniqueID_ = value;
				RefreshName();
			}
		}
		
		[SerializeField, SetProperty("ID")]
		private long ID_ = 0;
		public long ID
		{
			get
			{
				return ID_;
			}
			set
			{
				ID_ = value;
				RefreshName();
			}
		}
		public string mapIcon = string.Empty;

		public float rebornTime = 0.0f;
		public float bornRange = 0.0f;
		public float territory = 0.0f;
		public float searchRange = 0.0f;
		public float scaleMin = 1f;
		public float scaleMax = 1f;
		public int count = 1;
		public int ai = 0;
		public int level = 0;
		public int life = 0;
		public int originState = 0;
		public int pursue = 0;
		public int pursueTime = 0;
		public bool Private = false;

#if OBSOLETE
		public RoleInfo.Camp camp = RoleInfo.Camp.NEUTRAL;
#endif
		public bool moveable = false;
		public bool attackBack = false;
		public bool outRangeBack = false;
		public bool pickUp = false;
		public bool assistAttack = false;
		public bool selectTarget = false;
		public bool passivitySelectTarget = false;
		public bool damageAlways_1 = false;
		public bool cast = false;
		public bool gear = false;
		public bool skillNonselectable = false;
		public bool ghost = false;
		public bool demon = false;
		public bool fly = false;
		public bool setalCamera = false;
		public bool naughty = false;
		public bool alert = false;
		public bool expel = false;

		public string waiAction = string.Empty;

		public bool ignoreNavmesh = false;

		public bool local = false;
		public Transform parent = null;

//		public bool calcCanArrivedBornPoints = false;
//		public BornPoint[] canArrivedBornPoints;
		
		private Transform selfTransform_;
		
		public Behaviour behaviours
		{
			get
			{
				var v = Behaviour.NONE;
				if (moveable)
				{
					v |= Behaviour.MOVEABLE;
				}
				if (attackBack)
				{
					v |= Behaviour.ATTACK_BACK;
				}
				if (outRangeBack)
				{
					v |= Behaviour.OUT_RANGE_BACK;
				}
				if (pickUp)
				{
					v |= Behaviour.PICK_UP;
				}
				if (assistAttack)
				{
					v |= Behaviour.ASSIST_ATTACK;
				}
				if (selectTarget)
				{
					v |= Behaviour.SELECT_TARGET;
				}
				if (passivitySelectTarget)
				{
					v |= Behaviour.PASSIVITY_SELECT_TARGET;
				}
				if (damageAlways_1)
				{
					v |= Behaviour.DAMANGE_ALWAYS_1;
				}
				if (cast)
				{
					v |= Behaviour.CAST;
				}
				if (gear)
				{
					v |= Behaviour.GEAR;
				}
				if (skillNonselectable)
				{
					v |= Behaviour.SKILL_NONSELECTABLE;
				}
				if (ghost)
				{
					v |= Behaviour.GHOST;
				}
				if (demon)
				{
					v |= Behaviour.DEMON;
				}
				if (fly)
				{
					v |= Behaviour.FLY;
				}
				if (setalCamera)
				{
					v |= Behaviour.STEAL_CAMERA;
				}
				if (naughty)
				{
					v |= Behaviour.NAUGHTY;
				}
				if (alert)
				{
					v |= Behaviour.ALERT;
				}
				if (expel)
				{
					v |= Behaviour.EXPEL;
				}
				return v;
			}
		}
		
		public Vector3 position {
			
			get {
				return transform.position;
			}
		}

		protected virtual string namePrefix
		{
			get
			{
				return "";
			}
		}

		protected void RefreshName()
		{
			gameObject.name = string.Format("{0}NPC_{1}_{2}", namePrefix, ID, UniqueID);
		}

		void Reset()
		{
			RefreshName();
		}
		
		#if OBSOLETE
		void Start ()
		{
			if (local)
			{
				if (!(null != NPCPointManagerLocal.Me && NPCPointManagerLocal.Me.Add (this))) {
					GameObject.Destroy (gameObject);
				}
			}
			else
			{
				if (!(null != NPCPointManager.Me && NPCPointManager.Me.Add (this))) {
					GameObject.Destroy (gameObject);
				}
			}
		}

		void OnDestroy ()
		{
			if (local)
			{
				if (null != NPCPointManagerLocal.Me) {
					NPCPointManagerLocal.Me.Remove (this);
				}
			}
			else
			{
				if (null != NPCPointManager.Me) {
					NPCPointManager.Me.Remove (this);
				}
			}
		}
#endif
		
#if DEBUG_DRAW
		private void DebugDraw(Color color)
		{
			DebugUtils.DrawCircle(transform.position, new Quaternion(), DEBUG_DRAW_RADIUS, 30, color);
			Debug.DrawLine(transform.position, transform.position+transform.forward*DEBUG_DRAW_RADIUS, color);
			Gizmos.DrawIcon(transform.position, "np.png", true);
		}

		void OnDrawGizmos()
		{
			DebugDraw(Color.white);
		}
		
		void OnDrawGizmosSelected()
		{
			DebugDraw(Color.red);
		}
#endif // DEBUG_DRAW
	}

	[SLua.CustomLuaClassAttribute]
	public class NPCPoint : NPCInfo
	{
	}
}