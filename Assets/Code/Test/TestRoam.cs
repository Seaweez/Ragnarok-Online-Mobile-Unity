using UnityEngine;
using System.Collections.Generic;

namespace RO.Test
{
	public class TestRoam : MonoBehaviour 
	{
		public float DEFAULT_SCREEN_SIZE_INCH = 4;
		public float touchSenseInch = 0.1f;

		public float roamSpeed = 0.1f;

		private InputHelper[] helpers{get;set;}

		private StateMachine<InputController> controllerStateMachine{get; set;}
		private RoamInputController roamController{get;set;}

		void Awake()
		{
			var dpi = Screen.dpi;
			if (0 == dpi)
			{
				dpi = new Vector2(Screen.width, Screen.height).magnitude/DEFAULT_SCREEN_SIZE_INCH;
			}
			var touchSensePixels = touchSenseInch*dpi;
			
			helpers = new InputHelper[]{new InputHelper(0)};
			foreach (var helper in helpers)
			{
				helper.touchSenseMin = touchSensePixels;
			}

			controllerStateMachine = new StateMachine<InputController>();
			roamController = new RoamInputController(helpers);
		}

		void Start()
		{
			roamController.speed = roamSpeed;
			roamController.target = transform;
			controllerStateMachine.ForceSwitch(roamController);
		}

		void Update()
		{
			controllerStateMachine.Update();
		}
	
	}
} // namespace RO.Test
