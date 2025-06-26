using UnityEngine;
using System.Collections;

[SLua.CustomLuaClassAttribute]
public class UILabelWithTail : UILabel
{
	public new string text
	{
		get
		{
			return base.text;
		}
		set
		{
			float width = 0;
			int validLength = 0;
			UpdateNGUIText();
			float tailWidth = NGUIText.CalculatePrintedSize("..").x;
			foreach(char ch in value)
			{
				width += NGUIText.CalculatePrintedSize(ch.ToString()).x;
				validLength++;
				float widgetWidth = localSize.x;
				if (width > widgetWidth)
				{
					while (widgetWidth - width < tailWidth)
					{
						validLength--;
						value = value.Substring(0, validLength);
						width = NGUIText.CalculatePrintedSize(value).x;
					}
					value += "...";
					break;
				}
			}
//			Vector2 size = NGUIText.CalculatePrintedSize(value);
//			int offset = CalculateOffsetToFit(value);
//			if (offset > 0)
//			{
//				offset++;
//				value = value.Substring(0, value.Length - offset);
//				value += "...";
//			}
			base.text = value;
		}
	}
}
