using UnityEngine;
using System.Collections.Generic;
using System.IO;
using SLua;
using System.Text;
using System.Reflection;
using System;

public class NativeSDKGenerator {

	EOL eol = SLuaSetting.Instance.eol;
	int indent = 0;
	string paramRef;
	string NewLine{
		get{
			switch(eol){
			case EOL.Native:
				return System.Environment.NewLine;
			case EOL.CRLF:
				return "\r\n";
			case EOL.CR:
				return "\r";
			case EOL.LF:
				return "\n";
			default:
				return "";
			}
		}
	}
		
	public CMPair pair;
	public string path;

	void WriteBaseNameSpace (StreamWriter file)
	{
		Write(file, "using UnityEngine;");
		Write(file, "");
	}

	void WriteAttribute (StreamWriter file)
	{
		Write(file, "[SLua.CustomLuaClassAttribute]");
	}

	void WriteBaseHead (StreamWriter file)
	{
		Write(file, "public class ExClass {");
	}

	void WriteBaseField (StreamWriter file)
	{
		Write(file, "public static bool EnableLog;");
	}

	void WriteBaseFunction (StreamWriter file)
	{
		Write(file, "public static void Log(string str){");
		Write(file, "if(ExClass.EnableLog){");
		Write(file, "Debug.Log(\"[ExClass]: \"+str);");
		Write(file, "}");
		Write(file, "}");
	}

	public void GenerateBase ()
	{
		StreamWriter file = Begin("ExClass");
		WriteBaseNameSpace (file);
		WriteAttribute (file);
		WriteBaseHead (file);
		WriteBaseField (file);
		WriteBaseFunction (file);
		End (file);
	}
		
	public void Generate ()
	{
		StreamWriter file = Begin(ExportName ());
		WriteHead (file);
		WriteFunction (file);
		End (file);
	}

	string ExportName ()
	{
		string name = pair.classExport.Name;
		return "Ex_" + name;
	}

	StreamWriter Begin(string cName)
	{
		string clsname = cName;
		string f = path + clsname + ".cs";
		StreamWriter file = new StreamWriter(f, false, Encoding.UTF8);
		file.NewLine = NewLine;
		return file;
	}

	void Write(StreamWriter file, string fmt, params object[] args)
	{
		fmt = System.Text.RegularExpressions.Regex.Replace(fmt, @"\r\n?|\n|\r", NewLine);
		if (fmt.StartsWith("}")) indent--;

		for (int n = 0; n < indent; n++)
			file.Write("\t");


		if (args.Length == 0)
			file.WriteLine(fmt);
		else
		{
			string line = string.Format(fmt, args);
			file.WriteLine(line);
		}

		if (fmt.EndsWith("{")) indent++;
	}

	void WriteExtraNamespace(StreamWriter file) {
		List<string> nsList = new List<string> ();
		string c = pair.classExport.Namespace;
		if (!string.IsNullOrEmpty (c) && !nsList.Contains (c)) {
			nsList.Add (c);
			Write(file,"using {0};",c);
		}
		foreach (MethodInfo mi in pair.methodExports) {
			string r = mi.ReturnType.Namespace;
			if (!string.IsNullOrEmpty (r) && !nsList.Contains (r)) {
				nsList.Add (r);
				Write(file,"using {0};",r);
			}
			ParameterInfo[] pi = mi.GetParameters ();
			for (int i = 0; i < pi.Length; i++) {
				string p = pi [i].ParameterType.Namespace;
				if (!string.IsNullOrEmpty (p) && !nsList.Contains (p)) {
					nsList.Add (p);
					Write(file,"using {0};",p);
				}
			}
		}
		Write(file, "");
	}

	void WriteHead(StreamWriter file)
	{
		//Write(file, "using UnityEngine;");
		//Write(file, "using System;");
		//Write(file, "using LuaInterface;");
		//Write(file, "using SLua;");
		//Write(file, "using System.Collections.Generic;");
		WriteExtraNamespace(file);
		WriteAttribute (file);
		Write(file, "public class {0} {{", ExportName() + " : ExClass");
	}

	string BasicTypeCheck (string type)
	{
		if (type == "Int32") {
			return "int";
		} else if (type == "Single") {
			return "float";
		} else if (type == "Double") {
			return "double";
		} else if (type == "String") {
			return "string";
		} else if (type == "Boolean") {
			return "bool";
		} else if (type == "Char") {
			return "char";
		} else if (type == "Int16") {
			return "short";
		} else if (type == "Int64") {
			return "long";
		} else if (type == "Void") {
			return "void";
		} else {
			return type;
		}
	}

	string GetReturnTypeName (MethodInfo mi)
	{
		return BasicTypeCheck (mi.ReturnType.Name);
	}

	string GetParameterTypeName (MethodInfo mi)
	{
		paramRef = "";
		string pName = "";
		ParameterInfo[] pi = mi.GetParameters ();
		for (int i = 0; i < pi.Length; i++) {
			string _class = BasicTypeCheck (pi [i].ParameterType.Name);
			string _ref = _class.ToLower () + (i+1);
			paramRef += _ref;
			pName += _class + " " + _ref;
			if (i < pi.Length - 1) {
				pName += ",";
				paramRef += ",";
			}
		}
		return pName;
	}

	void WriteFunctionDec(StreamWriter file,MethodInfo mi,string name)
	{
		string rName = GetReturnTypeName (mi);
		string pName = GetParameterTypeName (mi);
		Write(file, "public static {0} {1}({2}) {{", rName,name,pName);
	}

	void WriteFunctionPre (StreamWriter file,string name)
	{
		Write(file, "Log (\"{0}\");",name);
	}

	void WriteFunctionImpl(StreamWriter file,MethodInfo mi,string cn)
	{
		string rName = GetReturnTypeName (mi);
		string pName = paramRef;
		string cName = cn;
		string mName = mi.Name;
		WriteFunctionPre (file,cn + "_" + mName);
		if (rName == "void") {
			Write(file, "{0}.{1} ({2});", cName,mName,pName);
		} else {
			Write(file, "return {0}.{1} ({2});", cName,mName,pName);
		}
		Write(file, "}");
	}

	void WriteFunction(StreamWriter file)
	{
		foreach (MethodInfo mi in pair.methodExports) {
			string cName = pair.classExport.Name;
			string mName = mi.Name;
			string fn = cName + "_" + mName;
			WriteFunctionDec(file,mi,fn);
			WriteFunctionImpl(file,mi,cName);
			Write(file, "");
		}
	}

	void End(StreamWriter file)
	{
		Write(file, "}");
		file.Flush();
		file.Close();
	}
}
