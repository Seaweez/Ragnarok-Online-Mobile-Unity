using UnityEngine;
using System.Collections.Generic;
using System.Text;
using Ghost.Utils;
using Ghost.Extensions;

[AddComponentMenu("NGUI/UI/InputEx Field")]
public class UIInputEx : UIInput
{
	[System.Flags]
	public enum Ignore
	{
		None = 0,
		Emoji = 1 << 0,
		Punctuation = 1 << 1,
		Symbol = 1 << 2,
		Separator = 1 << 3,
		Custom = 1 << 4
	}

	public int DBCCaseLimit = 0;
	public Ignore ignores = Ignore.None;

	public delegate bool IgnoreCharacterChecker (char c);

	public IgnoreCharacterChecker IsCustomIgnoreCharacter = null;

	public UIInputEx()
	{
		onValidate = delegate(string text, int charIndex, char addedChar) {
			var c = addedChar;
			if (validation != Validation.None)
			{
				c = Validate(text, charIndex, c);
			}
			if (0 != c)
			{
				if (IsIgnoreCharacter (c)) 
				{
					return (char)0;
				}
				var DBCCaseLen = text.GetDBCCaseLength ();
				if (DBCCaseLimit <= DBCCaseLen) 
				{
					return (char)0;
				}
				DBCCaseLen += StringUtils.GetDBCCaseLength (c);
				if (DBCCaseLimit < DBCCaseLen) 
				{
					return (char)0;
				}
			}
			return c;
		};
	}

	public static bool CheckIgnore (Ignore i, Ignore ignores)
	{
		return i == (ignores & i);
	}

	public void AddIgnore (Ignore i)
	{
		ignores |= i;
	}

	public void RemoveIgnore (Ignore i)
	{
		ignores &= ~i;
	}

//	protected override void Insert (string text)
//	{
//		if (0 < DBCCaseLimit) {
//			var fullText = value;
//			var originalDBCCaseLen = fullText.GetDBCCaseLength ();
//			var restDBCCaseCount = DBCCaseLimit - originalDBCCaseLen;
//			StringBuilder sb = new StringBuilder ();
//			foreach (var c in text) {
//				if (c == '\b') {
//					DoBackspace ();
//					fullText = value;
//					var tempDBCCaseLen = fullText.GetDBCCaseLength ();
//					restDBCCaseCount += originalDBCCaseLen - tempDBCCaseLen;
//					originalDBCCaseLen = tempDBCCaseLen;
//					continue;
//				}
//				if (IsIgnoreCharacter (c)) {
//					continue;
//				}
//				var DBCCaseLen = StringUtils.GetDBCCaseLength (c);
//				restDBCCaseCount -= DBCCaseLen;
//				if (0 > restDBCCaseCount) {
//					break;
//				}
//				sb.Append (c);
//				if (0 == restDBCCaseCount) {
//					break;
//				}
//			}
//			if (sb.Length < text.Length) {
//				text = sb.ToString ();
//			}
//		}
//		base.Insert (text);
//	}

	private bool IsIgnoreCharacter (char c)
	{
		if (Ignore.None == ignores) {
			return false;
		}
		if (CheckIgnore (Ignore.Emoji, ignores) && StringUtils.IsEmojiCharacter (c)) {
			return true;
		} else if (CheckIgnore (Ignore.Punctuation, ignores) && '\'' != c && StringUtils.IsPunctuationCharacter (c)) {
			return true;
		} else if (CheckIgnore (Ignore.Symbol, ignores) && StringUtils.IsSymbolCharacter (c)) {

		} else if (CheckIgnore (Ignore.Separator, ignores) && StringUtils.IsSeparatorCharacter (c)) {
			return true;
		} else if (CheckIgnore (Ignore.Custom, ignores) && null != IsCustomIgnoreCharacter && IsCustomIgnoreCharacter (c)) {
			return true;
		}
		return false;
	}

}