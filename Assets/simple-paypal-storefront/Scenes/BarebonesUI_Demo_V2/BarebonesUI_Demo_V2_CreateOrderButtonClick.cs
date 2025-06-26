using UnityEngine;

public class BarebonesUI_Demo_V2_CreateOrderButtonClick : MonoBehaviour
{

    public static BarebonesUI_Demo_V2_CreateOrderButtonClick INSTANCE;

    void Awake()
    {
        INSTANCE = this;
    }

    public void onClick()
    {
        PayPalLogger.Log("Entering CreateOrderButtonClick#Onclick()...");
        CreateOrderAPI_Call createOrderAPI_Call = BarebonesUI_Demo_V2_PayPalCallbackHandler.INSTANCE.gameObject.AddComponent<CreateOrderAPI_Call>();
        createOrderAPI_Call.orderAmount = "7.77";
        createOrderAPI_Call.payPallCallbackBase = BarebonesUI_Demo_V2_PayPalCallbackHandler.INSTANCE;
        createOrderAPI_Call.accessToken = BarebonesUI_Demo_V2_PayPalCallbackHandler.INSTANCE.accessToken;
    }

}
