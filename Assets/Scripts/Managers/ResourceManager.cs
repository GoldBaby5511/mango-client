using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System;

public class ResourceManager : MonoBehaviour 
{
    private static ResourceManager _instance;
    public static ResourceManager Instance
    {
        get
        {

            if (_instance == null)
            {
                string name = "GameManager";
                GameObject manager = GameObject.Find("GameManager");
                if (manager == null)
                {
                    manager = new GameObject(name);
                    _instance = manager.AddComponent<ResourceManager>();
                }
                else
                {
                    _instance = manager.GetComponent<ResourceManager>();
                    if (_instance == null)
                    {
                        _instance = manager.AddComponent<ResourceManager>();
                    }
                }
            }
            return _instance;
        }
    }

    [HideInInspector]
    public GameUpdateState hallUpdateState = GameUpdateState.Null;




    /// <summary>
    /// 检查游戏资源更新状态
    /// </summary>
    public void CheckUpdateState(Action callback)
    {
        StartCoroutine(CheckUpdateStateCor(callback));
    }

    IEnumerator CheckUpdateStateCor(Action callback)
    {
        //远程file文件
        string remoteFile = "";
        if (AppConfig.IsLocalMode)
        {
            remoteFile = Util.ResPath + "files.txt";
        }
        else
        {
            remoteFile = AppConfig.resUpdateUrl + "files.txt";
        }

        //本地file文件
        string localFile = Util.DataPath + "files.txt";
        if (!Directory.Exists(Util.DataPath))
        {
            Directory.CreateDirectory(Util.DataPath);
        }
        if (File.Exists(localFile))
        {
            File.Delete(localFile);
        }
        //加载远程file文件到本地
        WWW www = new WWW(remoteFile);
        yield return www;
        if (www.error != null)
        {
            Debug.LogError("加载files文件失败！");
            yield break;
        }
        File.WriteAllBytes(localFile, www.bytes);
        string[] files = File.ReadAllLines(localFile);
        //检查大厅更新状态
        for (int i = 0; i < files.Length; i++)
        {
            if (string.IsNullOrEmpty(files[i]))
            {
                continue;
            }
            string[] keyValue = files[i].Split('|');
            string localAssetFile = (Util.DataPath + keyValue[0]).Trim();
            string fileName = Path.GetFileName(localAssetFile);
            if (fileName.ToLower().StartsWith("hall") || fileName.ToLower().StartsWith("normal"))
            {
                string path = Path.GetDirectoryName(localAssetFile);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                if (File.Exists(localAssetFile))
                {
                    string remoteMd5 = keyValue[1].Trim();
                    string localMd5 = Util.GetMd5(localAssetFile);
                    if (!string.Equals(remoteMd5, localMd5))
                    {
                        if (hallUpdateState < GameUpdateState.Update)
                        {
                            hallUpdateState = GameUpdateState.Update;
                        }
                    }
                }
                else
                {
                    hallUpdateState = GameUpdateState.Download;
                }
            }
        }
        //检查游戏更新状态
        //foreach (GameInfo game in AppConfig.gameDic.Values)
        //{
        //    string gameName = game.flag.ToString();
        //    for (int i = 0; i < files.Length; i++)
        //    {
        //        if (string.IsNullOrEmpty(files[i]))
        //        {
        //            continue;
        //        }
        //        string[] keyValue = files[i].Split('|');
        //        string localAssetFile = (Util.DataPath + keyValue[0]).Trim();
        //        string fileName = Path.GetFileName(localAssetFile);
        //        if (fileName.ToLower().StartsWith(gameName.ToLower()))
        //        {
        //            string path = Path.GetDirectoryName(localAssetFile);
        //            if (!Directory.Exists(path))
        //            {
        //                Directory.CreateDirectory(path);
        //            }
        //            if (File.Exists(localAssetFile))
        //            {
        //                string remoteMd5 = keyValue[1].Trim();
        //                string localMd5 = Util.GetMd5(localAssetFile);
        //                if (!string.Equals(remoteMd5, localMd5))
        //                {
        //                    if (game.updateState < GameUpdateState.Update)
        //                    {
        //                        game.updateState = GameUpdateState.Update;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                game.updateState = GameUpdateState.Download;
        //            }
        //        }
        //    }
        //}
        //检查完成后回调
        yield return new WaitForEndOfFrame();
        if (callback != null)
        {
            callback.Invoke();
        }
    }






    /// <summary>
    /// 下载更新资源
    /// </summary>
    public void DownloadUpdateAsset(GameFlag flag, Action callback)
    {
        StartCoroutine(DownloadUpdateAssetCor(flag, callback));
    }

    IEnumerator DownloadUpdateAssetCor(GameFlag flag, Action callback)
    {
        //下载文件列表
        string url = AppConfig.resUpdateUrl;
        List<DownFile> downloadFiles = new List<DownFile>();
        //读取本地files文件
        string localFile = Util.DataPath + "files.txt";
        string[] files = File.ReadAllLines(localFile);
        //释放本地资源
        if (!AppConfig.IsLocalMode)
        {
            for (int i = 0; i < files.Length; i++)
            {
                if (string.IsNullOrEmpty(files[i]))
                {
                    continue;
                }
                string[] keyValue = files[i].Split('|');
                string localAssetFile = (Util.DataPath + keyValue[0]).Trim();
                string path = Path.GetDirectoryName(localAssetFile);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                //本地文件不存在
                if (!File.Exists(localAssetFile))
                {
                    WWW www = new WWW(Util.ResPath + keyValue[0].Trim());
                    yield return www;
                    if (www.error == null)
                    {
                        File.WriteAllBytes(localAssetFile, www.bytes);
                    }
                    else
                    {
                        Debug.LogError("加载本地文件失败！");
                    }
                }
            }
        }
        //检查更新内容
        for (int i = 0; i < files.Length; i++)
        {
            if (string.IsNullOrEmpty(files[i]))
            {
                continue;
            }
            string[] keyValue = files[i].Split('|');
            string localAssetFile = (Util.DataPath + keyValue[0]).Trim();
            string fileName = Path.GetFileName(localAssetFile);
            
            if (flag == GameFlag.Hall)
            {
                //大厅
                if (fileName.ToLower().StartsWith("hall") || fileName.ToLower().StartsWith("normal"))
                { 
                    string path = Path.GetDirectoryName(localAssetFile);
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    if (File.Exists(localAssetFile))
                    {
                        //文件本地已存在
                        string remoteMd5 = keyValue[1].Trim();
                        string localMd5 = Util.GetMd5(localAssetFile);
                        if (!string.Equals(remoteMd5, localMd5))
                        {
                            if (AppConfig.IsLocalMode)
                            {
                                downloadFiles.Add(new DownFile((Util.ResPath + keyValue[0]).Trim(), (Util.DataPath + keyValue[0]).Trim()));
                            }
                            else
                            {
                                downloadFiles.Add(new DownFile((url + keyValue[0]).Trim(), (Util.DataPath + keyValue[0]).Trim()));
                            }
                        }
                    }
                    else
                    { 
                        //文件本地不存在
                        if (AppConfig.IsLocalMode)
                        {
                            downloadFiles.Add(new DownFile((Util.ResPath + keyValue[0]).Trim(), (Util.DataPath + keyValue[0]).Trim()));
                        }
                        else
                        {
                            downloadFiles.Add(new DownFile((url + keyValue[0]).Trim(), (Util.DataPath + keyValue[0]).Trim()));
                        }
                    }
                }
            }
            else
            { 
                //游戏
                string flagName = flag.ToString();
                if (fileName.ToLower().StartsWith(flagName))
                {
                    string path = Path.GetDirectoryName(localAssetFile);
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    if (File.Exists(localAssetFile))
                    {
                        //文件本地已存在
                        string remoteMd5 = keyValue[1].Trim();
                        string localMd5 = Util.GetMd5(localAssetFile);
                        if (!string.Equals(remoteMd5, localMd5))
                        {
                            if (AppConfig.IsLocalMode)
                            {
                                downloadFiles.Add(new DownFile((Util.ResPath + keyValue[0]).Trim(), (Util.DataPath + keyValue[0]).Trim()));
                            }
                            else
                            {
                                downloadFiles.Add(new DownFile((url + keyValue[0]).Trim(), (Util.DataPath + keyValue[0]).Trim()));
                            }
                        }
                    }
                    else
                    {
                        //文件本地不存在
                        if (AppConfig.IsLocalMode)
                        {
                            downloadFiles.Add(new DownFile((Util.ResPath + keyValue[0]).Trim(), (Util.DataPath + keyValue[0]).Trim()));
                        }
                        else
                        {
                            downloadFiles.Add(new DownFile((url + keyValue[0]).Trim(), (Util.DataPath + keyValue[0]).Trim()));
                        }
                    }
                }
            }
            
        }
        yield return new WaitForEndOfFrame();
        //下载更新
        if (downloadFiles.Count > 0)
        {
            string random = "?v=" + DateTime.Now.ToString("yyyymmddhhmmss");        //随机数
            for (int i = 0; i < downloadFiles.Count; i++)
            {
                WWW www = new WWW(downloadFiles[i].url + random);
                yield return www;
                if (www.error == null)
                {
                    File.WriteAllBytes(downloadFiles[i].localPath, www.bytes); 
                }
                else
                {
                    Debug.LogError("游戏更新异常");
                }
            }
        }
        //下载完成后执行回调
        yield return new WaitForEndOfFrame();
        if (callback != null)
        {
            callback.Invoke();
        }
    }





    /// <summary>
    /// 创建单个面板
    /// </summary>
    public void CreatePanel(string bundleName, string prefabName, Transform parent)
    {
        AssetBundle bundle = LoadBundle(bundleName);
        StartCoroutine(StartCreatePanel(bundle, prefabName, parent));
    }

    IEnumerator StartCreatePanel(AssetBundle bundle, string name, Transform parent)
    {
        GameObject prefab = bundle.LoadAsset(name, typeof(GameObject)) as GameObject;
        yield return new WaitForEndOfFrame();
        if (parent.Find(name) != null || prefab == null)
        {
            yield break;
        }
        GameObject go = Instantiate(prefab) as GameObject;
        go.name = name;
        go.layer = prefab.layer;
        go.transform.parent = parent;
        go.transform.localScale = prefab.transform.localScale;
        go.transform.localPosition = prefab.transform.localPosition;
        yield return new WaitForEndOfFrame();
    }





    /// <summary>
    /// 批量创建面板
    /// </summary>
    public void CreatePanels(string bundleName, string[] prefabNames, Transform[] parents)
    {
        AssetBundle bundle = LoadBundle(bundleName);
        StartCoroutine(StartCreatePanels(bundle, prefabNames, parents));
    }

    IEnumerator StartCreatePanels(AssetBundle bundle, string[] names, Transform[] parents)
    {
        if (names.Length != parents.Length)
        {
            Debug.LogError("批量创建面板时，必须指定每一个prefab的parent");
            yield break;
        }
        for (int i = 0; i < names.Length; i++)
        {
            GameObject prefab = bundle.LoadAsset(name, typeof(GameObject)) as GameObject;
            bundle.Unload(false);
            yield return new WaitForEndOfFrame();
            if (parents[i].Find(name) != null || prefab == null)
            {
                Debug.LogError("创建prefab失败，当前parent下已存在prefab对象");
                yield break;
            }
            GameObject go = Instantiate(prefab) as GameObject;
            go.name = name;
            go.layer = prefab.layer;
            go.transform.parent = parents[i];
            go.transform.localScale = prefab.transform.localScale;
            go.transform.localPosition = prefab.transform.localPosition;
            yield return new WaitForEndOfFrame();
        }
    }



    // 加载AssetBundle
    private AssetBundle LoadBundle(string name)
    {
        byte[] stream = null;
        AssetBundle bundle = null;
        string url = Util.DataPath + name.ToLower() + ".assetbundle";
        stream = File.ReadAllBytes(url);
        bundle = AssetBundle.LoadFromMemory(stream);
        return bundle;
    }

}

/// <summary>
/// 下载文件
/// </summary>
public class DownFile
{
    public string url;          //下载地址
    public string localPath;    //本地存放地址

    public DownFile(string _url, string _path)
    {
        url = _url;
        localPath = _path;
    }
}
