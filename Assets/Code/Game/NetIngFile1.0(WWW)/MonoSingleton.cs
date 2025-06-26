using UnityEngine;
using System.Collections;

public abstract class Singleton<T> where T : class, new()
{
    protected static T s_instance = null;
    public static T ins
    {
        get
        {
            if (s_instance == null)
                s_instance = new T();
            return s_instance;
        }
    }

    public static T GetInstance()
    {
        return ins;
    }

    public static bool HasInstance { get { return (s_instance != null); } }

    public static void ResetInstance()
    {
        s_instance = null;
    }
}

public abstract class MonoSingleton<T> : MonoBehaviour where T : Component
{
    protected static T s_instance = null;
    public static T ins
    {
        get
        {
            if (s_instance == null)
                CreateSingleton();
            return s_instance;
        }
    }

    public static T GetInstance()
    {
        return ins;
    }

    public static T GetInstance(string prefabPath)
    {
        if (s_instance == null)
            CreateSingleton(prefabPath);
        return s_instance;
    }

    public static bool HasInstance { get { return (s_instance != null); } }

    private static Transform GetRootTrans()
    {
        GameObject root = GameObject.FindGameObjectWithTag("MonoSingleton");
        if (root == null)
        {
            root = new GameObject("MonoSingleton");
            //root.AddComponent<DebugMono>();
            root.tag = "MonoSingleton";
            DontDestroyOnLoad(root);
        }
        return root.transform;
    }

    protected static void CreateSingleton()
    {
        GameObject singleton = new GameObject(typeof(T).Name);
        singleton.transform.parent = GetRootTrans();
        s_instance = singleton.AddComponent<T>();
    }

    protected static void CreateSingleton(string prefabPath)
    {
        GameObject prefab = Resources.Load(prefabPath, typeof(GameObject)) as GameObject;
        if (prefab == null)
        {
            CreateSingleton();
            return;
        }
        Transform trans = MonoUtil.CreatePrefab(prefab, typeof(T).Name, GetRootTrans());
        s_instance = trans.gameObject.GetComponent<T>();
        if (s_instance == null)
        {
            s_instance = trans.gameObject.AddComponent<T>();
        }
    }

    private Transform m_cachedTrans;
    public Transform CachedTrans
    {
        get
        {
            if (m_cachedTrans == null)
                m_cachedTrans = transform;
            return m_cachedTrans;
        }
    }

    private void Awake()
    {
        if (s_instance != null)
        {
            if (this != s_instance)
                Destroy(this.gameObject);
            return;
        }
        s_instance = this as T;
        if (CachedTrans.root.tag != "MonoSingleton")
        {
            CachedTrans.parent = GetRootTrans();
            gameObject.name = typeof(T).Name;
        }
        OnAwake();
    }

    protected virtual void OnAwake()
    {
    }
}
