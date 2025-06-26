using UnityEngine;

public abstract class PayPallCallbackBase : MonoBehaviour
{

    public abstract void getAccessTokenSuccess_CallBack(PayPalGetAccessTokenJsonResponse payPalGetAccessTokenJsonResponse);

    public abstract void payPalFailure_Callback(PayPalErrorJsonResponse payPalErrorJsonResponse);

}