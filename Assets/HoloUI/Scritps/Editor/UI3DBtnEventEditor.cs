using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(UI3D_BtnEvent))]
public class UI3DBtnEventEditor : Editor
{
    private SerializedProperty nameHoverMethod;
    private SerializedProperty nameUnHoverMethod;
    private SerializedProperty isEventHover;
    private SerializedProperty isEventUnHover;
    private SerializedProperty eventReceiveObj;
    private UnityEngine.Object priEventReceiveObj;

    private Component[] tempComs;

    private Enum tempEnumClassHover;
    private Enum tempEnumClassUnHover;

    private Enum tempComponentEnumName;
    private string priComonentName = string.Empty;
    private List<string> tempListComsName = new List<string>();
    private void OnEnable()
    {
        nameHoverMethod = serializedObject.FindProperty("hoverEMN");
        nameUnHoverMethod = serializedObject.FindProperty("unHoverEMN");

        eventReceiveObj = serializedObject.FindProperty("eventReceiveObj");
        priEventReceiveObj = eventReceiveObj.objectReferenceValue;

        isEventHover =serializedObject.FindProperty("isEventHover");
        isEventUnHover =serializedObject.FindProperty("isEventUnHover");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        UI3D_BtnEvent tempTargetUBE = (UI3D_BtnEvent)target;
        eventReceiveObj.objectReferenceValue = EditorGUILayout.ObjectField("悬浮事件接收对象", eventReceiveObj.objectReferenceValue, typeof(UnityEngine.Object), true);

        if (null != eventReceiveObj.objectReferenceValue && priEventReceiveObj != eventReceiveObj.objectReferenceValue)
        {
            tempListComsName.Clear();
            priEventReceiveObj = eventReceiveObj.objectReferenceValue;
            GameObject tempObj = (GameObject)eventReceiveObj.objectReferenceValue;
            if (null != tempObj)
            {
                //先选择组件，注意如果有脚本挂在上面然后删掉了还是会获得并且是null
                tempComs = tempObj.GetComponents<Component>();
                for (int i = 0; i < tempComs.Length; i++)
                {
                    Component tempComponent = tempComs[i];
                    if (null == tempComponent) continue;
                    Type tempType = tempComponent.GetType();
                    tempListComsName.Add(tempType.ToString());
                }

                tempComponentEnumName = CreateEnum(tempListComsName);
            }
        }
        //选择绑定对象
        if (null != tempComponentEnumName)
        {
            tempComponentEnumName = EditorGUILayout.EnumPopup("选择绑定对象", tempComponentEnumName);

            //根据选择对象不一样选择对象里面的方法，刷新方法的选择
            if (tempComponentEnumName.ToString() != priComonentName && null != tempComs && tempComs.Length == tempListComsName.Count)
            {
                priComonentName = tempComponentEnumName.ToString();
                int tempIndexEnumName = tempListComsName.FindIndex(p => p == tempComponentEnumName.ToString());
                if (-1 != tempIndexEnumName)
                {
                    priComonentName = tempComponentEnumName.ToString();
                    //Debug.Log(tempIndexEnumName);
                    //再选择组件里面的方法
                    Type tempType = tempComs[tempIndexEnumName].GetType();
                    //使用反射将方法全部反射出来
                    MethodInfo[] methods = tempType.GetMethods();
                    List<string> tempListStrEnuMethods = new List<string>();
                    for (int i = 0; i < methods.Length; i++)
                    {
                        MethodBase tempMB = methods[i];
                        //如果是该类定义的方法就装载进来
                        if (tempMB.DeclaringType.Name == tempType.Name && tempMB.IsPublic && !tempMB.IsSpecialName)
                        {
                            tempListStrEnuMethods.Add(methods[i].Name);
                        }
                    }
                    tempEnumClassHover = CreateEnum(tempListStrEnuMethods);
                    tempEnumClassUnHover = CreateEnum(tempListStrEnuMethods);
                }
            }
        }
        
        isEventHover.boolValue = EditorGUILayout.Toggle("悬浮事件", isEventHover.boolValue);
        if (isEventHover.boolValue&&null!= tempEnumClassHover)
        {
            tempEnumClassHover = EditorGUILayout.EnumPopup("绑定Hover的方法", tempEnumClassHover);
            nameHoverMethod.stringValue = tempEnumClassHover.ToString();
        }
        isEventUnHover.boolValue = EditorGUILayout.Toggle("退出悬浮事件", isEventUnHover.boolValue);
        if (isEventUnHover.boolValue&&null!=tempEnumClassUnHover)
        {
            tempEnumClassUnHover = EditorGUILayout.EnumPopup("绑定UnHover的方法", tempEnumClassUnHover);
            nameUnHoverMethod.stringValue = tempEnumClassUnHover.ToString();
        }



        serializedObject.ApplyModifiedProperties();
        //将原本脚本中序列化的也画出来
        DrawDefaultInspector();

    }


    //private void GetMethod(GameObject obj)
    //{
    //    UI3D_PanelInfo tempComponent = obj.GetComponent<UI3D_PanelInfo>();
    //    if (null != tempComponent)
    //    {
    //        Type tempType = tempComponent.GetType();
    //        //使用反射将方法全部反射出来
    //        MethodInfo[] methods = tempType.GetMethods();
    //        List<string> tempListStrEnuMethods = new List<string>();
    //        for (int i = 0; i < methods.Length; i++)
    //        {
    //            //如果是该类定义的方法就装载进来
    //            if (methods[i].DeclaringType.Name == tempType.Name)
    //            {
    //                tempListStrEnuMethods.Add(methods[i].ToString());
    //            }
    //        }
    //        if (null == tempEnumClass)
    //        {
    //            tempEnumClass = CreateEnum(tempListStrEnuMethods);
    //        }
    //        tempEnumClass = EditorGUILayout.EnumPopup("动态创建的枚举", tempEnumClass);
    //        int tempIndex = tempListStrEnuMethods.FindIndex(p => p == tempEnumClass.ToString());
    //        if (-1 != tempIndex)
    //        {
    //            Debug.Log(tempListStrEnuMethods[tempIndex]);
    //        }

    //    }
    //}
    private List<string> GetComponentsName(GameObject tempObj)
    {
        List<string> tempListName = new List<string>();
        if (null != tempObj)
        {
            //先选择组件，注意如果有脚本挂在上面然后删掉了还是会获得并且是null
            Component[] tempComs = tempObj.GetComponents<Component>();
            List<string> tempListComsName = new List<string>();
            for (int i = 0; i < tempComs.Length; i++)
            {
                Component tempComponent = tempComs[i];
                if (null == tempComponent) continue;
                Type tempType = tempComponent.GetType();
                tempListComsName.Add(tempType.ToString());
            }
        }

        return tempListName;
    }
    private Enum CreateEnum(List<string> al)
    {
        AppDomain domain = Thread.GetDomain();
        AssemblyName name = new AssemblyName();
        name.Name = "EnumAssembly";
        AssemblyBuilder asmBuilder = domain.DefineDynamicAssembly(
        name, AssemblyBuilderAccess.Run);
        ModuleBuilder modBuilder =
        asmBuilder.DefineDynamicModule("EnumModule");
        EnumBuilder enumBuilder = modBuilder.DefineEnum("Language",
        TypeAttributes.Public,
        typeof(System.Int32));
        //string[] al = { "en-UK", "ar-SA", "da-DK", "French", "Cantonese" };
        for (int i = 0; i < al.Count; i++)
        {
            // here al is an array list with a list of string values
            enumBuilder.DefineLiteral(al[i].ToString(), i);
        }
        Type enumType = enumBuilder.CreateType();
        //using debug mode, watch ty, it's type is Language. only has one value "en-UK"
        //also in debug mode, watch "Language", it's an existent enum, type is int.
        Enum enumObj = (Enum)Activator.CreateInstance(enumType);

        return enumObj;
    }

}
