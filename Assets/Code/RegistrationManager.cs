using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI; // If using UI components

namespace RO
{
    public class RegistrationManager : MonoBehaviour
    {
        public InputField reaccInput;
        public InputField repwdInput;
        public InputField recodeInput;
        public Text errorMessage; // Assuming you're displaying errors using a Text component

        // Call this method when your submit button is pressed
        public void RegisterCall()
        {
            string reacc = reaccInput.text;
            string repwd = repwdInput.text;
            string recode = recodeInput.text;

            if (!string.IsNullOrEmpty(reacc) && !string.IsNullOrEmpty(repwd) && !string.IsNullOrEmpty(recode))
            {
                StartCoroutine(RegisterUser(reacc, repwd, recode));
            }
            else
            {
                // Update this method to display errors as needed
                ErrorMsg("Please fill in all fields.");
               // LuaLuancher.Me.Call();
            }
        }

        private IEnumerator RegisterUser(string username, string password, string sls)
        {
            WWWForm form = new WWWForm();
            form.AddField("username", username);
            form.AddField("password", password);
            form.AddField("sls", sls);

            string registerUrl = "http://api.pinidea.online/function/check_register.php";
            using (UnityWebRequest www = UnityWebRequest.Post(registerUrl, form))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    ErrorMsg($"Registration failed. Please try again. Error: {www.error}");
                }
                else
                {
                    // Handle successful registration
                    // Assuming the server returns a simple success message or JSON
                    // You might need to parse the response depending on what the server sends back
                    ErrorMsg("Register Success! Login Now!");
                }
            }
        }

        private void ErrorMsg(string message)
        {
            if (errorMessage != null)
            {
                errorMessage.text = message;
            }
            else
            {
                Debug.LogError("Error message component not set.");
            }
        }
    }
}