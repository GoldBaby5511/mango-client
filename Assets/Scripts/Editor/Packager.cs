using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class Packager
{

    private static List<string> paths = new List<string>();
    private static List<string> files = new List<string>();  

    private static BuildTarget target
    {
        get
        {
#if UNITY_IOS
            return BuildTarget.iOS;
#elif UNITY_ANDROID
            return BuildTarget.Android;
#else
            return BuildTarget.StandaloneWindows;
#endif
        }
    }

    [MenuItem("Tools/Build AssetBundles", false)]
    public static void BulidBaseReource()
    {
        BuildBaseAssetResource();
    }

    public static void BuildBaseAssetResource()
    {

        string resPath = Application.dataPath.ToLower() + "/StreamingAssets/";          //StreamingAssets路径
        string bundlePath = resPath + "AssetBundles/";                                  //assetbundle保存路径
        //string luaPath = resPath + "Lua";                                             //lua文件保存路径

        //创建目录
        if (!Directory.Exists(bundlePath)) Directory.CreateDirectory(bundlePath);
        //打包AssetBundle
        BuildPipeline.BuildAssetBundles(bundlePath, BuildAssetBundleOptions.None, target);


        //复制Lua文件
        //if (!Directory.Exists(luaPath))
        //{
        //    Directory.CreateDirectory(luaPath);
        //}

        EditorUtility.ClearProgressBar();

        //创建文件列表 files 文件
        string fileTextPath = resPath + "files.txt";
        if (File.Exists(fileTextPath))
        {
            File.Delete(fileTextPath);
        }

        paths.Clear();
        files.Clear();
        Recursive(resPath);     //遍历StreamingAssets下所有目录，及其子目录

        FileStream fs = new FileStream(fileTextPath, FileMode.CreateNew);
        StreamWriter sw = new StreamWriter(fs);
        for (int i = 0; i < files.Count; i++)
        {
            string file = files[i];
            if (file.EndsWith(".meta") || file.Contains(".DS_Store")) continue;

            string md5 = Util.GetMd5(file);
            string value = file.Replace(resPath, string.Empty);
            sw.WriteLine(value + "|" + md5);
        }
        sw.Close(); 
        fs.Close();
        AssetDatabase.Refresh();
        Debug.Log("Assets Build Successful");
    }

    /// <summary>
    /// 遍历目录及其子目录
    /// </summary>
    private static void Recursive(string path)
    {
        string[] names = Directory.GetFiles(path);
        string[] dirs = Directory.GetDirectories(path);
        foreach (string filename in names)
        {
            files.Add(filename.Replace('\\', '/'));
        }
        foreach (string dir in dirs)
        {
            paths.Add(dir.Replace('\\', '/'));
            Recursive(dir);
        }
    }
}
