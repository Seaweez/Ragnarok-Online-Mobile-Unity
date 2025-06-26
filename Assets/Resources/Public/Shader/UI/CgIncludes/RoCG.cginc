#ifndef RO_CG
#define RO_CG

inline float RoTest123(half3 test)
{
	float b = step(0, test.r);
	b *= step(test.r, 2);
	b *= step(1, test.g);
	b *= step(test.g, 3);
	b *= step(2, test.b);
	b *= step(test.b, 4);
	return b;
}

inline half4 RoGreyCorrect(half4 color, half4 inColor)
{
	half4 col = color * inColor;
	half3 test = inColor.rgb * 255;
	float b = RoTest123(test);
	half grey = dot(color.rgb, half3(0.299, 0.587, 0.114));
	half4 a = grey*inColor.a+(1-inColor.a);
	a.a = color.a;
	col = lerp(col, a, b);
	return col;
}

inline half4 RoGreyCorrect2(half4 color, half4 inColor, out float b)
{
	half4 col = color * inColor;
	half3 test = inColor.rgb * 255;
	b = RoTest123(test);
	half grey = dot(color.rgb, half3(0.299, 0.587, 0.114));
	half4 a = grey*inColor.a+(1-inColor.a);
	a.a = color.a;
	col = lerp(col, a, b);
	return col;
}

inline half4 RoMaskColor(half4 src, half4 mask)
{
	mask = saturate(mask);

	float a = step(mask.a * src.a, 0);
	float b = step(2.99, mask.r + mask.g + mask.b);
	half3 dest = lerp((mask.rgb*src.rgb)/0.5, (1.0-(1.0-mask.rgb)*(1.0-src.rgb)/0.5), step(0.5, src.r));
	dest = lerp(dest, src, b);
	dest = lerp(dest, src.rgb, a);
	return half4(dest.rgb, src.a);
}

#endif