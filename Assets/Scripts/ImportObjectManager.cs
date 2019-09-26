using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImportObjectManager : MonoBehaviour
{
    public float angleSpeed;
    private float nowSpeed;
    public float[] angleSpeeds;
    public RotatePartsModel[] list;
    public bool isStart;

    public Text tip;
    public Button import;
    public InputField inputSpeed;
    public Text outSpeed;
    private float speed = 1;

    CanvasGroup tipCanvas;
    Text tipTitle;
    Text tipText;

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
            SetInputSpeed();
            SetOutSpeed();
        }
    }

    public float NowSpeed { get => nowSpeed; set => nowSpeed = value; }

    void Awake()
    {
        tipCanvas = ThreeDTouchAnimationControl._Instance.infoCanvas;
        tipTitle = ThreeDTouchAnimationControl._Instance.tipTitle;
        tipText = ThreeDTouchAnimationControl._Instance.tipText;
        list = GetComponentsInChildren<RotatePartsModel>();
        nowSpeed = angleSpeed;
        CalculateSpeed();
        InputField.SubmitEvent ev = inputSpeed.onEndEdit;
        ev.AddListener((string value) => {
            nowSpeed = float.Parse(value);
            CalculateSpeed();
            for (int i = 0; i < list.Length; i++)
            {
                RotatePartsModel model = list[i];
                model.Speed = angleSpeeds[model.level];
            }
            SetOutSpeed();
        });
    }

    void SetOutSpeed()
    {
        outSpeed.text = angleSpeeds[5].ToString("F2") + "°/s";
        tipTitle.text = "实时速度";
        tipText.text = "输入速度：" + nowSpeed.ToString("F2") + "°/s \n\n" + "输出速度：" + angleSpeeds[5].ToString("F2") + "°/s";
    }

    void SetInputSpeed()
    {
        inputSpeed.text = nowSpeed.ToString("F2");
    }

    private void OnEnable()
    {
        tipCanvas.alpha = 1;
        tipText.text = "";
        tipTitle.text = "";
        tip.gameObject.SetActive(true);
        import.gameObject.SetActive(true);
        inputSpeed.gameObject.SetActive(true);
        outSpeed.gameObject.SetActive(true);

    }
    private void OnDisable()
    {
        tipCanvas.alpha = 0;
        tip.gameObject.SetActive(false);
        import.gameObject.SetActive(false);
        inputSpeed.gameObject.SetActive(false);
        outSpeed.gameObject.SetActive(false);

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

    void CalculateSpeed()
    {
        angleSpeeds = new float[6];
        angleSpeeds[0] = nowSpeed;
        angleSpeeds[1] = nowSpeed * 0.8f;
        angleSpeeds[2] = angleSpeeds[0] * (26f / 105f);
        angleSpeeds[3] = angleSpeeds[2] * 0.8f;
        angleSpeeds[4] = angleSpeeds[2] * (26f / 141f);
        angleSpeeds[5] = angleSpeeds[4] * 0.8f;
    }

    public void StartRotate()
    {
        if (!this.gameObject.activeSelf) return;
        isStart = true;
        for (int i = 0; i < list.Length; i++)
        {
            RotatePartsModel model = list[i];
            model.StartDoRotate(angleSpeeds[model.level]);
        }
        SetOutSpeed();
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


    public void PauseRotate()
    {
        if (isStart)
        {
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
        if (!isStart)
        {
            isStart = true;
            for (int i = 0; i < list.Length; i++)
            {
                RotatePartsModel model = list[i];
                model.PlayTw();
            }
        }
    }

}
