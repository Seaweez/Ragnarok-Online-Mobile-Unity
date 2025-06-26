#if !UNITY_3_5
#define DYNAMIC_FONT
#endif
using UnityEngine;
using System.Collections.Generic;

namespace RO
{
	[SLua.CustomLuaClassAttribute]
	public class UIRichLabel:UILabel
	{
		string[] stringSeparators = new string[] { "\n" };
		public List<RichSpriteTextData> symbols = new List<RichSpriteTextData> ();
		public List<UISprite> sprites = new List<UISprite> ();
		List<RichSpriteLineInfo> lineList = new List<RichSpriteLineInfo> ();
		SDictionary<int,float> lineHasSp = new SDictionary<int, float> ();
		SDictionary<int,List<RichSpriteTextData>> lineSymbols = new SDictionary<int, List<RichSpriteTextData>> ();
		public string space = "     ";
		public int BASELINEWIDTH = 300;
		public Vector2 iconSize;
		public int m_offsetX = 0;
		int _Lines = 0;
		int _minHeight = 2;

		public int Lines {
			get {
				return _Lines;
			}
		}

		public int MinHeight {
			get {
				return _minHeight;
			}
			set {
				_minHeight = value;
			}
		}

		public override int minHeight {
			get {
				return _minHeight;
			}
		}

		public void RemoveSprites ()
		{
			for (int i = 0; i < sprites.Count; i++) {
				GameObject.Destroy (sprites [i].gameObject);
			}
			sprites.Clear ();
		}

		static bool IsSpace (int ch)
		{
			return (ch == ' ' || ch == 0x200a || ch == 0x200b || ch == '\u2009');
		}

		string parseText (string text, int line)
		{
			UpdateNGUIText ();
			NGUIText.finalSize = defaultFontSize;//设置当前使用字体大小
			NGUIText.regionWidth += (int)(iconSize.x);
			lineList.Clear ();
			int row = line;
			int textWidth = 0;
			int lastStartIndex = 0;
			string curLine = "";
			int length = text.Length;
			bool findStart = false, findEnd = false;
			bool symbolStart = false, symbolEnd = false;
			string findPartn = "";
			char c;
			char nextC;
			bool eastern = false;
			int lastSpaceIndex = 0;
			bool wordend = true;
			for (int offset = 0; offset < length; offset++) { 
				c = text [offset];
				if (c > 12287)
					eastern = true;
				
				if (offset + 1 < length) {
					nextC = text [offset + 1];
				} else {
					nextC = '\0';
				}
				wordend = true;
				if (!eastern) {
					if (IsSpace (c)) {
						lastSpaceIndex = offset;				
					} else {
						if (!IsSpace (nextC)) {
							wordend = false;
						}
					}
				}
				if (c == '[' && !symbolStart) {
					//如果是字母的话
					if ((nextC >= 'a' && nextC <= 'z') || (nextC >= 'A' && nextC <= 'Z') || nextC == '-' || nextC == '/')
						symbolStart = true;
				}
				if (c == ']' && symbolStart)
					symbolStart = false;
				if (c == '{' && !findStart) {
					findPartn = "";
					findStart = true;
				}
				if (findStart && !findEnd) {
					if (c == '}')
						findEnd = true;
					findPartn += c;
				}
				if (!findStart && !findEnd) {
					if (offset - lastStartIndex < 0)
						continue;
					
					int curWidth = 0;
					if (!symbolStart && !symbolEnd) {
						curWidth = Mathf.RoundToInt (
							NGUIText.CalculatePrintedSize (text.Substring (lastStartIndex, offset - lastStartIndex + 1)).x);
						if (curWidth > BASELINEWIDTH) {
							if (!wordend && lastSpaceIndex > lastStartIndex) {
								curLine = text.Substring (lastStartIndex, lastSpaceIndex - lastStartIndex);
								// revert to last space index
								offset = lastSpaceIndex + 1;
							} else {
								curLine = text.Substring (lastStartIndex, offset - lastStartIndex);
							}
							lineList.Add (new RichSpriteLineInfo (curLine, GetTextWidth (curLine)));
							lastStartIndex = offset;
							row++;
						}
					}
					if (offset == length - 1) {
						if (offset - lastStartIndex < 0)
							continue;
						
						curLine = text.Substring (lastStartIndex, offset - lastStartIndex + 1);
						lineList.Add (new RichSpriteLineInfo (curLine, curWidth));
					}
				} else if (findStart && findEnd) {
					Vector3 ePos = Vector3.zero;
					float fx = 0;
					findStart = false;
					findEnd = false;
					offset = text.IndexOf (findPartn);
					text = text.Remove (offset, findPartn.Length);
					text = text.Insert (offset, space);
					length = text.Length;
					//这里的CalculatePrintedSize是重载过的，
					//与原本方法相比多的一个参数自定义行款，替换原方法中的rectWidth即可
					textWidth = Mathf.RoundToInt (
						NGUIText.CalculatePrintedSize (text.Substring (lastStartIndex, offset - lastStartIndex)).x);
					//BASELINEWIDTH为标准行宽度，30是根据表情大小确定的，
					//这里的表情大小是30*30
					if (textWidth > BASELINEWIDTH - iconSize.x) {
						if (!wordend && lastSpaceIndex > lastStartIndex) {
							curLine = text.Substring (lastStartIndex, lastSpaceIndex - lastStartIndex + 1);
							// revert to last space index
							offset = lastSpaceIndex + 1;
						} else {
							curLine = text.Substring (lastStartIndex, offset - lastStartIndex + 1);
						}

						lineList.Add (new RichSpriteLineInfo (curLine, GetTextWidth (curLine)));
						
						if (textWidth <= BASELINEWIDTH - 0 ||
						    textWidth >= BASELINEWIDTH) {//行末尾不够需换行
							fx = 0;
							row++;
							lastStartIndex = offset;
							ePos.x = fx - m_offsetX;
							ePos.y = row;
							ePos.z = 0;
						} else {   //行末尾足够不需换行
							fx = textWidth;
							lastStartIndex = offset + space.Length;
							ePos.x = fx - m_offsetX;
							ePos.y = row;
							ePos.z = 0;
							row++;
						}
					} else {
						fx = textWidth;
						ePos.x = fx - m_offsetX;
						ePos.y = row;
						ePos.z = 0;
					}
					RichSpriteTextData rst = new RichSpriteTextData (findPartn, ePos);
					symbols.Add (rst);
					List<RichSpriteTextData> sys = lineSymbols [row];
					if (sys == null) {
						sys = new List<RichSpriteTextData> ();
						lineSymbols [row] = sys;
					}
					sys.Add (rst);
					lineHasSp [row] = iconSize.y;
				}
			}
			_Lines = row + 1;
			string res = "";
			Vector2 pivot = this.pivotOffset;
			int width = this.width;
			RichSpriteLineInfo lineInfo;
			for (int i = 0; i < lineList.Count; i++) {
				lineInfo = lineList [i];
				res += lineInfo.lineText + (i != lineList.Count - 1 ? "\n" : "");
				if (pivot.x != 0) {
					List<RichSpriteTextData> sys = lineSymbols [i + line];
					if (sys != null) {
						foreach (RichSpriteTextData rstd in sys) {
							rstd.lineWidth = lineInfo.width;
							if (pivot.x == 0.5f)
								rstd.pos.x -= (lineInfo.width) / 2;
						}
					}
				}
			}
			return res;
		}

		public string ParseText (string text)
		{
			Reset ();
			string[] lines = text.Split (stringSeparators, System.StringSplitOptions.None);
			string res = "";
			_Lines = 0;
			for (int i = 0; i < lines.Length; i++) {
				res += parseText (lines [i], _Lines) + (i != lines.Length - 1 ? "\n" : "");
			}
			return res;
		}

		public int GetSpLineTotalHeight (int line)
		{
			//TODO 强制认为scale为1
			float finalLineHeight = (fontSize + spacingY) * 1;
			float height = 0;
			float temp = 0;
			for (; line > 0; line--) {
				temp = lineHasSp [line];
				if (temp > 0) {
					height += Mathf.Max (finalLineHeight, temp);
				} else
					height += finalLineHeight;
			}
			return Mathf.RoundToInt (height);
		}

		int GetTextWidth (string text)
		{
			return Mathf.RoundToInt (NGUIText.CalculatePrintedSize (text).x);
		}

		public void Reset ()
		{
			symbols.Clear ();
			lineList.Clear ();
			lineHasSp.Clear ();
			lineSymbols.Clear ();
		}

		#if DYNAMIC_FONT
		bool isValid { get { return bitmapFont != null || trueTypeFont != null; } }
		#else
		bool isValid { get { return bitmapFont != null; } }
		#endif

		public override void OnFill (BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
		{
			if (!isValid)
				return;
			
			int offset = verts.size;
			Color col = color;
			col.a = finalAlpha;
			
			if (bitmapFont != null && bitmapFont.premultipliedAlphaShader)
				col = NGUITools.ApplyPMA (col);
			
			if (QualitySettings.activeColorSpace == ColorSpace.Linear) {
				col.r = Mathf.GammaToLinearSpace (col.r);
				col.g = Mathf.GammaToLinearSpace (col.g);
				col.b = Mathf.GammaToLinearSpace (col.b);
			}
			
			string text = processedText;
			int start = verts.size;
			
			UpdateNGUIText ();
			RichTextUtil.lineHasSp = lineHasSp;
			
			NGUIText.tint = col;
			RichTextUtil.Print (text, verts, uvs, cols);
			NGUIText.bitmapFont = null;
			#if DYNAMIC_FONT
			NGUIText.dynamicFont = null;
			#endif
			// Center the content within the label vertically
			Vector2 pos = ApplyOffset (verts, start);
			
			// Effects don't work with packed fonts
			if (bitmapFont != null && bitmapFont.packedFontShader)
				return;
			
			// Apply an effect if one was requested
			if (effectStyle != Effect.None) {
				int end = verts.size;
				pos.x = effectDistance.x;
				pos.y = effectDistance.y;
				
				ApplyShadow (verts, uvs, cols, offset, end, pos.x, -pos.y);
				
				if ((effectStyle == Effect.Outline) || (effectStyle == Effect.Outline8)) {
					offset = end;
					end = verts.size;
					
					ApplyShadow (verts, uvs, cols, offset, end, -pos.x, pos.y);
					
					offset = end;
					end = verts.size;
					
					ApplyShadow (verts, uvs, cols, offset, end, pos.x, pos.y);
					
					offset = end;
					end = verts.size;
					
					ApplyShadow (verts, uvs, cols, offset, end, -pos.x, -pos.y);
					
					if (effectStyle == Effect.Outline8) {
						offset = end;
						end = verts.size;
						
						ApplyShadow (verts, uvs, cols, offset, end, -pos.x, 0);
						
						offset = end;
						end = verts.size;
						
						ApplyShadow (verts, uvs, cols, offset, end, pos.x, 0);
						
						offset = end;
						end = verts.size;
						
						ApplyShadow (verts, uvs, cols, offset, end, 0, pos.y);
						
						offset = end;
						end = verts.size;
						
						ApplyShadow (verts, uvs, cols, offset, end, 0, -pos.y);
					}
				}
			}
			
			if (onPostFill != null)
				onPostFill (this, offset, verts, uvs, cols);
		}

	}

	[SLua.CustomLuaClassAttribute]
	public class RichSpriteTextData
	{
		public string info;
		public Vector3 pos;
		public int lineWidth;

		public RichSpriteTextData (string info, Vector3 pos)
		{
			this.info = info;
			this.pos = pos;
		}
	}

	public class RichSpriteLineInfo
	{
		public string lineText;
		public int width;

		public RichSpriteLineInfo (string lineText, int width)
		{
			this.lineText = lineText;
			this.width = width;
		}
	}
}
// namespace RO
