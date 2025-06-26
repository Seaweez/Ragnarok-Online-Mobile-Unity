using UnityEngine;

public class BarebonesUI_Demo_V2_CapturePaymentButtonClick : MonoBehaviour
{
    public static BarebonesUI_Demo_V2_CapturePaymentButtonClick INSTANCE;

    void Awake()
    {
        INSTANCE = this;
    }

    public void onClick()
    {
        PayPalLogger.Log("Entering CapturePaymentButtonClick#Onclick()...");
        CapturePaymentAPI_Call capturePaymentAPI_Call = BarebonesUI_Demo_V2_PayPalCallbackHandler.INSTANCE.gameObject.AddComponent<CapturePaymentAPI_Call>();
        capturePaymentAPI_Call.payPallCallbackBase = BarebonesUI_Demo_V2_PayPalCallbackHandler.INSTANCE;
        capturePaymentAPI_Call.accessToken = BarebonesUI_Demo_V2_PayPalCallbackHandler.INSTANCE.accessToken;
        capturePaymentAPI_Call.orderId = BarebonesUI_Demo_V2_PayPalCallbackHandler.INSTANCE.gameObject.GetComponent<CreateOrderAPI_Call>().API_SuccessResponse.id;
        capturePaymentAPI_Call.paypalRequestId = BarebonesUI_Demo_V2_PayPalCallbackHandler.INSTANCE.gameObject.GetComponent<ShowOrderDetailsAPI_Call>().API_SuccessResponse.purchase_units[0].reference_id;
    }
}
