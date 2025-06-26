using UnityEngine;
using UnityEditor;
using LitJson;
using System.Collections.Generic;
using RO;
using Ghost.Utils;

namespace EditorTool
{
	public static class BuildParamsJsonEditor
	{
		public static void CmdSetPushParams ()
		{
			List<string> args = CommandArgs.GetCommandArgs ();
			if (args.Count >= 1) {
				bool need = false;
				bool.TryParse (args [0], out need);
				bool isRelease = false;
				bool.TryParse (args [2], out isRelease);
				NeedJPush (need, args [1], isRelease);
			}
		}

		public static void NeedJPush (bool need, string appKey, bool release)
		{
			BuildParams param = BuildParams.Instance;
			if (param == null) {
				param = new BuildParams (new JsonData ());
			}
			JsonData data = new JsonData ();
			data [BuildParams.NEED_JPUSH] = need;
			data [BuildParams.JPUSH_APP_KEY] = appKey;
			data [BuildParams.JPUSH_IS_RELEASE] = release;
			param.data [BuildParams.JPUSH_KEY] = data;
			string path = PathUnity.Combine (Application.dataPath, string.Format ("Resources/{0}.txt", BuildParams.FILENAME));
			param.SaveToFile (path);
			AssetDatabase.Refresh ();
		}

		[MenuItem ("AssetBundle/测试buildparam动态数据")]
		public static void Test ()
		{
			NeedJPush (true, "1aecc9662adf1725e5db1a8f", false);
//			NeedJPush (false);
		}

		[MenuItem ("AssetBundle/out put buildparam动态数据")]
		public static void Test1 ()
		{
			BuildParams param = BuildParams.Instance;
			if (param != null) {
				Debug.LogFormat ("push:{0}", param.Get_JPush_Enable);
			}
		}

		[MenuItem ("ceshi/xuclassWriteblow")]
		public static void Test2 ()
		{
			ROXClass c = new ROXClass (System.IO.Path.Combine (Application.dataPath, "Resources/Keyboard.mm"));
			c.WriteBelow ("#endif // FILTER_EMOJIS_IOS_KEYBOARD", "\n- (BOOL)textView:(UITextView *)textView shouldChangeTextInRange:(NSRange)range replacementText:(NSString *)text{" +
			"\nif ([text isEqualToString:@\"\\n\"]){" +
			"\n[self hide];" +
			"\nreturn NO;" +
			"\n}" +
			"\nreturn YES;" +
			"\n}\n" +
			"\n- (BOOL)textViewShouldReturn:(UITextView*)textFieldObj" +
			"\n{" +
			"\n[self hide];" +
			"\nreturn YES;" +
			"\n}\n");
			c.WriteBelow ("textView.hidden = YES;", "textView.returnKeyType = UIReturnKeyDone;");

			c.WriteBelow ("#endif // FILTER_EMOJIS_IOS_KEYBOARD", "\n- (BOOL)textView:(UITextView *)textView shouldChangeTextInRange:(NSRange)range replacementText:(NSString *)text{" +
			"\nif ([text isEqualToString:@\"\\n\"]){" +
			"\n[self hide];" +
			"\nreturn NO;" +
			"\n}" +
			"\nreturn YES;" +
			"\n}\n" +
			"\n- (BOOL)textViewShouldReturn:(UITextView*)textFieldObj" +
			"\n{" +
			"\n[self hide];" +
			"\nreturn YES;" +
			"\n}\n");
			c.WriteBelow ("textView.hidden = YES;", "textView.returnKeyType = UIReturnKeyDone;");
			c.Replace ("_multiline = param.multiline;", "_multiline = true;");
			c.Replace ("_multiline = param.multiline;", "_multiline = true;");
		}
	}
}
// namespace EditorTool
