using UnityEngine;
using UnityEngine.UI;

public class StoreStartupBehaviour : MonoBehaviour
{
	public static StoreStartupBehaviour INSTANCE;

	void Awake()
	{
		INSTANCE = this;
	}

	public enum StoreTheme
	{
		BASIC,
		AQUA_PAPER,
		DARK_STONE,
		DIAMOND,
		BUBBLES,
		MARBLE,
		METAL,
		MOSS,
		PINSTRIPE,
		WEATHERED,
		WOOD
	}

	public StoreTheme storeTheme = StoreTheme.BASIC;

	public GameObject[] storeScreens;

	public Text[] textBoxes;

	[HideInInspector]
	public string accessToken;

	// Use this for initialization
	void Start()
	{

		//if basic is selected then don't change background
		if (storeTheme != StoreTheme.BASIC)
		{

			for (int i = 0; i < storeScreens.Length; i++)
			{
				GameObject nextStoreScreen = storeScreens[i];
				nextStoreScreen.GetComponent<Image>().sprite = Resources.Load<Sprite>("StoreThemes/" + storeTheme.ToString());
				nextStoreScreen.GetComponent<Image>().color = Color.white;
			}
		}

		setTextColours();

		DialogScreenActions.INSTANCE.setContextStoreOpen();
		DialogScreenActions.INSTANCE.ShowDialogScreen();

		GetAccessTokenAPI_Call getAccessTokenAPI_Call = PayPalCallbackStore.INSTANCE.gameObject.AddComponent<GetAccessTokenAPI_Call>();
		getAccessTokenAPI_Call.payPallCallbackBase = PayPalCallbackStore.INSTANCE;

	}

	private void setTextColours()
	{

		Color textColorToUse = Color.black;

		switch (storeTheme)
		{

			case StoreTheme.METAL:
			case StoreTheme.DARK_STONE:
			case StoreTheme.BASIC:
			case StoreTheme.WEATHERED:
				{

					textColorToUse = Color.white;

				}
				break;

		}

		foreach (Text t in textBoxes)
		{
			t.color = textColorToUse;
		}

	}

}
