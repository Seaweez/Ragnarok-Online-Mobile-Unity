using UnityEngine;
using SLua;
using UnityEngine.Networking;
using System.Collections;

public class PaymentManager : MonoBehaviour
{
    public int status;

    
    public void StartPayment(string userId, string productId, LuaFunction callback)
    {
       
        string paymentLink = GeneratePaymentLink(userId, productId);

      
        Application.OpenURL(paymentLink);

       
        StartCoroutine(CheckLatestPaymentStatus(userId, productId, callback));
    }

   
    private string GeneratePaymentLink(string userId, string productId)
    {
        string paymentLink = "";

        
        switch (productId)
        {           
            case "221":
                paymentLink = $"https://buy.stripe.com/7sIcO6da1ebaaqs009?client_reference_id={userId}";
                break;
            case "7":
                paymentLink = $"https://buy.stripe.com/8wM15oee57MMdCEdQU?client_reference_id={userId}";
                break;
            case "11":
                paymentLink = $"https://buy.stripe.com/dR6bK22vnffegOQfZ9?client_reference_id={userId}";
                break;
            case "12":
                paymentLink = $"https://buy.stripe.com/cN2eWegmd8QQ7eg4gt?client_reference_id={userId}";
                break;
            case "13":
                paymentLink = $"https://buy.stripe.com/28ocO63zr9UUcyAdR2?client_reference_id={userId}";
                break;
            case "15":
                paymentLink = $"https://buy.stripe.com/dR601k3zr4AA5685ky?client_reference_id={userId}";
                break;
            case "17":
                paymentLink = $"https://buy.stripe.com/7sIg0i9XP8QQ9mocMP?client_reference_id={userId}";
                break;
            case "18":
                paymentLink = $"https://buy.stripe.com/cN26pI0nfgjieGI14j?client_reference_id={userId}";
                break;
            case "231":
                paymentLink = $"https://buy.stripe.com/fZebK23zr8QQ424cMR?client_reference_id={userId}";
                break;
            case "241":
                paymentLink = $"https://buy.stripe.com/cN2aFY9XPffe1TW006?client_reference_id={userId}";
                break;
            case "251":
                paymentLink = $"https://buy.stripe.com/9AQ4hA1rj1oofKM14b?client_reference_id={userId}";
                break;
            case "250":
                paymentLink = $"https://buy.stripe.com/9AQ4hA1rj1oofKM14b?client_reference_id={userId}";
                break;
            case "999999":
                paymentLink = $"https://buy.stripe.com/aEU7tMgmd0kkeGI008?client_reference_id={userId}";
                break;
            case "7777":
                paymentLink = $"https://buy.stripe.com/test_bIY29181V8I6h0c000?client_reference_id={userId}";
                break;
            default:
                Debug.LogError("Unknown productId.");
                break;
        }

        return paymentLink;
    }

  
    private IEnumerator CheckLatestPaymentStatus(string userId, string productId, LuaFunction callback)
    {
     
        string checkPaymentUrl = $"http://api.pinidea.online/check_payment_status.php?user_id={userId}&product_id={productId}";

        int retryCount = 0;
        const int maxRetries = 100;

        while (true) 
        {
            UnityWebRequest www = UnityWebRequest.Get(checkPaymentUrl);
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
               
                string result = www.downloadHandler.text;
               // Debug.Log("Payment status response: " + result);

               
                PaymentStatusResult jsonResult = JsonUtility.FromJson<PaymentStatusResult>(result);

                if (jsonResult != null && !string.IsNullOrEmpty(jsonResult.status))
                {
                    if (jsonResult.status == "success")
                    {
                        status = 1;
                        callback.call("success");
                        break; 
                    }
                    else if (jsonResult.status == "pending")
                    {
                        //Debug.Log("Payment is pending. Checking again in 10 seconds.");
                     
                    }
                    else if (jsonResult.status == "failed")
                    {
                      //  callback.call("failed");
                       // break;
                    }
                }
                else
                {
                    Debug.LogError("Invalid response from server.");
                    //callback.call("failed");
                   // break;  
                }
            }
            else
            {
                if (www.responseCode == 400 || www.responseCode == 404 || www.responseCode >= 500)
                {
                   // Debug.LogError($"Stopping due to HTTP error: {www.responseCode} - {www.error}");
                    callback.call("failed");
                    break;
                }

                retryCount++;
             //   Debug.LogError("Error checking payment status: " + www.error);

                if (retryCount >= maxRetries)
                {
                    callback.call("failed");
                    break; 
                }
                else
                {
                   // Debug.Log("Retrying to check payment status in 10 seconds...");
                }
            }

            yield return new WaitForSeconds(3); 
        }
    }


    [System.Serializable]
    public class PaymentStatusResult
    {
        public string status;
        public string payment_id;
        public string error;
    }
}
