using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayPalCallbackStore : PayPallCallbackBase_V1 {

    public static PayPalCallbackStore INSTANCE;

    void Awake()
    {
        INSTANCE = this;
    }

    public string payId;

    public override void createPaymentSuccess_CallBack(PayPalCreatePaymentJsonResponse payPalCreatePaymentJsonResponse)
    {
        Debug.Log("entered PayPallCallbackStore createPaymentSuccess_CallBack()...");
        payId = payPalCreatePaymentJsonResponse.id;
        StoreActions.INSTANCE.changePurchaseStatus(StoreActions.PurchaseStatus.CHECKOUT_READY);
    }

    public override void executePaymentSuccess_CallBack(PayPalExecutePaymentJsonResponse payPalExecutePaymentJsonResponse)
    {
        Debug.Log("entered PayPallCallbackStore executePaymentSuccess_CallBack()...");

        if (payPalExecutePaymentJsonResponse.state == "approved")
        {
            StoreActions.INSTANCE.changePurchaseStatus(StoreActions.PurchaseStatus.COMPLETE);
        }
        else
        {
            StoreActions.INSTANCE.changePurchaseStatus(StoreActions.PurchaseStatus.INCOMPLETE);
        }

    }

    public override void getAccessTokenSuccess_CallBack(PayPalGetAccessTokenJsonResponse payPalGetAccessTokenJsonResponse)
    {
        Debug.Log("entered PayPallCallbackStore getAccessTokenSuccess_CallBack()...");

        StoreActions.INSTANCE.OpenStore();
        DialogScreenActions.INSTANCE.HideDialogScreen();
        StoreStartupBehaviour.INSTANCE.accessToken = payPalGetAccessTokenJsonResponse.access_token;
    }

    public override void payPalFailure_Callback(PayPalErrorJsonResponse payPalErrorJsonResponse)
    {
        Debug.LogWarning("entered PayPallCallbackStore payPalFailure_Callback()...");
        DialogScreenActions.INSTANCE.setContextStoreLoadFailure();
    }

    public override void showPaymentSuccess_CallBack(PayPalShowPaymentJsonResponse payPalShowPaymentJsonResponse)
    {
        Debug.Log("entered PayPallCallbackStore showPaymentSuccess_CallBack()...");

        if (payPalShowPaymentJsonResponse.payer.status == "VERIFIED")
        {
            ExecutePaymentAPI_Call apiCall = this.gameObject.AddComponent<ExecutePaymentAPI_Call>();
            apiCall.payPallCallbackBase = INSTANCE;

            apiCall.accessToken = StoreStartupBehaviour.INSTANCE.accessToken;
            apiCall.paymentID = payId;
            apiCall.payerID = payPalShowPaymentJsonResponse.payer.payer_info.payer_id;
        } else 
        {
            StartCoroutine("retryCallShowPayment_API", payPalShowPaymentJsonResponse);     
        }
    }

    IEnumerator retryCallShowPayment_API(PayPalShowPaymentJsonResponse payPalShowPaymentJsonResponse)
    {
        Debug.Log("Waiting 5 seconds before retrying call to ShowPaymentAPI...");
        yield return new WaitForSeconds(5);

        if (StoreActions.INSTANCE.currentPurchaseStatus == StoreActions.PurchaseStatus.WAITING)
        {
            ShowPaymentAPI_Call lastAPIcall = this.GetComponent<ShowPaymentAPI_Call>();

            if (lastAPIcall != null)
            {
                Destroy(lastAPIcall);
            }

            Debug.Log("creating new api call");
            ShowPaymentAPI_Call apiCall = this.gameObject.AddComponent<ShowPaymentAPI_Call>();
            apiCall.payPallCallbackBase = INSTANCE;

            apiCall.accessToken = StoreStartupBehaviour.INSTANCE.accessToken;
            apiCall.payID = payId;
        }

    }

}
