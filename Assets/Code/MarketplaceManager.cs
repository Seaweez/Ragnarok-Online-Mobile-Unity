using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Thirdweb;
using SLua;
using Newtonsoft.Json;
using Thirdweb.Contracts.DirectListingsLogic.ContractDefinition;
using System.Numerics;
using System.Linq;
using Nethereum.ABI.FunctionEncoding;

public class MarketplaceManager : MonoBehaviour
{
    private ThirdwebSDK sdk;
    private Contract marketplaceContract;
    private string ERC1155Contract = "0x2a1f1D59D0F7fc26Eadd7d3788536351f95dEB75";
    private string CurencyContract = "0xc2132D05D31c914a87C6611C10748AEb04B58e8F"; //
    public string status;
    public LuaTable listingTable;
    public LuaTable metadataTable;
    public string approvalStatus;
    public string transactionStatus = "pending";
    public string address;
    public string balanceamount;

    async void Start()
    {
        // Initialize Thirdweb SDK
        sdk = ThirdwebManager.Instance.SDK;

        // Set up the Marketplace Contract
        marketplaceContract = sdk.GetContract("0x4ab276eE9f746aCB303D37b82a1CffCdE57add81");

        // Connect to MetaMask Wallet
        await ConnectWallet();
        
    }

    // Function to connect wallet (MetaMask in this case)
    private async Task ConnectWallet()
    {
        var wallet = sdk.Wallet;

        if (wallet != null)
        {
            var walletAddress = await wallet.GetAddress();
            var balance = await wallet.GetBalance(CurencyContract);
            Debug.Log("Connected Wallet Address: " + walletAddress);
            Debug.Log("Connected Wallet Address: " + balance.value);
            address = walletAddress;
            balanceamount = balance.value;
        }
    }

    
    public async Task CheckAndUpdateApprovalStatus(string nftContractAddress, string ownerAddress)
    {
        var nftContract = sdk.GetContract(nftContractAddress);
        bool isApproved = await nftContract.ERC1155.IsApprovedForAll(ownerAddress, marketplaceContract.Address);

        // �纤��ʶҹС��͹��ѵ��� string
        approvalStatus = isApproved ? "Approved" : "Not Approved";

        Debug.Log(approvalStatus);
    }

    // ������ҧ������¡��ѧ��ѹ��ҹ UI
    public async void OnClickCheckApprovalStatus()
    {
        var ownerAddress = await sdk.Wallet.GetAddress();
        await CheckAndUpdateApprovalStatus(ERC1155Contract, ownerAddress);
    }

    // Function to get and display all Direct Listings from the marketplace
    public async Task GetDirectListings()
    {
        Debug.Log("Fetching all direct listings...");

        var directListings = await marketplaceContract.Marketplace.DirectListings.GetAll();
        foreach (var listing in directListings)
        {
            Debug.Log($"Listing ID: {listing.id}, Asset: {listing.asset}, Price: {listing.pricePerToken}");
        }
    }

    // Function to buy an NFT from the marketplace
    public async Task BuyNFT(string listingId, string quantity)
    {
        var listing = await marketplaceContract.Marketplace.DirectListings.GetListing(listingId);

        // ��Ǩ�ͺʶҹТͧ��¡�â������纤�� status ���
        if (listing.status == MarkteplaceStatus.CREATED)
        {
            Debug.Log("Buying NFT...");
            await marketplaceContract.Marketplace.DirectListings.BuyFromListing(listingId, quantity, await sdk.Wallet.GetAddress());
            Debug.Log("NFT Purchased!");
            status = "CREATED";
        }
        else if (listing.status == MarkteplaceStatus.COMPLETED)
        {
            Debug.Log("This NFT has already been sold.");
            status = "COMPLETED";
        }
        else if (listing.status == MarkteplaceStatus.CANCELLED)
        {
            Debug.Log("This listing has been cancelled.");
            status = "CANCELLED";
        }
    }

    public async Task<string> CheckStatus(string listingId)
    {
        var listing = await marketplaceContract.Marketplace.DirectListings.GetListing(listingId);

        // ��˹���� status ���ʶҹТͧ��¡��
        if (listing.status == MarkteplaceStatus.CREATED)
        {
            status = "CREATED";
        }
        else if (listing.status == MarkteplaceStatus.COMPLETED)
        {
            status = "COMPLETED";
        }
        else if (listing.status == MarkteplaceStatus.CANCELLED)
        {
            status = "CANCELLED";
        }
        else if (listing.status == MarkteplaceStatus.DROP)
        {
            status = "CANCELLED";
        }

       // Debug.Log($"Listing ID: {listing.id}, Status: {status}, listsatus: {listing.status}");

        return status; // �׹��Ңͧʶҹ����ҹ���ǹ���
    }


    // Test functions for UI buttons
    public async void OnClickGetListings()
    {
        await GetDirectListings(); // Get all direct listings in the marketplace
    }

    public async Task OnClickBuyNFT(string listingId, string quantity)
    {
        Debug.Log($"Buying {quantity} NFTs from listing ID {listingId}...");

        // ��Ǩ�ͺ��� listingId ��� quantity ����繤����ҧ
        if (!string.IsNullOrEmpty(listingId) && !string.IsNullOrEmpty(quantity))
        {
            await BuyNFT(listingId, quantity); // ���� NFT ����������������к�
            Debug.Log("NFT Purchased!");
        }
        else
        {
            Debug.LogError("Invalid listing ID or quantity!");
        }
    }

    public async Task GetDirectListingsByOwner()
    {
        Debug.Log("Fetching direct listings by owner...");

        var walletAddress = await sdk.Wallet.GetAddress(); // Get the wallet address of the current user
        var directListings = await marketplaceContract.Marketplace.DirectListings.GetAll();

        foreach (var listing in directListings)
        {
            if (listing.creatorAddress == walletAddress)
            {
                Debug.Log($"Listing ID: {listing.id}, Asset: {listing.asset}, Price: {listing.pricePerToken}");
            }
        }
    }

    public async Task CancelListing(string listingId)
    {
        Debug.Log($"Canceling listing with ID: {listingId}");

        var walletAddress = await sdk.Wallet.GetAddress(); // Get the wallet address of the current user
        var listing = await marketplaceContract.Marketplace.DirectListings.GetListing(listingId);

        // Check if the current user is the creator of the listing
        if (listing.creatorAddress == walletAddress)
        {
            await marketplaceContract.Marketplace.DirectListings.CancelListing(listingId);
            Debug.Log("Listing canceled successfully!");
        }
        else
        {
            Debug.Log("You are not the owner of this listing.");
        }
    }

    public async void OnClickGetListingsByOwner()
    {
        await GetDirectListingsByOwner(); // Show listings owned by the current user
    }

    public async void OnClickCancelListing(string listid, Action<string> callback)
    {
        // �ӡ��¡��ԡ��¡�� NFT
        var transaction = await marketplaceContract.Marketplace.DirectListings.CancelListing(listid);

        // ��Ǩ�ͺʶҹС�÷Ӹ�á�����ҹ transaction.receipt
        string transactionStatus;
        if (transaction.receipt.status.ToString() == "1")
        {
            Debug.Log("Listing Cancelled Successfully!");
            transactionStatus = "success"; // �ѻവʶҹ��� success
        }
        else if (transaction.receipt.status.ToString() == "0")
        {
            Debug.LogError("Listing Cancellation Failed!");
            transactionStatus = "failed"; // �ѻവʶҹ��� failed
        }
        else
        {
            Debug.LogError("Unknown transaction status!");
            transactionStatus = "unknown";
        }

        // �觢�����ʶҹС�Ѻ��ҹ callback
        callback?.Invoke(transactionStatus);
    }


    public async Task SetApprovalForAll(string nftContractAddress, bool approved)
    {
        Debug.Log("Setting approval for all...");

        var nftContract = sdk.GetContract(nftContractAddress);

        // Approve or revoke approval for the Marketplace to manage all tokens on behalf of the user
        await nftContract.ERC1155.SetApprovalForAll(marketplaceContract.Address, approved);

        Debug.Log(approved ? "Marketplace approved for all NFTs!" : "Marketplace approval revoked!");
    }

    public async void OnClickSetApprovalForAll()
    {
        await SetApprovalForAll(ERC1155Contract, true); // Approve all NFTs for the marketplace
        OnClickCheckApprovalStatus();
    }

    public async void OnClickRevokeApprovalForAll()
    {
        await SetApprovalForAll(ERC1155Contract, false); // Revoke approval for the marketplace
    }

    public async Task LoadListingsAndMetadataToLua()
    {
        if (listingTable == null)
        {
            listingTable = new LuaTable(LuaState.main);
        }
        if (metadataTable == null)
        {
            metadataTable = new LuaTable(LuaState.main);
        }

        // Fetch listings and metadata
        var listings = await marketplaceContract.Marketplace.DirectListings.GetAllValid();

        if (listings != null && listings.Count > 0)
        {
            var newListingData = new List<string>();
            var newMetadataData = new List<string>();

            // Run listing and metadata processing concurrently
            var tasks = listings.Select(async listing =>
            {
                // ���¡��ѧ��ѹ CheckStatus ���͵�Ǩ�ͺʶҹ�
                var status = await CheckStatus(listing.id.ToString());

                // �ҡʶҹ����ç�Ѻ CREATED, CANCELLED, ���� COMPLETED ���������Ź��
                if (status != "CREATED" && status != "CANCELLED")
                {
                    // Serialize listing and add to the list
                    string listingJson = JsonConvert.SerializeObject(listing);
                    newListingData.Add(listingJson);

                    // Fetch and process metadata if available
                    if (listing.asset.HasValue)
                    {
                        NFTMetadata metadata = listing.asset.Value;
                        string metadataJson = JsonConvert.SerializeObject(metadata);
                        newMetadataData.Add(metadataJson);
                    }
                }
            });

            await Task.WhenAll(tasks);

            // Update LuaTable with new listings and metadata
            UpdateLuaTable(listingTable, newListingData);
           // UpdateLuaTable(metadataTable, newMetadataData);

            Debug.Log("Listings and Metadata updated in LuaTable.");
        }
        else
        {
            Debug.Log("No listings found.");
        }
    }

    public async Task LoadListingsAndMetadataByCreatorToLua()
    {
        if (listingTable == null)
        {
            listingTable = new LuaTable(LuaState.main);
        }
        if (metadataTable == null)
        {
            metadataTable = new LuaTable(LuaState.main);
        }

        // �֧��������¡�÷������ҡ Marketplace
        var listings = await marketplaceContract.Marketplace.DirectListings.GetAllValid();
        var walletAddress = await sdk.Wallet.GetAddress();

        // ��Ǩ�ͺ�������¡���������
        if (listings != null && listings.Count > 0)
        {
            var newListingData = new List<string>();
            var newMetadataData = new List<string>();

            // �����ż���¡����� metadata Ẻ��ҹ
            var tasks = listings.Select(async listing =>
            {
                // ��Ǩ�ͺʶҹТͧ��¡��
                var status = await CheckStatus(listing.id.ToString());

                // ��Ǩ�ͺ�����¡����������������
                long currentTimeInSeconds = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                bool isExpired = currentTimeInSeconds > listing.endTimeInSeconds;

                // ��ͧ੾����¡�âͧ creator ����ѧ�������������ʶҹ������ "CANCELLED"
                if (listing.creatorAddress == walletAddress && status != "CANCELLED" && !isExpired)
                {
                    string listingJson = JsonConvert.SerializeObject(listing);
                    newListingData.Add(listingJson);

                    if (listing.asset.HasValue)
                    {
                        NFTMetadata metadata = listing.asset.Value;
                        string metadataJson = JsonConvert.SerializeObject(metadata);
                        newMetadataData.Add(metadataJson);
                    }
                }
            });

            // ������û����żŷ������������
            await Task.WhenAll(tasks);

            // �ѻവ LuaTable ���¢�������¡����� metadata
            UpdateLuaTable(metadataTable, newListingData);

            Debug.Log("Creator's Listings and Metadata updated in LuaTable.");
        }
        else
        {
            // �������¡��� Marketplace
            Debug.Log("No listings found for this creator.");
        }
    }



    private void UpdateLuaTable(LuaTable luaTable, List<string> dataList)
    {
        if (luaTable == null)
        {
            Debug.LogError("LuaTable is null");
            return;
        }

        // ��������������� LuaTable ���¡�õ�駤���� null
        for (int i = 1; i <= luaTable.length(); i++)
        {
            luaTable[i] = null;  // ��駤���� null ����ź������
        }

        // �������������� LuaTable
        for (int i = 0; i < dataList.Count; i++)
        {
            luaTable[i + 1] = dataList[i]; // LuaTable �������� index 1
        }

        Debug.Log("LuaTable updated.");
    }


    public async void OnClickLoadListingsAndMetadata(Action callback)
    {
        await LoadListingsAndMetadataToLua();
        callback?.Invoke();
    }

    public async void Updateprice(Action callback)
    {
        var wallet = sdk.Wallet;

        if (wallet != null)
        {
            var walletAddress = await wallet.GetAddress();
            var balance = await wallet.GetBalance(CurencyContract);
            address = walletAddress;
            balanceamount = balance.value;
        }
        callback?.Invoke();
    }

    public async void OnClickLoadListingsAndMetadataCreator(Action callback)
    {
        await LoadListingsAndMetadataByCreatorToLua();
        callback?.Invoke();
    }


    public async Task ListNFTWithParameters(string assetContract, string tokenId, string quantity, string pricePerToken)
    {
        var listingParams = new CreateListingInput
        {
            assetContractAddress = assetContract,
            tokenId = tokenId,
            quantity = quantity,
            pricePerToken = pricePerToken,
            currencyContractAddress = CurencyContract,
        };

        try
        {
            // ���ҧ��¡�â������Ѻ������ transaction
            var transaction = await marketplaceContract.Marketplace.DirectListings.CreateListing(listingParams);
            var receipt = transaction.receipt;

            // ��Ǩ�ͺʶҹТͧ��á�����ҹ receipt.status
            if (receipt.status.ToString() == "1")
            {
              //  Debug.Log("NFT listed for sale successfully!");
                transactionStatus = "success"; // �ѻവʶҹ��� success
            }
            else if (receipt.status.ToString() == "0")
            {
            //    Debug.LogError("NFT listing failed!");
                transactionStatus = "failed"; // �ѻവʶҹ��� failed
            }
        }
        catch (Exception ex)
        {
        //    Debug.LogError($"Failed to list NFT: {ex.Message}");
            transactionStatus = "failed"; // �ѻവʶҹ��� failed ������Դ��ͼԴ��Ҵ
        }
    }

    // �ѧ��ѹ������� Lua ����ö���¡����
    public string GetTransactionStatus()
    {
        return transactionStatus; // �׹���ʶҹС�÷Ӹ�á���
    }

    public async void OnClickListNFT(string tokenId, string pricePerToken, Action<string> callback)
    {
        string quantity = "1";

        // ���¡��������˹� startTimestamp ��� endTimestamp
        await ListNFTWithParameters(ERC1155Contract, tokenId, quantity, pricePerToken);

        // ���¡ callback ������ѺʶҹТͧ��÷Ӹ�á���
        callback?.Invoke(transactionStatus);
    }

    public async void OnClickBuyNFT(string listingId, Action<string> callback)
    {
        string quantity = "1";
        try
        {
            // �֧������ listing ���ʹ��Ҥ�
            var listing = await marketplaceContract.Marketplace.DirectListings.GetListing(listingId);
            var listingPrice = BigInteger.Parse(listing.pricePerToken);  // �Ҥҵ�� NFT ˹����¡��
            var totalCost = listingPrice * BigInteger.Parse(quantity);

            var wallet = sdk.Wallet;
            var data = await sdk.Wallet.GetBalance(CurencyContract);
            BigInteger balanceAmount = BigInteger.Parse(data.value);

            // ��Ǩ�ͺ�ʹ�Թ��§���������
            if (balanceAmount < totalCost)
            {
                //  Debug.LogError($"Insufficient funds to purchase NFT. Available: {balanceAmount}, Required: {totalCost}");
                transactionStatus = "Insufficient";
                callback?.Invoke(transactionStatus);
                return;
            }

            // ��Ǩ�ͺ Allowance
            var usdtContract = sdk.GetContract(CurencyContract);
            var allowance = await usdtContract.ERC20.Allowance(marketplaceContract.Address);
            BigInteger allowanceAmount = BigInteger.Parse(allowance.value);

            if (allowanceAmount < totalCost)
            {
                // Debug.Log($"Allowance is insufficient. Setting allowance for {totalCost}");
                // �ҡ Allowance ���� ����駤������
                var transaction2 = await usdtContract.ERC20.SetAllowance(marketplaceContract.Address, totalCost.ToString());

                if (transaction2.receipt.status.ToString() == "1")
                {
                    var transaction = await marketplaceContract.Marketplace.DirectListings.BuyFromListing(listingId, quantity, await sdk.Wallet.GetAddress());
                    if (transaction.receipt.status.ToString() == "1")
                    {
                        transactionStatus = "success";
                        var balance = await wallet.GetBalance(CurencyContract);
                        balanceamount = balance.value;
                        callback?.Invoke(transactionStatus);
                    }
                    else
                    {
                        transactionStatus = "failed";
                        callback?.Invoke(transactionStatus);
                    }

                }
                else
                {
                    transactionStatus = "failed";
                    callback?.Invoke(transactionStatus);
                }


            }
            else{ 
                 var transaction = await marketplaceContract.Marketplace.DirectListings.BuyFromListing(listingId, quantity, await sdk.Wallet.GetAddress());

                // Debug.Log($"Status {transaction.receipt.status.ToString()}");

                if (transaction.receipt.status.ToString() == "1")
                {
                    transactionStatus = "success";
                    var balance = await wallet.GetBalance(CurencyContract);
                    balanceamount = balance.value;
                    callback?.Invoke(transactionStatus);
                }
                else
                {
                    transactionStatus = "failed";
                    callback?.Invoke(transactionStatus);
                }
             }
            
        }
        catch (SmartContractRevertException ex)
        {
            Debug.LogError($"Smart contract error: {ex.Message}");
            transactionStatus = "failed";
            callback?.Invoke(transactionStatus);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Unexpected error: {ex.Message}");
            transactionStatus = "failed";
            callback?.Invoke(transactionStatus);
        }

        callback?.Invoke(transactionStatus);
    }



}
