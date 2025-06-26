using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Ghost.Utils;

namespace RO
{
    [SLua.CustomLuaClassAttribute]
    sealed public class JoystickInputController : InputController
    {
        private const int CENTER_SPRITE_INDEX = 0;
        private const int BUTTON_SPRITE_INDEX = 1;

        public bool holdEnable = false;
        public Vector3 centerPoint { get; set; }
        private Quaternion cameraPrevRotation { get; set; }

        private SpriteFade drawer = null;

        private float radius_;

#if OBSOLETE
		private RoleMoveDirectionController roleController{get;set;}
#endif

        public JoystickInputController(InputHelper[] ihs, SpriteFade d)
            : base(ihs)
        {
            drawer = d;
            ResetSpriteSize();
        }

        private void ResetSpriteSize()
        {
            radius_ = Screen.height / 5.0f;

            var centerSize = radius_ * 1.5f;
            var buttonSize = radius_;
            SetSpriteSize(CENTER_SPRITE_INDEX, new Vector2(centerSize, centerSize));
            SetSpriteSize(BUTTON_SPRITE_INDEX, new Vector2(buttonSize, buttonSize));
        }

        private void SetSpriteSize(int index, Vector2 size)
        {
            if (null == drawer || null == drawer.spriteInfos || index >= drawer.spriteInfos.Length)
            {
                return;
            }
            drawer.spriteInfos[index].drawRect.size = size;
        }
        private void SetSpritePosition(int index, Vector2 position)
        {
            if (null == drawer || null == drawer.spriteInfos || index >= drawer.spriteInfos.Length)
            {
                return;
            }
            drawer.spriteInfos[index].drawRect.center = position;
        }

        protected override bool DoEnter()
        {
            if (!base.DoEnter())
            {
                return false;
            }

            ResetSpriteSize();

            var tempPoint = centerPoint;
            tempPoint.y = Screen.height - tempPoint.y;
            SetSpritePosition(CENTER_SPRITE_INDEX, tempPoint);
            SetSpritePosition(BUTTON_SPRITE_INDEX, tempPoint);

            if (null != drawer)
            {
                drawer.FadeIn();
            }
            return true;
        }

        protected override void DoExit()
        {
            base.DoExit();
#if OBSOLETE
			if (null != roleController)
			{
				roleController.EndMove();
			}
#endif

            if (null != drawer)
            {
                drawer.FadeOut();
            }

            if (null != LuaLuancher.Me)
            {
                LuaLuancher.Me.Call("Input_JoyStickEnd");
            }
        }

        protected override void DoUpdate()
        {
            base.DoUpdate();
            if (0 >= touchingCount)
            {
                Exit();
            }
        }

        protected override void OnTouchBegin()
        {
            if (0 == pointerID)
            {
                return;
            }

            if (isOverUI)
            {
                return;
            }

#if OBSOLETE
			var role = Player.Me.role;
			if (null != role)
			{
				Ray ray = Camera.main.ScreenPointToRay(touchPoint);
				
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit, float.PositiveInfinity, LayerMask.GetMask(Config.Layer.ACCESSABLE.Key)))
				{
					var accessableObj = hit.collider.gameObject.GetComponent<Accessable>();
					if (null != accessableObj)
					{
						// lock
						role.lockTarget = accessableObj;

						if (!role.skillPendingOrRunning)
						{
							var objData = accessableObj.data; 
							if (null != objData && RoleInfo.Camp.ENEMY == objData.camp)
							{
								// attack
								if (role.AttackLockedTarget())
								{
									// ManualControlled
									role.OnManualControlled();
									role.SetController(roleController);
								}
							}
						}
					}
				}
			}
#endif
        }

        protected override void OnTouchMoved()
        {
            if (0 != pointerID)
            {
                return;
            }
            Step();
        }

        protected override void OnTouchHold()
        {
            if (!holdEnable)
            {
                return;
            }
            if (0 != pointerID)
            {
                return;
            }
            if (null == Camera.main)
            {
                return;
            }
            if (GeometryUtils.RotationEqual(cameraPrevRotation, Camera.main.transform.rotation))
            {
                return;
            }
            Step();
        }

        protected override void OnTouchEnd()
        {
            if (0 < touchingCount)
            {
                return;
            }
            DelayExit();
        }

        protected override bool allowExtra()
        {
            return 0 < pointerID;
        }

        private void Step()
        {
            if (null == Camera.main)
            {
                return;
            }
            var dir = touchPoint - centerPoint;

            var buttonPoint = centerPoint + dir.normalized * Mathf.Min(radius_, Vector2.Distance(touchPoint, centerPoint));
            buttonPoint.y = Screen.height - buttonPoint.y;
            SetSpritePosition(BUTTON_SPRITE_INDEX, buttonPoint);

            if (null != LuaLuancher.Me)
            {
                cameraPrevRotation = Camera.main.transform.rotation;
                dir.z = dir.y;
                dir.y = 0;
                dir = Quaternion.Euler(new Vector3(0, cameraPrevRotation.eulerAngles.y, 0)) * dir;
                dir.Normalize();
                LuaLuancher.Me.Call("Input_JoyStick", dir.x, dir.y, dir.z);
            }
        }

    }
} // namespace RO
