using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.CompilerServices;

[Serializable]
public class PayPalCreateOrderJsonRequest
{
	public string intent;

	public List<PurchaseUnit> purchase_units;

	public ApplicationContext application_context;

	[Serializable]
	public class Amount
	{
		public string value;
		public string currency_code;
	}

	[Serializable]
	public class PurchaseUnit
	{
		public Amount amount;
	}

	[Serializable]
	public class ApplicationContext
	{
		public string brand_name;
		public string return_url;
		public string cancel_url;
	}

	public static PayPalCreateOrderJsonRequest instatiateBasic(string pAmount)
	{
		PayPalCreateOrderJsonRequest request = new PayPalCreateOrderJsonRequest();
		request.intent = "CAPTURE";

		request.purchase_units = new List<PurchaseUnit>();
		PurchaseUnit purchaseUnit = new PurchaseUnit();
		request.purchase_units.Add(purchaseUnit);
		Amount amount = new Amount();
		purchaseUnit.amount = amount;
		amount.value = pAmount;
		amount.currency_code = GlobalPayPalProperties.INSTANCE.currencyCode;

		ApplicationContext applicationContext = new ApplicationContext();
		applicationContext.brand_name = GlobalPayPalProperties.INSTANCE.brandName;
		applicationContext.cancel_url = GlobalPayPalProperties.INSTANCE.cancelUrl;
		applicationContext.return_url = GlobalPayPalProperties.INSTANCE.returnUrl;
		request.application_context = applicationContext;
		
		return request;
	}

}
