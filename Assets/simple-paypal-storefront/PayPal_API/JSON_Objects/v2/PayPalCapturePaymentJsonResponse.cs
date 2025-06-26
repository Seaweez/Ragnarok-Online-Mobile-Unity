using System;
using System.Collections.Generic;

[Serializable]
public class PayPalCapturePaymentJsonResponse
{

	public string id;
	public string status;
	public Payer payer;
	public List<PurchaseUnit> purchase_units;
	public List<Link> links;

	[Serializable]
	public class Payer
	{
		public Name name;
		public string email_address;
		public string payer_id;
	}

	[Serializable]
	public class Name
	{
		public string surname;
		public string method;
	}

	[Serializable]
	public class PurchaseUnit
	{
		public string reference_id;
		public Shipping shipping;
		public Payments payments;
	}

	[Serializable]
	public class Shipping
	{
		public Address address;
	}

	[Serializable]
	public class Address
	{
		public string address_line_1;
		public string address_line_2;
		public string admin_area_2;
		public string admin_area_1;
		public string postal_code;
		public string country_code;
	}

	[Serializable]
	public class Payments
	{
		public List<Capture> captures;
	}

	[Serializable]
	public class Capture
	{
		public string id;
		public string status;
		public Amount amount;
		public SellerProtection seller_protection;
		public string final_capture;
		public string disbursement_mode;
		public SellerReceivableBreakdown seller_receivable_breakdown;
		public string create_time;
		public string update_time;
		public List<Link> links;
	}

	[Serializable]
	public class SellerReceivableBreakdown
	{
		public Amount gross_amount;
		public Amount paypal_fee;
		public Amount net_amount;
	}

	[Serializable]
	public class SellerProtection
	{
		public string status;
		public string[] dispute_categories;
	}

	[Serializable]
	public class Amount
	{
		public string value;
		public string currency_code;
	}

	[Serializable]
	public class Link
	{
		public string href;
		public string rel;
		public string method;
	}
}
