
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using System;

public class BarebonesUI_Demo_V2_OpenCheckoutUrlInBrowserButtonClick : MonoBehaviour
{

    [DllImport("__Internal")]
    private static extern void openWindow(string url);

    public InputField checkoutUrlInputField;

    public void onClick()
    {
        string checkoutUrl = checkoutUrlInputField.text;

        if (!Uri.IsWellFormedUriString(checkoutUrl, UriKind.Absolute)) {
            Debug.LogWarning("Invalid URI launch attempt");
            return;
        }

        if (Application.platform.Equals(RuntimePlatform.WebGLPlayer))
        {
            openWindow(checkoutUrl);
        }
        else
        {
            Application.OpenURL(checkoutUrl);
        }
    }

}
