using UnityEngine;
using System;
using System.Runtime.InteropServices;
using System.Text;
using Debug = UnityEngine.Debug;

public class ProcessProtection : MonoBehaviour
{
    [SerializeField]
    private float checkInterval = 5f;

    private float timer = 0f;
    private bool isQuitting = false;

    [StructLayout(LayoutKind.Sequential)]
    private struct PROCESSENTRY32
    {
        public uint dwSize;
        public uint cntUsage;
        public uint th32ProcessID;
        public IntPtr th32DefaultHeapID;
        public uint th32ModuleID;
        public uint cntThreads;
        public uint th32ParentProcessID;
        public int pcPriClassBase;
        public uint dwFlags;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 260)]
        public char[] szExeFile;
    }

    [DllImport("kernel32.dll")]
    private static extern IntPtr CreateToolhelp32Snapshot(uint dwFlags, uint th32ProcessID);

    [DllImport("kernel32.dll")]
    private static extern bool Process32First(IntPtr hSnapshot, ref PROCESSENTRY32 lppe);

    [DllImport("kernel32.dll")]
    private static extern bool Process32Next(IntPtr hSnapshot, ref PROCESSENTRY32 lppe);

    [DllImport("kernel32.dll")]
    private static extern bool CloseHandle(IntPtr hObject);

    [DllImport("kernel32.dll")]
    private static extern uint GetCurrentProcessId();

    private const uint TH32CS_SNAPPROCESS = 0x00000002;

    void Start()
    {
        CheckParentProcess();
    }

    void Update()
    {
        if (isQuitting) return;

        timer += Time.deltaTime;
        if (timer >= checkInterval)
        {
            CheckParentProcess();
            timer = 0f;
        }
    }

    private string GetProcessNameFromEntry(PROCESSENTRY32 entry)
    {
        // แปลงจาก char array เป็น string และทำความสะอาด
        string name = new string(entry.szExeFile);
        name = name.TrimEnd('\0').Trim();  // ลบ null terminator และ whitespace

        // แสดงค่าที่ได้ทุกขั้นตอน
       // Debug.Log($"Raw process name: '{name}'");
       // Debug.Log($"After ToLower: '{name.ToLower()}'");
       // Debug.Log($"Character codes: {string.Join(", ", Encoding.ASCII.GetBytes(name))}");

        return name.ToLower();
    }

    private void CheckParentProcess()
    {
        IntPtr snapshot = IntPtr.Zero;
        try
        {
            uint currentPID = GetCurrentProcessId();
            uint parentPID = 0;
            string parentName = "";

           // Debug.Log("\n=== Process Check Started ===");
           // Debug.Log($"Current PID: {currentPID}");

            // หา Parent PID
            snapshot = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0);
            if (snapshot != IntPtr.Zero)
            {
                PROCESSENTRY32 processEntry = new PROCESSENTRY32();
                processEntry.dwSize = (uint)Marshal.SizeOf(typeof(PROCESSENTRY32));

                if (Process32First(snapshot, ref processEntry))
                {
                    do
                    {
                        if (processEntry.th32ProcessID == currentPID)
                        {
                            parentPID = processEntry.th32ParentProcessID;
                         //   Debug.Log($"Found current process. Parent PID: {parentPID}");
                            break;
                        }
                    } while (Process32Next(snapshot, ref processEntry));
                }
            }

            // Reset snapshot และหา Parent Process Name
            CloseHandle(snapshot);
            snapshot = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0);
            if (snapshot != IntPtr.Zero && parentPID != 0)
            {
                PROCESSENTRY32 processEntry = new PROCESSENTRY32();
                processEntry.dwSize = (uint)Marshal.SizeOf(typeof(PROCESSENTRY32));

                if (Process32First(snapshot, ref processEntry))
                {
                    do
                    {
                        string currentProcessName = GetProcessNameFromEntry(processEntry);
                      //  Debug.Log($"Checking Process: '{currentProcessName}' (PID: {processEntry.th32ProcessID})");

                        if (processEntry.th32ProcessID == parentPID)
                        {
                            parentName = currentProcessName;
                          //  Debug.Log($"Found Parent Process: '{parentName}' (PID: {parentPID})");

                            // แสดงการเปรียบเทียบอย่างละเอียด
                            //Debug.Log($"Comparing:");
                            //Debug.Log($"- Parent name: '{parentName}'");
                            //Debug.Log($"- Expected: 'explorer.exe'");
                           // Debug.Log($"- Direct equality: {parentName == "explorer.exe"}");
                           // Debug.Log($"- Contains: {parentName.Contains("explorer")}");
                          //  Debug.Log($"- StartsWith: {parentName.StartsWith("explorer")}");
                        //    Debug.Log($"- EndsWith: {parentName.EndsWith(".exe")}");
                            break;
                        }
                    } while (Process32Next(snapshot, ref processEntry));
                }
            }

            // ตรวจสอบชื่อ process
            if (string.IsNullOrEmpty(parentName))
            {
              //  Debug.LogError("Could not determine parent process name");
                QuitGame();
            }
            else
            {
                // เปรียบเทียบแบบละเอียด
                bool isExplorerBasic = parentName == "explorer.exe";
                bool isExplorerIgnoreCase = parentName.Equals("explorer.exe", StringComparison.OrdinalIgnoreCase);
                bool containsExplorer = parentName.Contains("explorer");

              //  Debug.Log($"Comparison Results:");
               // Debug.Log($"- Basic equality: {isExplorerBasic}");
               // Debug.Log($"- Ignore case: {isExplorerIgnoreCase}");
               // Debug.Log($"- Contains: {containsExplorer}");

                if (!containsExplorer)
                {
                  //  Debug.LogWarning($"Parent process is not explorer.exe (Found: '{parentName}')");
                    QuitGame();
                }
                else
                {
                    //Debug.Log($"Verified: Parent process is explorer.exe (PID: {parentPID})");
                }
            }

          //  Debug.Log("=== Process Check Completed ===\n");
        }
        catch (Exception e)
        {
            //Debug.LogError($"Process check failed: {e.Message}");
            QuitGame();
        }
        finally
        {
            if (snapshot != IntPtr.Zero)
            {
                CloseHandle(snapshot);
            }
        }
    }

    private void QuitGame()
    {
        if (isQuitting) return;
        isQuitting = true;

       // Debug.LogWarning("Application must be launched from Explorer.exe");

        try
        {
            Application.Quit();
        }
        catch
        {
            try
            {
                System.Environment.Exit(0);
            }
            catch
            {
                try
                {
                    var currentProc = System.Diagnostics.Process.GetCurrentProcess();
                    currentProc.Kill();
                }
                catch (Exception e)
                {
                   // Debug.LogError($"Failed to quit: {e.Message}");
                }
            }
        }
    }
}