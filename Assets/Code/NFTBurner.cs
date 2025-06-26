using Thirdweb;
using UnityEngine;
using System.Threading.Tasks;
using SLua; // Add SLua namespace

public class NFTBurner : MonoBehaviour
{
    private ThirdwebSDK sdk;
    private string ERC1155Contract = "0x2a1f1D59D0F7fc26Eadd7d3788536351f95dEB75";

    void Start()
    {
        sdk = ThirdwebManager.Instance.SDK;
    }

    // Add LuaFunction as a parameter for the callback
    public async Task BurnNFT(string tokenId, int amount, LuaFunction callback)
    {
        try
        {
            string ownerAddress = await sdk.Wallet.GetAddress();
            if (string.IsNullOrEmpty(ownerAddress))
            {
                Debug.LogWarning("No wallet connected or address is null.");
                return;
            }

            Contract contract = sdk.GetContract(ERC1155Contract);

            // Burn the specified amount of the tokenId
            var burnResult = await contract.ERC1155.Burn(tokenId, amount);

            Debug.Log("Burn successful: " + burnResult.receipt.status);

            // If burn is successful, call the callback with success status
            if (burnResult.receipt.status == 1) // status "1" usually means success
            {
                callback?.call(tokenId, true); // Pass tokenId and success status to Lua
            }
            else
            {
                callback?.call(tokenId, false); // Pass tokenId and failure status to Lua
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error burning NFT: " + e.Message);
            callback?.call(tokenId, false); // Pass failure status to Lua
        }
    }
}
