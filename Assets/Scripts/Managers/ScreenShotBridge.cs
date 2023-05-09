using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;

public class ScreenShotBridge
{

    private static string filePath = "";

    //IOS参数
    [DllImport("__Internal")]
    private static extern void _SavePhoto(string readAddr);

    public static IEnumerator SaveScreenShot(string fileName, string albumName, bool isScreenShotWithDateTime, Action<string> callBack)
    {
        string finalFileName = "";
        switch (Application.platform)
        { 
            case RuntimePlatform.Android:
                const string path = "com.plugin.screenshot.ScreenShotPlugin";
                ScreenCapture.CaptureScreenshot(fileName + ".png");
                string origin = System.IO.Path.Combine(Application.persistentDataPath, fileName + ".png");
                for (float fPastSec = 0; (!System.IO.File.Exists(origin)) && (fPastSec < 10.0); fPastSec += Time.deltaTime)
                {
                    yield return 0;
                }
                string destination = "/sdcard/" + albumName + "/";
                if (!System.IO.Directory.Exists("/sdcard/" + albumName))
                {
                    System.IO.Directory.CreateDirectory(destination);
                }
                if (System.IO.File.Exists(origin))
                {
                    if (isScreenShotWithDateTime)
                    {
                        finalFileName = destination + "" + fileName + System.DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".png";
                    }
                    else
                    {
                        int totalScreenShots = PlayerPrefs.GetInt("TotalScreenShots", 0);
                        finalFileName = destination + "" + fileName + "_" + totalScreenShots + ".png";
                        totalScreenShots++;
                        PlayerPrefs.SetInt("TotalScreenShots", totalScreenShots);
                    }
                    System.IO.File.Move(origin, finalFileName);
                    filePath = finalFileName;
                    AndroidJavaClass playerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                    AndroidJavaObject activity = playerClass.GetStatic<AndroidJavaObject>("currentActivity");
                    AndroidJavaClass pluginClass = new AndroidJavaClass(path);
                    pluginClass.CallStatic("RefreshGallery", new object[2] { activity, finalFileName });
                }
                break;
            case RuntimePlatform.IPhonePlayer:
                //动态设定截图名字（路径）
                if (isScreenShotWithDateTime)
                {
                    finalFileName = fileName + System.DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".png";
                }
                else
                {
                    int totalScreenShots = PlayerPrefs.GetInt("TotalScreenShots", 0);
                    finalFileName = fileName + "_" + totalScreenShots + ".png";
                    totalScreenShots++;
                    PlayerPrefs.SetInt("TotalScreenShots", totalScreenShots);
                }
                ScreenCapture.CaptureScreenshot(finalFileName);
                string readFilePath = Application.persistentDataPath + "/" + finalFileName;
                filePath = readFilePath;
                for (float fPastSec = 0; (!System.IO.File.Exists(readFilePath)) && (fPastSec < 10.0); fPastSec += Time.deltaTime)
                {
                    yield return 0;
                }
                _SavePhoto(readFilePath);
                break;
            default:
                Debug.Log("截图操作请在真机环境下进行....");
                break;
        }
        //回调
        yield return new WaitForSeconds(0.1f);
        if (callBack != null)
        {
            callBack.Invoke(filePath);
        }
    }


}
