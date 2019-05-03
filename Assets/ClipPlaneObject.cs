using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClipPlaneObject : MonoBehaviour {

    [SerializeField]
    Transform gearboxRoot;

    [SerializeField]
    List<Material> materials;

    bool inited = false;

    // Use this for initialization
    IEnumerator Start()
    {
        materials = new List<Material>();
        GetChildMat(gearboxRoot, materials);
        inited = true;
        EnableClip();
        yield return null;
    }

    MeshRenderer tempMR;
    void GetChildMat(Transform root,List<Material> mats) {
        tempMR = root.GetComponent<MeshRenderer>();
        if (tempMR != null && tempMR.material != null)
            mats.Add(tempMR.material);
        if(root.childCount!=0)
        {
            for (int i = 0; i < root.childCount; i++)
                GetChildMat(root.GetChild(i), mats);
        }
    }

	// Update is called once per frame
	void Update () {
        //if (Input.GetKeyDown(KeyCode.DownArrow))
        //    EnableClip();
        //if (Input.GetKeyDown(KeyCode.UpArrow))
        //    EnableClip();

        for (int i = 0; i < materials.Count; i++)
        {
            if (materials[i]!=null)
            {
                materials[i].SetVector("_Point", new Vector4(transform.position.x, transform.position.y, transform.position.z, 1));
                materials[i].SetVector("_V", new Vector4(transform.up.x, transform.up.y, transform.up.z, 1));
            }
        }
	}

    public void EnableClip()
    {
        if (!inited)
            return;

        for (int i = 0; i < materials.Count; i++)
        {
            if (materials[i] != null)
            {
                materials[i].shader = Shader.Find("ClipShader/Object");
            }
        }
    }

    
    /*
    public void DiableClip ()
    {
        if (!inited)
            return;

        for (int i = 0; i < materials.Count; i++)
        {
            if (materials[i] != null)
            {
                materials[i].shader = shader;
                Debug.Log(shader.name);
            }
        }
    }
    */

}
