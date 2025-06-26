using UnityEngine;

public abstract class PayPallCallbackBase_V1 : PayPallCallbackBase
{
    public abstract void createPaymentSuccess_CallBack(PayPalCreatePaymentJsonResponse payPalCreatePaymentJsonResponse);

    public abstract void executePaymentSuccess_CallBack(PayPalExecutePaymentJsonResponse payPalExecutePaymentJsonResponse);

    public abstract void showPaymentSuccess_CallBack(PayPalShowPaymentJsonResponse payPalShowPaymentJsonResponse);

}
