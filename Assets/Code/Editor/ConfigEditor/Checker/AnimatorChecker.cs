using UnityEngine;

namespace EditorTool
{
	public partial class ScriptChecker
	{
		public static void CheckAnimator(ref string error, Animator animator, string prefixPath = "")
		{
			if(animator != null && animator.runtimeAnimatorController != null)
			{
				prefixPath += animator.name + " Animator->AnimatorController->AnimationClips->";
				for(int i=0; i < animator.runtimeAnimatorController.animationClips.Length; i++)
				{
					if (animator.runtimeAnimatorController.animationClips [i] == null)
					{
						AppendError(ref error, prefixPath + "AnimatorController中含有空的状态！");
						break;
					}
				}
			}
		}
	}
}

