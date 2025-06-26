using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

/// <summary>
/// detail of file or directory be used
/// </summary>
[XmlRoot("FilesUsedDetail")]
public class FilesUsedDetail
{
	[XmlRoot("FileUsedDetail")]
	public class FileUsedDetail
	{
		[XmlAttribute("id")]
		public int id;
		[XmlElement("path")]
		public string path;
		[XmlElement("usedTime")]
		public DateTime usedTime;

		public FileUsedDetail()
		{
			this.id = 0;
			this.path = "";
			this.usedTime = DateTime.Now;
		}
		public FileUsedDetail(int id, string path, DateTime used_time)
		{
			this.id = id;
			this.path = path;
			this.usedTime = used_time;
		}
	}

	private string m_path;
	[XmlElement("path")]
	public string Path
	{
		get {return m_path;}
		set {m_path = value;}
	}

	private List<FileUsedDetail> m_listDetial;
	[XmlArray("details"), XmlArrayItem("detail")]
	public FileUsedDetail[] Details
	{
		get
		{
			return m_listDetial.ToArray();
		}
		set
		{
			m_listDetial.Clear();
			m_listDetial.AddRange(value);
		}
	}

	public FilesUsedDetail()
	{
		m_path = "";
		m_listDetial = new List<FileUsedDetail>();
	}

	public FilesUsedDetail(string path)
	{
		this.m_path = path;
		m_listDetial = new List<FileUsedDetail>();
	}
	
	public FilesUsedDetail(string path, FileUsedDetail[] s_file_used_detail)
	{
		this.m_path = path;
		m_listDetial = new List<FileUsedDetail>(s_file_used_detail);
	}

	public void AddDetail(FileUsedDetail file_used_detail)
	{
		if (file_used_detail == null) return;
		int id = file_used_detail.id;
		if (ExistDetail(id)) return;
		m_listDetial.Add(file_used_detail);
	}

	public void UpdateDetail(int id, FileUsedDetail file_used_detail)
	{
		FileUsedDetail fileUsedDetail = m_listDetial.Find(x => x.id == id);
		if (fileUsedDetail == null) return;
		if (file_used_detail == null) return;
		fileUsedDetail.usedTime = file_used_detail.usedTime;
	}

	public void DeleteDetail(int id)
	{
		FileUsedDetail fileUsedDetail = m_listDetial.Find(x => x.id == id);
		if (fileUsedDetail == null) return;
		m_listDetial.Remove(fileUsedDetail);
	}

	public bool ExistDetail(int id)
	{
		FileUsedDetail fileUsedDetail = m_listDetial.Find(x => x.id == id);
		return fileUsedDetail != null;
	}

	public void Clean()
	{
		for (int i = m_listDetial.Count - 1; i >= 0; i--)
		{
			FileUsedDetail fileUsedDetail = m_listDetial[i];
			string path = fileUsedDetail.path;
			if (FileDirectoryHandler.ExistFile(path)) continue;
			if (FileDirectoryHandler.ExistDirectory(path)) continue;
			m_listDetial.RemoveAt(i);
			fileUsedDetail = null;
		}
	}

	public void Sort()
	{
		m_listDetial.Sort((x, y) => {
			return DateTime.Compare(x.usedTime, y.usedTime);
		});
	}
}