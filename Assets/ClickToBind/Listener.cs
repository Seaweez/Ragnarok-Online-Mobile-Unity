using UnityEngine;

public class Listener : MonoBehaviour {

    public GameObject box1;
    public GameObject box2;
    public GameObject box3;
    public GameObject box4;
    public GameObject box5;
    public GameObject box6;

    void Update() {

        if(KeyBindingManager.GetKeyDown(KeyAction.up)) {
            box1.SetActive(!box1.activeSelf);
        }

        if(KeyBindingManager.GetKeyDown(KeyAction.down)) {
            box2.SetActive(!box2.activeSelf);
        }

        if(KeyBindingManager.GetKeyDown(KeyAction.left)) {
            box3.SetActive(!box3.activeSelf);
        }
                
        if(KeyBindingManager.GetKeyDown(KeyAction.right)) {
            box4.SetActive(!box4.activeSelf);
        }

        // สำหรับ box5 และ box6 คุณต้องการกำหนดคีย์ใหม่
        // สมมติว่ามี KeyAction.forward และ KeyAction.backward
        if(KeyBindingManager.GetKeyDown(KeyAction.forward)) {
            box5.SetActive(!box5.activeSelf);
        }

        if(KeyBindingManager.GetKeyDown(KeyAction.backward)) {
            box6.SetActive(!box6.activeSelf);
        }
    }
}
