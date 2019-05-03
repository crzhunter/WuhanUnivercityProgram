using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProcedureAnimationManager : MonoBehaviour {
    public int NowStep;
    List<ProcedurePartsModel> Steps;
    public bool isForwardPlay;
    public bool isRevertPlay;
    private float speed=1;
    public UnityAction callback;
    BoxCollider[] boxs;

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

    void Start() {
        Steps = new List<ProcedurePartsModel>();
        ProcedurePartsModel[] list = GetComponentsInChildren<ProcedurePartsModel>();
        for (int i = 0; i < list.Length; i++) {
            Steps.Add(list[i]);
        }
        boxs = this.GetComponentsInChildren<BoxCollider>();
        ToggleGrabObjec(false);
    }

    private void OnDisable()
    {
        ResetAni();
        StopAni();
    }

    public void StartAni(UnityAction action) {
        if (!this.gameObject.activeSelf) return;
        ResetAni();
        isForwardPlay = true;
        isRevertPlay = false;
        NowStep = 0;
        Steps[0].StartAni(JumpNextStep, Speed);
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
    

    public void JumpNextStep(int step)
    {
        Debug.Log("jump to :" + step);
        if (step <= Steps.Count - 1)
        {
            NowStep = step;
            Steps[NowStep].StartAni(JumpNextStep, Speed);
        }
        else
        {
            ToggleGrabObjec(true);
             isForwardPlay = false;
            StopAni();
        }
    }


    public void JumpLastStep(int step)
    {
        if (step >= 0)
        {
            NowStep = step;
            Steps[NowStep].RevertAni(JumpLastStep, Speed);
        }
        else {
            isRevertPlay = false;
            StopAni();
        }
    }

    public void StopAni() {
        for (int i = 0; i < Steps.Count; i++)
        {
            Steps[i].StopAni();
        }
        if (callback != null) {
            callback();
        }
    }

    public void ResetAni() {
        NowStep = 0;
        for (int i = 0; i < Steps.Count; i++)
        {
            Steps[Steps.Count - 1 - i].StopAni();
            Steps[Steps.Count - 1 - i].Reset();
        }
        isForwardPlay = false;
        isRevertPlay = false;
        Speed = 1;
    }

    public void FinishAni()
    {
        NowStep = Steps.Count-1;
        for (int i = 0; i < Steps.Count; i++)
        {
            Steps[i].StopAni();
            Steps[i].Finish();
        }
        isForwardPlay = false;
        isRevertPlay = false;
        ToggleGrabObjec(true);
    }

    public void PauseAni() {
        ToggleGrabObjec(true);
         Steps[NowStep].Pause();
    }

    public void PlayAni()
    {
        ToggleGrabObjec(false);
          Steps[NowStep].Play();
    }

    public void ToggleGrabObjec(bool isOn) {
        for (int i = 0; i < boxs.Length; i++)
        {
            boxs[i].enabled = isOn;
        }
        if (isOn) {
            for (int i = 0; i < boxs.Length; i++)
            {
                boxs[i].GetComponent<GrapObjectModel>().SetNowTransform();
            }
        }
    }
}
