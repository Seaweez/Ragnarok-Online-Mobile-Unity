using System;
using UnityEditor;
using UnityEngine;
using SLua;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace EditorTool
{
    public class LuaEncrypt
    {
        static string resourcesRoot = Application.dataPath + "/Resources/";

        // แปลง Base64 string เป็น byte array โดยใช้ Convert.FromBase64String
        private static readonly byte[] aesKey = Convert.FromBase64String("Cu+obWg1Ktl42Rjn+5xQvDRX1j1o+dxswxKtTc4UiFs=");
        private static readonly byte[] aesIV = Convert.FromBase64String("22s411Xw+uyJir2ye0gyig==");

        [MenuItem("Lua/Encrypt/Test")]
        public static void EncryptTest()
        {
            string sLuaRequire = @"
		    return function(path)
			    require(path)
		    end
		    ";
            #region testPB
            LuaSvrForEditor.Me.Dispose();
            LuaSvrForEditor.Me.init();
            LuaDLL.luaS_openextlibs(LuaSvrForEditor.Me.luaState.L);
            LuaSvr.doBind(LuaSvrForEditor.Me.luaState.L);
            LuaFunction oLuaFunction = LuaSvrForEditor.Me.DoString(sLuaRequire) as LuaFunction;
            oLuaFunction.call("Script/Net/NewProtoBuf/test");

            #endregion
            #region test
            // ข้ามส่วนนี้ไปเพราะไม่เกี่ยวข้องกับการเข้ารหัส
            #endregion
        }

        public static void EncryptTobyte()
        {
            string src = Application.dataPath + "/Resources/Script/";
            string dest = Application.dataPath + "/Resources/Script2/";

            if (Directory.Exists(dest))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(dest);
                FileInfo[] files = dirInfo.GetFiles("*.bytes", SearchOption.AllDirectories);
                for (int i = 0, max = files.Length; i < max; ++i)
                {
                    File.Delete(files[i].FullName);
                }
            }

            DirectoryInfo sourceInfo = new DirectoryInfo(src);
            CopyFilesToBytes(sourceInfo, Directory.CreateDirectory(dest));

            AssetDatabase.Refresh();
        }

        public static void EncryptDo()
        {
            List<string> dirList = GetEncryptDirectoryList();
            for (int i = 0; i < dirList.Count; i++)
            {
                DirectoryInfo directory = new DirectoryInfo(resourcesRoot + dirList[i]);
                GetAllFiles(directory);
            }

            for (int i = 0; i < FileList.Count; i++)
            {
                EncryptFile(FileList[i]);
            }

            AssetDatabase.Refresh();
            FileList.Clear();
        }

        [MenuItem("Lua/Encrypt/T&&D")]
        public static void EncryptTD()
        {
            EncryptTobyte();
            EncryptDo();
        }

        public static List<string> GetEncryptDirectoryList()
        {
            List<string> dirList = new List<string>();
            dirList.Add("Script2");
            return dirList;
        }

        public static void EncryptFile(string path)
        {
            Debug.Log(path);
            TextAsset asset = (TextAsset)Resources.Load(path);
            byte[] b = asset.bytes;
            if (b.Length < 1)
                return;
            if (0 == LuaDLL.ecode_IsDesCode(b, (uint)b.Length))
                return;

            using (Aes aes = Aes.Create())
            {
                aes.Key = aesKey;
                aes.IV = aesIV;

                byte[] encryptedData;
                using (ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                {
                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            csEncrypt.Write(b, 0, b.Length);
                            csEncrypt.FlushFinalBlock();
                            encryptedData = msEncrypt.ToArray();
                        }
                    }
                }

                string encryptedFilePath = resourcesRoot + path + ".bytes";
                using (FileStream fs = new FileStream(encryptedFilePath, FileMode.OpenOrCreate))
                {
                    fs.Write(encryptedData, 0, encryptedData.Length);
                }

                Debug.Log("<color=#9400D3>" + "Encrypt Success!!" + "</color>");
            }
        }

        static public void CopyFilesToBytes(DirectoryInfo sourceInfo, DirectoryInfo tarInfo)
        {
            FileInfo[] files = sourceInfo.GetFiles("*.txt", SearchOption.TopDirectoryOnly);
            for (int i = 0, max = files.Length; i < max; ++i)
            {
                string oldFile = (sourceInfo.FullName + '/' + files[i].Name);
                string newFile = (tarInfo.FullName + '/' + files[i].Name).Replace(".txt", ".bytes");
                FileStream fsA = new FileStream(oldFile, FileMode.Open, FileAccess.Read);
                byte[] buffer = new byte[fsA.Length];
                fsA.Read(buffer, 0, buffer.Length);
                fsA.Close();
                if (buffer.Length > 2)
                {
                    if (buffer[0] == 0xef
                        && buffer[1] == 0xbb
                        && buffer[2] == 0xbf)
                    {
                        string f = new UTF8Encoding(false).GetString(buffer, 3, buffer.Length - 3);
                        byte[] b = System.Text.Encoding.UTF8.GetBytes(f);

                        FileStream fs = new FileStream(newFile, FileMode.OpenOrCreate);
                        fs.Write(b, 0, b.Length);
                        fs.Close();
                    }
                    else
                        files[i].CopyTo(newFile, true);
                }
                else
                    files[i].CopyTo(newFile, true);
            }
            DirectoryInfo[] dirs = sourceInfo.GetDirectories();
            for (int i = 0, max = dirs.Length; i < max; ++i)
            {
                DirectoryInfo dirInfo = Directory.CreateDirectory(tarInfo.FullName + '/' + dirs[i].Name);
                CopyFilesToBytes(dirs[i], dirInfo);
            }
        }

        static List<string> FileList = new List<string>();
        public static List<string> GetAllFiles(DirectoryInfo dir)
        {
            FileInfo[] allFile = dir.GetFiles();
            foreach (FileInfo fi in allFile)
            {
                if (fi.FullName.EndsWith(".bytes"))
                {
                    FileList.Add(fi.FullName.Replace("\\", "/").Replace(Application.dataPath + "/Resources/", "").Replace(".bytes", ""));
                }
            }
            DirectoryInfo[] allDir = dir.GetDirectories();
            foreach (DirectoryInfo d in allDir)
            {
                GetAllFiles(d);
            }
            return FileList;
        }
    }
}
