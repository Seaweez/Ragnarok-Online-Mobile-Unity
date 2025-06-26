using UnityEngine;

public class BarebonesUI_Demo_V2_ShowOrderDetailsButtonClick : MonoBehaviour
{

    public static BarebonesUI_Demo_V2_ShowOrderDetailsButtonClick INSTANCE;

    void Awake()
    {
        INSTANCE = this;
    }

    public void onClick()
    {
        PayPalLogger.Log("Entering ShowOrderDetailsButtonClick#Onclick()...");
        ShowOrderDetailsAPI_Call showOrderDetailsAPI_Call = BarebonesUI_Demo_V2_PayPalCallbackHandler.INSTANCE.gameObject.AddComponent<ShowOrderDetailsAPI_Call>();
        showOrderDetailsAPI_Call.payPallCallbackBase = BarebonesUI_Demo_V2_PayPalCallbackHandler.INSTANCE;
        showOrderDetailsAPI_Call.accessToken = BarebonesUI_Demo_V2_PayPalCallbackHandler.INSTANCE.accessToken;
        showOrderDetailsAPI_Call.orderId = BarebonesUI_Demo_V2_PayPalCallbackHandler.INSTANCE.gameObject.GetComponent<CreateOrderAPI_Call>().API_SuccessResponse.id;
    }


}
