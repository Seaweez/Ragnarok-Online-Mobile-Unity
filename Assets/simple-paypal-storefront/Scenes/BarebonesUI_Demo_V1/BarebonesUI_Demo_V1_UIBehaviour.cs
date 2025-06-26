using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.UI;

public class BarebonesUI_Demo_V1_UIBehaviour : MonoBehaviour
{

    public static BarebonesUI_Demo_V1_UIBehaviour INSTANCE;

    public void Awake()
    {
        INSTANCE = this;
    }

    public Text hasPaidTextField;
    public GameObject paypalApiCallHistoryGO;

    [DllImport("__Internal")]
    private static extern void openWindow(string url);

    public void Start()
    {
        // acquire access token if it doesn't exist
        if (string.IsNullOrEmpty(BarebonesUI_Demo_V1_Properties.INSTANCE.accessToken))
        {
            GetAccessTokenAPI_Call getAccessTokenAPI_Call = paypalApiCallHistoryGO.AddComponent<GetAccessTokenAPI_Call>();
            getAccessTokenAPI_Call.payPallCallbackBase = BarebonesUI_Demo_V1_PayPalCallbackHandler.INSTANCE;
        }

        refreshUIPaidStatus();
    }

    public void refreshUIPaidStatus()
    {

        if (BarebonesUI_Demo_V1_Properties.INSTANCE.paymentStatus == "approved")
        {
            hasPaidTextField.text = "PAID";
            hasPaidTextField.color = Color.green;
        }
        else
        {
            hasPaidTextField.text = "NOT PAID";
            hasPaidTextField.color = Color.red;
        }

    }

    public void openLink()
    {

        string checkoutUrl = BarebonesUI_Demo_V1_Properties.INSTANCE.payUrl;

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
