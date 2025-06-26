using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using RO;

namespace EditorTool
{
	[CustomEditor(typeof(NPCPoint)), CanEditMultipleObjects]
	public class NPCPonitCopyEditor : NPCInfoEditor
	{
		NPCInfo _target;
		TextEditor te = new TextEditor();
		string[] str_Info = new string[20];
		string str_Debug;
//		int behavior=0;
		public override void OnInspectorGUI ()
		{
			_target = target as NPCInfo;
			base.OnInspectorGUI ();
			if(GUILayout.Button("复制参数"))
			{
				str_Info[0] = "id="+_target.ID.ToString()+",";
				str_Info[1] = "pos="+_target.transform.position.ToString("f2")+",";
				str_Info[2] = "num="+_target.count.ToString()+",";
				str_Info[3] = "search="+_target.searchRange.ToString()+",";
				str_Info[4] = "range="+_target.territory.ToString()+",";
				str_Info[5] = "territory="+_target.territory.ToString()+",";
				str_Info[6] = "life="+_target.life.ToString()+",";
				str_Info[7] = "reborn="+_target.rebornTime.ToString()+",";

				//str_Info[7] = "camp="+_target.camp.ToString()+",";

				str_Info[8] = "behavior="+System.Convert.ToInt32(_target.behaviours).ToString()+",";
				str_Info[9] = "ai="+_target.ai.ToString()+",";
				str_Info[10] = "scale={"+_target.scaleMin.ToString()+","+_target.scaleMax.ToString()+"}";
				str_Info[11] = "dir="+_target.transform.rotation.eulerAngles.y.ToString()+",";
				str_Info[12] = "level="+_target.level.ToString();
//				if(!_target.attackBack&&!_target.moveable)
//				{
//					str_Info[7] = "behavior=0";
//				}
//				if(!_target.attackBack&&_target.moveable)
//				{
//					str_Info[7] = "behavior=1";
//				}
//				if(_target.attackBack&&!_target.moveable)
//				{
//					str_Info[7] = "behavior=2";
//				}
//				if(_target.attackBack&&_target.moveable)
//				{
//					str_Info[7] = "behavior=3";
//				}
			
				if(_target.count==0)
				{
					str_Info[2] = "";
				}
				if(_target.transform.rotation.eulerAngles.y==0)
				{
					str_Info[3] = "";
				}
				if(_target.rebornTime==0)
				{
					str_Info[4] = "";
				}
				if(_target.territory==0)
				{
					str_Info[5] = "";
				}
				if(_target.scaleMin==1&&_target.scaleMax==1)
				{
					str_Info[6] = "";
				}
				str_Debug  = str_Info[0]+str_Info[1]+str_Info[2]+str_Info[3]+str_Info[4]+str_Info[5]+str_Info[6]+str_Info[7]+str_Info[8]+str_Info[9]+str_Info[10]+str_Info[11]+str_Info[12];
				te.text = str_Debug;
				te.OnFocus();
				te.Copy();


			}
//			behavior=0;
		}
	}
} 