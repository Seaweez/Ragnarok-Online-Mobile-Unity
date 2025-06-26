using UnityEngine;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using Ghost.Utils;
using RO; // You need to import Newtonsoft.Json

public class LocalizationManager : MonoBehaviour
{
    private Dictionary<string, string> localizedText;
    public string currentLanguage; // Default language

    // Ensure there is only one LocalizationManager
    public static LocalizationManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        currentLanguage = PlayerPrefs.GetString("CurrentLanguage", "WW_English");
    
    }

    void Start()
    {
        if (localizedText == null)
        {
            LoadLocalizedText(currentLanguage);
        }
        
    }

    private void Update()
    {
        if (currentLanguage != PlayerPrefs.GetString("CurrentLanguage", "WW_English"))
        {
            currentLanguage = PlayerPrefs.GetString("CurrentLanguage", "WW_English");
            LoadLocalizedText(currentLanguage);
        }
    }

    public void Runscript()
    {
        LoadLocalizedText(currentLanguage);
    }

    public void LoadLocalizedText(string language)
    {
   #if RESOURCE_LOAD

        localizedText = new Dictionary<string, string>();

        TextAsset localizationFile = Resources.Load<TextAsset>($"Localization/{language}");
        if (localizationFile != null)
        {
            localizedText = JsonConvert.DeserializeObject<Dictionary<string, string>>(localizationFile.text);
            Debug.Log("Loaded localization for: " + language);
        }
        else
        {
            Debug.LogError("Cannot find localization file for: " + language);
        }

   #else

        localizedText = new Dictionary<string, string>();

        // �������ͧ AssetBundle
        // string bundlePath = Path.Combine(Application., "resources/localization.unity3d");

        string bundlePath = PathUnity.Combine(PathUnity.Combine(Application.persistentDataPath, ApplicationHelper.platformFolder), "resources/localization.unity3d");

        // ��Ŵ AssetBundle
        AssetBundle localizationBundle = AssetBundle.LoadFromFile(bundlePath);
        if (localizationBundle == null)
        {
            Debug.LogError("Failed to load AssetBundle!");
            return;
        }

        // ��Ŵ TextAsset �ҡ AssetBundle
        TextAsset localizationFile = localizationBundle.LoadAsset<TextAsset>(language);
        if (localizationFile != null)
        {
            localizedText = JsonConvert.DeserializeObject<Dictionary<string, string>>(localizationFile.text);
            Debug.Log("Loaded localization for: " + language);
        }
        else
        {
            Debug.LogError("Cannot find localization file for: " + language);
        }

        // �Դ�����ҹ AssetBundle
        localizationBundle.Unload(false);
     #endif
    }


    public string GetLocalizedText(string key)
    {
        if (localizedText != null && localizedText.ContainsKey(key))
        {
            string text = localizedText[key];

            // Use regular expression to find '##number' and replace with "##number"
            text = Regex.Replace(text, @"'##(\d+)'", "\"##$1\"");

            text = Regex.Replace(text, "\"([^\"]*)\"", m => ProcessText(m.Groups[1].Value));

            return text;
        }
        else
        {
          //  Debug.LogError("Localization key not found: " + key);
            return null;
        }
    }

    private string ProcessText(string text)
    {
        // Placeholder: Mark the text for processing, or translate here
        return $"[[{text}]]";
    }

}
