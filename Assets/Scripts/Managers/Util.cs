using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Net.Sockets;
using System;
using UnityEngine.Events;
using System.Text;
using System.Net;
using System.Security.Cryptography;

public class Util : MonoBehaviour
{
    private static Util _instance;
    public static Util Instance
    {
        get
        {
            if (_instance == null)
            {
                string name = "GameManager";
                GameObject obj = GameObject.Find(name);
                if (obj == null)
                {
                    obj = new GameObject(name);
                    _instance = obj.AddComponent<Util>();
                }
                else
                {
                    _instance = obj.GetComponent<Util>();
                    if (_instance == null)
                    {
                        _instance = obj.AddComponent<Util>();
                    }
                }
            }
            return _instance;
        }
    }

    #region IPV6优化

    [DllImport("__Internal")]
    private static extern string getIPv6(string mHost, string mPort);

    private static string GetIPv6(string serverIp, string serverPort)
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            string ipv6 = getIPv6(serverIp, serverPort);
            return ipv6;
        }
        else
        {
            return serverIp + "&&ipv4";
        }
    }

    public static void GetIPType(string serverIp, string serverPort, out string newServerIp, out AddressFamily mIPType)
    {
        mIPType = AddressFamily.InterNetwork;
        newServerIp = serverIp;
        try
        {
            string mIPv6 = GetIPv6(serverIp, serverPort);
            if (!string.IsNullOrEmpty(mIPv6))
            {
                string[] m_StrTemp = System.Text.RegularExpressions.Regex.Split(mIPv6, "&&");
                if (m_StrTemp != null && m_StrTemp.Length >= 2)
                {
                    string IPType = m_StrTemp[1];
                    if (IPType == "ipv6")
                    {
                        newServerIp = m_StrTemp[0];
                        mIPType = AddressFamily.InterNetworkV6;
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("IP解析异常 ： " + e.Message);
        }
    }

    #endregion

    #region 方法调用

    public void DoAction(UnityAction action)
    {
        if (action != null)
        {
            action();
        }
    }


    public void DoAction<T>(UnityAction<T> action, T t)
    {
        if (action != null)
        {
            action.Invoke(t);
        }
    }




    public void DoActionDelay(UnityAction action, float delay)
    {
        StartCoroutine(DoActionDelayCor(action, delay));
    }

    IEnumerator DoActionDelayCor(UnityAction action, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (action != null)
        {
            action.Invoke();
        }
    }




    public void DoActionDelay<T>(UnityAction<T> action, float delay, T t)
    {
        StartCoroutine(DoActionDelayCor(action, delay, t));
    }

    IEnumerator DoActionDelayCor<T>(UnityAction<T> action, float delay, T t)
    {
        yield return new WaitForSeconds(delay);
        if (action != null)
        {
            action.Invoke(t);
        }
    }

    #endregion

    //获取token，长度为120的字符串
    public static string GetToken()
    {
        string str = "0123456789qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM";
        System.Random random = new System.Random();
        StringBuilder token = new StringBuilder();
        for (int i = 0; i < 120; i++)
        {
            int k = random.Next() % 62;
            token.Append(str[k].ToString());
        }
        return token.ToString();
    }

    //获取设备mac地址，若获取失败，随机生成一个mac地址，并保存
    public static string GetMacCode()
    {
        string mac = "";
        if (SystemInfo.deviceUniqueIdentifier != null)
        {
            mac = SystemInfo.deviceUniqueIdentifier;
            if (mac.Length > 50)
            {
                mac = SystemInfo.deviceUniqueIdentifier.Remove(0, SystemInfo.deviceUniqueIdentifier.Length - 50);
            }
        }
        else
        {
            if (PlayerPrefs.HasKey("Mac"))
            {
                mac = PlayerPrefs.GetString("Mac");
            }
            else
            {
                string str = "0123456789qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM";
                System.Random random = new System.Random();
                StringBuilder macCode = new StringBuilder();
                for (int i = 0; i < 50; i++)
                {
                    int k = random.Next() % 62;
                    macCode.Append(str[k].ToString());
                }
                mac = macCode.ToString();
                PlayerPrefs.SetString("Mac", mac);
            }
        }
        return mac;
    }

    //获取IP地址
    public static string GetIPAddress()
    {
        return "";
        //return Network.player.ipAddress;
    }

    /// <summary>
    /// MD5加密
    /// </summary>
    public static string GetMd5(string str)
    {
        MD5 md5 = MD5.Create();
        byte[] byte_md5 = md5.ComputeHash(Encoding.UTF8.GetBytes(str));

        StringBuilder md5Out = new StringBuilder();
        foreach (byte b in byte_md5)
        {
            md5Out.Append(b.ToString("x2"));
        }
        return md5Out.ToString();
    }

    /// <summary>
    /// 将数字转化为大写字符串
    /// </summary>
    /// <returns></returns>
    public static string ConvertNumToString(Int64 num)
    {
        num = (int)(Mathf.Abs(num));
        int num01 = (int)(num / 100000000);                      //单位 ： 亿
        int num02 = (int)((num % 100000000) / 10000);            //单位 ： 万
        int num03 = (int)(num % 10000);                          //个位数

        string result = "";
        if (num01 > 0)
        {
            result += Convert4Digit(num01) + "亿";
        }

        if (num02 >= 1000)
        {
            result += Convert4Digit(num02) + "万";
        }
        else if (num02 > 0)
        {
            if (num01 > 0)
            {
                result += "零" + Convert4Digit(num02) + "万";
            }
            else
            {
                result += Convert4Digit(num02) + "万";
            }
        }

        if (num03 >= 1000)
        {
            if (result.Length > 0)
            {
                if (num02 % 10 > 0)
                {
                    result += Convert4Digit(num03);
                }
                else if (!result.EndsWith("零"))
                {
                    result += "零" + Convert4Digit(num03);
                }
            }
            else
            {
                result += Convert4Digit(num03);
            }
        }
        else if (num03 > 0 && !result.EndsWith("零"))
        {
            result += "零" + Convert4Digit(num03);
        }

        if (result.EndsWith("零"))
        {
            result = result.Remove(result.Length - 1);
        }

        return result;
    }

    /// <summary>
    /// 转换四位数字
    /// </summary>
    private static string Convert4Digit(Int64 num)
    {
        string[] numStrs = { "零", "一", "二", "三", "四", "五", "六", "七", "八", "九" };

        int num01 = (int)(num / 1000);              //单位 ： 仟
        int num02 = (int)((num % 1000) / 100);      //单位 ： 佰
        int num03 = (int)((num % 100) / 10);        //单位 ： 拾 
        int num04 = (int)(num % 10);

        string result = "";
        if (num01 > 0)
        {
            result += numStrs[num01] + "千";
        }

        if (num02 > 0)
        {
            result += numStrs[num02] + "百";
        }
        else if (num01 > 0 && !result.EndsWith("零"))
        {
            result += "零";
        }

        if (num03 > 0)
        {
            result += numStrs[num03] + "十";
        }
        else if (result.Length > 0 && !result.EndsWith("零"))
        {
            result += "零";
        }

        if (num04 > 0)
        {
            result += numStrs[num04];
        }

        if (result.EndsWith("零"))
        {
            result = result.Remove(result.Length - 1);
        }

        return result;
    }

    /// <summary>
    /// 获取金币简称
    /// </summary>
    public static string GetCoinNumStr(Int64 num)
    {
        if (num >= 100000000)
        {
            string res = ((num / 1000000) / 100f) + "亿";
            return res;
        }
        else if (num >= 10000)
        {
            string res = ((num / 100) / 100f) + "万";
            return res;
        }
        return num.ToString();
    }

    /// <summary>
    /// 设备类型
    /// </summary>
    public static Byte GetDeviceType()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            return 0x40;
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            return 0x10;
        }
        return 0x10;
    }

    /// <summary>
    /// 资源加载路径
    /// </summary>
    public static string DataPath
    {
        get
        {
            string appName = AppConfig.AppName.ToLower();
            if (Application.isMobilePlatform)
            {
                if (Application.platform == RuntimePlatform.IPhonePlayer)
                {
                    return Application.temporaryCachePath + "/" + appName + "/";
                }
                else
                {
                    return Application.persistentDataPath + "/" + appName + "/";
                }
            }
            if (Application.platform == RuntimePlatform.WindowsPlayer)
            {
                return Application.streamingAssetsPath + "/";
            }
            //if (AppConst.DebugMode)
            //{
            //    if (Application.isEditor)
            //    {
            //        return Application.dataPath + "/StreamingAssets/";
            //    }
            //}
            return "c:/" + appName + "/";
        }
    }

    /// <summary>
    /// 本地资源存放路径
    /// </summary>
    public static string ResPath
    {
        get
        {
            string path = string.Empty;
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    path = "jar:file://" + Application.dataPath + "!/assets/";
                    break;
                case RuntimePlatform.IPhonePlayer:
                    path = Application.dataPath + "/Raw/";
                    break;
                default:
                    //path = "file://" + Application.dataPath + "/StreamingAssets/";
                    path = Application.dataPath + "/StreamingAssets/";
                    break;
            }
            return path;
        }
    }

    /// <summary>
    /// 校验身份证号码是否符合规范
    /// </summary>
    public static bool CheckIdCard(string idNumber)
    {
        if (idNumber.Length == 18)
        {
            long n = 0;
            if (long.TryParse(idNumber.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(idNumber.Replace('x', '0').Replace('X', '0'), out n) == false)
            {
                return false;//数字验证    
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(idNumber.Remove(2)) == -1)
            {
                return false;//省份验证    
            }
            string birth = idNumber.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证    
            }
            string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            char[] Ai = idNumber.Remove(17).ToCharArray();
            int sum = 0;
            for (int i = 0; i < 17; i++)
            {
                sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
            }
            int y = -1;
            Math.DivRem(sum, 11, out y);
            Console.WriteLine("Y的理论值: " + y);
            if (arrVarifyCode[y] != idNumber.Substring(17, 1).ToLower())
            {
                return false;//校验码验证    
            }
            return true;//符合GB11643-1999标准   
        }
        else
        {
            return false;
        }
    }

    public static Vector3 currentOpPosition
    {
        get
        {
            Vector3 pos = Vector3.zero;
            pos.x = Input.mousePosition.x * 1280 / Screen.width - 1280 / 2;
            pos.y = Input.mousePosition.y * 720 / Screen.height - 720 / 2;
            return pos;
        }
    }

    //自适应屏幕旋转
    public static void AdaptScreenOrientation()
    {
        //自适应翻转
        if (Application.platform == RuntimePlatform.Android)
        {
            Screen.orientation = ScreenOrientation.AutoRotation;
            Screen.autorotateToLandscapeLeft = true;
            Screen.autorotateToLandscapeRight = true;
            Screen.autorotateToPortrait = false;
            Screen.autorotateToPortraitUpsideDown = false;
        }
    }

    //当前是否使用wifi网络
    public static bool IsWifi
    {
        get
        {
            return Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork;
        }
    }

    //获取手机电量信息
    public static float GetBatteryLevel()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            try
            {
                string CapacityString = System.IO.File.ReadAllText("/sys/class/power_supply/battery/capacity");
                return int.Parse(CapacityString)/100f;
            }
            catch (Exception e)
            {
                return 1f;
            }
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            return PluginManager.Instance.GetIOSBatteryLevel();
        }
        else 
        {
            return 1f;
        }
    }

    //复制信息到剪切板
    public static void CopyMessage(string message)
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            PluginManager.Instance.CopyToClipboard(message);
          }
        else
        {
            TextEditor te = new TextEditor();
            te.text = message;
            te.OnFocus();
            te.Copy();
        }
    }

    //获取当前时间 并以秒显示
    public static string GetDateTimeSeconds()
    {
        long xdateseconds = 0;
        DateTime xdatenow = DateTime.UtcNow; //当前UTC时间


        long xminute = 60; //一分种60秒
        long xhour = 3600;
        long xday = 86400;
        long byear = 1970;//从1970-1-1 0：00：00开始到现在所过的秒
        long[] xmonth = { 0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334 };
        long[] xyear = { 365, 366 };
        long num = 0;

        xdateseconds += xdatenow.Second; //算秒
        xdateseconds += xdatenow.Minute * xminute; //算分
        xdateseconds += xdatenow.Hour * xhour; //算时
        xdateseconds += (xdatenow.Day - 1) * xday; //算天

        //算月(月换成天算)
        if (DateTime.IsLeapYear(xdatenow.Year))
        {
            xdateseconds += (xmonth[xdatenow.Month - 1] + 1) * xday;
        }
        else
        {
            xdateseconds += (xmonth[xdatenow.Month - 1]) * xday;
        }

        //算年（年换成天算）
        long lyear = xdatenow.Year - byear;

        for (int i = 0; i < lyear; i++)
        {
            if (DateTime.IsLeapYear((int)byear + i))
            {
                num++;
            }
        }

        xdateseconds += ((lyear - num) * xyear[0] + num * xyear[1]) * xday;

        return xdateseconds.ToString();
    }


    public string GetNameByPropID(int propID, int propCount)
    {
        switch (propID)
        {
            case 0:
                {
                    if (propCount > 50) return "钻石03";
                    if (propCount > 10) return "钻石02";
                    return "钻石01";
                }
            case 1:
                {
                    if (propCount > 5000) return "金币03";
                    if (propCount > 1000) return "金币02";
                    return "金币01";
                }
            case 600: return "flag奖券";
        }
        return "";
    }

    public string GetNameByPropID(int propID)
    {
        switch (propID)
        {
            case 0: return "钻石";
            case 1: return "金币";
            case 600: return "礼券";
        }
        return "";
    }

    //合成64
    public static UInt64 CombineUInt64(UInt32 high, UInt32 low)
    {
        return (((UInt64)high << 32) | low); 
    }

    //高32位
    public static UInt32 GetHighUint32(UInt64 value)
    {
        return (uint)(value >> 32);       // 高32位
    }

    //低32位
    public static UInt32 GetLowUint32(UInt64 value)
    {
        return (uint)(value & 0xFFFFFFFF); // 低32位
    }
}
