using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class ExplosionAnimationManager : MonoBehaviour
{
    public float[] timeLine;
    Dictionary<int, List<ExplosionPartsModel>> Steps;
    public bool isForwardPlay;
    public bool isRevertPlay;
    Sequence ctrl;
    private float speed=1;
    private bool isPlaying;
    UnityAction callback;
    BoxCollider[] boxs;

    public float Speed
    {
        get
        {
            return speed;
        }

        set
        {
            if (ctrl != null) {
                ctrl.timeScale = speed;
            }
            speed = value;
        }
    }

    public bool IsPlaying
    {
        get
        {
            return ctrl!=null;
        }
        
    }

    private void OnDisable()
    {
        ctrl = null;
        speed = 1;
    }

    void Start()
    {
        Steps = new Dictionary<int, List<ExplosionPartsModel>>();
        ExplosionPartsModel[] list = GetComponentsInChildren<ExplosionPartsModel>();
        for(int i = 0; i < timeLine.Length; i++)
        {
            Steps[i] = new List<ExplosionPartsModel>();
        }
        for (int i = 0; i < list.Length; i++)
        {
            int stepIndex = list[i].myStep;
            Steps[stepIndex].Add(list[i]);
        }
        boxs = this.GetComponentsInChildren<BoxCollider>();
        ToggleGrabObjec(false);
    }

    public void ChoosePlayAni(UnityAction action)
    {
        if (isForwardPlay == false && isRevertPlay == false)
        {
            StartAni();
        }
        else if (isForwardPlay == true && isRevertPlay == false)
        {
            RevertAni();
        }
        else if (isForwardPlay == false && isRevertPlay == true)
        {
            StartAni();
        }
        callback = action;
    }

    public void StartAni()
    {
        if (!this.gameObject.activeSelf) return;
        if (ctrl == null)
        {
            ToggleGrabObjec(false);
            ctrl = DOTween.Sequence();
            ctrl.timeScale = Speed;
            for (int i = 0; i < timeLine.Length; i++)
            {
                float time = timeLine[i];
                if (Steps.ContainsKey(i)) {
                    List<ExplosionPartsModel> list = Steps[i];
                    for (int ii = 0; ii < list.Count; ii++)
                    {
                        ExplosionPartsModel model = list[ii];
                        model.AddToForwardSequence(time, ref ctrl);
                    }
                }
            }
            ctrl.OnComplete(() => {
                ToggleGrabObjec(true);
                StopAni(); });
            ctrl.PlayForward();
            isForwardPlay = true;
            isRevertPlay = false;
        }
    }

    public void RevertAni()
    {
        if (!this.gameObject.activeSelf) return;
        if (ctrl == null)
        {
            ToggleGrabObjec(false);
            ctrl = DOTween.Sequence();
            ctrl.timeScale = Speed;
            for (int i = timeLine.Length - 1; i >= 0; i--)
            {
                float time = timeLine[timeLine.Length - 1] - timeLine[i];
                Debug.Log(time);
                List<ExplosionPartsModel> list = Steps[i];
                for (int ii = list.Count-1; ii >=0; ii--)
                {
                    ExplosionPartsModel model = list[ii];
                    model.AddToBackwardSequence(time, ref ctrl);
                }
            }
            ctrl.OnComplete(() => {
                ToggleGrabObjec(false);
                StopAni();
            });
            ctrl.PlayForward();
            isForwardPlay = false;
            isRevertPlay = true;
        }
    }
    

    public void StopAni()
    {
        Debug.Log("stop");
        ctrl = null;
        if (callback != null) {
            callback();
        }
    }


    public void PauseAni()
    {
        if (!this.gameObject.activeSelf) return;
        if (ctrl!=null&&ctrl.IsPlaying()) {
            ctrl.TogglePause();
            ToggleGrabObjec(true);
        }
    }

    public void PlayAni()
    {
        if (!this.gameObject.activeSelf) return;
        if (!ctrl.IsPlaying())
        {
            ctrl.TogglePause();
            ToggleGrabObjec(false);
        }
    }

    public void ResetAni()
    {
        StopAni();
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
