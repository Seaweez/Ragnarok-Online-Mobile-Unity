using System;
using System.Collections.Generic;

[Serializable]
public class PayPalCreatePaymentJsonRequest {

	public string intent;
	public Payer payer;
	public List<Transaction> transactions;
	public RedirectUrls redirect_urls;

	[Serializable]
	public class Payer {
		public string payment_method;
	}

	[Serializable]
	public class Transaction {
		public Amount amount;
		public ItemList item_list;
		public string description;
		public string invoice_number;
	}

	[Serializable]
	public class Amount {
		public string total;
		public string currency;
	}

	[Serializable]
	public class Item {
		public string name;
		public string description;
		public string quantity;
		public string price;
		public string currency;
	}

	[Serializable]
	public class ItemList {
		public List<Item> items;
	}

	[Serializable]
	public class RedirectUrls {
		public string return_url;
		public string cancel_url;
	}

	public static PayPalCreatePaymentJsonRequest instatiateBasic()
    {
		PayPalCreatePaymentJsonRequest request = new PayPalCreatePaymentJsonRequest();
		request.payer = new Payer();
		request.redirect_urls = new RedirectUrls();
		request.transactions = new List<Transaction>();
		request.transactions.Add(new Transaction());
		request.transactions[0].amount = new Amount();
		request.transactions[0].item_list = new ItemList();
		request.transactions[0].item_list.items = new List<Item>();
		request.transactions[0].item_list.items.Add(new Item());

		request.intent = "sale";
		request.payer.payment_method = "paypal";
		request.transactions[0].item_list.items[0].quantity = "1";
		request.redirect_urls.return_url = GlobalPayPalProperties.INSTANCE.returnUrl;
		request.redirect_urls.cancel_url = GlobalPayPalProperties.INSTANCE.cancelUrl;

		return request;
	}

}
	