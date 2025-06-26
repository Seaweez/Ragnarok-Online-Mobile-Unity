using System.Threading.Tasks;
using Thirdweb;
using UnityEngine;
using Newtonsoft.Json;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using System;
using Contract = Thirdweb.Contract;
using Nethereum.Util;
using System.Collections.Generic;
using Nethereum.RPC.Eth.DTOs;
using System.Net.Http;
using SLua;


public class MintAndUploadMetadata : MonoBehaviour
{
    private ThirdwebSDK sdk;
    private Contract tokenERC1155Contract;

    private string ERC1155Contract = "0x2a1f1D59D0F7fc26Eadd7d3788536351f95dEB75";
    string privateKey = "bf339b7ff19aa9e8a36eb8bfc7c63d6cdc0bdd94eb406ff51c4e43108e943b55";
    private Web3 web3;
    private string rpcUrl = "https://wider-quaint-water.matic.quiknode.pro/16663e5f888eacd7f2ce71528b4a992903c7a01c";
    private static readonly HttpClient client = new HttpClient();
    private Queue<Func<Task>> transactionQueue = new Queue<Func<Task>>();
    private BigInteger currentNonce;
    private string addressnoce;
    private bool isProcessingQueue = false;

    void Start()
    {
        sdk = ThirdwebManager.Instance.SDK; // Reference the SDK
        tokenERC1155Contract = sdk.GetContract(ERC1155Contract);
        var account = new Account(privateKey);
        web3 = new Web3(account, rpcUrl);
        addressnoce = account.Address;
    }


    public async void QueueTransaction(Func<Task> transactionFunc)
    {
        try
        {
            // ดึง nonce ล่าสุดจาก blockchain
            currentNonce = await web3.Eth.Transactions.GetTransactionCount.SendRequestAsync(addressnoce, BlockParameter.CreatePending());
            Debug.Log($"Fetched nonce from blockchain: {currentNonce}");

            // บันทึก nonce ที่ดึงมาจาก blockchain ไปยัง PHP
            bool saveSuccess = await SaveNonceToPHP(addressnoce, currentNonce);
            if (!saveSuccess)
            {
                Debug.LogError("Failed to save nonce to PHP, aborting transaction.");
                return; // หยุดการดำเนินการหากการบันทึก nonce ล้มเหลว
            }
            Debug.Log($"Saved nonce {currentNonce} to PHP");

          
         

            // เพิ่มฟังก์ชันเข้าสู่คิว
            transactionQueue.Enqueue(async () =>
            {
               // Debug.Log($"Executing queued transaction with nonce: {currentNonce}");
                await transactionFunc();
          
            });

            // ตรวจสอบสถานะการประมวลผลคิว
            if (!isProcessingQueue)
            {
                ProcessQueue();
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error in QueueTransaction: " + ex.Message);
        }
    }



    private async void ProcessQueue()
    {
        if (isProcessingQueue) return; // ป้องกันการเรียกซ้ำซ้อน

        isProcessingQueue = true;

        while (transactionQueue.Count > 0)
        {
            var transactionFunc = transactionQueue.Dequeue();
            try
            {
                await transactionFunc();

           
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error processing transaction: {ex.Message}");
                // อาจต้องการเพิ่มการจัดการข้อผิดพลาดเพิ่มเติม เช่น การบันทึกสถานะ หรือการ retry ธุรกรรม
            }
        }

        isProcessingQueue = false; // ตั้งค่าสถานะว่าการประมวลผลคิวเสร็จสิ้นแล้ว
    }



    public async void Mint(string itemInfoJson, string itemDataBase64, LuaFunction callback)
    {
        string address = await sdk.Wallet.GetAddress();

        if (string.IsNullOrEmpty(itemDataBase64))
        {
            Debug.LogError("Error: itemDataBase64 is null or empty.");
            callback.call(false, "Error: itemDataBase64 is null or empty.");
            return;
        }

        if (!IsBase64String(itemDataBase64))
        {
            Debug.LogError("Error: itemDataBase64 contains invalid characters.");
            callback.call(false, "Error: itemDataBase64 contains invalid characters.");
            return;
        }

        byte[] itemDataBinary;
        try
        {
            itemDataBinary = Convert.FromBase64String(itemDataBase64);
        }
        catch (FormatException ex)
        {
            Debug.LogError("Error decoding Base64 string: " + ex.Message);
            callback.call(false, "Error decoding Base64 string: " + ex.Message);
            return;
        }

        Debug.Log("Decoded itemDataBinary Length: " + itemDataBinary.Length);

        var itemInfo = JsonConvert.DeserializeObject<ItemInfo>(itemInfoJson);

        NFTMetadata metadata = new NFTMetadata()
        {
            name = itemInfo.name,
            description = "",
            image = "",
            attributes = new Dictionary<string, object>
        {
            { "itemid", itemInfo.itemid },
            { "price", itemInfo.price },
            { "count", itemInfo.count },
            { "guid", itemInfo.guid },
            { "publicity_id", itemInfo.publicity_id },
            { "item_data", itemDataBase64 },
            { "isNFT", itemInfo.isNFT }
        }
        };

        string metadataJson = JsonConvert.SerializeObject(metadata);

        var uploadResult = await sdk.Storage.UploadText(metadataJson);

        string uri = uploadResult.IpfsHash.CidToIpfsUrl();

        var mintFunctionMessage = new MintFunction()
        {
            To = address,
            TokenId = BigInteger.Subtract(BigInteger.Pow(2, 256), BigInteger.One),
            Uri = uri,
            Amount = new BigInteger(1)
        };

        QueueTransaction(async () =>
        {
            try
            {
                var transactionHandler = web3?.Eth?.GetContractTransactionHandler<MintFunction>();
                if (transactionHandler == null)
                {
                    Debug.LogError("TransactionHandler is not initialized.");
                    callback.call(false, "TransactionHandler is not initialized.", true); // ส่ง isPending = true เพื่อบอกว่ากำลังทำงานอยู่
                    return;
                }

                var transactionInput = await transactionHandler.CreateTransactionInputEstimatingGasAsync(ERC1155Contract, mintFunctionMessage);
                if (transactionInput == null)
                {
                    Debug.LogError("TransactionInput creation failed.");
                    callback.call(false, "TransactionInput creation failed.", true);
                    return;
                }

                // ส่งธุรกรรมไปยัง blockchain และรับ transaction hash
                var signedTransaction = await web3.TransactionManager.SignTransactionAsync(transactionInput);
                var transactionHash = await web3.Eth.Transactions.SendRawTransaction.SendRequestAsync(signedTransaction);

                Debug.Log($"Mint transaction successful with EIP-1559: {transactionHash}");

                // ตรวจสอบสถานะธุรกรรมและ callback กลับไปยัง Lua
                bool isPending = true;
                callback.call(true, transactionHash, isPending); // แสดง isPending = true ใน Lua เพื่อบอกว่ากำลังรอ
                while (isPending)
                {
                    var receipt = await web3.Eth.Transactions.GetTransactionReceipt.SendRequestAsync(transactionHash);
                    if (receipt != null)
                    {
                        isPending = false;
                        Debug.Log("Transaction confirmed!");
                        callback.call(true, transactionHash, isPending); // แสดง isPending = false เมื่อทำรายการเสร็จแล้ว
                    }
                    else
                    {
                        Debug.Log("Transaction is still pending...");
                        await Task.Delay(10000); // รอ 10 วินาทีแล้วตรวจสอบอีกครั้ง
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Mint failed: " + ex.Message);
                callback.call(false, "Mint failed: " + ex.Message, false); // ส่ง isPending = false ถ้ามีข้อผิดพลาด
            }
        });
    }



    private async Task<BigInteger> GetNonceFromPHP(string address)
    {
        try
        {
            string url = $"http://api.pinidea.online/function/get_nonce.php?account_address={address}";
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();

            var jsonResponse = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseBody);

            if (jsonResponse != null && jsonResponse.ContainsKey("nonce"))
            {
                BigInteger nonce = BigInteger.Parse(jsonResponse["nonce"]);
                Debug.Log($"Retrieved nonce from PHP: {nonce}");
                return nonce;
            }
            else
            {
                throw new Exception("Failed to retrieve nonce: " + (jsonResponse != null && jsonResponse.ContainsKey("error") ? jsonResponse["error"] : "Unknown error"));
            }
        }
        catch (HttpRequestException e)
        {
            throw new Exception("Request error: " + e.Message);
        }
    }

    private async Task<bool> UpdateNonceInPHP(string address, BigInteger nonce)
    {
        var postData = new Dictionary<string, string>
    {
        { "account_address", address },
        { "nonce", nonce.ToString() }
    };

        var content = new FormUrlEncodedContent(postData);
        HttpResponseMessage response = await client.PostAsync("http://api.pinidea.online/function/update_nonce.php", content);

        if (!response.IsSuccessStatusCode)
        {
            Debug.LogError("Failed to update nonce in PHP: " + response.ReasonPhrase);
            return false;
        }

        string responseBody = await response.Content.ReadAsStringAsync();
        Debug.Log("Nonce updated successfully. Server response: " + responseBody);

        return responseBody.Contains("success"); // ตรวจสอบว่าการอัปเดตสำเร็จ
    }


    private async Task<bool> SaveNonceToPHP(string address, BigInteger nonceFromBlockchain)
    {
        try
        {
            var postData = new Dictionary<string, string>
        {
            { "account_address", address },
            { "nonce_from_blockchain", nonceFromBlockchain.ToString() }
        };

            var content = new FormUrlEncodedContent(postData);
            HttpResponseMessage response = await client.PostAsync("http://api.pinidea.online/function/save_nonce_if_empty.php", content);

            if (!response.IsSuccessStatusCode)
            {
                Debug.LogError("Failed to save nonce to PHP: " + response.ReasonPhrase);
                return false; // บันทึกไม่สำเร็จ
            }

            string responseBody = await response.Content.ReadAsStringAsync();
            Debug.Log("Nonce saved successfully. Server response: " + responseBody);

            // ตรวจสอบผลลัพธ์จาก server หากจำเป็น เช่น การตรวจสอบข้อความใน responseBody
            if (responseBody.Contains("success")) // ปรับตามเงื่อนไขที่เหมาะสม
            {
                return true; // บันทึกสำเร็จ
            }
            else
            {
                Debug.LogError("Nonce save failed according to server response: " + responseBody);
                return false; // บันทึกไม่สำเร็จตามข้อมูลจากเซิร์ฟเวอร์
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Exception occurred while saving nonce to PHP: " + ex.Message);
            return false; // บันทึกไม่สำเร็จเนื่องจากข้อผิดพลาด
        }
    }


    private bool IsBase64String(string base64)
    {
        Span<byte> buffer = new Span<byte>(new byte[base64.Length]);
        return Convert.TryFromBase64String(base64, buffer, out _);
    }


    // สร้าง class สำหรับฟังก์ชัน mint
    [Function("mintTo")]
    public class MintFunction : FunctionMessage
    {
        [Parameter("address", "to", 1)]
        public string To { get; set; }

        [Parameter("uint256", "tokenId", 2)]
        public BigInteger TokenId { get; set; }

        [Parameter("string", "uri", 3)]
        public string Uri { get; set; }

        [Parameter("uint256", "amount", 4)]
        public BigInteger Amount { get; set; }
    }


}

// Define ItemInfo class
public class ItemInfo
{
    public string name { get; set; }
    public string itemid { get; set; }
    public int price { get; set; }
    public int count { get; set; }
    public string guid { get; set; }
    public string publicity_id { get; set; }
    public string item_data { get; set; }
    public bool isNFT { get; set; }  // เพิ่มฟิลด์ isNFT
    public ItemInfo()
    {
        isNFT = true;
    }
}
