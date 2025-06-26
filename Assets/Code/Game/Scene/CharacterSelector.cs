using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	sealed class CharacterSelectorInputController : InputController 
	{
		public System.Action<GameObject> selectedListener = null;

		private bool valid{get;set;}
		
		public CharacterSelectorInputController()
			: base(new InputHelper[]{new InputHelper(0)})
		{

		}
		
		protected override void OnTouchBegin()
		{
			if (isOverUI)
			{
				return;
			}
			
			if (0 == pointerID)
			{
				valid = true;
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
			
			Ray ray = Camera.main.ScreenPointToRay(touchPoint);
			
			RaycastHit hit;
			
			int layerMast = LayerMask.GetMask(Config.Layer.ACCESSABLE.Key);
			if (Physics.Raycast(ray, out hit, float.PositiveInfinity, layerMast))
			{
				if (null != selectedListener)
				{
					selectedListener(hit.collider.gameObject);
				}
			}
			#region test
			else
			{
				if (null != selectedListener)
				{
					selectedListener(null);
				}
			}
			#endregion test
		}	
	}

	[SLua.CustomLuaClassAttribute]
	public class CharacterSelector : MonoBehaviour
 	{
		public Animator rootAnimator = null;
		public System.Action<GameObject> selectedListener = null;
		public System.Action updateListener = null;
		
		private CharacterSelectorInputController inputController = null;
		private bool running = false;

		public void Launch()
		{
			if (running)
			{
				return;
			}

			running = true;

			if (null == inputController)
			{
				inputController = new CharacterSelectorInputController();
				inputController.selectedListener = SetSelectedRole;
			}

			inputController.Enter();
		}

		public void Shutdown()
		{
			if (!running)
			{
				return;
			}
			running = false;

			if (null != inputController)
			{
				inputController.Exit();
			}
		}

		private void SetSelectedRole(GameObject obj)
		{
			if (null != selectedListener)
			{
				selectedListener(obj);
			}
		}

		void Start()
		{
			if (null != LuaLuancher.Me)
			{
				LuaLuancher.Me.Call("OnCharacterSelectorStart", this);
			}
		}

		void OnDestroy()
		{
			if (null != LuaLuancher.Me)
			{
				LuaLuancher.Me.Call("OnCharacterSelectorDestroy", this);
			}
		}

		void LateUpdate()
		{
			if (null != updateListener)
			{
				updateListener();
			}
			if (!running)
			{
				return;
			}
			if (null != inputController)
			{
				inputController.Update();
			}
		}
	}
} // namespace RO
