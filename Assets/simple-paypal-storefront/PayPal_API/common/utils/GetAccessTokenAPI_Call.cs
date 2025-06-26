using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class GetAccessTokenAPI_Call : MonoBehaviour {

	public PayPalGetAccessTokenJsonResponse API_SuccessResponse;

	public PayPalErrorJsonResponse API_ErrorResponse;

	public PayPallCallbackBase payPallCallbackBase;

	void Start () {
		PayPalLogger.Log("Entering GetAccessTokenAPI_Call Start()... ");
		StartCoroutine (MakePayAPIcall ());
	}

	void handleSuccessResponse(string responseText) {

		//attempt to parse reponse text
		API_SuccessResponse = JsonUtility.FromJson<PayPalGetAccessTokenJsonResponse>(responseText);

		if (payPallCallbackBase != null)
		{
			payPallCallbackBase.getAccessTokenSuccess_CallBack(API_SuccessResponse);
		}

	}

	void handleErrorResponse(string responseText, string errorText) {

		//attempt to parse error response 
		API_ErrorResponse = JsonUtility.FromJson<PayPalErrorJsonResponse>(responseText);

		//if no responseText and only error text
		if (API_ErrorResponse == null) {
			API_ErrorResponse = new PayPalErrorJsonResponse ();
			API_ErrorResponse.message = errorText;
		}

		if (payPallCallbackBase != null)
		{
			payPallCallbackBase.payPalFailure_Callback(API_ErrorResponse);
		}

	}

	IEnumerator MakePayAPIcall() {

        string endpointURL = GlobalPayPalProperties.INSTANCE.isUsingSandbox() ?
            "https://api.sandbox.paypal.com/v1/oauth2/token" :
            "https://api.paypal.com/v1/oauth2/token";

        WWWForm postData = new WWWForm();
        postData.AddField("grant_type", "client_credentials");

        UnityWebRequest uwr = new UnityWebRequest(endpointURL, "POST");
        uwr.SetRequestHeader("Content-Type", "application/json");
        uwr.SetRequestHeader("Accept-Language", "en_US");
        uwr.SetRequestHeader("Authorization",
            "Basic " + System.Convert.ToBase64String(Encoding.ASCII.GetBytes(
            GlobalPayPalProperties.INSTANCE.clientId + ":" + GlobalPayPalProperties.INSTANCE.secret))
        );

        uwr.uploadHandler = new UploadHandlerRaw(postData.data);
        uwr.downloadHandler = new DownloadHandlerBuffer();

        yield return uwr.SendWebRequest();

        if (!uwr.isHttpError && !uwr.isNetworkError)
        {
            PayPalLogger.LogFormat("GetAccessTokenAPI_Call success | HttpResponseCode = {0} | responseText: \n\n{1}\n", new object[] {uwr.responseCode, uwr.downloadHandler.text});
            handleSuccessResponse(uwr.downloadHandler.text);
        }
        else
        {			
			PayPalLogger.LogErrorFormat("GetAccessTokenAPI_Call failure | HttpResponseCode = {0} | errorText: \n\n{1}\n", new object[] { uwr.responseCode, uwr.error });
            handleErrorResponse(uwr.downloadHandler.text, uwr.error);
        }
    }
}
