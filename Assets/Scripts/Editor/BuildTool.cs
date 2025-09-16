using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class BuildTool : Editor
{

    [MenuItem("Tools/Build Windows Bundles")]
    static void BuildWindowsBundles()
    {
       Build(BuildTarget.StandaloneWindows64);
    }

    [MenuItem("Tools/Build Android Bundles")]
    static void BuildAndroidBundles()
    {
        Build(BuildTarget.Android);
    }

    [MenuItem("Tools/Build iOS Bundles")]
    static void BuildiOSBundles()
    {
        Build(BuildTarget.iOS);
    }

    static void Build(BuildTarget target)
   {
        List<AssetBundleBuild> assetBundleBuilds = new List<AssetBundleBuild>();

        //文件信息列表
        List<string> bundleInfos = new List<string>();

        string[] files = Directory.GetFiles(PathUtil.BuildResourcesPath, "*", SearchOption.AllDirectories);
        for (int i = 0; i < files.Length; i++)
        {
            if (files[i].EndsWith(".meta")) continue;
           
            AssetBundleBuild assetBundle = new AssetBundleBuild();

            string fileName = PathUtil.GetStandardPath(files[i]);
            Debug.Log("file:"+fileName);

            string assetName = PathUtil.GetUnityPath(files[i]);
            assetBundle.assetNames = new string[] { assetName };
            string bundleName = assetName.Replace("Assets/BuildResources/", "").Replace(Path.GetExtension(assetName), "").ToLower();
            assetBundle.assetBundleName = bundleName+".ab";
            assetBundleBuilds.Add(assetBundle);

            //添加文件和依赖信息
            List<string> dependenceInfo = GetDependence(assetName);
            string bundleInfo = assetName + "|" + bundleName + ".ab";

            if (dependenceInfo.Count > 0)
                bundleInfo = bundleInfo + "|" + string.Join("|", dependenceInfo);

            bundleInfos.Add(bundleInfo);
        }

        //删除旧的bundle目录，重新创建
        if (Directory.Exists(PathUtil.BundleOutPath)==false)
        {
            Directory.CreateDirectory(PathUtil.BundleOutPath);
        }
        else
        {
            Directory.Delete(PathUtil.BundleOutPath,true);
            Directory.CreateDirectory(PathUtil.BundleOutPath);
        }
        BuildPipeline.BuildAssetBundles(PathUtil.BundleOutPath, assetBundleBuilds.ToArray(), BuildAssetBundleOptions.ChunkBasedCompression, target);

        File.WriteAllLines(PathUtil.BundleOutPath + "/" + AppConst.FileListName, bundleInfos);

        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 获取依赖文件列表
    /// </summary>
    /// <param name="curFile"></param>
    /// <returns></returns>
    static List<string> GetDependence(string curFile)
    {
        List<string> dependence = new List<string>();
        string[] files = AssetDatabase.GetDependencies(curFile);
        dependence = files.Where(file => !file.EndsWith(".cs") && !file.Equals(curFile)).ToList();
        return dependence;
    }
}
