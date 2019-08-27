using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface UI3D_BtnEventInterface  {

    void Hover();
    void UnHover();
    void PressDown();
    void PressUp();
    void Selected();
    void UnSelected();
    void ToggleSelected();
    void DoubleClick();
}
