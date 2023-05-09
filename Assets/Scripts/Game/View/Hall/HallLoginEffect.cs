using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallLoginEffect : MonoBehaviour 
{
    public Sprite[] sprite_0;
    public Sprite[] sprite_1;
    public Sprite[] sprite_2;
    public Sprite[] sprite_3;
    public Sprite[] sprite_4;
    public Sprite[] sprite_5;

    private GameObject[] effects = new GameObject[12];
    private List<Sprite[]> spriteList = new List<Sprite[]>();


    void Awake()
    {
        for (int i = 0; i < 12; i++)
        {
            effects[i] = transform.Find("effect_" + i).gameObject;
        }
        spriteList.Add(sprite_0);
        spriteList.Add(sprite_1);
        spriteList.Add(sprite_2);
        spriteList.Add(sprite_3);
        spriteList.Add(sprite_4);
        spriteList.Add(sprite_5);

        for (int i = 0; i < 12; i++)
        {
            effects[i].SetActive(true);
        }
    }

    void Start()
    {
        PlayAnim(0);
        PlayAnim(4);
        PlayAnim(11);
        PlayAnim(7);
    }

    void PlayAnim(int index)
    {
        effects[index].SetActive(true);
        effects[index].GetComponent<UI2DSpriteAnimation>().Pause();
        effects[index].GetComponent<UI2DSpriteAnimation>().frames = spriteList[Random.Range(0, spriteList.Count)];
        effects[index].GetComponent<UI2DSpriteAnimation>().ResetToBeginning();
        effects[index].GetComponent<UI2DSpriteAnimation>().Play();
    }


	void Update () 
    {
        int count1 = 0;
        int count2 = 0;
        for (int i = 0; i < effects.Length; i++)
        {
            if (effects[i].activeSelf)
            {
                if (!effects[i].GetComponent<UI2DSpriteAnimation>().isPlaying)
                {
                    effects[i].SetActive(false);
                    int index = 0;
                    if (i < 7)
                    {
                        index = Random.Range(0, 7);
                    }
                    else
                    {
                        index = Random.Range(7, 12);
                    }
                    PlayAnim(index);
                }
                if (i < 7)
                {
                    count1++;
                }
                else
                {
                    count2++;
                }
            }
        }
        if (count1 < 2)
        {
            PlayAnim(Random.Range(0, 7));
        }
        if (count2 < 2)
        {
            PlayAnim(Random.Range(7, 12));
        }
	}

}
