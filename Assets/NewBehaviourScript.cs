using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class ModelTouched : MonoBehaviour
{
    public VRTK_InteractableObject obj;
    // Use this for initialization
    void Start()
    {
        obj = GetComponent<VRTK_InteractableObject>();
        obj.InteractableObjectTouched += Obj_InteractableObjectTouched;
        obj.InteractableObjectUntouched += Obj_InteractableObjectUntouched;
    }

    private void Obj_InteractableObjectUntouched(object sender, InteractableObjectEventArgs e)
    {

    }

    private void Obj_InteractableObjectTouched(object sender, InteractableObjectEventArgs e)
    {
        Debug.Log(e.interactingObject);
    }
}