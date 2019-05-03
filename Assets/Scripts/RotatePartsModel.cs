using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RotatePartsModel : MonoBehaviour {
    public int level;
    public bool reverse;
    bool dorotate;
    [HideInInspector]
    private float speed;
    public AnimationCurve ac;
    float tempSpeed;
    float time;

    public float Speed
    {
        get
        {
            return speed;
        }

        set
        {
            speed = value;
        }
    }

    public void StartDoRotate(float angleSpeed)
    {
        time = 0;
        dorotate = true;
        Speed = angleSpeed;
    }

    public void PauseTw()
    {
        time = 0;
        if (dorotate == true) {
            dorotate = false;
        }
    }

    public void PlayTw()
    {
        time = 0;
        if (dorotate == false)
        {
            dorotate = true;
        }
    }

    private void Update()
    {
        if (dorotate)
        {
            time += Time.deltaTime;
            tempSpeed = Speed * ac.Evaluate(time);
            if (!reverse)
            {
                this.transform.Rotate(Vector3.up, tempSpeed * Time.deltaTime, Space.Self);
            }
            else
            {
                this.transform.Rotate(Vector3.down, tempSpeed * Time.deltaTime, Space.Self);
            }
        }
        else {
            time += Time.deltaTime;
            tempSpeed = Speed-Speed * ac.Evaluate(time);
            if (!reverse)
            {
                this.transform.Rotate(Vector3.up, tempSpeed * Time.deltaTime, Space.Self);
            }
            else
            {
                this.transform.Rotate(Vector3.down, tempSpeed * Time.deltaTime, Space.Self);
            }
        }
    }
}
