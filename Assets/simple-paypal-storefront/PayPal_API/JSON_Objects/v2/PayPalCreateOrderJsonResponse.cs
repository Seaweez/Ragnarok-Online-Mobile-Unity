using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PayPalCreateOrderJsonResponse
{

	public string id;
	public string status;

	public List<Link> links;

	[Serializable]
	public class Link
	{
		public string href;
		public string rel;
		public string method;
	}

}
