using UnityEngine;

public class BarebonesUI_Demo_V2_AuthorizeOrderButtonClick : MonoBehaviour
{
    public static BarebonesUI_Demo_V2_AuthorizeOrderButtonClick INSTANCE;

    void Awake()
    {
        INSTANCE = this;
    }

    public void onClick()
    {
        PayPalLogger.Log("Entering AuthorizeOrderButtonClick#Onclick()...");
        AuthorizeOrderAPI_Call authorizeOrderDetailsAPI_Call = BarebonesUI_Demo_V2_PayPalCallbackHandler.INSTANCE.gameObject.AddComponent<AuthorizeOrderAPI_Call>();
        authorizeOrderDetailsAPI_Call.payPallCallbackBase = BarebonesUI_Demo_V2_PayPalCallbackHandler.INSTANCE;
        authorizeOrderDetailsAPI_Call.accessToken = BarebonesUI_Demo_V2_PayPalCallbackHandler.INSTANCE.accessToken;
        authorizeOrderDetailsAPI_Call.orderId = BarebonesUI_Demo_V2_PayPalCallbackHandler.INSTANCE.gameObject.GetComponent<CreateOrderAPI_Call>().API_SuccessResponse.id;
        authorizeOrderDetailsAPI_Call.paypalRequestId = BarebonesUI_Demo_V2_PayPalCallbackHandler.INSTANCE.gameObject.GetComponent<ShowOrderDetailsAPI_Call>().API_SuccessResponse.purchase_units[0].reference_id;
    }
}
