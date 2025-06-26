using UnityEngine;
using System.Collections;
using System.Text;
using UnityEngine.Networking;
using System.Text.RegularExpressions;

public class TranslateButton : MonoBehaviour
{
    public UILabel chatText;
    public GameObject translateButton;
    public UISprite buttonSprite;
    public string normalIcon = "translate_icon";
    public string translatedIcon = "revert_icon";

    private string originalText;
    private bool isTranslated = false;
    private string libreTranslateUrl = "https://translate.eonhub.net";
    private string apiKey = "";

    private void Start()
    {
        UIEventListener.Get(translateButton).onClick += OnTranslateClick;
        UpdateUI();
    }

    private void OnTranslateClick(GameObject go)
    {
        if (!isTranslated)
        {
            originalText = chatText.text; // เก็บข้อความต้นฉบับ
            StartCoroutine(TranslateWithFallback(originalText)); // เริ่มการแปล
        }
        else
        {
            chatText.text = originalText; // คืนข้อความต้นฉบับ
            isTranslated = false;
            UpdateUI();
        }
    }

    IEnumerator TranslateWithFallback(string text)
    {
        yield return StartCoroutine(TranslateText(text, "auto", "en"));
    }

    IEnumerator TranslateText(string text, string from, string to)
    {
        string translateUrl = $"{libreTranslateUrl}/translate";
        string json = $"{{\"q\":\"{text}\",\"source\":\"{from}\",\"target\":\"{to}\",\"format\":\"text\"{(string.IsNullOrEmpty(apiKey) ? "" : $", \"api_key\":\"{apiKey}\"")}}}";

        using (UnityWebRequest request = new UnityWebRequest(translateUrl, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string responseText = request.downloadHandler.text;
                Debug.Log("🌍 Translation Response: " + responseText);

                TranslationResponse translation = JsonUtility.FromJson<TranslationResponse>(responseText);
                if (translation != null && !string.IsNullOrEmpty(translation.translatedText))
                {
                    string cleaned = SanitizeText(translation.translatedText);

                    if (IsSafeText(cleaned))
                    {
                        chatText.text = cleaned;
                        isTranslated = true;
                        UpdateUI();
                    }
                    else
                    {
                        Debug.LogWarning("⚠️ ข้อความแปลมีอักขระที่อาจทำให้ UILabel พัง");
                    }
                }
            }
            else
            {
                Debug.LogError("❌ Translation failed: " + request.error);
                chatText.text = "[Translation failed]";
                isTranslated = false;
                UpdateUI();
            }
        }
    }

    void UpdateUI()
    {
        if (buttonSprite == null)
        {
            Debug.LogError("❌ ไม่พบ UISprite สำหรับปุ่มแปล!");
            return;
        }

        buttonSprite.spriteName = isTranslated ? translatedIcon : normalIcon;
    }

    string SanitizeText(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return "[Translation Empty]";

        // ล้าง control char, emoji, directional unicode, escape char
        input = Regex.Replace(input, @"[\p{C}\uFFFD\u202E\u200E\u200F\u200B]", "");
        input = Regex.Replace(input, @"[\uD800-\uDBFF][\uDC00-\uDFFF]", "");
        input = Regex.Replace(input, @"&[^;]+;", "");
        input = Regex.Replace(input, @"\s+", " ");

        // ตัดความยาว
        if (input.Length > 200)
            input = input.Substring(0, 200) + "...";

        return input.Trim();
    }

    bool IsSafeText(string text)
    {
        if (string.IsNullOrEmpty(text)) return false;

        // ตรวจ NaN/Infinity ทางอ้อม (เช็คด้วย Vector)
        try
        {
            Vector3 test = new Vector3(0, text.Length, 0);
            if (!float.IsFinite(test.y)) return false;
        }
        catch { return false; }

        return !Regex.IsMatch(text, @"[\u0000-\u001F\u007F]") && !text.Contains("\uFFFD");
    }
}

[System.Serializable]
public class TranslationResponse
{
    public string translatedText;
}
