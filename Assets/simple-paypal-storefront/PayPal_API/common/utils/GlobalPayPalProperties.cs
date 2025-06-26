using UnityEngine;

public class GlobalPayPalProperties : MonoBehaviour {

	public static GlobalPayPalProperties INSTANCE;

	public string clientId;
	public string secret;

	void Awake() {
		INSTANCE = this;
	}

	public string currencyCode;

	public enum PayPalEndpoint {
		SANDBOX,
		LIVE
	}

	public PayPalEndpoint payPalEndpoint;

	public string returnUrl;
	public string cancelUrl;
	public string brandName;

	public bool isUsingSandbox() {

		return payPalEndpoint == PayPalEndpoint.SANDBOX;

	}
	
}
