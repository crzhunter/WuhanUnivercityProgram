//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using VRTK;
//using UnityEngine.UI;
//public class AnimationControl : MonoBehaviour
//{
//    public ProcedureAnimationManager am;
//    public ExplosionAnimationManager em;
//    public RotateAnimationManager rm;
//    public VRTK_ControllerEvents leftHand;
//    public VRTK_ControllerEvents RightHand;
//    public Text text;

//    public int startIndex = 0;
//    // Use this for initialization
//    void Start()
//    {
//        //leftHand.TouchpadAxisChanged += LeftHand_TouchpadAxisChanged;
//        leftHand.TouchpadReleased += LeftHand_TouchpadAxisChanged;
//        leftHand.TriggerClicked += LeftHand_TriggerClicked;
//       // leftHand.StartMenuPressed += LeftHand_StartMenuPressed;
//    }

//    //private void LeftHand_StartMenuPressed(object sender, ControllerInteractionEventArgs e)
//    //{
//    //    Debug.Log(1111111111 + "  " + startIndex);
//    //    if (startIndex== 0)
//    //    {
//    //        am.PauseAni();
//    //    }
//    //    else if (startIndex == 1)
//    //    {
//    //        em.PauseAni();
//    //    }
//    //    else if (startIndex  == 2)
//    //    {
//    //        rm.PauseRotate();
//    //    }
//    //}

//    private void LeftHand_TriggerClicked(object sender, ControllerInteractionEventArgs e)
//    {
//        Debug.Log(1111111111 + "  " + startIndex);
//        if (startIndex == 0)
//        {
//            if (!am.isForwardPlay && !am.isRevertPlay)
//            {
//                am.StartAni();
//            }
//            else if (am.isForwardPlay)
//            {
//                am.FinishAni();
//                am.RevertAni();
//            }
//            else if (am.isRevertPlay) {
//                am.ResetAni();
//                am.StartAni();
//            }
            
//        }
//        else if (startIndex== 1)
//        {
//            if (!em.isForwardPlay && !em.isRevertPlay) {
//                em.StartAni();
//            }
//            else if (em.isForwardPlay)
//            {
//                em.StopAni();
//                em.RevertAni();
//            }
//            else if (em.isRevertPlay)
//            {
//                em.StopAni();
//                em.StartAni();
//            }
//        }
//        else if (startIndex == 2)
//        {
//            if (!rm.isStart) {
//                rm.StartRotate();
//            }
//            else { rm.PauseRotate(); }
           
//        }
//    }

//    private void LeftHand_TouchpadAxisChanged(object sender, ControllerInteractionEventArgs e)
//    {
//        Debug.Log(1111111111 + "  " + startIndex + "   " + e.buttonPressure + "  " + e.touchpadAngle);
//        if (e.touchpadAngle <= 315f && e.touchpadAngle > 225)
//        {
//            startIndex = 0;
//            am.gameObject.SetActive(true);
//            em.gameObject.SetActive(false);
//            rm.gameObject.SetActive(false);
//        }
//        if (e.touchpadAngle > 315f || e.touchpadAngle <= 45f)
//        {
//            startIndex = 1;
//            am.gameObject.SetActive(false);
//            em.gameObject.SetActive(true);
//            rm.gameObject.SetActive(false);
//        }
//        if (e.touchpadAngle > 45f && e.touchpadAngle <= 135f)
//        {
//            startIndex = 2;
//            am.gameObject.SetActive(false);
//            em.gameObject.SetActive(false);
//            rm.gameObject.SetActive(true);
//        }

//        if (e.touchpadAngle > 135f && e.touchpadAngle <= 225f)
//        {
//            if (startIndex == 0)
//            {
//                am.PauseAni();
//            }
//            else if (startIndex == 1)
//            {
//                em.PauseAni();
//            }
//            else if (startIndex == 2)
//            {
//                if (!rm.isStart)
//                {
//                    rm.StartRotate();
//                }
//                else { rm.PauseRotate(); }
//            }
//        }

//    }
    

//    // Update is called once per frame
//    void Update()
//    {
//        if (startIndex == 0)
//        {
//            text.text = "当前正在观看<color=#10F624>流程动画</color> ";
//        }
//       else if (startIndex == 1)
//        {
//            text.text = "当前正在观看<color=#10F624>爆炸动画</color> ";
//        }
//       else if (startIndex == 2)
//        {
//            text.text = "当前正在观看<color=#10F624>工作原理动画</color> ";
//        }

//        //if (leftHand.IsButtonPressed(VRTK_ControllerEvents.ButtonAlias.TriggerClick))
//        //{

//        //}

//        //if (RightHand.IsButtonPressed(VRTK_ControllerEvents.ButtonAlias.TriggerClick))
//        //{
//        //    Debug.Log(222222222 + "  " + startIndex % 3);
//        //    startIndex++;
//        //    if (startIndex % 3 == 0)
//        //    {
//        //        am.gameObject.SetActive(true);
//        //        em.gameObject.SetActive(false);
//        //        rm.gameObject.SetActive(false);
//        //    }
//        //    else if (startIndex % 3 == 1)
//        //    {
//        //        am.gameObject.SetActive(false);
//        //        em.gameObject.SetActive(true);
//        //        rm.gameObject.SetActive(false);
//        //    }
//        //    else if (startIndex % 3 == 2)
//        //    {
//        //        am.gameObject.SetActive(false);
//        //        em.gameObject.SetActive(false);
//        //        rm.gameObject.SetActive(true);
//        //    }
//        //}

//        //if (RightHand.IsButtonPressed(VRTK_ControllerEvents.ButtonAlias.StartMenuPress)) {
//        //    Debug.Log(333333333 + "  " + startIndex % 3);
//        //    if (startIndex % 3 == 0)
//        //    {
//        //        am.PauseAni();
//        //    }
//        //    else if (startIndex % 3 == 1)
//        //    {
//        //        em.PauseAni();
//        //    }
//        //    else if (startIndex % 3 == 2)
//        //    {
//        //        rm.PauseRotate();
//        //    }
//        //}

//        //if (leftHand.IsButtonPressed(VRTK_ControllerEvents.ButtonAlias.StartMenuPress))
//        //{
//        //    if (startIndex % 3 == 0)
//        //    {
//        //        am.PauseAni();
//        //    }
//        //    else if (startIndex % 3 == 1)
//        //    {
//        //        em.PauseAni();
//        //    }
//        //    else if (startIndex % 3 == 2)
//        //    {
//        //        rm.PauseRotate();
//        //    }
//        //}


//        //if (Input.GetKeyDown(KeyCode.T))
//        //{
//        //    rm.StartRotate();
//        //}

//        //if (Input.GetKeyDown(KeyCode.Y))
//        //{
//        //    rm.PauseRotate();
//        //}

//        //if (Input.GetKeyDown(KeyCode.O)) {
//        //    em.StartAni();
//        //}

//        //if (Input.GetKeyDown(KeyCode.I))
//        //{
//        //    em.RevertAni();
//        //}

//        if (Input.GetKeyDown(KeyCode.W))
//        {
//            if (am.isRevertPlay)
//            {
//                Debug.Log("nowRevert");
//            }
//            else
//            {
//                am.StartAni();
//            }

//        }
//        if (Input.GetKeyDown(KeyCode.Q))
//        {
//            am.PauseAni();
//        }
//        if (Input.GetKeyDown(KeyCode.S))
//        {
//            am.ResetAni();
//        }
//        if (Input.GetKeyDown(KeyCode.E))
//        {
//            if (am.isForwardPlay)
//            {
//                Debug.Log("nowForward");
//            }
//            else
//            {
//                am.RevertAni();
//            }
//        }
//    }
//}
