using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ExplosionPartsModel : MonoBehaviour {
    public int myStep;
    // Use this for initialization
    public List<OnePart> parts;

    public void AddToForwardSequence(float timeline,ref Sequence ctrl) {
        //  ctrl.Insert
        for (int i = 0; i < parts.Count; i++)
        {
            OnePart one = parts[i];
            ctrl.Insert(timeline, one.obj.transform.DOLocalMove(one.endPos, one.duration));
            if (one.startEular != one.endEular) {
                ctrl.Insert(timeline, one.obj.transform.DOLocalRotate(one.endEular, one.duration, RotateMode.Fast));
            }
            Debug.Log(timeline);
        }
    }
    public void AddToBackwardSequence(float timeline, ref Sequence ctrl)
    {
        Debug.Log(timeline);
        for (int i = 0; i < parts.Count; i++)
        {
            OnePart one = parts[i];
            ctrl.Insert(timeline, one.obj.transform.DOLocalMove(one.startPos, one.duration));
            if (one.startEular != one.endEular)
            {
                ctrl.Insert(timeline, one.obj.transform.DOLocalRotate(one.startPos, one.duration, RotateMode.Fast));
            }
        }
    }
}
