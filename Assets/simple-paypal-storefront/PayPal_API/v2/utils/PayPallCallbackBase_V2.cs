using UnityEngine;

public abstract class PayPallCallbackBase_V2 : PayPallCallbackBase
{

    public abstract void createOrderSuccess_CallBack(PayPalCreateOrderJsonResponse payPalCreateOrderJsonResponse);

    public abstract void showOrderDetailsSuccess_CallBack(PayPalShowOrderDetailsJsonResponse payPalShowOrderDetailsJsonResponse);

    public abstract void authorizeOrderSuccess_CallBack(PayPalAuthorizeOrderJsonResponse payPalAuthorizeOrderJsonResponse);

    public abstract void capturePaymentSuccess_CallBack(PayPalCapturePaymentJsonResponse payPalCapturePaymentJsonResponse);

}