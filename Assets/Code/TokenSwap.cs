using UnityEngine;
using Thirdweb;
using System.Threading.Tasks;
using Nethereum.Web3;

public class TokenSwap : MonoBehaviour
{
    private ThirdwebSDK sdk;
    string erc20ContractAddress = "0x9EedC54D3CFDB1247c097019aea9fd232Dce851F";


    void Start()
    {
        sdk = ThirdwebManager.Instance.SDK;
    }

    public async void SwapTokenForowner(string amountToSwap, string erc20Amount)
    {     
        string ownerAddress = "0x63C37149486fe71bCB404E11F13e24e32B225883";

        var transferResult = await TransferMatic(ownerAddress, amountToSwap);

        if (transferResult)
        {
            await SwapMaticForERC20(erc20ContractAddress, erc20Amount);
        }
        else
        {
            Debug.LogError("MATIC Transfer failed, cannot proceed to claim tokens.");
        }
    }

    public async void SwapTokenForseller(string sellerAddress, string erc20Amount)
    {
        try
        {
            var contract = sdk.GetContract(erc20ContractAddress);
            var data = await contract.ERC20.Transfer(sellerAddress, erc20Amount);
            Debug.Log($"Transaction successful: {data.receipt.transactionHash}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Transaction failed: {e.Message}");
        }
    }

    async Task<bool> TransferMatic(string to, string amount)
    {
        try
        {
            var result = await sdk.Wallet.Transfer(to, amount);
            Debug.Log($"MATIC Transfer successful: {result.receipt.transactionHash}");
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"MATIC Transfer failed: {e.Message}");
            return false;
        }
    }

    async Task SwapMaticForERC20(string erc20Contract, string amount)
    {
        try
        {
            var contract = sdk.GetContract(erc20Contract);
            var data = await contract.ERC20.Claim(amount);
            Debug.Log($"Transaction successful: {data.receipt.transactionHash}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Transaction failed: {e.Message}");
        }
    }
    public async Task<string> GetTokenBalance()
    {
        try
        {
            var contract = sdk.GetContract(erc20ContractAddress);
            var balance = await contract.ERC20.Balance();
            string balanceOf = balance.ToString();
            return balanceOf;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to get token balance: {e.Message}");
            return "0";
        }
    }

    public async Task<string> GetAddrress()
    {
        try
        {
            string connectedAddress = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
            return connectedAddress;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to get token balance: {e.Message}");
            return "0x";
        }
    }
}
