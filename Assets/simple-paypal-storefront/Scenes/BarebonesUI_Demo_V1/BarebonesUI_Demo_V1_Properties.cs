using UnityEngine;

public class BarebonesUI_Demo_V1_Properties : MonoBehaviour
{

    public static BarebonesUI_Demo_V1_Properties INSTANCE;

    private void Awake()
    {
        INSTANCE = this;
    }

    public string payUrl;

    public string accessToken;

    public string payerId;

    public string paymentStatus;

    public string paymentId;

}
