using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using DG.Tweening;
using UnityEngine.UI;
using LitJson;
using System.IO;

public class ThreeDTouchAnimationControl : MonoBehaviour {
    public ProcedureAnimationManager am;
    public ExplosionAnimationManager em;
    public RotateAnimationManager rm;
    public VRTK_ControllerEvents leftHand;
    public VRTK_ControllerEvents RightHand;
    public VRTK_InteractGrab grabHand;
    public int startSpeedIndex;
    public float[] speedScales;

    public GameObject play;
    public GameObject pause;
    public GameObject modeObj;
    public GameObject runObj;

    public CanvasGroup infoCanvas;

    public Text tipText;

    public int startMode = 0;
    public int startIndex = 0;
    int resetTime = 0;
    
    JsonData partInfoData;
    // Use this for initialization
    void Start () {
        leftHand.GetComponent<VRTK_InteractTouch>().ControllerTouchInteractableObject += ThreeDTouchAnimationControl_ControllerTouchInteractableObject;
        leftHand.GetComponent<VRTK_InteractTouch>().ControllerUntouchInteractableObject += ThreeDTouchAnimationControl_ControllerUntouchInteractableObject;
        RightHand.GetComponent<VRTK_InteractTouch>().ControllerTouchInteractableObject += ThreeDTouchAnimationControl_ControllerTouchInteractableObject;
        RightHand.GetComponent<VRTK_InteractTouch>().ControllerUntouchInteractableObject += ThreeDTouchAnimationControl_ControllerUntouchInteractableObject;
        grabHand.ControllerUngrabInteractableObject += GrabHand_ControllerUngrabInteractableObject;
        grabHand.ControllerGrabInteractableObject +=        GrabHand_ControllergrabInteractableObject;
JsonParse();
    }

    public void JsonParse(){
        string partInfoJson = File.ReadAllText(Application.dataPath+"/json/partInfo.json");
       partInfoData= JsonMapper.ToObject(partInfoJson);
    }

    private void GrabHand_ControllerUngrabInteractableObject(object sender, ObjectInteractEventArgs e)
    {
        Debug.Log(e.target.gameObject.name);
        Tweener tw= DOTween.To(()=>infoCanvas.alpha,x=>infoCanvas.alpha=x,0,1);
        tw.SetDelay(1f);
        e.target.GetComponent<Rigidbody>().isKinematic = false;
        Debug.Log(e.target.gameObject.name);
        GrapObjectModel model = e.target.GetComponent<GrapObjectModel>();
        Sequence ctrl = DOTween.Sequence();
        ctrl.OnStart(() =>
        {
            e.target.GetComponent<Rigidbody>().isKinematic = true;
        });
        ctrl.AppendInterval(2f);
        ctrl.Append(model.transform.DOLocalMove(model.pos, 1.5f));
        ctrl.Join(model.transform.DOLocalRotate(model.rotation, 1.5f));
        ctrl.PlayForward();

    }

    private void GrabHand_ControllergrabInteractableObject(object sender, ObjectInteractEventArgs e)
    {
        Debug.Log(e.target.gameObject.name);
        Pause();
        GrapObjectModel model = e.target.GetComponent<GrapObjectModel>();
        int partNum = model.partNum;
        if(partInfoData==null) return;
        JsonData list = partInfoData["partList"];
        if(list.IsArray){
            foreach(JsonData part in list){
                if(int.Parse( part["partNum"].ToString())==partNum){
                    string partInfo = part["partInfo"].ToString();
                    tipText.text = partInfo;
                    if(infoCanvas.alpha<1){
                        Tweener tw= DOTween.To(()=>infoCanvas.alpha,x=>infoCanvas.alpha=x,1,1);
                    }
                    break;
                }
            }
        }
    }

    private void ThreeDTouchAnimationControl_ControllerUntouchInteractableObject(object sender, ObjectInteractEventArgs e)
    {
        Debug.Log(e.target.name);
        ControlModel model = e.target.GetComponent<ControlModel>();
        if (model != null)
        {
            ChooseControl(model.MYMode, model);
        }
    }

    private void ThreeDTouchAnimationControl_ControllerTouchInteractableObject(object sender, ObjectInteractEventArgs e)
    {
        if (e.target.GetComponent<GrapObjectModel>()) return;
        Tweener tw = e.target.transform.DOLocalMoveY(e.target.transform.localPosition.y - 0.01f, 0.2f);
        tw.OnComplete(() =>
        {
            e.target.transform.localPosition = new Vector3(e.target.transform.localPosition.x, 0, e.target.transform.localPosition.z);
            e.target.transform.DOLocalMoveY(e.target.transform.localPosition.y + 0.01f, 0.2f);
        });
        Debug.Log(e.target.name);
       
    }

    void ChooseControl(ControlMode mode, ControlModel target =null)
    {
        switch (mode)
        {
            case ControlMode.运行:
                Running();
                break;
            case ControlMode.暂停:
                Pause();
                break;
            case ControlMode.快进:
                SpeedUp();
                break;
            case ControlMode.慢放:
                SpeedDown();
                break;
            case ControlMode.重置:
                ResetAni();
                break;
            case ControlMode.选择模式:
                modeObj.SetActive(true);
                Tweener tw = modeObj.transform.DOScale(Vector3.zero, 2f);
                tw.SetDelay(5);
                target.enabled = false;
                tw.OnComplete(() =>
                {
                    modeObj.SetActive(false);
                    modeObj.transform.localScale = new Vector3(0.03633308f, 1f, 0.05442299f);
                    target.enabled = true;
                });
                break;
            case ControlMode.选择运行图:
                runObj.SetActive(true);
                Tweener tw1 = runObj.transform.DOScale(Vector3.zero, 2f);
                tw1.SetDelay(5);
                target.enabled = false;
                tw1.OnComplete(() =>
                {
                    runObj.SetActive(false);
                    runObj.transform.localScale = new Vector3(0.05047391f, 1f, 0.05442299f);
                    target.enabled = true;
                });
                break;
            case ControlMode.拆装图:
                startIndex = 0;
                startSpeedIndex = 3;
                am.gameObject.SetActive(true);
                em.gameObject.SetActive(false);
                rm.gameObject.SetActive(false);
                AniPlayOver();
                break;
            case ControlMode.爆炸图:
                startIndex = 1;
                startSpeedIndex = 3;
                am.gameObject.SetActive(false);
                em.gameObject.SetActive(true);
                rm.gameObject.SetActive(false);
                AniPlayOver();
                break;
            case ControlMode.原理图:
                startIndex = 2;
                startSpeedIndex = 3;
                am.gameObject.SetActive(false);
                em.gameObject.SetActive(false);
                rm.gameObject.SetActive(true);
                AniPlayOver();
                break;
        }
    }

    void Running() {
        play.SetActive(false);
        pause.SetActive(true);
        if (startMode == 0)
        {
            if (startIndex == 0)
            {
                if (!am.isForwardPlay && !am.isRevertPlay)
                {
                    Debug.Log(resetTime);
                    if (resetTime % 2 == 0)
                    {
                        am.StartAni(AniPlayOver);
                    }
                    else {
                        am.RevertAni(AniPlayOver);
                    }
                    
                }
                else {
                    am.PlayAni();
                }
            }
            else if (startIndex == 1)
            {
                if (!em.IsPlaying)
                {
                    em.ChoosePlayAni(AniPlayOver);
                }
                else
                {
                    em.PlayAni();
                }
            }
            else if (startIndex == 2)
            {
                if (!rm.isStart)
                {
                    rm.StartRotate();
                }
                else
                {
                    rm.PlayRotate();
                }
            }
        }
        else {

        }
    }

    void Pause() {
        play.SetActive(true);
        pause.SetActive( false);
        if (startMode == 0)
        {
            if (startIndex == 0)
            {
                am.PauseAni();
            }
            else if (startIndex == 1)
            {
                em.PauseAni();
            }
            else if (startIndex == 2)
            {
                rm.PauseRotate();
            }
        }
        else
        {

        }
    }

    void SpeedUp()
    {
        if (startSpeedIndex + 1 < speedScales.Length)
        {
            startSpeedIndex++;
        }
        else
        {
            return;
        }
        if (startMode == 0)
        {
            if (startIndex == 0)
            {
                am.Speed = speedScales[startSpeedIndex];
            }
            else if (startIndex == 1)
            {
                em.Speed = speedScales[startSpeedIndex];
            }
            else if (startIndex == 2)
            {
                rm.Speed = speedScales[startSpeedIndex];
            }
        }
        else
        {

        }
    }

    void SpeedDown()
    {
        if (startSpeedIndex-1 >=0)
        {
            startSpeedIndex--;
        }
        else
        {
            return;
        }
        if (startMode == 0)
        {
            if (startIndex == 0)
            {
                am.Speed = speedScales[startSpeedIndex];
            }
            else if (startIndex == 1)
            {
                em.Speed = speedScales[startSpeedIndex];
            }
            else if (startIndex == 2)
            {
                rm.Speed = speedScales[startSpeedIndex];
            }
        }
        else
        {

        }
    }

    void ResetAni() {
        resetTime++;
        pause.SetActive(false);
        play.SetActive(true);
        if (startMode == 0)
        {
            if (startIndex == 0)
            {
                if (resetTime % 2 == 0)
                {
                    am.ResetAni();
                }
                else {
                    am.FinishAni();
                }
               
            }
            else if (startIndex == 1)
            {
                em.ResetAni();
            }
            else if (startIndex == 2)
            {
                rm.PauseRotate();
            }
        }
        else {

        }
    }

    public void AniPlayOver() {
        play.SetActive(true);
        pause.SetActive(false);
    }
    
}
public enum ControlMode {
    运行,
    暂停,
    快进,
    慢放,
    重置,
    选择模式,
    选择运行图,
    爆炸图,
    拆装图,
    原理图,
    模式1,
    模式2,
}