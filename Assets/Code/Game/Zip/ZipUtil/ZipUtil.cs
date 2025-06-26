using UnityEngine;
using System.Collections.Generic;
using System.IO;
using RO;

namespace EditorTool
{
	public class ZipUtil
	{
		public static void compressDir (string sourceDir, int levelOfCompression, string zipArchive, string[] exceptExtensions = null, bool includeRoot = false, bool deleteOld = true, string customRdir = "",string[] exceptFolders = null)
		{
			string fdir = @sourceDir.Replace ("\\", "/");
			if (File.Exists (zipArchive) && deleteOld)
				File.Delete (zipArchive);
			string rdir = null;
			if (string.IsNullOrEmpty (customRdir)) {
				string[] ss = fdir.Split ('/');
				rdir = ss [ss.Length - 1];
			} else {
				rdir = fdir.Replace(customRdir,"");
			}
			string root = rdir;
			
			lzip.cProgress = 0;
			
			if (levelOfCompression < 0)
				levelOfCompression = 0;
			if (levelOfCompression > 10)
				levelOfCompression = 10;
			
			try {
				bool invalidExtension = false;
				foreach (string f in Directory.GetFiles(fdir, "*", SearchOption.AllDirectories)) {
					invalidExtension = false;
					string s = f.Replace (fdir, rdir).Replace ("\\", "/");
					if (!includeRoot)
						s = s.Replace (root + "/", "");
					if (exceptExtensions != null) {
						for (int i=0; i<exceptExtensions.Length; i++) {
							if (s.EndsWith (exceptExtensions [i])) {
								invalidExtension = true;
								break;
							}
						}
					}

					if(exceptFolders!=null)
					{
						for (int i=0; i<exceptFolders.Length; i++) {
							if (s.Contains (exceptFolders [i])) {
								invalidExtension = true;
								break;
							}
						}
					}
					if (invalidExtension)
						continue;
					lzip.compress_File (levelOfCompression,  @zipArchive, f, true, s);
					lzip.cProgress++;
				}
				
			} catch (System.Exception excpt) {
				RO.LoggerUnused.Log ("#" + excpt.Message);
			}
		}

		public static void compressFile (string filePath, string absolutePath, int levelOfCompression, string zipArchive, string[] exceptExtensions = null, bool includeRoot = false, bool deleteOld = true)
		{
			string fdir = @filePath.Replace ("\\", "/");
			if (File.Exists (zipArchive) && deleteOld)
				File.Delete (zipArchive);
			string root = absolutePath;
			
			lzip.cProgress = 0;
			
			if (levelOfCompression < 0)
				levelOfCompression = 0;
			if (levelOfCompression > 10)
				levelOfCompression = 10;
			try {
				bool invalidExtension = false;
				invalidExtension = false;
				if (exceptExtensions != null) {
					for (int i=0; i<exceptExtensions.Length; i++) {
						if (fdir.EndsWith (exceptExtensions [i])) {
							invalidExtension = true;
							break;
						}
					}
				}
				if (invalidExtension)
					return;
				string s = fdir.Replace (absolutePath, "").Replace ("\\", "/");
				if (!includeRoot)
					s = s.Replace (root + "/", "");
				lzip.compress_File (levelOfCompression,  @zipArchive, fdir, true, s);
				lzip.cProgress++;
				
			} catch (System.Exception excpt) {
				RO.LoggerUnused.Log ("#" + excpt.Message);
			}
		}
	}
} // namespace EditorTool
