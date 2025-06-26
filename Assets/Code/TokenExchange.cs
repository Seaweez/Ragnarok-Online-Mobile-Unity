using Nethereum.Web3;
using System;
using System.Numerics;
using System.Threading.Tasks;
using Thirdweb;
using UnityEngine;

public class TokenExchange : MonoBehaviour
{
    private ThirdwebSDK sdk;
    private Contract tokenExchangeContract;
    private Contract catroTokenContract;

    private string exchangeContractAddress = "0x00C6FbF0ceCFA751d13a2E69f1cf370360c2b2Cc"; // Replace with your new deployed contract address on Mumbai
    private string catroTokenAddress = "0x9EedC54D3CFDB1247c097019aea9fd232Dce851F"; // Replace with your CATRO token contract address


    void Start()
    {
        // Initializing the Thirdweb SDK
        sdk = ThirdwebManager.Instance.SDK;

        // Getting the exchange contract
        tokenExchangeContract = sdk.GetContract(exchangeContractAddress);

        // Getting the CATRO token contract
        catroTokenContract = sdk.GetContract(catroTokenAddress);
    }

    public async void ApproveTokens()
    {
        try
        {
            // Approve maximum uint256 value
            BigInteger maxApproval = BigInteger.Pow(2, 256) - 1;
            var result = await catroTokenContract.Write("approve", new object[] { exchangeContractAddress, maxApproval });
            Debug.Log("Tokens approved: " + result.receipt.transactionHash);
        }
        catch (Exception e)
        {
            Debug.LogError("Error approving tokens: " + e.Message);
        }
    }


    public async void BuyTokens()
    {
        try
        {
            decimal tokenPriceInMATIC = 0.01m; // Replace with your token price in MATIC
            decimal lowFeeThreshold = 1000m; // Replace with your low fee threshold
            decimal highFeePercentage = 15m; // Replace with your high fee percentage
            decimal lowFeePercentage = 5m; // Replace with your low fee percentage
            decimal tokenAmount = 1;
            decimal totalMatic = tokenAmount * tokenPriceInMATIC;
            decimal fee = totalMatic < lowFeeThreshold ? (totalMatic * highFeePercentage) / 100 : (totalMatic * lowFeePercentage) / 100;
            decimal amountRequired = totalMatic + fee;
            BigInteger tokenAmount2 = 1;

            Debug.Log("Amounsst required (in MATIC): " + amountRequired);

            var transactionRequest = new TransactionRequest
            {
                value = Web3.Convert.ToWei(amountRequired).ToString()
            };

            var transaction = await tokenExchangeContract.Prepare("buyTokens", tokenAmount2);

            transaction.SetValue(amountRequired.ToString());

            var result = await transaction.Send();

            Debug.Log("Tokens purchased: " + result);
        }
        catch (Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
    }

    public async void ExchangeTokens(decimal tokenAmount)
    {
        var result = await tokenExchangeContract.Write("exchangeTokens", new object[] { Web3.Convert.ToWei(tokenAmount) });
        Debug.Log("Tokens exchanged: " + result);
    }

    public async void WithdrawMATIC(decimal amount)
    {
        var weiAmount = Web3.Convert.ToWei(amount);
        var result = await tokenExchangeContract.Write("withdrawMATIC", new object[] { weiAmount });
        Debug.Log("MATIC withdrawn: " + result);
    }

    public async void WithdrawTokens(decimal amount)
    {
        var weiAmount = Web3.Convert.ToWei(amount);
        var result = await tokenExchangeContract.Write("withdrawTokens", new object[] { weiAmount });
        Debug.Log("Tokens withdrawn: " + result);
    }

    public async void GetMaticBalance()
    {
        try
        {
            var balance = await tokenExchangeContract.Read<BigInteger>("getMaticBalance");
            Debug.Log("MATIC Balance: " + balance.ToString());
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error: " + e.Message);
        }
    }

    private decimal CalculateFee(decimal totalMatic, decimal lowFeeThreshold, decimal highFeePercentage, decimal lowFeePercentage)
    {
        if (totalMatic < lowFeeThreshold)
        {
            return (totalMatic * highFeePercentage) / 100;
        }
        else
        {
            return (totalMatic * lowFeePercentage) / 100;
        }
    }
}
