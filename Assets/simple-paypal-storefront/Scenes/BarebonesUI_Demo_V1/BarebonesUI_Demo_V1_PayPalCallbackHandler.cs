using UnityEngine.UI;

public class BarebonesUI_Demo_V1_PayPalCallbackHandler : PayPallCallbackBase_V1
{

    public static BarebonesUI_Demo_V1_PayPalCallbackHandler INSTANCE;
    public Button purchaseButton;

    public void Awake()
    {
        INSTANCE = this;
    }

    public override void createPaymentSuccess_CallBack(PayPalCreatePaymentJsonResponse payPalCreatePaymentJsonResponse)
    {
        BarebonesUI_Demo_V1_Properties.INSTANCE.payUrl = payPalCreatePaymentJsonResponse.links[1].href;
        BarebonesUI_Demo_V1_Properties.INSTANCE.paymentId = payPalCreatePaymentJsonResponse.id;
        BarebonesUI_Demo_V1_UIBehaviour.INSTANCE.openLink();

    }

    public override void executePaymentSuccess_CallBack(PayPalExecutePaymentJsonResponse payPalExecutePaymentJsonResponse)
    {
        BarebonesUI_Demo_V1_Properties.INSTANCE.paymentStatus = payPalExecutePaymentJsonResponse.state;
        BarebonesUI_Demo_V1_UIBehaviour.INSTANCE.refreshUIPaidStatus();
    }

    public override void getAccessTokenSuccess_CallBack(PayPalGetAccessTokenJsonResponse payPalGetAccessTokenJsonResponse)
    {
        BarebonesUI_Demo_V1_Properties.INSTANCE.accessToken = payPalGetAccessTokenJsonResponse.access_token;
        purchaseButton.enabled = true;

    }

    public override void payPalFailure_Callback(PayPalErrorJsonResponse payPalErrorJsonResponse)
    {
        throw new System.NotImplementedException();
    }

    public override void showPaymentSuccess_CallBack(PayPalShowPaymentJsonResponse payPalShowPaymentJsonResponse)
    {

        if (payPalShowPaymentJsonResponse.payer.status == "VERIFIED")
        {
            ExecutePaymentAPI_Call apiCall = this.gameObject.AddComponent<ExecutePaymentAPI_Call>();

            apiCall.accessToken = BarebonesUI_Demo_V1_Properties.INSTANCE.accessToken;
            apiCall.paymentID = BarebonesUI_Demo_V1_Properties.INSTANCE.paymentId;
            apiCall.payerID = payPalShowPaymentJsonResponse.payer.payer_info.payer_id;

            apiCall.payPallCallbackBase = BarebonesUI_Demo_V1_PayPalCallbackHandler.INSTANCE;
        }



    }

}
