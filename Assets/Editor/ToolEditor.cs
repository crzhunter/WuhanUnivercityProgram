using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ToolEditor : MonoBehaviour {

    //[MenuItem("Tool/GreatBundle")]
    //static void GreatBundle()
    //{
    //    Object[] selects = Selection.objects;
    //    foreach (Object item in selects)
    //    {
    //        Debug.Log(item);
    //        BuildPipeline.BuildAssetBundles(Application.dataPath , BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
    //        AssetDatabase.Refresh();//打包后刷新，不加这行代码的话要手动刷新才可以看得到打包后的Assetbundle包
    //    }
    //}

    [MenuItem("Tool/CreatBundle")]
    static void GreatAllBundle()
    {
        AssetBundleBuild[] builds = new AssetBundleBuild[1];
        Object[] selects = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        string[] TestAsset = new string[selects.Length];
        for (int i = 0; i < selects.Length; i++)
        {
            TestAsset[i] = AssetDatabase.GetAssetPath(selects[i]);
            Debug.Log(TestAsset[i]);
        }
        builds[0].assetNames = TestAsset;
        builds[0].assetBundleName = selects[0].name;
        BuildPipeline.BuildAssetBundles(Application.dataPath , builds, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
        AssetDatabase.Refresh();
    }
}
