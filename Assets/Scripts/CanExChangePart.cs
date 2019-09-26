using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class CanExChangePart :MonoBehaviour
{
    public ChangePart partName;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<BindScript>()!=null&&other.gameObject.GetComponent<BindScript>().part == partName)
        {
            if (other.GetComponent<Rigidbody>() != null) {
                other.GetComponent<Rigidbody>().isKinematic = true;
            }
            Transform import = other.transform;
            import.GetComponent<VRTK_InteractableObject>().enabled = false;
            import.transform.parent = this.transform;
            import.transform.localPosition = Vector3.zero;
            import.transform.localRotation = Quaternion.identity;
        }
    }
}

public enum ChangePart {
    part1,
    part2,
    part3
}