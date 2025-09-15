using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathUtil 
{
    //��Ŀ¼
        public static readonly string AssetPath = Application.dataPath ;
    //��Ҫ��bundle����ԴĿ¼
    public static readonly string BuildResourcesPath = Application.dataPath + "/BuildResources";
    //�������Դ���Ŀ¼��bundle���Ŀ¼��
    public static readonly string BundleOutPath = Application.streamingAssetsPath;

    //<summary>
    //��ȡUnity�����·��
    //</summary>
    //<param name="path"></param>
    ///<returns></returns>
    public static string GetUnityPath(string path)
    {
        //return "Assets" + path.Replace(AssetPath, "").Replace("\\", "/");
        if(string.IsNullOrEmpty(path)) return string.Empty;
        return path.Substring(path.IndexOf("Assets"));
    }

    public static string GetStandardPath(string path)
    {
        if(string.IsNullOrEmpty(path)) return string.Empty;
        return path.Trim().Replace("\\", "/");
    }   
}
