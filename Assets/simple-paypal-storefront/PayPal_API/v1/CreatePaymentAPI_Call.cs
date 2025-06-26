using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class CreatePaymentAPI_Call : MonoBehaviour {

	public string accessToken;

	public string transactionDescription;

	public string itemName;

	public string itemDescription;

	public string itemPrice;

	public string itemCurrency;

	public PayPalCreatePaymentJsonResponse API_SuccessResponse;

	public PayPalErrorJsonResponse API_ErrorResponse;

	public PayPallCallbackBase_V1 payPallCallbackBase;

	void Start () {
		PayPalLogger.Log("Entering CreatePaymentAPI_Call Start()... ");
		StartCoroutine (MakePayAPIcall ());
	}

	void handleSuccessResponse(string responseText) {

		//attempt to parse reponse text
		API_SuccessResponse = JsonUtility.FromJson<PayPalCreatePaymentJsonResponse>(responseText);

		if (payPallCallbackBase != null)
        {
			payPallCallbackBase.createPaymentSuccess_CallBack(API_SuccessResponse);
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

        string endpointURL = GlobalPayPalProperties.INSTANCE.isUsingSandbox() ?
            "https://api.sandbox.paypal.com/v1/payments/payment" :
            "https://api.paypal.com/v1/payments/payment";

        byte[] formData = System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(createRequest()));

		UnityWebRequest uwr = new UnityWebRequest(endpointURL, "POST");
		uwr.SetRequestHeader("Authorization", "Bearer " + accessToken);
        uwr.SetRequestHeader("Content-Type", "application/json");
        uwr.uploadHandler = new UploadHandlerRaw(formData);
		uwr.downloadHandler = new DownloadHandlerBuffer();

		yield return uwr.SendWebRequest();

		if (!uwr.isHttpError && !uwr.isNetworkError)
		{
			PayPalLogger.LogFormat("CreatePaymentAPI_Call success | HttpResponseCode = {0} | responseText: \n\n{1}\n", new object[] { uwr.responseCode, uwr.downloadHandler.text });
			handleSuccessResponse(uwr.downloadHandler.text);
		}
		else
		{
			PayPalLogger.LogErrorFormat("CreatePaymentAPI_Call failure | HttpResponseCode = {0} | errorText: \n\n{1}\n", new object[] { uwr.responseCode, uwr.error });
			handleErrorResponse(uwr.downloadHandler.text, uwr.error);
		}
	}

	PayPalCreatePaymentJsonRequest createRequest() {

		//create skeleton request object
		PayPalCreatePaymentJsonRequest request = PayPalCreatePaymentJsonRequest.instatiateBasic();

		//map provided values into skeleton object
		request.transactions [0].amount.total = itemPrice;
		request.transactions [0].amount.currency = itemCurrency;
		request.transactions [0].description = transactionDescription;
		request.transactions [0].invoice_number = System.Guid.NewGuid().ToString();
		request.transactions [0].item_list.items [0].name = itemName;
		request.transactions [0].item_list.items [0].description = itemDescription;
		request.transactions [0].item_list.items [0].price = itemPrice;
		request.transactions [0].item_list.items [0].currency = itemCurrency;

		return request;

	}
}
