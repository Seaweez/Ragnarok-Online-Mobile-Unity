using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics;

namespace RO
{
	public class RichTextUtil
	{
		static public SDictionary<int,float> lineHasSp = new SDictionary<int, float> ();
		static BetterList<Color> mColors = new BetterList<Color> ();
		static float mAlpha = 1f;
		static public UIFont bitmapFont;
		static public float finalSpacingX = 0f;
		static public float finalLineHeight = 0f;
		static public bool encoding = false;
		static public bool premultiply = false;
		static public bool gradient = false;
		static public bool useSymbols = false;
		static public NGUIText.Alignment alignment = NGUIText.Alignment.Left;
		static public int fontSize = 16;
		static public int regionWidth = 1000000;
		static public float fontScale = 1f;
		static public float pixelDensity = 1f;
		static public Color tint = Color.white;
		static public Color gradientBottom = Color.white;
		static public Color gradientTop = Color.white;
		static public NGUIText.SymbolStyle symbolStyle;

		[DebuggerHidden]
		[DebuggerStepThrough]
		static bool IsSpace (int ch)
		{
			return (ch == ' ' || ch == 0x200a || ch == 0x200b || ch == '\u2009');
		}

		static Color32 s_c0, s_c1;
		static float[] mBoldOffset = new float[]
		{
			-0.25f, 0f, 0.25f, 0f,
			0f, -0.25f, 0f, 0.25f
		};

		static public void Print (string text, BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
		{
			if (string.IsNullOrEmpty (text))
				return;
			tint = NGUIText.tint;
			gradientBottom = NGUIText.gradientBottom;
			gradientTop = NGUIText.gradientTop;
			pixelDensity = NGUIText.pixelDensity;
			bitmapFont = NGUIText.bitmapFont;
			alignment = NGUIText.alignment;
			finalSpacingX = NGUIText.finalSpacingX;
			finalLineHeight = NGUIText.finalLineHeight;
			encoding = NGUIText.encoding;
			premultiply = NGUIText.premultiply;
			gradient = NGUIText.gradient;
			useSymbols = NGUIText.useSymbols;
			fontScale = NGUIText.fontScale;
			fontSize = NGUIText.fontSize;
			regionWidth = NGUIText.regionWidth;
			symbolStyle = NGUIText.symbolStyle;

			int indexOffset = verts.size;
			NGUIText.Prepare (text);
			
			// Start with the white tint
			mColors.Add (Color.white);
			mAlpha = 1f;
			
			int ch = 0, prev = 0;
			float x = 0f, y = 0f, maxX = 0f;
			float sizeF = NGUIText.finalSize;
			
			Color gb = NGUIText.tint * gradientBottom;
			Color gt = NGUIText.tint * gradientTop;
			Color32 uc = tint;
			int textLength = text.Length;
			
			Rect uvRect = new Rect ();
			float invX = 0f, invY = 0f;
			float sizePD = sizeF * pixelDensity;
			
			// Advanced symbol support contributed by Rudy Pangestu.
			bool subscript = false;
			int subscriptMode = 0;  // 0 = normal, 1 = subscript, 2 = superscript
			bool bold = false;
			bool italic = false;
			bool underline = false;
			bool strikethrough = false;
			bool ignoreColor = false;
			const float sizeShrinkage = 0.75f;
			
			float v0x;
			float v1x;
			float v1y;
			float v0y;
			float prevX = 0;
			
			if (bitmapFont != null) {
				uvRect = bitmapFont.uvRect;
				invX = uvRect.width / bitmapFont.texWidth;
				invY = uvRect.height / bitmapFont.texHeight;
			}
			//行数
			int lines = 0;

			for (int i = 0; i < textLength; ++i) {
				ch = text [i];
				
				prevX = x;
				
				// New line character -- skip to the next line
				if (ch == '\n') {
					if (x > maxX)
						maxX = x;
					
					if (alignment != NGUIText.Alignment.Left) {
						NGUIText.Align (verts, indexOffset, x - finalSpacingX);
						indexOffset = verts.size;
					}
					
					x = 0;
					float iconHeight = lineHasSp [lines+1];
					if (iconHeight>0) {
						y += Mathf.Max(finalLineHeight,iconHeight);
					} else
						y += finalLineHeight;

					prev = 0;
					lines++;
					continue;
				}
				
				// Invalid character -- skip it
				if (ch < ' ') {
					prev = ch;
					continue;
				}
				
				// Color changing symbol
				if (encoding && NGUIText.ParseSymbol (text, ref i, mColors, premultiply, ref subscriptMode, ref bold,
				                            ref italic, ref underline, ref strikethrough, ref ignoreColor)) {
					Color fc;
					
					if (ignoreColor) {
						fc = mColors [mColors.size - 1];
						fc.a *= mAlpha * tint.a;
					} else {
						fc = tint * mColors [mColors.size - 1];
						fc.a *= mAlpha;
					}
					uc = fc;
					
					for (int b = 0, bmax = mColors.size - 2; b < bmax; ++b)
						fc.a *= mColors [b].a;
					
					if (gradient) {
						gb = gradientBottom * fc;
						gt = gradientTop * fc;
					}
					--i;
					continue;
				}
				
				// See if there is a symbol matching this text
				BMSymbol symbol = useSymbols ? NGUIText.GetSymbol (text, i, textLength) : null;
				
				if (symbol != null) {
					v0x = x + symbol.offsetX * fontScale;
					v1x = v0x + symbol.width * fontScale;
					v1y = -(y + symbol.offsetY * fontScale);
					v0y = v1y - symbol.height * fontScale;
					
					// Doesn't fit? Move down to the next line
					if (Mathf.RoundToInt (x + symbol.advance * fontScale) > regionWidth) {
						if (x == 0f)
							return;
						
						if (alignment != NGUIText.Alignment.Left && indexOffset < verts.size) {
							NGUIText.Align (verts, indexOffset, x - finalSpacingX);
							indexOffset = verts.size;
						}
						
						v0x -= x;
						v1x -= x;
						v0y -= finalLineHeight;
						v1y -= finalLineHeight;
						
						x = 0;
						y += finalLineHeight;
						prevX = 0;
					}
					
					verts.Add (new Vector3 (v0x, v0y));
					verts.Add (new Vector3 (v0x, v1y));
					verts.Add (new Vector3 (v1x, v1y));
					verts.Add (new Vector3 (v1x, v0y));
					
					x += finalSpacingX + symbol.advance * fontScale;
					i += symbol.length - 1;
					prev = 0;
					
					if (uvs != null) {
						Rect uv = symbol.uvRect;
						
						float u0x = uv.xMin;
						float u0y = uv.yMin;
						float u1x = uv.xMax;
						float u1y = uv.yMax;
						
						uvs.Add (new Vector2 (u0x, u0y));
						uvs.Add (new Vector2 (u0x, u1y));
						uvs.Add (new Vector2 (u1x, u1y));
						uvs.Add (new Vector2 (u1x, u0y));
					}
					
					if (cols != null) {
						if (symbolStyle == NGUIText.SymbolStyle.Colored) {
							for (int b = 0; b < 4; ++b)
								cols.Add (uc);
						} else {
							Color32 col = Color.white;
							col.a = uc.a;
							for (int b = 0; b < 4; ++b)
								cols.Add (col);
						}
					}
				} else { // No symbol present
					NGUIText.GlyphInfo glyph = NGUIText.GetGlyph (ch, prev);
					if (glyph == null)
						continue;
					prev = ch;
					
					if (subscriptMode != 0) {
						glyph.v0.x *= sizeShrinkage;
						glyph.v0.y *= sizeShrinkage;
						glyph.v1.x *= sizeShrinkage;
						glyph.v1.y *= sizeShrinkage;
						
						if (subscriptMode == 1) {
							glyph.v0.y -= fontScale * fontSize * 0.4f;
							glyph.v1.y -= fontScale * fontSize * 0.4f;
						} else {
							glyph.v0.y += fontScale * fontSize * 0.05f;
							glyph.v1.y += fontScale * fontSize * 0.05f;
						}
					}
					
					v0x = glyph.v0.x + x;
					v0y = glyph.v0.y - y;
					v1x = glyph.v1.x + x;
					v1y = glyph.v1.y - y;
					
					float w = glyph.advance;
					if (finalSpacingX < 0f)
						w += finalSpacingX;
					
					// Doesn't fit? Move down to the next line
					if (Mathf.RoundToInt (x + w) > regionWidth) {
						if (x == 0f)
							return;
						
						if (alignment != NGUIText.Alignment.Left && indexOffset < verts.size) {
							NGUIText.Align (verts, indexOffset, x - finalSpacingX);
							indexOffset = verts.size;
						}
						
						v0x -= x;
						v1x -= x;
						v0y -= finalLineHeight;
						v1y -= finalLineHeight;
						
						x = 0;
						y += finalLineHeight;
						prevX = 0;
					}
					
					if (IsSpace (ch)) {
						if (underline) {
							ch = '_';
						} else if (strikethrough) {
							ch = '-';
						}
					}
					
					// Advance the position
					x += (subscriptMode == 0) ? finalSpacingX + glyph.advance :
						(finalSpacingX + glyph.advance) * sizeShrinkage;
					
					// No need to continue if this is a space character
					if (IsSpace (ch))
						continue;
					
					// Texture coordinates
					if (uvs != null) {
						if (bitmapFont != null) {
							glyph.u0.x = uvRect.xMin + invX * glyph.u0.x;
							glyph.u2.x = uvRect.xMin + invX * glyph.u2.x;
							glyph.u0.y = uvRect.yMax - invY * glyph.u0.y;
							glyph.u2.y = uvRect.yMax - invY * glyph.u2.y;
							
							glyph.u1.x = glyph.u0.x;
							glyph.u1.y = glyph.u2.y;
							
							glyph.u3.x = glyph.u2.x;
							glyph.u3.y = glyph.u0.y;
						}
						
						for (int j = 0, jmax = (bold ? 4 : 1); j < jmax; ++j) {
							uvs.Add (glyph.u0);
							uvs.Add (glyph.u1);
							uvs.Add (glyph.u2);
							uvs.Add (glyph.u3);
						}
					}
					
					// Vertex colors
					if (cols != null) {
						if (glyph.channel == 0 || glyph.channel == 15) {
							if (gradient) {
								float min = sizePD + glyph.v0.y / fontScale;
								float max = sizePD + glyph.v1.y / fontScale;
								
								min /= sizePD;
								max /= sizePD;
								
								s_c0 = Color.Lerp (gb, gt, min);
								s_c1 = Color.Lerp (gb, gt, max);
								
								for (int j = 0, jmax = (bold ? 4 : 1); j < jmax; ++j) {
									cols.Add (s_c0);
									cols.Add (s_c1);
									cols.Add (s_c1);
									cols.Add (s_c0);
								}
							} else {
								for (int j = 0, jmax = (bold ? 16 : 4); j < jmax; ++j)
									cols.Add (uc);
							}
						} else {
							// Packed fonts come as alpha masks in each of the RGBA channels.
							// In order to use it we need to use a special shader.
							//
							// Limitations:
							// - Effects (drop shadow, outline) will not work.
							// - Should not be a part of the atlas (eastern fonts rarely are anyway).
							// - Lower color precision
							
							Color col = uc;
							
							col *= 0.49f;
							
							switch (glyph.channel) {
							case 1:
								col.b += 0.51f;
								break;
							case 2:
								col.g += 0.51f;
								break;
							case 4:
								col.r += 0.51f;
								break;
							case 8:
								col.a += 0.51f;
								break;
							}
							
							Color32 c = col;
							for (int j = 0, jmax = (bold ? 16 : 4); j < jmax; ++j)
								cols.Add (c);
						}
					}
					
					// Bold and italic contributed by Rudy Pangestu.
					if (!bold) {
						if (!italic) {
							verts.Add (new Vector3 (v0x, v0y));
							verts.Add (new Vector3 (v0x, v1y));
							verts.Add (new Vector3 (v1x, v1y));
							verts.Add (new Vector3 (v1x, v0y));
						} else { // Italic
							float slant = fontSize * 0.1f * ((v1y - v0y) / fontSize);
							verts.Add (new Vector3 (v0x - slant, v0y));
							verts.Add (new Vector3 (v0x + slant, v1y));
							verts.Add (new Vector3 (v1x + slant, v1y));
							verts.Add (new Vector3 (v1x - slant, v0y));
						}
					} else { // Bold
						for (int j = 0; j < 4; ++j) {
							float a = mBoldOffset [j * 2];
							float b = mBoldOffset [j * 2 + 1];
							
							float slant = (italic ? fontSize * 0.1f * ((v1y - v0y) / fontSize) : 0f);
							verts.Add (new Vector3 (v0x + a - slant, v0y + b));
							verts.Add (new Vector3 (v0x + a + slant, v1y + b));
							verts.Add (new Vector3 (v1x + a + slant, v1y + b));
							verts.Add (new Vector3 (v1x + a - slant, v0y + b));
						}
					}
					
					// Underline and strike-through contributed by Rudy Pangestu.
					if (underline || strikethrough) {
						NGUIText.GlyphInfo dash = NGUIText.GetGlyph (strikethrough ? '-' : '_', prev);
						if (dash == null)
							continue;
						
						if (uvs != null) {
							if (bitmapFont != null) {
								dash.u0.x = uvRect.xMin + invX * dash.u0.x;
								dash.u2.x = uvRect.xMin + invX * dash.u2.x;
								dash.u0.y = uvRect.yMax - invY * dash.u0.y;
								dash.u2.y = uvRect.yMax - invY * dash.u2.y;
							}
							
							float cx = (dash.u0.x + dash.u2.x) * 0.5f;
							
							for (int j = 0, jmax = (bold ? 4 : 1); j < jmax; ++j) {
								uvs.Add (new Vector2 (cx, dash.u0.y));
								uvs.Add (new Vector2 (cx, dash.u2.y));
								uvs.Add (new Vector2 (cx, dash.u2.y));
								uvs.Add (new Vector2 (cx, dash.u0.y));
							}
						}
						
						if (subscript && strikethrough) {
							v0y = (-y + dash.v0.y) * sizeShrinkage;
							v1y = (-y + dash.v1.y) * sizeShrinkage;
						} else {
							v0y = (-y + dash.v0.y);
							v1y = (-y + dash.v1.y);
						}
						
						if (bold) {
							for (int j = 0; j < 4; ++j) {
								float a = mBoldOffset [j * 2];
								float b = mBoldOffset [j * 2 + 1];
								
								verts.Add (new Vector3 (prevX + a, v0y + b));
								verts.Add (new Vector3 (prevX + a, v1y + b));
								verts.Add (new Vector3 (x + a, v1y + b));
								verts.Add (new Vector3 (x + a, v0y + b));
							}
						} else {
							verts.Add (new Vector3 (prevX, v0y));
							verts.Add (new Vector3 (prevX, v1y));
							verts.Add (new Vector3 (x, v1y));
							verts.Add (new Vector3 (x, v0y));
						}
						
						if (gradient) {
							float min = sizePD + dash.v0.y / fontScale;
							float max = sizePD + dash.v1.y / fontScale;
							
							min /= sizePD;
							max /= sizePD;
							
							s_c0 = Color.Lerp (gb, gt, min);
							s_c1 = Color.Lerp (gb, gt, max);
							
							for (int j = 0, jmax = (bold ? 4 : 1); j < jmax; ++j) {
								cols.Add (s_c0);
								cols.Add (s_c1);
								cols.Add (s_c1);
								cols.Add (s_c0);
							}
						} else {
							for (int j = 0, jmax = (bold ? 16 : 4); j < jmax; ++j)
								cols.Add (uc);
						}
					}
				}
			}
			
			if (alignment != NGUIText.Alignment.Left && indexOffset < verts.size) {
				NGUIText.Align (verts, indexOffset, x - finalSpacingX);
				indexOffset = verts.size;
			}
			mColors.Clear ();
			lineHasSp = null;
		}
	
	}
} // namespace RO
