using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AuthorizeOrderAPI_Call : MonoBehaviour
{
	public string accessToken;

	public string orderId;

	public string paypalRequestId;

	public PayPalAuthorizeOrderJsonResponse API_SuccessResponse;

	public PayPalErrorJsonResponse API_ErrorResponse;

	public PayPallCallbackBase_V2 payPallCallbackBase;

	void Start()
	{
		PayPalLogger.Log("Entering ShowOrderDetailsAPI_Call Start()... ");
		StartCoroutine(MakePayAPIcall());
	}

	void handleSuccessResponse(string responseText)
	{

		//attempt to parse reponse text
		API_SuccessResponse = JsonUtility.FromJson<PayPalAuthorizeOrderJsonResponse>(responseText);

		if (payPallCallbackBase != null)
		{
			payPallCallbackBase.authorizeOrderSuccess_CallBack(API_SuccessResponse);
		}
	}

	void handleErrorResponse(string responseText, string errorText)
	{

		//attempt to parse error response 
		API_ErrorResponse = JsonUtility.FromJson<PayPalErrorJsonResponse>(responseText);

		//if no responseText and only error text
		if (API_ErrorResponse == null)
		{
			API_ErrorResponse = new PayPalErrorJsonResponse();
			API_ErrorResponse.message = errorText;
		}

		if (payPallCallbackBase != null)
		{
			payPallCallbackBase.payPalFailure_Callback(API_ErrorResponse);
		}

	}

	IEnumerator MakePayAPIcall()
	{

		string endpointURL = GlobalPayPalProperties.INSTANCE.isUsingSandbox() ?
			"https://api.sandbox.paypal.com/v2/checkout/orders/" + orderId + "/authorize" :
			"https://api.paypal.com/v2/checkout/orders/" + orderId + "/authorize";

		UnityWebRequest uwr = new UnityWebRequest(endpointURL, "POST");
		uwr.SetRequestHeader("Authorization", "Bearer " + accessToken);
		uwr.SetRequestHeader("Content-Type", "application/json");
		uwr.SetRequestHeader("PayPal-Request-Id", paypalRequestId);
		uwr.downloadHandler = new DownloadHandlerBuffer();

		yield return uwr.SendWebRequest();

		if (!uwr.isHttpError && !uwr.isNetworkError)
		{
			PayPalLogger.LogFormat("AuthorizeOrderDetailsAPI_Call success | HttpResponseCode = {0} | responseText: \n\n{1}\n", new object[] { uwr.responseCode, uwr.downloadHandler.text });
			handleSuccessResponse(uwr.downloadHandler.text);
		}
		else
		{
			PayPalLogger.LogErrorFormat("AuthorizeOrderDetailsAPI_Call failure | HttpResponseCode = {0} | errorText: \n\n{1}\n", new object[] { uwr.responseCode, uwr.error });
			handleErrorResponse(uwr.downloadHandler.text, uwr.error);
		}
	}
}
