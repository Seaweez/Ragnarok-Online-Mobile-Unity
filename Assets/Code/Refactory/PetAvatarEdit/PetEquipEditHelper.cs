using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

[SLua.CustomLuaClassAttribute]
public class PetEquipEditHelper : MonoBehaviour
{
    public enum Mode
    {
        Move = 1,
        Position,
        Rotation,
        Scale
    }

    int nCameraMoveSpeed = 5;
    int nCameraRotateSpeed = 30;
    int nDragEquipMoveSpeed = 1;
    int nDragEquipRotateSpeed = 15;
    int nDragLabelMoveSpeed = 1;
    int nDragLabelRotateSpeed = 15;

    public Camera camera3D;
    public Transform tsfEditHelper;
    public Transform tsfModeBtnsParent;
    public Transform tsfModelPoint;
    public Transform tsfViewControl;
    public float fStandardCameraDistance; //helper大小为1时的镜头距离
    public Vector3 vecCameraFocusDistance;

    public GameObject objTarget;

    GameObject[] objEditHelpers = new GameObject[3];
    GameObject[] objEditModeSelect = new GameObject[4];
    Camera cameraUI;
    Action<int> cbOnDrag;
    Action<int> cbOnDragOver;
    Mode curMode = Mode.Position;
    bool bDisableScreenMove = false;
    float fLastDiatance;

    Transform tsfUselessObjs;
    Transform tsfUICamera;
    GameObject objUIRoot;
    GameObject objToolsView;

    private void Start()
    {
        objUIRoot = GameObject.Find("UIRoot");
        tsfUICamera = Instantiate(objUIRoot.transform.Find("Camera").gameObject).transform;
        tsfUICamera.parent = GetComponentInParent<UIRoot>().transform;
        for (int i = 0, max = tsfUICamera.childCount; i < max; ++i)
            tsfUICamera.GetChild(i).gameObject.SetActive(false);
        Transform parent = transform;
        while (parent.parent) parent = parent.parent;
        if (!tsfUselessObjs) tsfUselessObjs = new GameObject("UselessObjs").transform;
        GameObject[] objs = SceneManager.GetActiveScene().GetRootGameObjects();
        for (int i = 0, max = objs.Length; i < max; ++i)
            if (objs[i] != parent.gameObject)
                objs[i].transform.parent = tsfUselessObjs;
        tsfUselessObjs.gameObject.SetActive(false);
        objUIRoot.SetActive(false);
        (objToolsView = GameObject.Find("ToolsView(Clone)")).SetActive(false);

        objEditHelpers[0] = tsfEditHelper.Find("Position").gameObject;
        objEditHelpers[1] = tsfEditHelper.Find("Rotation").gameObject;
        objEditHelpers[2] = tsfEditHelper.Find("Scale").gameObject;

        objEditModeSelect[0] = tsfModeBtnsParent.Find("BtnMove/Select").gameObject;
        objEditModeSelect[1] = tsfModeBtnsParent.Find("BtnPos/Select").gameObject;
        objEditModeSelect[2] = tsfModeBtnsParent.Find("BtnRotate/Select").gameObject;
        objEditModeSelect[3] = tsfModeBtnsParent.Find("BtnScale/Select").gameObject;

        cameraUI = FindObjectOfType<UICamera>().GetComponent<Camera>();
        SetEditMode((int)Mode.Move);
    }

    public void SetCallback(Action<int> onDrag, Action<int> onDragOver)
    {
        cbOnDrag = onDrag;
        cbOnDragOver = onDragOver;
    }

    public void SetEditMode(int mode)
    {
        int nCurMode = (int)curMode;
        if ((mode = Mathf.Clamp(mode, 1, 4)) == nCurMode) return;
        if (nCurMode > 1) objEditHelpers[nCurMode - 2].SetActive(false);
        if (mode > 1) objEditHelpers[mode - 2].SetActive(true);
        objEditModeSelect[nCurMode - 1].SetActive(false);
        objEditModeSelect[mode - 1].SetActive(true);
        curMode = (Mode)mode;
    }

    public void SetEditTarget(GameObject obj)
    {
        if (objTarget == obj) return;
        objTarget = obj;
    }

    public void FocusOnEquipObj()
    {
        if (!objTarget) return;
        camera3D.transform.position = objTarget.transform.position + vecCameraFocusDistance;
        camera3D.transform.LookAt(objTarget.transform);
        tsfEditHelper.localScale = new Vector3(0.7f, 0.7f, 0.7f);
    }

    private void Update()
    {
        tsfEditHelper.gameObject.SetActive(objTarget && Application.isFocused);
        if (objTarget)
        {
            tsfEditHelper.position = objTarget.transform.position;
            tsfEditHelper.rotation = objTarget.transform.rotation;
            float distance = Vector3.Distance(camera3D.transform.position, objTarget.transform.position);
            if (distance != fLastDiatance)
            {
                fLastDiatance = distance;
                float size = distance / fStandardCameraDistance;
                tsfEditHelper.transform.localScale = new Vector3(size, size, size);
            }
        }
        tsfViewControl.transform.rotation = camera3D.transform.rotation;

        if (Input.GetKeyDown(KeyCode.Q))
            SetEditMode((int)Mode.Move);
        else if (Input.GetKeyDown(KeyCode.W))
            SetEditMode((int)Mode.Position);
        else if (Input.GetKeyDown(KeyCode.E))
            SetEditMode((int)Mode.Rotation);
        else if (Input.GetKeyDown(KeyCode.R))
            SetEditMode((int)Mode.Scale);

        //Process Camera Move
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            float moveDis = scroll * Time.deltaTime * nCameraMoveSpeed;
            Vector3 vec = camera3D.transform.localPosition;
            vec += camera3D.transform.forward * moveDis;
            camera3D.transform.localPosition = vec;
        }
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            Ray ray = cameraUI.ScreenPointToRay(Input.mousePosition);
            bDisableScreenMove = Physics.Raycast(ray);
        }
        if (!Input.GetMouseButton(0) && !Input.GetMouseButton(1))
            bDisableScreenMove = false;
        if (bDisableScreenMove) return;

        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");
        if (curMode == Mode.Move && Input.GetMouseButton(0))
        {
            Vector3 vec = camera3D.transform.localPosition;
            vec -= camera3D.transform.right * x * Time.deltaTime * nCameraMoveSpeed;
            vec -= camera3D.transform.up * y * Time.deltaTime * nCameraMoveSpeed;
            camera3D.transform.localPosition = vec;
            return;
        }
        if (Input.GetMouseButton(1))
        {
            Vector3 vec = camera3D.transform.localEulerAngles;
            vec.y += x * Time.deltaTime * nCameraRotateSpeed;
            vec.x -= y * Time.deltaTime * nCameraRotateSpeed;
            camera3D.transform.localEulerAngles = vec;
            return;
        }
    }

    //Drag On Helper
    public void OnDrag(EquipMouseOverOperate.Axis axis)
    {
        if (!objTarget) return;
        Vector3 dir = Vector3.zero, vec = Vector3.zero;
        switch (curMode)
        {
            case Mode.Position:
                vec = objTarget.transform.localPosition;
                break;
            case Mode.Rotation:
                vec = objTarget.transform.localEulerAngles;
                break;
            case Mode.Scale:
                vec = objTarget.transform.localScale;
                break;
        }
        float x = Input.GetAxis("Mouse X"), y = Input.GetAxis("Mouse Y");
        Vector2 tar, origin = camera3D.WorldToScreenPoint(objTarget.transform.position);
        float speed = curMode == Mode.Rotation ? nDragEquipRotateSpeed : nDragEquipMoveSpeed;
        switch (axis)
        {
            case EquipMouseOverOperate.Axis.X:
                dir = curMode == Mode.Rotation ? objTarget.transform.forward : objTarget.transform.right;
                tar = camera3D.WorldToScreenPoint(objTarget.transform.position + dir);
                dir = tar - origin;
                if (x * dir.x != 0)
                    vec.x += Mathf.Abs(x) * Mathf.Sign(x * dir.x) * Time.deltaTime * speed;
                if (y * dir.y != 0)
                    vec.x += Mathf.Abs(y) * Mathf.Sign(y * dir.y) * Time.deltaTime * speed;
                break;
            case EquipMouseOverOperate.Axis.Y:
                dir = curMode == Mode.Rotation ? objTarget.transform.right : objTarget.transform.up;
                tar = camera3D.WorldToScreenPoint(objTarget.transform.position + dir);
                dir = tar - origin;
                if (x * dir.x != 0)
                    vec.y += Mathf.Abs(x) * Mathf.Sign(x * dir.x) * Time.deltaTime * speed;
                if (y * dir.y != 0)
                    vec.y += Mathf.Abs(y) * Mathf.Sign(y * dir.y) * Time.deltaTime * speed;
                break;
            case EquipMouseOverOperate.Axis.Z:
                dir = curMode == Mode.Rotation ? objTarget.transform.up : objTarget.transform.forward;
                tar = camera3D.WorldToScreenPoint(objTarget.transform.position + dir);
                dir = tar - origin;
                if (x * dir.x != 0)
                    vec.z += Mathf.Abs(x) * Mathf.Sign(x * dir.x) * Time.deltaTime * speed;
                if (y * dir.y != 0)
                    vec.z += Mathf.Abs(y) * Mathf.Sign(y * dir.y) * Time.deltaTime * speed;
                break;
        }
        switch (curMode)
        {
            case Mode.Position:
                objTarget.transform.localPosition = vec;
                break;
            case Mode.Rotation:
                objTarget.transform.localEulerAngles = vec;
                break;
            case Mode.Scale:
                objTarget.transform.localScale = vec;
                break;
        }
        if (cbOnDrag != null) cbOnDrag((int)curMode);
    }


    public void OnDragOver()
    {
        if (cbOnDragOver != null) cbOnDragOver((int)curMode);
    }

    //Drag On Label
    public void OnLabelDrag(Mode mode, EquipDragOnLabel.Axis axis)
    {
        if (!objTarget) return;
        float x = Input.GetAxis("Mouse X"), change = 0;
        Vector3 vec;
        switch (mode)
        {
            case Mode.Position:
                vec = objTarget.transform.localPosition;
                change = x * Time.deltaTime * nDragLabelMoveSpeed;
                objTarget.transform.localPosition = CalVector(axis, vec, change);
                break;
            case Mode.Rotation:
                vec = objTarget.transform.localEulerAngles;
                change = x * Time.deltaTime * nDragLabelRotateSpeed;
                objTarget.transform.localEulerAngles = CalVector(axis, vec, change);
                break;
            case Mode.Scale:
                vec = objTarget.transform.localScale;
                change = x * Time.deltaTime * nDragLabelMoveSpeed;
                objTarget.transform.localScale = CalVector(axis, vec, change);
                break;
        }
        if (cbOnDrag != null) cbOnDrag((int)mode);
    }

    Vector3 CalVector(EquipDragOnLabel.Axis axis, Vector3 vec, float change)
    {
        switch (axis)
        {
            case EquipDragOnLabel.Axis.X:
                vec.x += change;
                break;
            case EquipDragOnLabel.Axis.Y:
                vec.y += change;
                break;
            case EquipDragOnLabel.Axis.Z:
                vec.z += change;
                break;
        }
        return vec;
    }

    public void OnLabelDragOver(Mode mode)
    {
        if (cbOnDragOver != null) cbOnDragOver((int)mode);
    }

    public void OnClickViewControl(string name)
    {
        Vector3 pos = tsfModelPoint.transform.position;
        float fDistance = Vector3.Distance(camera3D.transform.position, pos);
        switch (name)
        {
            case "X":
                pos.x += fDistance;
                break;
            case "-X":
                pos.x -= fDistance;
                break;
            case "Y":
                pos.y += fDistance;
                break;
            case "-Y":
                pos.y -= fDistance;
                break;
            case "Z":
                pos.z -= fDistance;
                break;
            case "-Z":
                pos.z += fDistance;
                break;
        }
        camera3D.transform.position = pos;
        camera3D.transform.LookAt(tsfModelPoint);
    }

    public void Exit()
    {
        tsfUselessObjs.DetachChildren();
        objUIRoot.SetActive(true);
        objToolsView.SetActive(true);
        Destroy(tsfUICamera.gameObject);
        Destroy(tsfUselessObjs.gameObject);
    }
}
