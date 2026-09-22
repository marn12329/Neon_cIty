using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // ตัวละครหรือ GameObject ที่กล้องจะติดตาม
    public float smoothSpeed = 0.125f; // ความเร็วในการติดตาม (ทำให้การเคลื่อนไหวราบรื่น)
    public Vector3 offset; // ระยะห่างระหว่างกล้องกับตัวละคร

    void LateUpdate()
    {
        if (target != null)
        {
            // คำนวณตำแหน่งใหม่ของกล้อง
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}