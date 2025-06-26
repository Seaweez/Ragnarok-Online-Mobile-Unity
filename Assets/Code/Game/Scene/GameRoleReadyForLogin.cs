using UnityEngine;
using System.Collections;

public class GameRoleReadyForLogin : MonoBehaviour
{
	void Start ()
	{
		if (RO.InputManager.Instance != null)
		{
			RO.InputManager.Instance.disable = true;
		}
	}

	void OnDestroy()
	{
		if (RO.InputManager.Instance != null)
		{
			RO.InputManager.Instance.disable = false;
		}
	}
}
