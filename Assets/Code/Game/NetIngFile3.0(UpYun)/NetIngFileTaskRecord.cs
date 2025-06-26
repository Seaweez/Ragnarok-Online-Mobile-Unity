using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Xml.Serialization;

[SLua.CustomLuaClassAttribute]
public class NetIngFileTaskRecord : Singleton<NetIngFileTaskRecord>
{
	public class RecordTaskData
	{
		[XmlElement("url")]
		public string url;
		[XmlElement("path")]
		public string path;
	}

	[XmlRoot("RecordDownloadTaskData")]
	public class RecordDownloadTaskData : RecordTaskData
	{
		
	}

	[XmlRoot("RecordUploadTaskData")]
	public class RecordUploadTaskData : RecordTaskData
	{
		
	}

	[XmlRoot("RecordBlocksUploadTaskData")]
	public class RecordBlocksUploadTaskData : RecordTaskData
	{
		[XmlRoot("DynamicData")]
		public class DynamicData
		{
			[XmlElement("currentStep")]
			public int currentStep;
			[XmlElement("blockIndex")]
			public int blockIndex;
			[XmlElement("saveTokenFromInitializeSuccess")]
			public string saveTokenFromInitializeSuccess;
			[XmlElement("saveToken")]
			public string saveToken;
			[XmlElement("tokenSecret")]
			public string tokenSecret;
		}

		[XmlElement("serverPath")]
		public string serverPath;
		[XmlElement("dynamicData")]
		public DynamicData dynamicData;
	}

	public static NetIngFileTaskRecord Ins
	{
		get {return ins;}
	}

	private const string RECORD_ROOT_PATH = "NetIngFileTaskRecord";
	private const string DOWNLOAD_RECORD_FILE_PATH = RECORD_ROOT_PATH + "/Download.xml";
	private const string UPLOAD_RECORD_FILE_PATH = RECORD_ROOT_PATH + "/Upload.xml";
	private const string BLOCKS_UPLOAD_RECORD_FILE_PATH = RECORD_ROOT_PATH + "BlocksUpload.xml";

	private RecordDownloadTaskData[] m_sDownloadData;
	public RecordDownloadTaskData[] SDownloadData
	{
		get {return m_sDownloadData;}
	}
	private RecordUploadTaskData[] m_sUploadData;
	public RecordUploadTaskData[] SUploadData
	{
		get {return m_sUploadData;}
	}
	private RecordBlocksUploadTaskData[] m_sBlocksUploadData;
	public RecordBlocksUploadTaskData[] SBlocksUploadData
	{
		get {return m_sBlocksUploadData;}
	}

	public void Initialize()
	{
		if (!FileDirectoryHandler.ExistDirectory(RECORD_ROOT_PATH))
		{
			FileDirectoryHandler.CreateDirectory(RECORD_ROOT_PATH);
		}
		else
		{
			if (FileDirectoryHandler.ExistFile(DOWNLOAD_RECORD_FILE_PATH))
			{
				m_sDownloadData = MyXmlSerializer.Deserialize<RecordDownloadTaskData[]>(DOWNLOAD_RECORD_FILE_PATH);
			}
			if (FileDirectoryHandler.ExistFile(UPLOAD_RECORD_FILE_PATH))
			{
				m_sUploadData = MyXmlSerializer.Deserialize<RecordUploadTaskData[]>(UPLOAD_RECORD_FILE_PATH);
			}
			if (FileDirectoryHandler.ExistFile(BLOCKS_UPLOAD_RECORD_FILE_PATH))
			{
				m_sBlocksUploadData = MyXmlSerializer.Deserialize<RecordBlocksUploadTaskData[]>(BLOCKS_UPLOAD_RECORD_FILE_PATH);
			}
		}
	}

	public void AddDownloadTask(string url, string path)
	{
		RecordDownloadTaskData taskData = new RecordDownloadTaskData();
		taskData.url = url;
		taskData.path = path;
		List<RecordDownloadTaskData> listData = new List<RecordDownloadTaskData>();
		if (m_sDownloadData != null)
		{
			listData.AddRange(m_sDownloadData);
		}
		listData.Add(taskData);
		m_sDownloadData = listData.ToArray();
		MyXmlSerializer.Serialize<RecordDownloadTaskData[]>(m_sDownloadData, DOWNLOAD_RECORD_FILE_PATH);
	}

	public void AddUploadTask(string url, string path)
	{
		RecordUploadTaskData taskData = new RecordUploadTaskData();
		taskData.url = url;
		taskData.path = path;
		List<RecordUploadTaskData> listData = new List<RecordUploadTaskData>();
		if (m_sUploadData != null)
		{
			listData.AddRange(m_sUploadData);
		}
		listData.Add(taskData);
		m_sUploadData = listData.ToArray();
		MyXmlSerializer.Serialize<RecordUploadTaskData[]>(m_sUploadData, UPLOAD_RECORD_FILE_PATH);
	}

	public void AddBlocksUploadTask(string url, string path, string server_path)
	{
		RecordBlocksUploadTaskData taskData = new RecordBlocksUploadTaskData();
		taskData.url = url;
		taskData.path = path;
		taskData.serverPath = server_path;
		List<RecordBlocksUploadTaskData> listData = new List<RecordBlocksUploadTaskData>();
		if (m_sBlocksUploadData != null)
		{
			listData.AddRange(m_sBlocksUploadData);
		}
		listData.Add(taskData);
		m_sBlocksUploadData = listData.ToArray();
		MyXmlSerializer.Serialize<RecordBlocksUploadTaskData[]>(m_sBlocksUploadData, BLOCKS_UPLOAD_RECORD_FILE_PATH);
	}

	public void RemoveDownloadTask(string path)
	{
		if (!string.IsNullOrEmpty(path))
		{
			List<RecordDownloadTaskData> listData = new List<RecordDownloadTaskData>(m_sDownloadData);
			RecordDownloadTaskData data = listData.Find(x => x.path == path);
			if (data != null)
			{
				listData.Remove(data);
			}
			m_sDownloadData = listData.ToArray();
			MyXmlSerializer.Serialize<RecordDownloadTaskData[]>(m_sDownloadData, DOWNLOAD_RECORD_FILE_PATH);
		}
	}

	public void RemoveUploadTask(string path)
	{
		if (!string.IsNullOrEmpty(path))
		{
			List<RecordUploadTaskData> listData = new List<RecordUploadTaskData>(m_sUploadData);
			RecordUploadTaskData data = listData.Find(x => x.path == path);
			if (data != null)
			{
				listData.Remove(data);
			}
			m_sUploadData = listData.ToArray();
			MyXmlSerializer.Serialize<RecordUploadTaskData[]>(m_sUploadData, UPLOAD_RECORD_FILE_PATH);
		}
	}

	public void RemoveBlocksUploadTask(string path)
	{
		if (!string.IsNullOrEmpty(path))
		{
			List<RecordBlocksUploadTaskData> listData = new List<RecordBlocksUploadTaskData>(m_sBlocksUploadData);
			RecordBlocksUploadTaskData data = listData.Find(x => x.path == path);
			if (data != null)
			{
				listData.Remove(data);
			}
			m_sBlocksUploadData = listData.ToArray();
			MyXmlSerializer.Serialize<RecordBlocksUploadTaskData[]>(m_sBlocksUploadData, BLOCKS_UPLOAD_RECORD_FILE_PATH);
		}
	}

	public void DynamicDataComing(string path, string data_type, object data_value)
	{
		if (!string.IsNullOrEmpty(path))
		{
			List<RecordBlocksUploadTaskData> listData = new List<RecordBlocksUploadTaskData>(m_sBlocksUploadData);
			RecordBlocksUploadTaskData data = listData.Find(x => x.path == path);
			if (data != null)
			{
				if (data_value != null)
				{
					if (data.dynamicData == null)
					{
						data.dynamicData = new RecordBlocksUploadTaskData.DynamicData();
					}
					if (string.Equals(data_type, "cs"))
					{
						data.dynamicData.currentStep = (int)data_value;
					}
					else if (string.Equals(data_type, "bi"))
					{
						data.dynamicData.blockIndex = (int)data_value;
					}
					else if (string.Equals(data_type, "stfis"))
					{
						data.dynamicData.saveTokenFromInitializeSuccess = (string)data_value;
					}
					else if (string.Equals(data_type, "st"))
					{
						data.dynamicData.saveToken = (string)data_value;
					}
					else if (string.Equals(data_type, "ts"))
					{
						data.dynamicData.tokenSecret = (string)data_value;
					}
				}
			}
			m_sBlocksUploadData = listData.ToArray();
			MyXmlSerializer.Serialize<RecordBlocksUploadTaskData[]>(m_sBlocksUploadData, BLOCKS_UPLOAD_RECORD_FILE_PATH);
		}
	}

	public void Restore()
	{

	}
}
