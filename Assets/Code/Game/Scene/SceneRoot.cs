using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace RO
{
#if OBSOLETE
	[SLua.CustomLuaClassAttribute]
#endif
	public class SceneRoot : SingleTonGO<SceneRoot> 
	{
#if OBSOLETE
		public static SceneRoot Instance{get{return Me;}}
		
		public GameObject monoGameObject
		{
			get
			{
				return gameObject;
			}
		}

		public GameObject[] dontDestroyObjects = null;
		public Animator animator = null;
#endif
		public GameObject PVE = null;
		public GameObject PVP = null;

		public void FindPVEAndPVP()
		{
			var go = GameObject.FindGameObjectWithTag(Config.Tag.PVE);
			if (null != go)
			{
				PVE = go;
			}
			go = GameObject.FindGameObjectWithTag(Config.Tag.PVP);
			if (null != go)
			{
				PVP = go;
			}
		}

#if OBSOLETE
		public SpriteFade sceneMask = null;

		private Coroutine checkAnimationEnd = null;

		private CameraController cameraController = null;

		private System.Collections.IEnumerator CheckAnimationEnd()
		{
			while (true)
			{
				if (!(null != animator && animator.enabled))
				{
					break;
				}
				var currentState = animator.GetCurrentAnimatorStateInfo(0);
				if (!currentState.IsValid())
				{
					break;
				}
				var nextState = animator.GetNextAnimatorStateInfo(0);
				if (1 < currentState.normalizedTime && !nextState.IsValid())
				{
					break;
				}

				if (null != sceneMask 
				    && SpriteFade.Phase.VISIBLE == sceneMask.phase
				    && 1 <= sceneMask.alpha)
				{
					break;
				}
				yield return 0;
			}
			StopAnimation();
		}

		private void ResetCameraController(CameraController newCameraController)
		{
			var oldCameraController = cameraController;
			if (oldCameraController == newCameraController)
			{
				return;
			}

			cameraController = newCameraController;

			if (null != oldCameraController)
			{
				oldCameraController.beSingleton = true;
				oldCameraController.updateCameras = false;
			}

			if (null != newCameraController)
			{
				newCameraController.beSingleton = false;
				newCameraController.updateCameras = true;
			}
		}

		public void PlayAnimation()
		{
			if (null != checkAnimationEnd)
			{
				return;
			}

			var player = Player.Me;

			if (null == animator)
			{
				animator = GetComponent<Animator>();
				if (null == animator)
				{
					player.CaptureCamera();
					return;
				}
			}
			player.ignoreAreaTrigger = true;
			animator.enabled = true;

			checkAnimationEnd = StartCoroutine(CheckAnimationEnd());

			var e = CoreEventSingleton<SceneEventAnimationBegin>.Me;
			e.scene = Scene.Me;
			player.SceneEventHandler(e);

			ResetCameraController(CameraController.Me);
		}

		public void StopAnimation()
		{
			if (null == checkAnimationEnd)
			{
				return;
			}
			checkAnimationEnd = null;
			ResetCameraController(null);

			var player = Player.Me;
			player.ignoreAreaTrigger = false;
			player.CaptureCamera();

			if (null != sceneMask)
			{
				sceneMask.FadeOut();
			}
			
			var e = CoreEventSingleton<SceneEventAnimationEnd>.Me;
			e.scene = Scene.Me;
			player.SceneEventHandler(e);

			if (null == animator)
			{
				animator = GetComponent<Animator>();
				if (null == animator)
				{
					return;
				}
			}
			animator.enabled = false;
		}

		public void FadeStopAnimation(float duration)
		{
			if (null != sceneMask)
			{
				sceneMask.fadeDuration = duration;
				sceneMask.FullScreen();
				sceneMask.FadeIn();
			}
		}

		private void TryPlayAnimation()
		{
			var player = Player.Me;
			if (null != player && player.playSceneAnimation)
			{
				PlayAnimation();
			}
		}

		public void HandlePlayMode()
		{
			switch (Player.Me.playMode)
			{
			case Player.PlayMode.PVE:
				if (null != PVP)
				{
					GameObject.DestroyImmediate(PVP);
				}
				break;
			case Player.PlayMode.PVP:
				if (null != PVE)
				{
					GameObject.DestroyImmediate(PVE);
				}
				break;
			case Player.PlayMode.Raid:
				if (null != PVE)
				{
					GameObject.DestroyImmediate(PVE);
				}
				if (null != PVP)
				{
					GameObject.DestroyImmediate(PVP);
				}
				break;
			}
		}

		void Awake()
		{
			FindPVEAndPVP();
			Player.Me.sceneRoot = this;
			HandlePlayMode();
		}

		void Start()
		{
			if (null != dontDestroyObjects)
			{
				foreach (var obj in dontDestroyObjects)
				{
					if (null != obj)
					{
						GameObject.DontDestroyOnLoad(obj);
						obj.transform.parent = null;
					}
				}
			}

//			Invoke("TryPlayAnimation", 0);
		}
#endif
	
	}
} // namespace RO
