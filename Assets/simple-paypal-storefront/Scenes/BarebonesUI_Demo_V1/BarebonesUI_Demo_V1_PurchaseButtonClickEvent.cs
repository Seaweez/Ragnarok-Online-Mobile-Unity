using UnityEngine;

public class BarebonesUI_Demo_V1_PurchaseButtonClickEvent : MonoBehaviour
{

    public GameObject paypalApiCallHistoryGO;

    public void onClick()
    {

        // acquire pay url if it doesn't exist
        if (string.IsNullOrEmpty(BarebonesUI_Demo_V1_Properties.INSTANCE.payUrl))
        {
            // make api call to create payment
            CreatePaymentAPI_Call createPaymentAPI_Call = paypalApiCallHistoryGO.AddComponent<CreatePaymentAPI_Call>();
            createPaymentAPI_Call.itemCurrency = "USD";
            createPaymentAPI_Call.itemDescription = "Disable all the ads";
            createPaymentAPI_Call.itemName = "Disable ads";
            createPaymentAPI_Call.itemPrice = "1.00";
            createPaymentAPI_Call.accessToken = BarebonesUI_Demo_V1_Properties.INSTANCE.accessToken;
            createPaymentAPI_Call.payPallCallbackBase = BarebonesUI_Demo_V1_PayPalCallbackHandler.INSTANCE;

        }
        else
        {
            BarebonesUI_Demo_V1_UIBehaviour.INSTANCE.openLink();
        }

    }

}
