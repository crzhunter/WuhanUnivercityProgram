using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseGameObj : MonoBehaviour {

    private bool isInitSucceed = false;

    // Use this for initialization
    void Start()
    {
        //  Init_Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isInitSucceed) return;
        Update_State();

    }
    void LateUpdate()
    {
        if (!isInitSucceed) return;
        LateUpdate_State();
    }
    void FixedUpdate()
    {
        if (!isInitSucceed) return;
        FixedUpdate_State();

    }
    /// <summary>
    /// 初始化成功之后执行的Update方法
    /// </summary>
    protected abstract void Update_State();
    /// <summary>
    /// 初始化成功之后执行的LateUpdate方法
    /// </summary>
    protected abstract void LateUpdate_State();

    /// <summary>
    /// 初始化成功之后执行的固定帧率刷新方法
    /// </summary>
    protected abstract void FixedUpdate_State();

    /// <summary>
    /// 初始化成功之后执行的开始方法
    /// </summary>
    public virtual void Init()
    {
        isInitSucceed = true;
    }
}

