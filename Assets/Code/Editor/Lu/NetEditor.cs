using UnityEngine;
using UnityEditor;
using RO.Net;

namespace EditorTool
{
	[CustomEditor(typeof(NetConnectionManager))]
	public class NetEditor : Editor
	{
		NetConnectionManager _connectionM;
		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI();
			_connectionM = target as NetConnectionManager;
			EditorGUILayout.BeginVertical ();
			EditorGUILayout.BeginHorizontal();
//			GUILayout.Label("开启网络模块输出",GUILayout.Width (100));
//			_connectionM.EnableLog = GUILayout.Toggle(_connectionM.EnableLog,"");
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndVertical ();
			EditorGUILayout.BeginVertical ();
			EditorGUILayout.BeginHorizontal();
//			int val = EditorGUILayout.IntField("非zip压缩数据上限", _connectionM.maxZibNum, GUILayout.MinWidth(100f));
//			EditorGUILayout.IntField("Timeout", _connectionM.maxZibNum, GUILayout.MinWidth(100f));
			
			if (GUI.changed)
			{
				NGUIEditorTools.RegisterUndo("Dimensions Change", _connectionM);
//				_connectionM.maxZibNum = val;
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndVertical ();
			if(Application.isPlaying)
			{
				if(GUILayout.Button("测试网络短暂断开"))
				{
					NetManager.GameDisConnect();
				}
				if(GUILayout.Button("测试网络中断(返回登陆)"))
				{
					NetService.Call(NetService.ServiceConnProxy, "NotifyNetDown");
//					NetManager.GameDisConnect();
				}
			}
		}
	}
}
