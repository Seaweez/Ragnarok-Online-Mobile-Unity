using UnityEngine;
using System.Collections;

public class UIInputWithFlaLabDepndPlatform : UIInput
{
	protected override void OnPress (bool isPressed)
	{
		base.OnPress (isPressed);

#if UNITY_EDITOR
		label.enabled = true;
#endif
	}
}
