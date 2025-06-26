using UnityEngine;
using System.Collections.Generic;
using System.Text;

	public class LuaWriter 
	{
		private StringBuilder buffer;

		private bool currentTableFirstElement = true;
		private bool allowElementValue = false;

		private int tabCount = 0;

		public LuaWriter(StringBuilder sb)
		{
			buffer = sb;
		}

		public void WriteTableStart(bool asValue = false)
		{
			if (asValue)
			{
				if (!allowElementValue)
				{
					return;
				}
				allowElementValue = false;
				buffer.Append('=');
			}
			else
			{
				if (!currentTableFirstElement)
				{
					buffer.Append(',');
					buffer.Append('\n');
				}
				if (0 < tabCount)
				{
					buffer.Append('\t', tabCount);
				}
			}
			buffer.Append('{');
			buffer.Append('\n');
			++tabCount;
			currentTableFirstElement = true;
		}

		public void WriteTableEnd()
		{
			buffer.Append('\n');
			--tabCount;
			if (0 < tabCount)
			{
				buffer.Append('\t', tabCount);
			}
			buffer.Append('}');
			currentTableFirstElement = false;
		}

		public void WriteElement(object v)
		{
			if (!currentTableFirstElement)
			{
				buffer.Append(',');
				buffer.Append('\n');
			}
			if (0 < tabCount)
			{
				buffer.Append('\t', tabCount);
			}
		    if(v.GetType().ToString().Equals(typeof(string).ToString()))
			{
				buffer.Append("\""+v+"\"");
			}
			else
			{
				buffer.Append(v);
			}			
			currentTableFirstElement = false;
		}


		public void WriteElementName(string n, string format="{0}")
		{
			if (!currentTableFirstElement)
			{
				buffer.Append(',');
				buffer.Append('\n');
			}
			if (0 < tabCount)
			{
				buffer.Append('\t', tabCount);
			}
			buffer.AppendFormat(format, n);
			allowElementValue = true;
			currentTableFirstElement = false;
		}

		public void WriteElementName(int index)
		{
			WriteElementName(index.ToString(), "[{0}]");
		}
		public void WriteElementName(long index)
		{
			WriteElementName(index.ToString(), "[{0}]");
		}
		public void WriteElementName(short index)
		{
			WriteElementName(index.ToString(), "[{0}]");
		}

		public void WriteElementValue(object v, string format = "={0}")
		{
			if (!allowElementValue)
			{
				WriteElement(v);
				return;
			}
			buffer.AppendFormat(format, v);
			allowElementValue = false;
		}

		public void WriteElementValue(string v)
		{
			WriteElementValue(v, "=\"{0}\"");
		}

		public void WriteElementValue(bool v)
		{
			WriteElementValue((v ? "true" : "false"), "={0}");
		}
	
	}
