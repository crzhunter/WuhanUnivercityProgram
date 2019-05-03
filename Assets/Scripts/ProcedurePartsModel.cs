using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class ProcedurePartsModel : MonoBehaviour {
    public int myStep;
    public bool isSynchronize = false;
    public List<OnePart> parts;
    Sequence  ctrl;
    
    public void StartAni(UnityAction<int> callback,float timeScale=1) {
        if (ctrl == null)
        {
            ctrl = DOTween.Sequence() ;
            ctrl.timeScale = timeScale;
            for (int i = 0; i < parts.Count; i++)
            {
                OnePart one = parts[i];
                if (!isSynchronize)
                {
                    ctrl.Append(one.obj.transform.DOLocalMove(one.endPos, one.duration)).
                   Join(one.obj.transform.DOLocalRotate(one.endEular, one.duration, RotateMode.Fast));
                }
                else
                {
                    if (i == 0)
                    {
                        ctrl.Append(one.obj.transform.DOLocalMove(one.endPos, one.duration)).
                       Join(one.obj.transform.DOLocalRotate(one.endEular, one.duration, RotateMode.Fast));
                    }
                    else
                    {
                        ctrl.Join(one.obj.transform.DOLocalMove(one.endPos, one.duration)).
                       Join(one.obj.transform.DOLocalRotate(one.endEular, one.duration, RotateMode.Fast));
                    }
                }
            }
            ctrl.OnComplete(() => callback(myStep + 1));
            ctrl.PlayForward();
        }
    }

    public void RevertAni(UnityAction<int> callback, float timeScale = 1)
    {
       if (ctrl == null) {
            ctrl = DOTween.Sequence();
            ctrl.timeScale = timeScale;
            for (int i = 0; i < parts.Count; i++)
            {
                OnePart one = parts[parts.Count-1-i];
                if (!isSynchronize)
                {
                    ctrl.Append(one.obj.transform.DOLocalMove(one.startPos, one.duration)).
                   Join(one.obj.transform.DOLocalRotate(one.startEular, one.duration, RotateMode.Fast));
                }
                else {
                    if (i == 0)
                    {
                        ctrl.Append(one.obj.transform.DOLocalMove(one.startPos, one.duration)).
                       Join(one.obj.transform.DOLocalRotate(one.startEular, one.duration, RotateMode.Fast));
                    }
                    else
                    {
                        ctrl.Join(one.obj.transform.DOLocalMove(one.startPos, one.duration)).
                       Join(one.obj.transform.DOLocalRotate(one.startEular, one.duration, RotateMode.Fast));
                    }
                }
            }
            ctrl.OnComplete(() => callback(myStep - 1));
            ctrl.PlayForward();
       }
    }

    public void Pause() {
        if (ctrl != null) {
            if (ctrl.IsPlaying()) {
                ctrl.TogglePause();
            }
        }
    }

    public void Play()
    {
        if (ctrl != null)
        {
            if (!ctrl.IsPlaying()) {
                ctrl.TogglePause();
            }
        }
    }

    public void SetSpeed(float speed) {
        if (ctrl != null) {
            ctrl.timeScale = speed;
        }
    }

    public void StopAni() {
        if (ctrl != null)
        {
            ctrl.Kill();
            ctrl = null;
        }
    }

    public void Reset() {
        for (int i = 0; i < parts.Count; i++)
        {
            OnePart one = parts[i];
            one.obj.transform.localPosition = one.startPos;
            one.obj.transform.localEulerAngles = one.startEular;
        }
    }

    public void Finish()
    {
        for (int i = 0; i < parts.Count; i++)
        {
            OnePart one = parts[i];
            one.obj.transform.localPosition = one.endPos;
            one.obj.transform.localEulerAngles = one.endEular;
        }
    }

}
[System.Serializable]
public struct OnePart {
    public GameObject obj;
    public Vector3 startPos;
    public Vector3 endPos;
    public Vector3 startEular;
    public Vector3 endEular;
    public float duration;
    
}