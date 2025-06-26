using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class ExecutePaymentAPI_Call : MonoBehaviour {

	public string paymentID;

	public string payerID;

	public string accessToken;

	public PayPalExecutePaymentJsonResponse API_SuccessResponse;

	public PayPalErrorJsonResponse API_ErrorResponse;

	public PayPallCallbackBase_V1 payPallCallbackBase;

	// Use this for initialization
	void Start () {
		PayPalLogger.Log("Entering ExecutePaymentAPI_Call Start()... ");
		StartCoroutine (MakePayAPIcall ());
	}

	void handleSuccessResponse(string responseText) {

		//attempt to parse reponse text
		API_SuccessResponse = JsonUtility.FromJson<PayPalExecutePaymentJsonResponse>(responseText);

		if (payPallCallbackBase != null)
		{
			payPallCallbackBase.executePaymentSuccess_CallBack(API_SuccessResponse);
		}

	}

	void handleErrorResponse(string responseText, string errorText) {

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

	IEnumerator MakePayAPIcall() {

        string baseEndpointURL = GlobalPayPalProperties.INSTANCE.isUsingSandbox() ?
            "https://api.sandbox.paypal.com/v1/payments/payment/" :
            "https://api.paypal.com/v1/payments/payment/";

		string endpointURL = baseEndpointURL + paymentID + "/execute";

		PayPalExecutePaymentJsonRequest request = new PayPalExecutePaymentJsonRequest();
        request.payer_id = payerID;

        byte[] formData = System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(request));

        UnityWebRequest uwr = new UnityWebRequest(endpointURL, "POST");
		uwr.SetRequestHeader("Authorization", "Bearer " + accessToken);
		uwr.SetRequestHeader("Content-Type", "application/json");
		uwr.uploadHandler = new UploadHandlerRaw(formData);
		uwr.downloadHandler = new DownloadHandlerBuffer();

		yield return uwr.SendWebRequest();

		if (!uwr.isHttpError && !uwr.isNetworkError)
		{
			PayPalLogger.LogFormat("ExecutePaymentAPI_Call success | HttpResponseCode = {0} | responseText: \n\n{1}\n", new object[] { uwr.responseCode, uwr.downloadHandler.text });
			handleSuccessResponse(uwr.downloadHandler.text);
		}
		else
		{
			PayPalLogger.LogErrorFormat("ExecutePaymentAPI_Call failure | HttpResponseCode = {0} | errorText: \n\n{1}\n", new object[] { uwr.responseCode, uwr.error });
			handleErrorResponse(uwr.downloadHandler.text, uwr.error);
		}

	}
}
