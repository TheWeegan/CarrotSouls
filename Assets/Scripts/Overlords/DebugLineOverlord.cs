using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DebugLineOverlord
{
    public void DrawRectangle(GameObject gameObject, Vector3 targetPosition, float width, float length) {
        Vector3 back = targetPosition -  (gameObject.transform.forward * length);
        Vector3 front = targetPosition + (gameObject.transform.forward * length);
        Vector3 left = targetPosition -  (gameObject.transform.right * width);
        Vector3 right = targetPosition + (gameObject.transform.right * width);
        
        Debug.DrawLine(back, left, Color.white);
        Debug.DrawLine(left, front, Color.white);
        Debug.DrawLine(front, right, Color.white);
        Debug.DrawLine(right, back, Color.white);
    }

}
