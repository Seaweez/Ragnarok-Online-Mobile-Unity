using Thirdweb;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using SLua;
using System;

public class NFTLoader : MonoBehaviour
{
    private static NFTLoader _instance;

    public static NFTLoader Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("NFTLoader");
                _instance = go.AddComponent<NFTLoader>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    private ThirdwebSDK sdk;
    private string ERC1155Contract = "0x2a1f1D59D0F7fc26Eadd7d3788536351f95dEB75";
    private bool isWalletConnected = false;
    private bool isLoading = false;

    public List<string> LoadedMetadata { get; private set; }
    private LuaFunction onLoadingStatusChanged;

    void Start()
    {
        sdk = ThirdwebManager.Instance.SDK;
    }

    // ฟังก์ชันที่เชื่อมต่อกับ SLua เพื่อเช็คสถานะการเชื่อมต่อของ wallet
    public async Task<bool> CheckConnectWallet()
    {
        try
        {
            string ownerAddress = await sdk.Wallet.GetAddress();
            if (!string.IsNullOrEmpty(ownerAddress))
            {
                isWalletConnected = true;
                return true;
            }
            else
            {
                isWalletConnected = false;
                return false;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error checking wallet connection: {e.Message}");
            return false;
        }
    }

    // ฟังก์ชันที่สามารถเรียกใช้ได้จาก Lua
    public async Task<bool> CheckAndProceedWithWallet()
    {
        bool isConnected = await CheckConnectWallet();
        if (!isConnected)
        {
            Debug.LogWarning("Please connect your wallet before proceeding.");
            return false;
        }
        Debug.Log("Wallet connected. Proceeding with transaction.");
        return true;
    }

    // เปลี่ยนฟังก์ชันนี้เป็น public เพื่อเรียกจาก Lua ได้
    public async Task StartLoadingNFTs()
    {
        isLoading = true;
        onLoadingStatusChanged?.call(true);  // แจ้งสถานะการโหลดเริ่มต้นให้ Lua รู้

        await LoadNFTsOwnedByUser();

        isLoading = false;
        onLoadingStatusChanged?.call(false);  // แจ้งสถานะการโหลดเสร็จสิ้นให้ Lua รู้
    }

    public async Task LoadNFTsOwnedByUser()
    {
        bool isConnected = await CheckConnectWallet();
        if (isConnected)
        {
            Debug.LogWarning("Connect");
            isWalletConnected = true;
        }
        else
        {
            Debug.LogWarning("Please connect your wallet before proceeding.");
            isWalletConnected = false;
            return;
        }

        if (!isWalletConnected)
        {
            Debug.LogWarning("Cannot load NFTs because the wallet is not connected.");
            return;
        }

        SetLoadingStatus(true);

        try
        {
            string ownerAddress = await sdk.Wallet.GetAddress();

            if (string.IsNullOrEmpty(ownerAddress))
            {
                Debug.LogWarning("No wallet connected or address is null.");
                SetLoadingStatus(false);
                return;
            }

            LoadedMetadata = new List<string>();

            Contract contract = sdk.GetContract(ERC1155Contract);
            var nfts = await contract.ERC1155.GetOwned(ownerAddress);

            foreach (var nft in nfts)
            {
                string metadataJson = JsonConvert.SerializeObject(nft.metadata);
                LoadedMetadata.Add(metadataJson);
            }

            Debug.Log($"NFTs owned by user loaded successfully. Total NFTs: {LoadedMetadata.Count}");

        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error loading owned NFTs: {e.Message}");
            LoadedMetadata = null;
        }
        finally
        {
            SetLoadingStatus(false);
        }
    }

    private void SetLoadingStatus(bool status)
    {
        isLoading = status;
        onLoadingStatusChanged?.call(isLoading);
    }

    public void ClearMetadata()
    {
        LoadedMetadata?.Clear();
        LoadedMetadata = null;
    }

    // ลงทะเบียน callback เพื่อแจ้งสถานะการโหลดให้ Lua
    public void RegisterLoadingStatusCallback(LuaFunction callback)
    {
        onLoadingStatusChanged = callback;
    }

    public LuaTable UpdateMetadata(LuaTable luaTable = null)
    {
        if (luaTable == null)
        {
            Debug.LogError("LuaTable is null");
            return luaTable;
        }

        if (!isWalletConnected)
        {
            Debug.LogWarning("Cannot update metadata because the wallet is not connected.");
            return luaTable;
        }

        if (LoadedMetadata == null || LoadedMetadata.Count == 0)
        {
            Debug.LogWarning("LoadedMetadata is null or not loaded");
            return luaTable;
        }

        int luaTableCount = luaTable.length();
        for (int i = 1; i <= luaTableCount; i++)
        {
            luaTable[i] = null;
        }

        for (int i = 0; i < LoadedMetadata.Count; i++)
        {
            luaTable[i + 1] = LoadedMetadata[i];
        }

        return luaTable;
    }
}
