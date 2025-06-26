using UnityEngine;
using System.Collections;
using RO;

[SLua.CustomLuaClassAttribute]
public class UIInputInsertContent : MonoBehaviour 
{
	public UIInput m_input=null;
    int m_cursorPosition=0;

	// Use this for initialization
	void Start () 
	{
		m_input=GetComponent<UIInput>();
		if(m_input)
		{
			m_cursorPosition=m_input.cursorPosition;
			EventDelegate.Add(m_input.onChange,Callback);
			UIEventListener.Get(m_input.gameObject).onClick+=ClickInput;
		}
	}

	public void InsertContent(string str)
	{
		string content = m_input.value;
		if(m_input&&!string.IsNullOrEmpty(str))
		{
			if(!string.IsNullOrEmpty(content))
			{
				//Logger.Log("cursorPosition = " +m_cursorPosition);
				string leftText=content.Substring(0,m_cursorPosition);
				string rightText=content.Substring(m_cursorPosition);
				//Logger.Log("leftText is "+leftText);
				//Logger.Log("rightText is "+rightText);
				m_input.value=leftText+str+rightText;
			}
			else
			{
				m_input.value=str;
			}
		}
	}

	void Callback()
	{
		m_cursorPosition=m_input.cursorPosition;
	}

	void ClickInput(GameObject obj)
	{
		m_cursorPosition=m_input.cursorPosition;
	}
}
