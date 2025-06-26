using System.Collections.Generic;
using System;

[Serializable]
public class PayPalShowOrderDetailsJsonResponse
{

	public string id;
	public string status;
	public string intent;
	public string create_time;

	public List<Link> links;
	public List<PurchaseUnit> purchase_units;

	[Serializable]
	public class Link
	{
		public string href;
		public string rel;
		public string method;
	}

	[Serializable]
	public class Amount
	{
		public string value;
		public string currency_code;
	}

	[Serializable]
	public class PurchaseUnit
	{
		public string reference_id;
		public Amount amount;
	}


}
