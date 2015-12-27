using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;


/// <summary>
/// author huailiang.peng
/// 离线资源打包工具
/// </summary>

public class AssetbundleEditor : EditorWindow
{

    private int platformInt = 1;
    private string[] platformString = new string[] { "Android", "IOS", "WP", "WIN", "OSX" };
    private int[] platArray = new int[] { 0, 1, 2, 3, 4, 5 };

     public static string selectPath = Application.dataPath;

    public static AssetbundleEditor  window;

    [MenuItem("Tools/AssetbundleTools #&p")]
    private static void Init()
    {
        selectPath = selectPath.Remove(selectPath.Length-6)+"AssetsBundle/";
        window = (AssetbundleEditor)GetWindow(typeof(AssetbundleEditor), true, "离线资源打包工具");
        window.Show();
    }


    private void OnGUI()
    {
        GUILayout.Label("设置一些打包参数");
        GUILayout.Space(5);
        GUILayout.BeginHorizontal();
        platformInt = EditorGUILayout.IntPopup("选择平台: ", platformInt, platformString, platArray);
        GUILayout.EndHorizontal();
        GUILayout.Space(5);
        GUILayout.BeginHorizontal();
        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("选择路径"))
        {
            selectPath = GetSaveDirPath();
            Debug.Log("select path: "+selectPath);
        }
        GUI.backgroundColor = Color.white;
        EditorGUILayout.TextField(selectPath);
        GUILayout.EndHorizontal();

        GUILayout.Space(25);

        GUILayout.Label("选择要打包的内容");
        GUILayout.Space(5);

        GUI.backgroundColor = Color.red;
        GUILayout.BeginHorizontal();
        GUILayout.Label("小葵打包(selection)");
        if (GUILayout.Button("Package"))
        {
            PackHimaAssets(GetBuildTarget());
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        GUILayout.Label("小葵打包   (prefab) ");
        if (GUILayout.Button("Package"))
        {
            PackHimaAssetsFixed(GetBuildTarget());
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(10);
        GUI.backgroundColor = Color.white;

        GUILayout.BeginHorizontal();
        GUILayout.Label("选择打包");
        if (GUILayout.Button("Package"))
        {
            BuildSelectAssets(GetBuildTarget());
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(10);
    }
   

    public BuildTarget GetBuildTarget()
    {
        if (platformInt == 0)
        {
            return BuildTarget.Android;
        }
        else if (platformInt == 1)
        {
            return BuildTarget.iPhone;
        }
        else if (platformInt == 2)
        {
            return BuildTarget.WP8Player;
        }
        else if (platformInt == 3)
        {
            return BuildTarget.StandaloneWindows;
        }
        else if (platformInt == 4)
        {
            return BuildTarget.StandaloneOSXUniversal;
        }

        return BuildTarget.iPhone;
    }
   
    /// <summary>
    /// 资源文件目录
    /// </summary>
    public static string GetSaveDirPath()
    {
        return EditorUtility.SaveFolderPanel("选择目录", selectPath, "select");
    }
    
    public static void BuildSelectAssets(BuildTarget target)
    {
        string path = AssetbundleEditor.selectPath;
        if (path.Length != 0)
        {
            foreach (var item in Selection.objects)
            {
                Debug.Log("se: " + item.name);
            }
            // 选择的要保存的对象
            UnityEngine.Object[] selection = Selection.GetFiltered(typeof(UnityEngine.Object),SelectionMode.DeepAssets);
            
            //打包
            BuildPipeline.BuildAssetBundle(null, selection, path + "/" + selection[0].name+".assetbundle",
                                           BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets, target);
        }
    }
    
    
    /// <summary>
    /// 小葵资源打包
    /// </summary>
    public static void PackHimaAssets(BuildTarget target)
    {
        string path = AssetbundleEditor.selectPath;
        if (path.Length != 0)
        {
            foreach (var item in Selection.objects)
            {
                Debug.Log("se: " + item.name);
            }
            UnityEngine.Object[] selection = Selection.objects;

            BuildPipeline.BuildAssetBundle(null, selection, path + "/" + "CHR_Himawari.assetbundle",
                                           BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets, target);
        }
    }
    
    /// <summary>
    /// 固定场景小葵资源打包
    /// </summary>
    public static void PackHimaAssetsFixed(BuildTarget target)
    {
        UnityEngine.Object obj = Resources.LoadAssetAtPath<UnityEngine.Object>("Assets/NonBuildRes/assets/CHR_Himawari.prefab");
        Build(obj, AssetbundleEditor.selectPath, "CHR_Himawari", target);
    }

    
    public static void Build(UnityEngine.Object obj, string path, string name, BuildTarget target)
    {
        if (!Directory.Exists(path)) { EditorUtility.DisplayDialog("提示", "不存在路径", "确定"); }
        else
        {
            BuildPipeline.BuildAssetBundle(obj, null, path + "/" + name + ".assetbundle",
                                           BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets, target);
            
            EditorUtility.DisplayDialog("提示", "打包完成", "提示");
        }
    }


}
