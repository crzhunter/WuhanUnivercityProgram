using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gloabal_EnumCalss
{

    #region 无界眼镜

    /// <summary>
    /// 视觉模式
    /// </summary>
    public enum E_VisionMode
    {
        /// <summary>
        /// 单目
        /// </summary>
        MONOCULAR,
        /// <summary>
        /// 双目
        /// </summary>
        BINOCULAR
    }
    /// <summary>
    /// 眼镜镜片模组类型
    /// </summary>
    public enum E_ModuleType
    {
        BOUNDLESS_DLODLO,
        BOUNDLESS_JDI,
        PICO_JDI,
        MODULE55070_DAHUA,
        BOUNDLESS_BOE
    }
    #endregion

    #region UI

    /// <summary>
    /// 3DUI消息面板类型
    /// </summary>
    public enum E_UI3DPanelInfoType
    {
        WHITE,
        GREEN,
        //Archeologist
        ARCHE,
        //MaskedMap
        MASKEDMAP,
    }

    #endregion
}
