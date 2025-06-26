using UnityEngine;
using System.Collections.Generic;
using Ghost.Utils;

namespace RO
{
	public static class ResourcePathHelper 
	{
		#region bus
		public static string IDCarrier(int ID)
		{
			return "Public/BusCarrier/"+ID;
		}
		#endregion bus
		
		#region body
		public static string IDRoleBody(int ID)
		{
			return "Role/Body/"+ID;
		}
		#endregion body
		
		#region hair
		public static string IDRoleHair(int ID)
		{
			return "Role/Hair/"+ID;
		}
		#endregion hair
		
		#region weapon
		public static string IDWeapon(int ID)
		{
			return "Role/Weapon/"+ID;
		}
		#endregion weapon
		
		#region head
		public static string IDHead(int ID)
		{
			return "Role/Head/"+ID;
		}
		#endregion head
		
		#region wing
		public static string IDWing(int ID)
		{
			return "Role/Wing/"+ID;
		}
		#endregion wing
		
		#region face
		public static string IDFace(int ID)
		{
			return "Role/Face/"+ID;
		}
		#endregion face
		
		#region tail
		public static string IDTail(int ID)
		{
			return "Role/Tail/"+ID;
		}
		#endregion tail
		
		#region eye
		public static string IDEye(int ID)
		{
			return "Role/Eye/"+ID;
		}
		#endregion eye
		
		#region mount
		public static string IDMount(int ID)
		{
			return "Role/Mount/"+ID;
		}
		#endregion mount
		
		#region audio
		public static string IDAudioOneShot()
		{
			return "Public/Audio/AudioOneShot";
		}
		public static string IDAudioOneShot_2D()
		{
			return "Public/Audio/AudioOneShot_2D";
		}
		public static string IDBGM(string name)
		{
			return "Public/Audio/BGM/"+name;
		}
		public static string IDSE(string name)
		{
			return "Public/Audio/SE/"+name;
		}
		public static string IDSECommon(string name)
		{
			return "Public/Audio/SE/Common/"+name;
		}
		public static string IDSEUI(string name)
		{
			return "Public/Audio/SE/UI/"+name;
		}
		#endregion audio
		
		#region effect
		public static string IDEffectDictionary()
		{
			return "Public/Effect/EffectDictionary";
		}
		public static string IDEffect(string name)
		{
			return "Public/Effect/"+name;
		}
		public static string IDEffectCamera()
		{
			return "Public/Effect/EffectCamera";
		}
		public static string IDEffectCommon(string name)
		{
			return "Public/Effect/Common/"+name;
		}
		public static string IDEffectSkill(string name)
		{
			return "Public/Effect/Skill/"+name;
		}
		public static string IDEffectUI(string name)
		{
			return "Public/Effect/UI/"+name;
		}
		public static string IDEffectWeather(string name)
		{
			return "Public/Effect/Weather/"+name;
		}
		#endregion effect
		
		#region spine effect
		public static string IDSpineEffect(string name)
		{
			return "Public/SpineEffect/"+name;
		}
		public static string IDSpineEffectCommon(string name)
		{
			return "Public/SpineEffect/Common/"+name;
		}
		#endregion spine effect
		
		#region item
		public static string IDItem(int ID)
		{
			return "Public/Item/"+ID;
		}
		#endregion item
		
		#region emoji
		public static string IDEmoji(string name)
		{
			return "Public/Emoji/"+name;
		}
		#endregion emoji
		
		#region script
		public static string IDScript(string name)
		{
			return "Stript/"+name;
		}
		public static string IDScriptFunctionSystem(string name)
		{
			return "Stript/FunctionSystem/"+name;
		}
		#endregion script
		
		#region GUI
		public static string IDGUIAtlasMap()
		{
			return "GUI/atlas/AtlasMap";
		}
		#endregion GUI
	}
} // namespace RO
