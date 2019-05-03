using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapObjectModel : MonoBehaviour
{
    public Vector3 pos;
    public Vector3 rotation;

    public int partNum=-1;
    // Use this for initialization
    void Start()
    {
        pos = transform.localPosition;
        rotation = transform.localEulerAngles;
    }

    public void SetNowTransform() {
        pos = transform.localPosition;
        rotation = transform.localEulerAngles;
    }

    //public void ReBuild()
    //{
    //    transform.localPosition = pos;
    //    transform.localEulerAngles = rotation;
    //}

    // Update is called once per frame
    void Update()
    {

    }
}