using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class MicroPhoneManager : MonoBehaviour 
{
    private static MicroPhoneManager _instance;
    public static MicroPhoneManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = GameObject.Find("GameManager");
                if (go == null)
                {
                    go = new GameObject("GameManager");
                    _instance = go.AddComponent<MicroPhoneManager>();
                }
                else
                {
                    _instance = go.GetComponent<MicroPhoneManager>();
                    if (_instance == null)
                    {
                        _instance = go.AddComponent<MicroPhoneManager>();
                    }
                }
            }
            return _instance;
        }
    }
    //频率
    public static int frequency = 8000;
    //最大录制时长
    public static int maxRecordTime = 10;
    //最小录制时长
    public static float minRecordTime = 0.3f;
    //最大amr数据长度
    public static int maxAmrDataLen = 1024 * 10;
    //最打wav数据长度
    public static int maxWavDataLen = 1024 * 250;


#if UNITY_IOS
    [DllImport("__Internal")]
    public static extern int WavData2Amr(byte[] wavData, int wavDataLen, byte[] amrData, int amrDataLen);
#else
    [DllImport("misvoice")]
    public static extern int WavData2Amr(byte[] wavData, int wavDataLen, byte[] amrData, int amrDataLen);
#endif

#if UNITY_IOS
    [DllImport("__Internal")]
    public static extern int AmrData2Wav(byte[] amrData, int amrDataLen, byte[] wavData, int wavDataLen);
#else
    [DllImport("misvoice")]
    public static extern int AmrData2Wav(byte[] amrData, int amrDataLen, byte[] wavData, int wavDataLen);
#endif

    //设备是否支持
    public bool isSupport
    {
        get
        {
            return Microphone.devices.Length > 0;
        }
    }
    //是否正在录制
    public bool isRecording
    {
        get
        {
            return Microphone.IsRecording(null);
        }
    }
    //录制的音频
    public AudioClip recordClip = null;
    
    /// <summary>
    /// 开始录音
    /// </summary>
    public void StartRecord()
    {
        if (!isSupport || isRecording)
        {
            return;
        }
        recordClip = Microphone.Start(null, false, maxRecordTime, frequency);
    }

    /// <summary>
    /// 停止录音
    /// </summary>
    public void StopRecord()
    {
        float recordLength = 0;
        int lastPos = Microphone.GetPosition(null);

        if (Microphone.IsRecording(null))
        {
            recordLength = (float)lastPos / frequency;
        }
        else
        {
            recordLength = maxRecordTime;
        }
        Microphone.End(null);
        if(recordLength < minRecordTime)
        {
            recordClip = null;
        }
    }

    /// <summary>
    /// 获取录音数据,最大长度8000
    /// </summary>
    public byte[] GetAudioData()
    {
        //音频转数据
        if (recordClip == null)
        {
            return null;
        }
        float[] audioData = new float[recordClip.samples * recordClip.channels];
        recordClip.GetData(audioData, 0);
        //转码
        byte[] wavData = Float2Byte(audioData);
        byte[] amrData = Wav2Amr(wavData);
        return amrData;
    }

    /// <summary>
    /// 数据转音频文件
    /// </summary>
    public AudioClip GetAudioClip(byte[] data)
    {
        //转码
        byte[] wavData = Amr2Wav(data);

        //还原音频
        float[] clipData = Byte2Float(wavData, wavData.Length);
        
        if (clipData == null || clipData.Length == 0)
        {
            return null;
        }
        AudioClip clip = AudioClip.Create("clip", clipData.Length, 1, frequency, false);
        clip.SetData(clipData, 0);
        return clip;
    }
    
    byte[] Wav2Amr(byte[] wavData)
    {
        byte[] amrData = new byte[maxAmrDataLen];
        int len = WavData2Amr(wavData, wavData.Length, amrData, amrData.Length);
        byte[] data = new byte[len];
        Array.Copy(amrData, data, len);
        return data;
    }

    byte[] Amr2Wav(byte[] amrData)
    {
        byte[] wavData = new byte[maxWavDataLen];
        int len = AmrData2Wav(amrData, amrData.Length, wavData, wavData.Length);
        byte[] data = new byte[len];
        Array.Copy(wavData, data, len);
        return data;
    }

    //byte[] CompressData(byte[] sourceData)
    //{
    //    byte[] desData = ZlibStream.CompressBuffer(sourceData);
    //    return desData;
    //}

    //byte[] DecompressData(byte[] sourceData)
    //{
    //    byte[] desData = ZlibStream.UncompressBuffer(sourceData);
    //    return desData;
    //}
    
    /// <summary>
    /// float数组转byte数组
    /// </summary>
    private byte[] Float2Byte(float[] ary)
    {
        short[] intData = new short[ary.Length];
        byte[] bytesData = new byte[ary.Length * 2];
        int rescaleFactor = 32767; //to convert float to Int16

        for (int i = 0; i < ary.Length; i++)
        {
            intData[i] = (short)(ary[i] * rescaleFactor);
            byte[] byteArr = new byte[2];
            byteArr = BitConverter.GetBytes(intData[i]);
            byteArr.CopyTo(bytesData, i * 2);
        }
        return bytesData;
    }

    private float[] Byte2Float(byte[] ary,int len)
    {
        float[] result = new float[len / 2];
        for (int i = 0; i < result.Length; i++)
        {
            byte[] tmp = new byte[2];
            tmp[0] = ary[i * 2 + 0];
            tmp[1] = ary[i * 2 + 1];
            Int16 tmpShort = BitConverter.ToInt16(tmp, 0);
            result[i] = (float)tmpShort / 32767;
        }
        return result;
    }
}
