using UnityEngine;

public class CheckInvalidWorldAABB : MonoBehaviour
{
    void Update()
    {
        // Get all active GameObjects in the scene
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            // Check for both Renderer and Collider
            Renderer renderer = obj.GetComponent<Renderer>();
            Collider collider = obj.GetComponent<Collider>();

            if (renderer != null)
            {
                // Check renderer bounds
                Bounds bounds = renderer.bounds;
                if (IsInvalidAABB(bounds))
                {
                    Debug.LogError($"Invalid AABB detected for object: {obj.name} (Renderer) with bounds {bounds}", obj);
                }
            }
            else if (collider != null)
            {
                // Check collider bounds
                Bounds bounds = collider.bounds;
                if (IsInvalidAABB(bounds))
                {
                    Debug.LogError($"Invalid AABB detected for object: {obj.name} (Collider) with bounds {bounds}", obj);
                }
            }
            else
            {
                // Check position if no collider or renderer
                Vector3 position = obj.transform.position;
                if (IsInvalidVector(position))
                {
                    Debug.LogError($"Invalid position detected for object: {obj.name} with position {position}", obj);
                }
            }
        }
    }

    // Helper method to check if the bounds are invalid
    bool IsInvalidAABB(Bounds bounds)
    {
        // Check if any bounds value is NaN or infinity
        return float.IsNaN(bounds.min.x) || float.IsNaN(bounds.min.y) || float.IsNaN(bounds.min.z) ||
               float.IsNaN(bounds.max.x) || float.IsNaN(bounds.max.y) || float.IsNaN(bounds.max.z) ||
               float.IsInfinity(bounds.min.x) || float.IsInfinity(bounds.min.y) || float.IsInfinity(bounds.min.z) ||
               float.IsInfinity(bounds.max.x) || float.IsInfinity(bounds.max.y) || float.IsInfinity(bounds.max.z);
    }

    // Helper method to check if the position is invalid
    bool IsInvalidVector(Vector3 position)
    {
        // Check if any position value is NaN or infinity
        return float.IsNaN(position.x) || float.IsNaN(position.y) || float.IsNaN(position.z) ||
               float.IsInfinity(position.x) || float.IsInfinity(position.y) || float.IsInfinity(position.z);
    }
}
