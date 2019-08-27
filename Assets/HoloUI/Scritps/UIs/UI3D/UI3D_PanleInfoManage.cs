using Gloabal_EnumCalss;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI3D面板信息管理类
/// </summary>
public class UI3D_PanleInfoManage : BaseGameObj {

    #region 属性

    public static UI3D_PanleInfoManage M_Instance
    {
        get
        {
            if(null==_instance)
            {
                _instance = FindObjectOfType<UI3D_PanleInfoManage>();
            }
            return _instance;
        }
    }

    #endregion

    #region 私有变量

    private static UI3D_PanleInfoManage _instance;

    [SerializeField]
    private UI3D_PanelInfo[] prefabsUI3DPanelInfos;

    private List<UI3D_PanelInfo> listCacheUI3DPF = new List<UI3D_PanelInfo>();

    private UI3D_PanelInfo curShowUI3DPanelInfo;

    #endregion

    #region 测试

    public GameObject TestObj;
    public E_UI3DPanelInfoType testCurPanelInfoType;
    #endregion

    #region 系统方法


    // Use this for initialization
    void Start()
    {
        Init();
    }

    protected override void FixedUpdate_State()
    {

    }

    protected override void LateUpdate_State()
    {

    }

    protected override void Update_State()
    {
        #region 测试

        if(Input.GetKeyDown(KeyCode.T))
        {
            if (null != curShowUI3DPanelInfo)
            {
                curShowUI3DPanelInfo.SetToggleState();
            }
            else
            {
                curShowUI3DPanelInfo = ShowPanelInfo(TestObj, "标题！;测试一下;显示物体的信息", testCurPanelInfoType);
            }
        }

        #endregion
    }

    #endregion

    #region 公有方法

    /// <summary>
    /// 使用3DUI显示战斗元素的信息
    /// </summary>
    /// <param name="curObj">当前需要显示信息的物体</param>
    /// <param name="info">信息内容</param>
    /// <param name="panelInfoType">面板的类型</param>
    public UI3D_PanelInfo ShowPanelInfo(GameObject curObj,string info="", E_UI3DPanelInfoType panelInfoType=E_UI3DPanelInfoType.GREEN)
    {
        if(null==curObj)
        {
            Debug.Log("当先显示的3D物体为空！");
            return null;
        }

        UI3D_PanelInfo tempUI3DPI = null;

        //先检测缓冲列表中是否创建过该物体的该类型信息面板，如果有则更新信息并显示，如果没有则创建
        int tempIndex = listCacheUI3DPF.FindIndex(p => p.M_CurPanelInfoType == panelInfoType && curObj.Equals(p.M_ParentObj));
        if (-1 == tempIndex)
        {
            for (int i = 0; i < prefabsUI3DPanelInfos.Length; i++)
            {
                if (prefabsUI3DPanelInfos[i].M_CurPanelInfoType == panelInfoType)
                {
                    tempUI3DPI = Instantiate(prefabsUI3DPanelInfos[i], curObj.transform);
                    tempUI3DPI.Init(curObj, info);
                    listCacheUI3DPF.Add(tempUI3DPI);
                    break;
                }
            }
        }
        else
        {
            tempUI3DPI = listCacheUI3DPF[tempIndex];
            tempUI3DPI.Show(info);
        }

        return tempUI3DPI;
    }

    #endregion
}
