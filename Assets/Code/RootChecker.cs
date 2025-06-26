using UnityEngine;
using System.Collections;
using System;

public class RootChecker : MonoBehaviour
{
    void Start()
    {
        // ตรวจสอบว่าอุปกรณ์นี้มีการ root
        if (IsDeviceRooted())
        {
            // ถ้ามีการ root ให้ปิดแอปพลิเคชัน
            CloseApplication();
        }
    }

    // ฟังก์ชันสำหรับตรวจสอบอุปกรณ์ที่ root
    private bool IsDeviceRooted()
    {
        bool isRooted = false;

        // เช็คโดยใช้คำสั่ง shell
        try
        {
            string[] dangerousPaths = { "/system/app/Superuser.apk", "/sbin/su", "/system/bin/su", "/system/xbin/su", "/data/local/xbin/su", "/data/local/bin/su", "/system/sd/xbin/su", "/system/bin/failsafe/su", "/data/local/su" };

            foreach (string path in dangerousPaths)
            {
                if (System.IO.File.Exists(path))
                {
                    isRooted = true;
                    break;
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error checking for root: " + e.Message);
        }

        return isRooted;
    }

    // ฟังก์ชันสำหรับปิดแอปพลิเคชัน
    private void CloseApplication()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}