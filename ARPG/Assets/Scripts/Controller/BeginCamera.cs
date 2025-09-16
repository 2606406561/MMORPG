using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginCamera : MonoBehaviour
{
    private static BeginCamera instance;
    public static BeginCamera Instance => instance;
    public Vector3 v3;
    public Transform ts;
    public float speed = 0.3f;
    private Vector3 vector3;
    private float time;
    private bool ok;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        vector3 = transform.up;
    }

    // Update is called once per frame
    void Update()
    {
        if (ok) return;
        time += Time.deltaTime;
        if(time >= 0.5f)
        {
            time -= 0.5f;
            vector3 = Quaternion.AngleAxis(10, Vector3.up) * vector3;
        }
        transform.position = Vector3.Lerp(transform.position, vector3 + transform.position, speed);
    }
    public void SetPosition()
    {
        transform.position = v3;
        transform.rotation = Quaternion.LookRotation(ts.position - transform.position);
        ok = true;
    }
}
