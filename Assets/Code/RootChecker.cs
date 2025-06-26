using UnityEngine;
using System.Collections;
using System;

public class RootChecker : MonoBehaviour
{
    void Start()
    {
        // ��Ǩ�ͺ����ػ�ó����ա�� root
        if (IsDeviceRooted())
        {
            // ����ա�� root ���Դ�ͻ���पѹ
            CloseApplication();
        }
    }

    // �ѧ��ѹ����Ѻ��Ǩ�ͺ�ػ�ó��� root
    private bool IsDeviceRooted()
    {
        bool isRooted = false;

        // ���������� shell
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

    // �ѧ��ѹ����Ѻ�Դ�ͻ���पѹ
    private void CloseApplication()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}