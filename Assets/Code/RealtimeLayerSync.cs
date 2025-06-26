using UnityEngine;

public class RealtimeLayerSync : MonoBehaviour
{
    public GameObject sourceObject;
    public GameObject targetObject;

    private int previousLayer;

    // Start is called before the first frame update
    void Start()
    {
        if (sourceObject != null && targetObject != null)
        {
            previousLayer = sourceObject.layer;
            SetLayer(targetObject, previousLayer);
        }
        else
        {
            Debug.LogError("Source or Target Object is not set.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (sourceObject != null && targetObject != null)
        {
            int currentLayer = sourceObject.layer;
            if (currentLayer != previousLayer)
            {
                SetLayer(targetObject, currentLayer);
                previousLayer = currentLayer;
            }
        }
    }

    void SetLayer(GameObject obj, int newLayer)
    {
        if (obj == null)
        {
            return;
        }

        obj.layer = newLayer;

        // Optionally, set all child objects to the same layer
        foreach (Transform child in obj.transform)
        {
            if (child == null)
            {
                continue;
            }
            SetLayer(child.gameObject, newLayer);
        }
    }
}
