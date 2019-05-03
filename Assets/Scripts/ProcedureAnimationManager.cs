using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using LitJson;
using System.IO;

public class ProcedureAnimationManager : MonoBehaviour
{
    public int NowStep;
    List<ProcedurePartsModel> Steps;
    public bool isForwardPlay;
    public bool isRevertPlay;
    private float speed = 1;
    public UnityAction callback;
    BoxCollider[] boxs;

    JsonData assembleData;

    public float Speed
    {
        get
        {
            return speed;
        }

        set
        {
            speed = value;
            Steps[NowStep].SetSpeed(speed);
        }
    }

    void Awake()
    {
        Debug.Log(Application.streamingAssetsPath + "/json/partAssemble.json");
        string json = File.ReadAllText(Application.streamingAssetsPath + "/json/partAssemble.json");
        assembleData = JsonMapper.ToObject(json);
        assembleData = assembleData["assembleList"];
        Steps = new List<ProcedurePartsModel>();
        ProcedurePartsModel[] list = GetComponentsInChildren<ProcedurePartsModel>();
        for (int i = 0; i < list.Length; i++)
        {
            Steps.Add(list[i]);
        }
        boxs = this.GetComponentsInChildren<BoxCollider>();
        ThreeDTouchAnimationControl._Instance.infoCanvas.alpha = 1;
        ThreeDTouchAnimationControl._Instance.tipText.text = "";
        //ToggleGrabObjec(false);
    }

    private void OnEnable()
    {
        ThreeDTouchAnimationControl._Instance.infoCanvas.alpha = 1;
        ThreeDTouchAnimationControl._Instance.tipText.text = "";
    }

    private void OnDisable()
    {
        ThreeDTouchAnimationControl._Instance.infoCanvas.alpha = 0;
        ThreeDTouchAnimationControl._Instance.tipText.text = "";
        ResetAni();
        StopAni();
    }

    public void StartAni(UnityAction action)
    {
        if (!this.gameObject.activeSelf) return;
        ResetAni();
        isForwardPlay = true;
        isRevertPlay = false;
        NowStep = 0;
        Steps[0].StartAni(JumpNextStep, Speed);
        ThreeDTouchAnimationControl._Instance.tipText.text = assembleData[0]["partInfo"].ToString();
        Debug.Log("jump start");
        callback = action;
        ToggleGrabObjec(false);
    }

    public void RevertAni(UnityAction action)
    {
        if (!this.gameObject.activeSelf) return;
        FinishAni();
        isForwardPlay = false;
        isRevertPlay = true;
        NowStep = Steps.Count - 1;
        Steps[Steps.Count - 1].RevertAni(JumpLastStep, Speed);
        callback = action;
        ToggleGrabObjec(false);
    }

    void MoveStepChangeTip()
    {
        if (NowStep < assembleData.Count)
        {
            ThreeDTouchAnimationControl._Instance.infoCanvas.alpha = 1;
            ThreeDTouchAnimationControl._Instance.tipText.text = assembleData[NowStep]["partInfo"].ToString();
        }
        else
        {
            ThreeDTouchAnimationControl._Instance.tipText.text = "";
        }
    }

    public void JumpNextStep(int step)
    {
        Debug.Log("jump to :" + step);
        if (step <= Steps.Count - 1)
        {
            NowStep = step;
            Steps[NowStep].StartAni(JumpNextStep, Speed);
            MoveStepChangeTip();
        }
        else
        {
            ToggleGrabObjec(true);
            isForwardPlay = false;
            StopAni();
            ThreeDTouchAnimationControl._Instance.tipText.text = "";
        }
    }


    public void JumpLastStep(int step)
    {
        if (step >= 0)
        {
            NowStep = step;
            Steps[NowStep].RevertAni(JumpLastStep, Speed);
            MoveStepChangeTip();
        }
        else
        {
            isRevertPlay = false;
            StopAni();
            ThreeDTouchAnimationControl._Instance.tipText.text = "";
        }
    }

    public void StopAni()
    {
        for (int i = 0; i < Steps.Count; i++)
        {
            Steps[i].StopAni();
        }
        if (callback != null)
        {
            callback();
        }
        ThreeDTouchAnimationControl._Instance.tipText.text = "";
    }

    public void ResetAni()
    {
        NowStep = 0;
        for (int i = 0; i < Steps.Count; i++)
        {
            Steps[Steps.Count - 1 - i].StopAni();
            Steps[Steps.Count - 1 - i].Reset();
        }
        isForwardPlay = false;
        isRevertPlay = false;
        Speed = 1;
        ThreeDTouchAnimationControl._Instance.tipText.text = "";
    }

    public void FinishAni()
    {
        NowStep = Steps.Count - 1;
        for (int i = 0; i < Steps.Count; i++)
        {
            Steps[i].StopAni();
            Steps[i].Finish();
        }
        isForwardPlay = false;
        isRevertPlay = false;
        ToggleGrabObjec(true);
        ThreeDTouchAnimationControl._Instance.tipText.text = "";
    }

    public void PauseAni()
    {
        ToggleGrabObjec(true);
        Steps[NowStep].Pause();
        ThreeDTouchAnimationControl._Instance.tipText.text = "";
    }

    public void PlayAni()
    {
        ToggleGrabObjec(false);
        Steps[NowStep].Play();
        MoveStepChangeTip();
    }

    public void ToggleGrabObjec(bool isOn)
    {
        for (int i = 0; i < boxs.Length; i++)
        {
            boxs[i].enabled = isOn;
        }
        if (isOn)
        {
            for (int i = 0; i < boxs.Length; i++)
            {
                boxs[i].GetComponent<GrapObjectModel>().SetNowTransform();
            }
        }
    }
}
