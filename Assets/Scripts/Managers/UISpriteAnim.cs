using UnityEngine;
using System.Collections;

public class UISpriteAnim : MonoBehaviour 
{
    public enum LoopType
    { 
        Once,Loop,PingPong
    }

    public string[] names;
    public float deltaTime = 0.3f;
    public bool isAutoPlay = true;
    public LoopType loopType = LoopType.Once;

    private UISprite uisprite;
    private int index = 0;
    private int delIndex = 1;

    void Awake()
    {
        uisprite = GetComponent<UISprite>();
    }

	void OnEnable () 
    {
        if (isAutoPlay)
        {
            Play();
        }
	}

    void OnDisable()
    {
        Stop();
    }



    public void Play()
    {
        index = 0;
        delIndex = 1;
        CancelInvoke("PlayAnim");
        InvokeRepeating("PlayAnim", 0f, deltaTime);
    }

    public void Stop()
    {
        CancelInvoke("PlayAnim");
    }

    void PlayAnim()
    {
        if (names.Length == 0)
        {
            Stop();
            return;
        }
        uisprite.spriteName = names[index];
        index += delIndex;
        if (index >= names.Length)
        {
            switch(loopType)
            {
                case LoopType.Once:
                    index = 0;
                    Stop();
                    break;
                case LoopType.Loop:
                    index = 0;
                    break;
                case LoopType.PingPong:
                    index = name.Length - 1;
                    delIndex = -1;
                    break;
            }
        }
        if (index < 0 && loopType == LoopType.PingPong)
        {
            index = 0;
            delIndex = 1;
        }
    }



}
