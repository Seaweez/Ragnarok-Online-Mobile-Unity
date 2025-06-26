using UnityEngine;
using UnityEngine.UI;

public class BarebonesUI_Demo_V1_RefreshStatusButtonClickEvent : MonoBehaviour
{

    public Text hasPaidTextField;
    public GameObject paypalApiCallHistory;

    public void onClick()
    {

        if (string.IsNullOrEmpty(BarebonesUI_Demo_V1_Properties.INSTANCE.payUrl))
        {
            return;
        }

        ShowPaymentAPI_Call apiCall = paypalApiCallHistory.AddComponent<ShowPaymentAPI_Call>();

        apiCall.accessToken = BarebonesUI_Demo_V1_Properties.INSTANCE.accessToken;
        apiCall.payID = BarebonesUI_Demo_V1_Properties.INSTANCE.paymentId;
        apiCall.payPallCallbackBase = BarebonesUI_Demo_V1_PayPalCallbackHandler.INSTANCE;
    }

}
