using UnityEngine;

namespace RO
{
    public class AnyKeyDownScript : MonoBehaviour
    {
        private bool wasdEndCalled = false;

        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        void Update()
        {
            if (Camera.main == null)
            {
                // Debug.LogError("No main camera found.");
                return;
            }

            // Handle WASD input for 8-directional movement
            Vector3 wasdDir = new Vector3(Input.GetKey(KeyCode.A) ? -1 : Input.GetKey(KeyCode.D) ? 1 : 0,
                                          0,
                                          Input.GetKey(KeyCode.S) ? -1 : Input.GetKey(KeyCode.W) ? 1 : 0);

            if (wasdDir != Vector3.zero)
            {
                // Normalize the direction vector to ensure consistent speed in all directions
                wasdDir = Camera.main.transform.TransformDirection(wasdDir);
                wasdDir.y = 0; // Ensure that the character stays on the horizontal plane.
                wasdDir.Normalize();
                LuaLuancher.Me.Call("Input_JoyStick", wasdDir.x, wasdDir.y, wasdDir.z);
                wasdEndCalled = false;
            }
            else if (!wasdEndCalled)
            {
                // Call Lua function to stop movement if WASD input ends
                LuaLuancher.Me.Call("Input_JoyStickEnd");
                wasdEndCalled = true;
            }
        }
    }
}
