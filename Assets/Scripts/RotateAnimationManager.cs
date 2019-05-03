using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAnimationManager : MonoBehaviour {
    public float angleSpeed;
    public float nowSpeed;
    public  float[] angleSpeeds;
    public  RotatePartsModel[] list;
    public bool isStart;

    private float speed=1;

    public float Speed
    {
        get
        {
            return speed;
        }

        set
        {
            speed = value;
            nowSpeed = angleSpeed * speed;
            CalculateSpeed();
            for (int i = 0; i < list.Length; i++)
            {
                RotatePartsModel model = list[i];
                model.Speed = angleSpeeds[model.level];
            }
        }
    }

    void Start() {
        list = GetComponentsInChildren<RotatePartsModel>();
        nowSpeed = angleSpeed;
        CalculateSpeed();
    }

    void CalculateSpeed() {
        angleSpeeds = new float[6];
        angleSpeeds[0] = nowSpeed;
        angleSpeeds[1] = nowSpeed * 0.8f;
        angleSpeeds[2] = angleSpeeds[0] * (26f / 105f);
        angleSpeeds[3] = angleSpeeds[2] * 0.8f;
        angleSpeeds[4] = angleSpeeds[2] * (26f / 141f);
        angleSpeeds[5] = angleSpeeds[4] * 0.8f;
    }

    public void StartRotate() {
        if (!this.gameObject.activeSelf) return;
        isStart = true;
        for (int i = 0; i < list.Length; i++) {
            RotatePartsModel model = list[i];
            model.StartDoRotate(angleSpeeds[model.level]);
        }
    }

    public void ResetRotate()
    {
        isStart = false;
        for (int i = 0; i < list.Length; i++)
        {
            RotatePartsModel model = list[i];
            model.PauseTw();
        }
    }


    public void PauseRotate() {
        if (isStart) {
            isStart = false;
            for (int i = 0; i < list.Length; i++)
            {
                RotatePartsModel model = list[i];
                model.PauseTw();
            }
        }
     
    }

    public void PlayRotate()
    {
        if (!isStart) {
            isStart = true;
            for (int i = 0; i < list.Length; i++)
            {
                RotatePartsModel model = list[i];
                model.PlayTw();
            }
        }
    }

    private void OnDisable()
    {
        PauseRotate();
        speed = 1;
        nowSpeed = angleSpeed * speed;
        CalculateSpeed();
        for (int i = 0; i < list.Length; i++)
        {
            RotatePartsModel model = list[i];
            model.Speed = angleSpeeds[model.level];
        }
    }
}
