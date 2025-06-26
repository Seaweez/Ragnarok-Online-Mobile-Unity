using UnityEngine;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using System;
using System.Collections;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class UnZipFile : SingleTonGO<UnZipFile>
	{
		public enum UnZipState
		{
			WAIT = 1,
			PROCESSING = 2,
			SUCCESS = 3,
			FAILED = 4,
			MAX = 10
		}

		public const int ERROR_HAS_NO_FILE = -100;
		public const int ERROR_STILL_UNZIPPING = -101;
		public const int ERROR_READ_ZIPINFO = -102;
		public const int ERROR_READ_ANDROIDTMPFILEERROR = -103;

		public static UnZipFile Instance {
			get {
				return Me;
			}
		}

		public GameObject monoGameObject {
			get {
				return gameObject;
			}
		}

		UnZipTask _currentTask;

		public bool isProcessing {
			get { 
				return _currentTask != null;
			}
		}

		public void StartUnZipFile (string zipFile, string targetDir, object completeparam, Action<float> UnZipProgressCall, Action<UnZipFilesInfo> UnZipCompleteCall, Action<int,object> UnZipFailedCall)
		{
			if (!isProcessing) {
				if (File.Exists (zipFile)) {
					_currentTask = new UnZipTask (zipFile, targetDir, completeparam, UnZipProgressCall, UnZipCompleteCall, UnZipFailedCall);
					_currentTask.Start ();
				} else {
					if (UnZipFailedCall != null) {
						UnZipFailedCall (ERROR_HAS_NO_FILE,completeparam);
					}
				}
			} else {
				if (UnZipFailedCall != null) {
					UnZipFailedCall (ERROR_STILL_UNZIPPING,completeparam);
				}
			}
		}

		public void StartUnZipFileAndroid (string zipFile, string targetDir, object completeparam, Action<float> UnZipProgressCall, Action<UnZipFilesInfo> UnZipCompleteCall, Action<int,object> UnZipFailedCall)
		{
			StartCoroutine (LoadStreamingZip (zipFile, targetDir, completeparam, UnZipProgressCall, UnZipCompleteCall, UnZipFailedCall));
		}

		IEnumerator LoadStreamingZip (string zipFile, string targetDir, object completeparam, Action<float> UnZipProgressCall, Action<UnZipFilesInfo> UnZipCompleteCall, Action<int,object> UnZipFailedCall)
		{
			WWW www = new WWW (zipFile);
			yield return www;
			if (string.IsNullOrEmpty (www.error) == false) {
				if (www.error.Contains ("Couldn't open file")) {
					if (UnZipFailedCall != null) {
						UnZipFailedCall (ERROR_HAS_NO_FILE,completeparam);
					}
				}
				yield break;
			}
			StartUnZipBytes (www.bytes, null, targetDir, completeparam, UnZipProgressCall, UnZipCompleteCall, UnZipFailedCall);
			www.Dispose ();
			www = null;
		}

		public void StartUnZipBytes (byte[] contents, string tmpZipFile, string targetDir, object completeparam, Action<float> UnZipProgressCall, Action<UnZipFilesInfo> UnZipCompleteCall, Action<int,object> UnZipFailedCall)
		{
			if (!isProcessing && contents != null) {
				//创建zip文件先
				if (BufferToZipFile (contents, ref tmpZipFile)) {
					StartUnZipFile (tmpZipFile, targetDir, completeparam, UnZipProgressCall, (fileInfo) => {
						ROFileUtils.FileDelete (tmpZipFile);
						if (UnZipCompleteCall != null)
							UnZipCompleteCall (fileInfo);
					}, UnZipFailedCall);
				} else {
					if (UnZipFailedCall != null) {
						UnZipFailedCall (ERROR_READ_ANDROIDTMPFILEERROR,completeparam);
					}
				}
			} else {
				if (UnZipFailedCall != null) {
					UnZipFailedCall (ERROR_STILL_UNZIPPING,completeparam);
				}
			}
		}

		bool BufferToZipFile (byte[] bytes, ref string zipFile)
		{
			bool res = false;
			try {
				if (string.IsNullOrEmpty (zipFile))
					zipFile = Path.Combine (ApplicationHelper.persistentDataPath, "TmpPatch.zip");
				if (File.Exists (zipFile))
					File.Delete (zipFile);
				FileStream fs = new FileStream (zipFile, FileMode.Create);
				fs.Write (bytes, 0, bytes.Length);
				#if !NETFX_CORE
				fs.Close ();
				#endif
				fs.Dispose ();
				bytes = null;
				res = true;
			} catch (Exception ) {
				res = false;
			}
			System.GC.Collect ();
			return res;
		}

		void LateUpdate ()
		{
			if (_currentTask != null) {
				if (_currentTask.zipState == UnZipState.PROCESSING) {
					_currentTask.UnZipProgress (_currentTask.Progress);
				} else if (_currentTask.zipState == UnZipState.SUCCESS) {
					_currentTask.UnZipProgress (1);
					UnZipFinish ();
				} else if (_currentTask.zipState == UnZipState.FAILED) {
					UnZipFailed (_currentTask.unzipRet);
				}
			}
		}

		protected override void OnDestroy ()
		{
			StopUnZip ();
			base.OnDestroy ();
		}

		public void StopUnZip ()
		{
			if (_currentTask != null && _currentTask.zipState == UnZipState.PROCESSING) {
				Reset ();
			}
		}

		void UnZipFinish ()
		{
			if (_currentTask != null) {
				Action<UnZipFilesInfo> done = _currentTask.UnZipCompleteCall;
				UnZipFilesInfo info = _currentTask.unzipFilesInfo;
				Reset ();
				if (done != null)
					done (info);
			}
		}

		void UnZipFailed (int res)
		{
			if (_currentTask != null) {
				Action<int,object> done = _currentTask.UnZipFailedCallBack;
				object completeParam = _currentTask.completeParam;
				Reset ();
				if (done != null)
					done (res, completeParam);
			}
		}

		void Reset ()
		{
			_currentTask.Dispose ();
			_currentTask = null;
		}

		internal class UnZipTask
		{
			public string zipPath { get; private set; }

			public string unZipTargetPath { get; private set; }

			UnZipState _zipState = UnZipState.WAIT;

			public UnZipState zipState {
				get {
					return _zipState;
				}
			}

			public Action<UnZipFilesInfo> UnZipCompleteCall { get; private set; }

			public Action<int,object> UnZipFailedCallBack { get; private set; }

			public Action<float> UnZipProgressCall { get; private set; }

			Thread _unzipThread;

			int _totalFileNums = 0;
			int[] _unCompressedFileNumsIndex = new int[1];

			public long currentZipUncompressSize{ get; private set; }

			public UnZipFilesInfo unzipFilesInfo{ get; private set; }

			public List<string> currentUnzipFileNames{ get; private set; }

			public List<long> currentUnzipFileUncompressSize{ get; private set; }

			public List<long> currentUnzipFileCompressSize{ get; private set; }

			public int unzipRet { get; private set; }

			public float Progress {
				get {
					if (_totalFileNums != 0)
						return (float)_unCompressedFileNumsIndex [0] / (float)_totalFileNums;
					return 0;
				}
			}

			object _completeParam;

			public object completeParam {
				get { 
					return _completeParam;
				}
			}

			public UnZipTask (string zipfileName, string unzipDir, object completeparam, Action<float> UnZipProgressCall, Action<UnZipFilesInfo> UnZipCompleteCall, Action<int,object> UnZipFailedCall)
			{
				zipPath = zipfileName;

				unZipTargetPath = unzipDir;
				this._completeParam = completeparam;
				this.UnZipCompleteCall = UnZipCompleteCall;
				this.UnZipFailedCallBack = UnZipFailedCall;
				this.UnZipProgressCall = UnZipProgressCall;
			}

			public void Start ()
			{
				if (_zipState == UnZipState.WAIT) {
					_zipState = UnZipState.PROCESSING;
					UnZipProgress (0);
					currentZipUncompressSize = _ReadUnzipInfo (zipPath);
					if (currentZipUncompressSize < 0) {
						SetState (UnZipState.FAILED, ERROR_READ_ZIPINFO);
						return;
					}
					_totalFileNums = lzip.getTotalFiles (zipPath);
					try {
						if (Directory.Exists (unZipTargetPath) == false)
							Directory.CreateDirectory (unZipTargetPath);
						#if !NETFX_CORE
						_unzipThread = new Thread (decompressFunc);
						_unzipThread.Start ();
						#endif
					} catch (Exception ) {

					}
				}
			}

			void decompressFunc ()
			{
				int res = lzip.decompress_File (zipPath, unZipTargetPath, _unCompressedFileNumsIndex);
				if (res == 1) {
					SetState (UnZipState.SUCCESS, res);
				} else {
					SetState (UnZipState.FAILED, res);
				}
			}

			void SetState (UnZipState state, int ret = 0)
			{
				_zipState = state;
				unzipRet = ret;
			}

			public void UnZipProgress (float p)
			{
				if (UnZipProgressCall != null)
					UnZipProgressCall (p);
			}

			public void Dispose ()
			{
				UnZipCompleteCall = null;
				UnZipProgressCall = null;
				UnZipFailedCallBack = null;
				if (_unzipThread != null) {
					#if UNITY_IOS || UNITY_IPHONE
					_unzipThread.Interrupt ();
					#else
					_unzipThread.Abort ();
					#endif
					_unzipThread = null;
				}
			}

			long _ReadUnzipInfo (string path)
			{
				long res = -1;
				try {
					res = lzip.getFileInfo (path, Application.persistentDataPath);
					if (res > 0) {
						currentUnzipFileNames = new List<string> (lzip.ninfo);
						currentUnzipFileUncompressSize = new List<long> (lzip.uinfo);
						currentUnzipFileCompressSize = new List<long> (lzip.cinfo);
						string persistentDataPath = Application.persistentDataPath;
						// delete files
						foreach (string filename in currentUnzipFileNames) {
							string name = Path.Combine (persistentDataPath, filename);
							if (File.Exists (name)) {
								File.Delete (name);
							}
						}
					} else {
						currentUnzipFileNames = null;
						currentUnzipFileUncompressSize = null;
						currentUnzipFileCompressSize = null;
					}
				} catch (Exception ) {
					res = -1;
					currentUnzipFileNames = null;
					currentUnzipFileUncompressSize = null;
				}
				unzipFilesInfo = new UnZipFilesInfo (zipPath, unZipTargetPath, this._completeParam, currentUnzipFileNames, currentUnzipFileUncompressSize, currentUnzipFileCompressSize);
				return res;
			}
		}

		public class UnZipFilesInfo
		{
			public string zipPath { get; private set; }

			public string unZipTargetPath { get; private set; }

			public object param { get; private set; }

			public List<string> currentUnzipFileNames{ get; private set; }

			public List<long> currentUnzipFileUncompressSize{ get; private set; }

			public List<long> currentUnzipFileCompressSize{ get; private set; }

			public UnZipFilesInfo (string zipfileName, string unzipDir, object param, List<string> unzipFileNames, List<long> uncompressSize, List<long> compressSize)
			{
				zipPath = zipfileName;
				unZipTargetPath = unzipDir;
				this.param = param;
				this.currentUnzipFileNames = unzipFileNames;
				this.currentUnzipFileUncompressSize = uncompressSize;
				this.currentUnzipFileCompressSize = compressSize;
			}

			public bool CheckExtractFiles (ABZipMD5Infos zipmd5Infos)
			{
				SDictionary<string,MD5Info> infos = new SDictionary<string, MD5Info> ();
				if (zipmd5Infos != null) {
					MD5Info info;
					for (int i = 0; i < zipmd5Infos.md5Infos.Count; i++) {
						info = zipmd5Infos.md5Infos [i];
						infos [info.fileName] = info;
					}
				}
				List<string> fileNames = currentUnzipFileNames;
				List<long> fileUncompressSize = currentUnzipFileUncompressSize;
				if (fileNames != null && fileUncompressSize != null && fileNames.Count == fileUncompressSize.Count) {
					string fileName = null;
					string md5 = null;
					long fileSize = 0;
					string persistentDataPath = Application.persistentDataPath;
					for (int i = 0; i < fileNames.Count; i++) {
						fileName = Path.Combine (persistentDataPath, fileNames [i]);
						fileSize = fileUncompressSize [i];
						FileInfo fi = new FileInfo (fileName);
						if (fi.Exists) {
							// 1.check file exist & size
							if (fi.Length == fileSize) {
								// 2. if file extension is not unity3d or script, check md5
								Path.GetExtension (fileName);
								md5 = infos [fileNames [i]].md5;
								if (!string.IsNullOrEmpty (md5) && md5 != MyMD5.HashFile (fileName)) {
									return false;
								}
							} else {
								return false;
							}
						} else {
							return false;
						}
					}
				} else {
					return false;
				}
				return true;
			}
		}
	}
}
// namespace RO
