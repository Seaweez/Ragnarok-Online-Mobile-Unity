
using System;
using UnityEngine.UI;

public class BarebonesUI_Demo_V2_PayPalCallbackHandler : PayPallCallbackBase_V2
{

    public static BarebonesUI_Demo_V2_PayPalCallbackHandler INSTANCE;

    public Image[] checkIcons;
    public InputField checkoutUrlInputField;

    public void Awake()
    {
        INSTANCE = this;
    }

    public string accessToken;

    public override void getAccessTokenSuccess_CallBack(PayPalGetAccessTokenJsonResponse payPalGetAccessTokenJsonResponse)
    {
        accessToken = payPalGetAccessTokenJsonResponse.access_token;
        checkIcons[0].enabled = true;
    }

    public override void payPalFailure_Callback(PayPalErrorJsonResponse payPalErrorJsonResponse)
    {
        PayPalLogger.Log(payPalErrorJsonResponse.message);
    }


    public override void createOrderSuccess_CallBack(PayPalCreateOrderJsonResponse payPalCreateOrderJsonResponse)
    {
        checkIcons[1].enabled = true;

        checkoutUrlInputField.text = payPalCreateOrderJsonResponse.links[1].href;
    }


    public override void showOrderDetailsSuccess_CallBack(PayPalShowOrderDetailsJsonResponse payPalShowOrderDetailsJsonResponse)
    {
        checkIcons[2].enabled = true;

        if (payPalShowOrderDetailsJsonResponse.status == "APPROVED")
        {
            checkIcons[3].enabled = true;
        }
    }

    public override void capturePaymentSuccess_CallBack(PayPalCapturePaymentJsonResponse payPalCapturePaymentJsonResponse)
    {
        checkIcons[4].enabled = true;
    }

    // NOTE: This method should only be implemented if you're calling Create Order and passing in "intent":"AUTHORIZE" instead of the default "intent":"CAPTURE"
    public override void authorizeOrderSuccess_CallBack(PayPalAuthorizeOrderJsonResponse payPalCreateOrderJsonResponse)
    {
        throw new NotImplementedException();
    }

}
