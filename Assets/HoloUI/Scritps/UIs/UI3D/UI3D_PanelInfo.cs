using DG.Tweening;
using Gloabal_EnumCalss;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 详细UI3D信息面板
/// </summary>
public class UI3D_PanelInfo : MonoBehaviour {

    #region 属性
    public bool M_IsToggleShow
    {
        get
        {
            return isToggleShow;
        }
    }
    public E_UI3DPanelInfoType M_CurPanelInfoType
    {
        get
        {
            return curPanelInfoType;
        }
    }
    /// <summary>
    /// 当前面板的父物体
    /// </summary>
    public GameObject M_ParentObj
    {
        get
        {
            return curParentObj;
        }
    }
    #endregion

    #region 公有变量

    /// <summary>
    /// 显示的内容的分隔符
    /// </summary>
    public const char INFOSPLITCHAR = ';';

    #endregion

    #region 私有变量

    private bool isInitSucced = false;
    private bool isToggleShow = false;
    private bool isFollowParentObj = false;
    private bool isAutoHide = false;

    private float delayAutoHideTime = 5.0f;

    private InterfaceAnimManager curAnim;

    [SerializeField]
    E_UI3DPanelInfoType curPanelInfoType;
    [SerializeField]
    private Vector3 offsetRot;
    [SerializeField]
    private Vector3 offsetPos = new Vector3(0, 10, 0);

    /// <summary>
    /// 该UI显示谁的信息
    /// </summary>
    private GameObject curParentObj;
    [SerializeField]
    /// <summary>
    /// 子物体，连线起点
    /// </summary>
    private GameObject subObjLinePoint;
    [SerializeField]
    private Text[] subObjTextContent;
    [SerializeField]
    private Text subObjTextTitle;
    private LineRenderer subObjLineRenderer;
    /// <summary>
    /// 连线的点的数量
    /// </summary>
    private const int NUM_LINEPOINT= 10;

    private bool isHover = false;
    private bool isPress = false;
    private float priScaleX;
    private float HoverScaleRate = 1.2f;
    #endregion

    #region 系统方法

    // Use this for initialization
    void Start () {
	}

    // Update is called once per frame
    void Update()
    {
        if (!isInitSucced) return;

     //   transform.LookAt(Camera_DistortionManage.M_Instance.M_SubObjCamObtainRT.transform.position);
        //Vector3 tempRot = transform.eulerAngles;
        //transform.eulerAngles += offsetRot;

        UpdateLinePos();

        #region 测试



        #endregion
    }

    #endregion

    #region 私有方法

    /// <summary>
    /// 跟新线的位置
    /// </summary>
    private void UpdateLinePos()
    {
        Vector3 tempStartPos =subObjLinePoint.transform.position;
        Vector3 tempEndPos =curParentObj.transform.position;
        Vector3 tempDir = tempEndPos - tempStartPos;
        float tempDis = Vector3.Distance(tempStartPos, tempEndPos);
        float tempUnit = tempDis / NUM_LINEPOINT;
        Vector3[] tempInsertPos = new Vector3[NUM_LINEPOINT];

        for (int i = 0; i < tempInsertPos.Length; i++)
        {
            tempInsertPos[i] = tempStartPos + tempDir.normalized * tempUnit * i;
        }
        tempInsertPos[tempInsertPos.Length - 1] = tempEndPos;
        subObjLineRenderer.SetPositions(tempInsertPos);
    }

    private void ProcessInfoStr(string info)
    {
        ///字符内容的处理，第一个分隔符的为标题，后面的都为内容
        string[] tempStrContent = info.Split(INFOSPLITCHAR);
        if (null != tempStrContent && tempStrContent.Length >= subObjTextContent.Length + 1)
        {
            subObjTextTitle.text = tempStrContent[0];
            for (int i = 0; i < subObjTextContent.Length; i++)
            {
                subObjTextContent[i].text = tempStrContent[i + 1];
            }
        }
    }
    private IEnumerator DelayExeMethod(Action method,float delayTime)
    {
        yield return new WaitForSecondsRealtime(delayTime);
        if(null!=method)
        {
            method();
        }
    }

    #endregion

    #region 公有方法

    public void BntEvent_Hover()
    {
        //设置Hover不受TimeScale的影响
        TweenParams tempP = new TweenParams().SetUpdate(true);
        transform.DOScale(priScaleX * HoverScaleRate, 0.3f).SetAs(tempP);
        isHover = true;
        Debug.Log("Hover");
    }
    public void BtnEvent_UnHover()
    {
        TweenParams tempP = new TweenParams().SetUpdate(true);
        transform.DOScale(priScaleX, 0.3f).SetAs(tempP);
        isHover = false;
        Debug.Log("UnHover");
    }
    /// <summary>
    /// 开关动画
    /// </summary>
    public void SetToggleState()
    {
        isToggleShow = !isToggleShow;
        if (isToggleShow)
        {
            curAnim.startAppear();
            gameObject.SetActive(true);
        }
        else
        {
            curAnim.startDisappear();
        }
    }
    /// <summary>
    /// 显示动画
    /// </summary>
    public void Show(string info="")
    {
        ProcessInfoStr(info);
        curAnim.startAppear();
        if (isAutoHide)
        {
            StartCoroutine(DelayExeMethod(Hide, delayAutoHideTime));
        }
        isToggleShow = true;
    }
    /// <summary>
    /// 隐藏动画
    /// </summary>
    public void Hide()
    {
        curAnim.startDisappear();
        isToggleShow = false;
    }

    public void Init(GameObject curObj, string info)
    {
        isInitSucced = true;
        curAnim = GetComponent<InterfaceAnimManager>();
        curAnim.Init();

        subObjLineRenderer =subObjLinePoint.GetComponent<LineRenderer>();
        subObjLineRenderer.positionCount = NUM_LINEPOINT;

        curParentObj = curObj;

        transform.localPosition = offsetPos;
        transform.localEulerAngles = Vector3.zero;

        Show(info);

        priScaleX = transform.localScale.x;
    }

    #endregion

}
