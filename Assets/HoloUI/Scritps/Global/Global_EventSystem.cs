using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global_EventSystem : MonoBehaviour {

    private UI3D_BtnEventInterface curHoverObj;
    private UI3D_BtnEventInterface curSlectedObj;
    private float doubleClickTime;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        EditorTestHoverAndSelectedObj();
	}

    private void EditorTestHoverAndSelectedObj()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo = new RaycastHit();
        UI3D_BtnEventInterface tempObj = null;
        ////只检测生成的战斗元素所在的层级
        //int layerMask = 1 << LayerMask.NameToLayer("TransparentFX");
        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
        {
            tempObj = hitInfo.collider.GetComponent<UI3D_BtnEventInterface>();
            //鼠标选中的物体要是当前类型的角色创建的物体
        }
        if (null != tempObj)
        {
            tempObj.Hover();
            curHoverObj = tempObj;
        }
        else
        {
            if (null != curHoverObj)
            {
                curHoverObj.UnHover();
                curHoverObj = null;
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (null != curHoverObj)
            {
                if (curHoverObj != curSlectedObj)
                {
                    curHoverObj.Selected();

                    if (null != curSlectedObj)
                    {
                        curSlectedObj.UnSelected();
                    }
                }
                else
                {
                    curHoverObj.ToggleSelected();
                }
                curSlectedObj = curHoverObj;

                //当第二次点击鼠标，且时间间隔满足要求时双击鼠标
                if (Time.time - doubleClickTime <= 0.5f)
                {
                    curSlectedObj.DoubleClick();
                }
                doubleClickTime = Time.time;
            }
        }
    }
}
