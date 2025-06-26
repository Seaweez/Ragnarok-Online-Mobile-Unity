using UnityEngine;
using System.Collections.Generic;
using System;

namespace RO
{
    [SLua.CustomLuaClassAttribute]
    public class InputGesture : MonoBehaviour
    {
        public Action zoomInAction;
        public Action zoomOutAction;
        float catchDistance = 0;

        void Update()
        {
            // Detect touch gestures
            if (Input.touches.Length == 2)
            {
                Touch point1 = Input.touches[0];
                Touch point2 = Input.touches[1];
                if (point1.phase == TouchPhase.Began || point2.phase == TouchPhase.Began)
                {
                    catchDistance = Vector3.Distance(point1.position, point2.position);
                }
                else if (point1.phase == TouchPhase.Ended || point2.phase == TouchPhase.Ended)
                {
                    float currentDistance = Vector3.Distance(point1.position, point2.position);
                    if (currentDistance - catchDistance > 50)
                    {
                        zoomInAction?.Invoke();
                    }
                    else if (catchDistance - currentDistance > 50)
                    {
                        zoomOutAction?.Invoke();
                    }
                    catchDistance = 0;
                }
            }

            // Detect mouse scroll wheel
           float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll > 0f)
            {
                zoomInAction?.Invoke();
            }
            else if (scroll < 0f)
            {
                zoomOutAction?.Invoke();
            }
        }
    }
} // namespace RO
