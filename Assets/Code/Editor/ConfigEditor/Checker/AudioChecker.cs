using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace EditorTool
{
    public partial class ScriptChecker
    {
        static ITableBase _AudioTable = null;
        public static ITableBase audioTable
        {
            get
            {
                if (_AudioTable == null)
                    _AudioTable = ConfigManager.Instance.GetTable("Audios");
                return _AudioTable;
            }
        }
        public static string _BGMExt = ".mp3";
        public static string _AudioExt = ".wav";
        public static string _AudioPath = "Assets/Resources/Public/Audio/SE/";

        public static void CheckAudioByCfg(ref string error, string CfgValue)
        {
            if (!string.IsNullOrEmpty(CfgValue) && CfgValue != "0")
            {
                string[] audio = null;
                string[] pathArr = CfgValue.Split(':');
                for (int i = 0; i < pathArr.Length; i++)
                {
                    audio = pathArr[i].Split('-');
                    for (int k = 0; k < audio.Length; k++)
                    {
                        string path = _AudioPath + audio[k] + _AudioExt;
                        string name = Path.GetFileNameWithoutExtension(audio[k]);
                        CfgBase cfgbase = audioTable.FindCfg("Name", name);
                        if (cfgbase == null)
                            AppendError(ref error, string.Format(fileMissErrorFormat, path));
                    }
                }
            }
        }

        public static void CheckAudio(ref string error, string path)
        {
            AudioImporter im = AssetImporter.GetAtPath(path) as AudioImporter;
            bool m_bIsBgm = path.Contains("BGM");
            CheckAudioImporter_Size(ref error, im);
            if (m_bIsBgm)
                CheckAudioImporter_BGMMP3(ref error, im);
            else
                CheckAudioImporter_WAV(ref error, im);
        }

        static void CheckAudioImporter_Size(ref string error, AudioImporter importer)
        {
//            var property = importer.GetType().GetProperty("origSize", BindingFlags.Instance | BindingFlags.NonPublic);
//            int size = int.Parse(property.GetValue(importer, null).ToString());
//            if (size > 1024 * 1024)
//                AppendError(ref error, string.Format("原始大小超过1MB Origin Size = {0:N2}MB! ", (size / 1024.0f / 1024.0f)));

            var property = importer.GetType().GetProperty("compSize", BindingFlags.Instance | BindingFlags.NonPublic);
            int size = int.Parse(property.GetValue(importer, null).ToString());
            if (size > 1024 * 1024)
                AppendError(ref error, string.Format("压缩大小超过1MB Imported Size = {0:N2}MB! ", (size / 1024.0f / 1024.0f)));
        }

        static void CheckAudioImporter_BGMMP3(ref string error, AudioImporter importer)
        {
            if (importer.defaultSampleSettings.loadType != AudioClipLoadType.CompressedInMemory)
                AppendError(ref error, "加载方式不是CompressedInMemory");
        }

        static void CheckAudioImporter_WAV(ref string error, AudioImporter importer)
		{
			UnityEditor.SerializedObject serializedObject = new UnityEditor.SerializedObject (importer);
			UnityEditor.SerializedProperty normalize = serializedObject.FindProperty ("m_Normalize");

            if (importer.defaultSampleSettings.loadType != AudioClipLoadType.DecompressOnLoad)
                AppendError(ref error, "加载方式不是DecompressOnLoad");
            if(!importer.forceToMono)
                AppendError(ref error, "没有勾选ForceToMono!");
			else if(!normalize.boolValue)
                AppendError(ref error, "没有勾选Normalize!");
        }
    }
}
