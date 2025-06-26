using UnityEngine;
using System.Collections.Generic;
using System.Text;
using Ghost.Extensions;
using Ghost.Utils;
using LitJson;

	public abstract class WriterAdapter
	{
		public abstract void WriteStructStart();
		public abstract void WriteStructEnd();

		public abstract void WriteArrayStart();
		public abstract void WriteArrayEnd();

		public abstract void WriteMemberName(int n);
		public abstract void WriteMemberName(long n);
		public abstract void WriteMemberName(short n);
		public abstract void WriteMemberName(string n);
		public abstract void WriteMemberNameAsIndex(string n);

		public abstract void WriteMemberValue(long number);
		public abstract void WriteMemberValue(string str);
		public abstract void WriteMemberValue(ulong number);
		public abstract void WriteMemberValue(int number);
		public abstract void WriteMemberValue(bool boolean);
		public abstract void WriteMemberValue(decimal number);
		public abstract void WriteMemberValue(double number);
	}

	public sealed class JsonWriterAdapter : WriterAdapter
	{
		private JsonWriter writer;

		public JsonWriterAdapter(JsonWriter w)
		{
			writer = w;
		}

		public override void WriteStructStart()
		{
			writer.WriteObjectStart();
		}
		public override void WriteStructEnd()
		{
			writer.WriteObjectEnd();
		}

		public override void WriteArrayStart()
		{
			writer.WriteArrayStart();
		}
		public override void WriteArrayEnd()
		{
			writer.WriteArrayEnd();
		}

		public override void WriteMemberName(int n)
		{
			writer.WritePropertyName(n.ToString());
		}
		public override void WriteMemberName(long n)
		{
			writer.WritePropertyName(n.ToString());
		}
		public override void WriteMemberName(short n)
		{
			writer.WritePropertyName(n.ToString());
		}
		public override void WriteMemberName(string n)
		{
			writer.WritePropertyName(n);
		}
		public override void WriteMemberNameAsIndex(string n)
		{
			writer.WritePropertyName(n);
		}

		public override void WriteMemberValue(long v)
		{
			writer.Write(v);
		}
		public override void WriteMemberValue(string v)
		{
			writer.Write(v);
		}
		public override void WriteMemberValue(ulong v)
		{
			writer.Write(v);
		}
		public override void WriteMemberValue(int v)
		{
			writer.Write(v);
		}
		public override void WriteMemberValue(bool v)
		{
			writer.Write(v);
		}
		public override void WriteMemberValue(decimal v)
		{
			writer.Write(v);
		}
		public override void WriteMemberValue(double v)
		{
			writer.Write(v);
		}
	}

	public sealed class LuaWriterApdater : WriterAdapter
	{
		public string rootName;
		public bool local;

		private LuaWriter writer;

		private Stack<bool> arrayStates = new Stack<bool>();
		
		public LuaWriterApdater(LuaWriter w, string rn = "Root", bool l = false)
		{
			writer = w;
			rootName = rn;
			local = l;
		}

		private void WriteLuaTableStart(bool asArray)
		{
			if (0 >= arrayStates.Count)
			{
				if (string.IsNullOrEmpty(rootName))
				{
					writer.WriteTableStart(false);
				}
				else
				{
					WriteMemberName(local ? string.Format("local {0}", rootName) : rootName);
					writer.WriteTableStart(true);
				}
			}
			else
			{
				writer.WriteTableStart(!arrayStates.Peek());
			}
			arrayStates.Push(asArray);
		}

		public override void WriteStructStart()
		{
			WriteLuaTableStart(false);
		}
		public override void WriteStructEnd()
		{
			writer.WriteTableEnd();
			arrayStates.Pop();
		}

		public override void WriteArrayStart()
		{
			WriteLuaTableStart(true);
		}
		public override void WriteArrayEnd()
		{
			writer.WriteTableEnd();
			arrayStates.Pop();
		}

		public override void WriteMemberName(int n)
		{
			writer.WriteElementName(n);
		}
		public override void WriteMemberName(long n)
		{
			writer.WriteElementName(n);
		}
		public override void WriteMemberName(short n)
		{
			writer.WriteElementName(n);
		}
		public override void WriteMemberName(string n)
		{
			writer.WriteElementName(n);
		}
		public override void WriteMemberNameAsIndex(string n)
		{
		writer.WriteElementName(n, "[\"{0}\"]");
		}
		
		public override void WriteMemberValue(long v)
		{
			writer.WriteElementValue(v);
		}
		public override void WriteMemberValue(string v)
		{
			writer.WriteElementValue(v);
		}
		public override void WriteMemberValue(ulong v)
		{
			writer.WriteElementValue(v);
		}
		public override void WriteMemberValue(int v)
		{
			writer.WriteElementValue(v);
		}
		public override void WriteMemberValue(bool v)
		{
			writer.WriteElementValue(v);
		}
		public override void WriteMemberValue(decimal v)
		{
			writer.WriteElementValue(v);
		}
		public override void WriteMemberValue(double v)
		{
			writer.WriteElementValue(v);
		}
	}

	public static class DataWriter 
	{
		public static WriterAdapter GetAdapter(JsonWriter writer)
		{
			return new JsonWriterAdapter(writer);
		}

		public static WriterAdapter GetAdapter(LuaWriter writer, string rootName = "Root", bool local = false)
		{
			return new LuaWriterApdater(writer, rootName, local);
		}
	}

	public sealed class CSVWriter
	{
		public const string Separator = ",";

		public int row{get;private set;}
		public int col{get;private set;}

		private StringBuilder buffer;
		private int writtenCols = 0;

		public CSVWriter(int c, StringBuilder sb)
		{
			Debug.Assert(0 < c && null != sb);
			col = c;
			buffer = sb;
		}

		public bool WriteNewRow(object[] objs = null)
		{
			if (null != objs && objs.Length > col)
			{
				return false;
			}
			if (0 < row)
			{
				if (writtenCols < col)
				{
					buffer.Append(',', col-writtenCols);
				}
				buffer.AppendLine();
			}
			if (null != objs)
			{
				buffer.Append(StringUtils.ArrayToStringWithSeparator(Separator, objs));
				writtenCols = objs.Length;
			}
			else
			{
				writtenCols = 0;
			}
			++row;
			return true;
		}

		public bool Write(object[] objs)
		{
			if (objs.IsNullOrEmpty())
			{
				return false;
			}
			if (writtenCols + objs.Length > col)
			{
				return false;
			}
			if (0 < row)
			{
				return WriteNewRow(objs);
			}
			if (0 < writtenCols)
			{
				buffer.Append(Separator);
			}
			buffer.Append(StringUtils.ArrayToStringWithSeparator(Separator, objs));
			writtenCols += objs.Length;
			return true;
		}
	}