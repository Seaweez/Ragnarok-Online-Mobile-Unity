using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System;

public class Dailyreward : MonoBehaviour
{
    private string url = "http://api.pinidea.online/function/getdailyreward.php";
    private string encryptionKey = "6a2d8f0b68f5e0c04b0b8d7c1e7f8a9e";

    public void FetchAllRewards()
    {
        StartCoroutine(GetAllRewardsCoroutine());
    }

    private IEnumerator GetAllRewardsCoroutine()
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error: " + www.error);
        }
        else
        {
            string jsonResponse = www.downloadHandler.text;
            Debug.Log("Response: " + jsonResponse);

            EncryptedResponse response = JsonUtility.FromJson<EncryptedResponse>(jsonResponse);

            if (response.data != null)
            {
                try
                {
                    Debug.Log("Encrypted Data: " + response.data);
                    string decryptedData = DecryptString(response.data, encryptionKey);
                    RewardIdResponse rewardResponse = JsonUtility.FromJson<RewardIdResponse>(decryptedData);

                    if (rewardResponse.status == "success")
                    {
                       // Debug.Log("Reward IDs: " + string.Join(", ", rewardResponse.reward_ids));
                    }
                    else
                    {
                        Debug.LogError("Error: " + rewardResponse.message);
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError("Decryption error: " + ex.Message);
                }
            }
            else
            {
                Debug.LogError("Error: " + response.message);
            }
        }
    }

    private string DecryptString(string cipherText, string key)
    {
        try
        {
            byte[] fullCipher = Convert.FromBase64String(cipherText);
            //Debug.Log("Full Cipher Length: " + fullCipher.Length);

            if (fullCipher.Length < 16)
            {
                Debug.LogError("Invalid cipher text length");
                return null;
            }

            byte[] iv = new byte[16];
            byte[] cipherBytes = new byte[fullCipher.Length - 16];

            Array.Copy(fullCipher, 0, iv, 0, iv.Length);
            Array.Copy(fullCipher, iv.Length, cipherBytes, 0, cipherBytes.Length);

            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            Array.Resize(ref keyBytes, 32);

            using (Aes aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.IV = iv;

                using (ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                {
                    using (MemoryStream ms = new MemoryStream(cipherBytes))
                    {
                        using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader reader = new StreamReader(cs))
                            {
                                return reader.ReadToEnd();
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Decryption failed: " + ex.Message);
            return null;
        }
    }


    [System.Serializable]
    public class EncryptedResponse
    {
        public string data;
        public string message;
    }

    [System.Serializable]
    public class RewardIdResponse
    {
        public string status;
        public List<string> reward_ids;
        public string message;
    }
}
