using UnityEngine;
using Thirdweb;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Numerics;
using SLua;

namespace RO
{
    public class CryptoPurchase : MonoBehaviour
    {
        private ThirdwebSDK sdk;
        public int status;

        void Start()
        {
            sdk = ThirdwebManager.Instance.SDK;
        }

        public async void SendToken(string amount, LuaFunction callback)
        {
            string result;
            try
            {
                string receiverAddress = "0x63C37149486fe71bCB404E11F13e24e32B225883";
                string currencyAddress = "0xc2132d05d31c914a87c6611c10748aeb04b58e8f";
                var transactionResult = await sdk.Wallet.Transfer(receiverAddress, amount, currencyAddress);

                // แปลงผลลัพธ์เป็นโครงสร้าง Receipt
                Receipt receipt = JsonConvert.DeserializeObject<Receipt>(transactionResult.receipt.ToString());
                status = (int)receipt.status;

                result = receipt.status == 1 ? "success" : "failed";
            }
            catch (System.Exception e)
            {
                Debug.LogError("Transaction failed: " + e.Message);
                status = 0;
                result = "failed";
            }

            // เรียกใช้ callback พร้อมกับผลลัพธ์
            callback.call(result);
        }
    }
}
