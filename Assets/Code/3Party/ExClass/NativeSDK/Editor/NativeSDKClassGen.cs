using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System.Reflection;
using System;

public class CMPair {
	public Type classExport;
	public List<MethodInfo> methodExports = new List<MethodInfo>();
}

public class NativeSDKClassGen {

	static bool IsCompiling {
		get {
			if (EditorApplication.isCompiling) {
				Debug.Log("Unity Editor is compiling, please wait.");
			}
			return EditorApplication.isCompiling;
		}
	}

	static string GenPath = "Assets/Code/ExWrapper/";

	[MenuItem("RO/NativeSDK/Make")]
	public static void Make()
	{
		if (IsCompiling) {
			return;
		}
		CreateBaseClass ();
		CreateWrapperClass ();
		AssetDatabase.Refresh();
	}
		
	static void CreateBaseClass ()
	{
		if (!Directory.Exists(GenPath))
		{
			Directory.CreateDirectory(GenPath);
		}

		NativeSDKGenerator gen = new NativeSDKGenerator ();
		gen.path = GenPath;
		gen.GenerateBase ();
	}

	static void CreateWrapperClass ()
	{
		CollectNativeSDKClass ("Assembly-CSharp-firstpass");
		CollectNativeSDKClass ("Assembly-CSharp");
	}

	static void CollectNativeSDKClass (string dllName)
	{
		Assembly assembly = Assembly.Load(dllName);
		Type[] types;
		if (assembly != null) {
			types = assembly.GetExportedTypes();
			foreach (Type t in types) {
				if (t.IsDefined (typeof(NativeSDKClassAttribute), false)) {
					CMPair pair = new CMPair ();
					pair.classExport = t;
					pair.methodExports = CollectNativeSDKMethods (t);
					Generate (pair);
				}
			}
		}
	}

	static List<MethodInfo> CollectNativeSDKMethods (Type t)
	{
		List<MethodInfo> mi = new List<MethodInfo> ();
		MethodInfo[] methods = t.GetMethods (BindingFlags.Static | BindingFlags.Public);
		for (int i = 0; i < methods.Length; i++) {
			if (!methods[i].IsDefined (typeof(DoNotNativeSDKAttribute), false)) {
				if (!methods [i].IsAbstract) {
					mi.Add (methods[i]);
				}
			}
		}
		return mi;
	}

	static void Generate(CMPair pair)
	{
		if (!Directory.Exists(GenPath))
		{
			Directory.CreateDirectory(GenPath);
		}

		NativeSDKGenerator gen = new NativeSDKGenerator ();
		gen.pair = pair;
		gen.path = GenPath;
		gen.Generate ();
	}
}
