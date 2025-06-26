using System.Collections.Generic;
using System.Text;

class AstrolabeWriter
{
#if UNITY_EDITOR
    private StringBuilder buffer;
    private bool currentTableFirstElement = true;
    private bool allowElementValue = false;
    private int tabCount = 0;

    public AstrolabeWriter(StringBuilder sb)
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
                buffer.Append(' ');
            }
            if (0 < tabCount)
            {
                //buffer.Append('\t', tabCount);
            }
        }
        buffer.Append('{');
        //buffer.Append(' ');
        ++tabCount;
        currentTableFirstElement = true;
    }

    public void WriteTableEnd()
    {
        //buffer.Append(' ');
        --tabCount;
        if (0 < tabCount)
        {
            //buffer.Append('\t', tabCount);
        }
        buffer.Append('}');
        currentTableFirstElement = false;
    }

    public void WriteElement(object v)
    {
        if (!currentTableFirstElement)
        {
            buffer.Append(',');
            buffer.Append(' ');
        }
        if (0 < tabCount)
        {
            //buffer.Append('\t', tabCount);
        }
        if (v.GetType().ToString().Equals(typeof(string).ToString()))
        {
            buffer.Append("\"" + v + "\"");
        }
        else
        {
            buffer.Append(v);
        }
        currentTableFirstElement = false;
    }


    public void WriteElementName(string n, string format = "{0}", bool isLineBreaks = false, int tabsNum = 0)
    {
        if (!currentTableFirstElement)
        {
            buffer.Append(',');
        }
        if (0 < tabCount)
        {
            //buffer.Append('\t', tabCount);
        }
        if (isLineBreaks)
        {
            buffer.Append('\n');
            buffer.Append('\t', tabsNum);
        }
        else
        {
            buffer.Append(' ');
        }
        buffer.AppendFormat(format, n);
        allowElementValue = true;
        currentTableFirstElement = false;
    }

    public void WriteElementName(int index, bool isLineBreaks = false, int tabsNum = 0)
    {
        if (isLineBreaks)
            WriteElementName(index.ToString(), "[{0}]", isLineBreaks, tabsNum);
        else
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

    public void WriteLineBreaks()
    {
        buffer.Append('\n');
    }

    public void WriteTabs()
    {
        buffer.Append('\t');
    }
#endif
}

sealed class AstrolabeWriterApdater
{
#if UNITY_EDITOR
    public string rootName;
    public bool local;

    private AstrolabeWriter writer;

    private Stack<bool> arrayStates = new Stack<bool>();

    public AstrolabeWriterApdater(AstrolabeWriter w, string rn = "Root", bool l = false)
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

    public void WriteStructStart()
    {
        WriteLuaTableStart(false);
    }
    public void WriteStructEnd()
    {
        writer.WriteTableEnd();
        arrayStates.Pop();
    }

    public void WriteArrayStart()
    {
        WriteLuaTableStart(true);
    }
    public void WriteArrayEnd()
    {
        writer.WriteTableEnd();
        arrayStates.Pop();
    }

    public void WriteMemberName(int n, bool isLineBreaks = false, int tabsNum = 0)
    {
        if (isLineBreaks)
            writer.WriteElementName(n, isLineBreaks, tabsNum);
        else
            writer.WriteElementName(n);
    }
    public void WriteMemberName(string n)
    {
        writer.WriteElementName(n);
    }

    public void WriteMemberValue(long v)
    {
        writer.WriteElementValue(v);
    }
    public void WriteMemberValue(string v, string format = "={0}")
    {
        writer.WriteElementValue(v, format);
    }
    public void WriteMemberValue(ulong v)
    {
        writer.WriteElementValue(v);
    }
    public void WriteMemberValue(int v)
    {
        writer.WriteElementValue(v);
    }
    public void WriteMemberValue(bool v)
    {
        writer.WriteElementValue(v);
    }
    public void WriteMemberValue(decimal v)
    {
        writer.WriteElementValue(v);
    }
    public void WriteMemberValue(double v)
    {
        writer.WriteElementValue(v);
    }

    public void WriteLineBreaks()
    {
        writer.WriteLineBreaks();
    }

    public void WriteTabs()
    {
        writer.WriteTabs();
    }
#endif
}