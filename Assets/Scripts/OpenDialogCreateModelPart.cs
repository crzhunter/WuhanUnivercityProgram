using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using System.IO;
using VRTK;

public class OpenDialogCreateModelPart : MonoBehaviour
{
    public Button OpenDialog;
    public CanExChangePart[] canChanges;

    // Start is called before the first frame update
    void Start()
    {
        canChanges = ThreeDTouchAnimationControl._Instance.im.GetComponentsInChildren<CanExChangePart>();
        OpenDialog.onClick.AddListener(OpenWindow);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OpenWindow() {
        OpenFileName openFileName = new OpenFileName();
        openFileName.structSize = Marshal.SizeOf(openFileName);
        openFileName.filter = "Bundle文件(*.manifest)\0*.manifest";
        openFileName.file = new string(new char[256]);
        openFileName.maxFile = openFileName.file.Length;
        openFileName.fileTitle = new string(new char[64]);
        openFileName.maxFileTitle = openFileName.fileTitle.Length;
        openFileName.initialDir = Application.streamingAssetsPath.Replace('/', '\\');//默认路径
        openFileName.title = "选择零件模型";
        openFileName.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008;

        if (LocalDialog.GetOpenFileName(openFileName))
        {
            string path = Path.GetDirectoryName(openFileName.file) + "/" + Path.GetFileNameWithoutExtension(openFileName.file);
            Debug.Log(path);
            StartCoroutine(LoadBundle(path));
        }
    }

    IEnumerator LoadBundle(string bundlepath){

        AssetBundleCreateRequest req = AssetBundle.LoadFromFileAsync(bundlepath);
        yield return req;
        if(req.isDone){
            string mainAsset = req.assetBundle.GetAllAssetNames()[0];
            GameObject modelPart = req.assetBundle.LoadAsset(mainAsset) as GameObject;
            GameObject go= Instantiate(modelPart.gameObject,new Vector3(2.322f, 0.584f, -1.334f),modelPart.transform.rotation);
            VRTK_InteractableObject inter = go.AddComponent<VRTK_InteractableObject>();
            inter.isGrabbable = true;
            inter.holdButtonToGrab = true;
            inter.stayGrabbedOnTeleport = true;
            inter.touchHighlightColor = Color.white;
            foreach (var item in canChanges) {
                if (item.partName == go.GetComponent<BindScript>().part) {
                    item.GetComponent<MeshRenderer>().enabled = false;
                    MeshRenderer[] childs = item.GetComponentsInChildren<MeshRenderer>();
                    foreach (var mesh in childs) {
                        mesh.enabled = false;
                    }
                }
            }
        }
        else
        {
            Debug.Log("加载bundle失败");
        }

    }
  

}
