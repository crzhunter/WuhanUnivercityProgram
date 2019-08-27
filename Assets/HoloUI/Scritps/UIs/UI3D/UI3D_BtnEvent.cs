using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class UI3D_BtnEvent : MonoBehaviour ,UI3D_BtnEventInterface{

    [SerializeField]
    private bool isEventHover;
    [SerializeField]
    private bool isEventUnHover;

    /// <summary>
    /// 悬浮事件的接收方法名
    /// </summary>
    [SerializeField]
    private string hoverEMN = string.Empty;
    /// <summary>
    /// 退出悬浮的接收方法名
    /// </summary>
    [SerializeField]
    private string unHoverEMN = string.Empty;

    [SerializeField]
    /// 事件处理方法的接收对象
    /// </summary>
    private GameObject eventReceiveObj;
    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TestEventHover()
    {
       // Debug.Log("TestEventHover");
    }
    public void Hover()
    {
        if (null != eventReceiveObj && hoverEMN != null)
        {
            eventReceiveObj.SendMessage(hoverEMN);
        }
    }
    public void UnHover()
    {
        if (null != eventReceiveObj && hoverEMN != null)
        {
            eventReceiveObj.SendMessage(unHoverEMN);
        }
    }
    public void PressDown()
    {

    }

    public void PressUp()
    {

    }


    public void TogglePress()
    {
       
    }

    public void DoubleClick()
    {
       
    }
    public void Selected()
    {

    }
    public void UnSelected()
    {

    }

    public void ToggleSelected()
    {
       
    }
}
public enum E_EventType
{
    None,
    Hover,
    UnHover,
}