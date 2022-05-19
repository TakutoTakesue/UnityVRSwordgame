using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseAction : MonoBehaviour
{

    Vector2 Rot = Vector2.zero;
    public Vector2 Bias = new Vector2(5.0f, 5.0f); //回転速度
    public float maxPitch = 60.0f; //仰角制限
    public float minPitch = -60.0f; //俯角制限

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        Rot.x = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * Bias.x;
        Rot.y += Input.GetAxis("Mouse Y") * Bias.y;
        Rot.y = Mathf.Clamp(Rot.y, minPitch, maxPitch);
        transform.localEulerAngles = new Vector3(-Rot.y, Rot.x, 0);
# endif
    }
}
