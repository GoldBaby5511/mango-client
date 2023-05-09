using UnityEngine;
using System.Collections;
using System.Collections.Generic;


enum DDZ_POKER_TYPE {
    DDZ_PASS    = 0,   //过牌，不出
    SINGLE  = 1,//单牌
    TWIN    = 2,//对子
    TRIPLE  = 3,//三张
    TRIPLE_WITH_SINGLE   = 4,//三带一
    TRIPLE_WITH_TWIN     = 5,//三带对
    STRAIGHT_SINGLE = 6,//顺子（单顺）
    STRAIGHT_TWIN   = 7,//连对（双顺）
    PLANE_PURE      = 8,//飞机（三顺）
    PLANE_WITH_SINGLE    = 9,//飞机带单
    PLANE_WITH_TWIN      = 10,//飞机带双
    FOUR_WITH_SINGLE  = 11,//四带单
    FOUR_WITH_TWIN  = 12,//四带双
    FOUR_BOMB       = 13,//炸弹
    KING_BOMB       = 14,//王炸
}

public class LandlordsModel
{
    #region audio
    public static AudioClip audioStartGame;
    public static AudioClip audioClickCard;
    public static AudioClip audioOutCard;
    public static AudioClip audioSendCard;
    public static AudioClip audioGameFail;
    public static AudioClip audioGameWin;
    public static AudioClip audioTimer;

    public static AudioClip audioCallPolice, audioFeiJi, audioShunZi, audioZhaDan;

    //男配音
    public static AudioClip[] man_DanZhang = new AudioClip[15];  
    public static AudioClip[] man_DuiZi = new AudioClip[13];
    public static AudioClip[] man_SanZhang = new AudioClip[13];
    public static AudioClip   man_lianDui,  man_sanDaiYi, man_sanDaiDui, man_shunZi, man_siDaiEr, man_siDaiLiangDui, man_zhaDan, man_wangZha, man_feiJi;
    public static AudioClip[] man_pass = new AudioClip[4];
    public static AudioClip[] man_gradLandlord = new AudioClip[3];
    public static AudioClip man_notDouble, man_double, man_notCall, man_notGrad, man_callLandlord;


    //女配音
    public static AudioClip[] woman_DanZhang = new AudioClip[15];
    public static AudioClip[] woman_DuiZi = new AudioClip[13];
    public static AudioClip[] woman_SanZhang = new AudioClip[13];
    public static AudioClip  woman_lianDui, woman_sanDaiYi, woman_sanDaiDui, woman_shunZi, woman_siDaiEr, woman_siDaiLiangDui, woman_zhaDan, woman_wangZha, woman_feiJi;
    public static AudioClip[] woman_pass = new AudioClip[4];
    public static AudioClip[] woman_gradLandlord = new AudioClip[3];
    public static AudioClip woman_notDouble, woman_double, woman_notCall, woman_notGrad, woman_callLandlord;

    #endregion

    #region 常量
    public static int callTime = 0;//叫分时间
    public static int addTime = 0;//加倍时间
    public static int firstOutCardTime = 0;//首出时间
    public static int outCardTime = 0;//出牌时间
    public static int canotAfford = 0;//要不起

    public static int GAME_NUM = 3;//游戏人数
    public static int NORMAL_HANDCARDNUM = 17;//正常手牌数目
    public static int MAX_COUNT = 20;//最大出牌数目
    public static int FULL_COUNT = 54;//全牌数目
    #endregion

    #region 暂存
    public static int currentUser;//当前玩家
    public static int nextUser;//下个玩家
    public static int currentCallScore;//当前叫分
    public static int lastCallScore;//上次叫分玩家

    public static int giveUpOutCardUser;//不出玩家

    public static int bankerChairdId = 65535;//庄家座位号
    public static int bankerCallScore;//庄家叫分

    public static int finallyOutCardType = 0;   //最后出牌类型（游戏结束前最后一手牌的类型）
    public static byte[] myHandCards = new byte[NORMAL_HANDCARDNUM];//自己的初始手牌
    public static byte[] bankerHoleCards = new byte[3];//庄家底牌

    public static bool isPlayerCallLandlord = false; //是否玩家叫庄
    public static bool[] isPlayerTrustee = new bool[GAME_NUM];             //是否托管中
    public static byte[] playerInGame = new byte[GAME_NUM];    //玩家游戏状态

    public static Dictionary<int, Poker> pokers = new Dictionary<int, Poker>();

    #endregion

    #region 初始化音效
    public static void InitAudio()
    {
        audioOutCard = Resources.Load<AudioClip>("sound/Landlords/effect/outCard");
        audioSendCard = Resources.Load<AudioClip>("sound/Landlords/effect/sendCard");
        audioGameFail = Resources.Load<AudioClip>("sound/Landlords/effect/gameLost");
        audioGameWin = Resources.Load<AudioClip>("sound/Landlords/effect/gameWin");
        audioTimer = Resources.Load<AudioClip>("sound/Landlords/effect/timer");

        //audioCallPolice = Resources.Load<AudioClip>("sound/Landlords/effect/callPolice");
        audioFeiJi = Resources.Load<AudioClip>("sound/Landlords/effect/feiJi");
        audioShunZi = Resources.Load<AudioClip>("sound/Landlords/effect/shunZi");
        audioZhaDan = Resources.Load<AudioClip>("sound/Landlords/effect/zhaDan");

        for (int i = 0; i < 15; i++)
		{
            man_DanZhang[i] = Resources.Load<AudioClip>("sound/Landlords/man/Man_" + i);
            woman_DanZhang[i] = Resources.Load<AudioClip>("sound/Landlords/woman/Woman_" + i);
            if (i < 13)
            {
                man_DuiZi[i] = Resources.Load<AudioClip>("sound/Landlords/man/Man_dui" + i);
                man_SanZhang[i] = Resources.Load<AudioClip>("sound/Landlords/man/Man_tuple" + i);                
                woman_DuiZi[i] = Resources.Load<AudioClip>("sound/Landlords/woman/Woman_dui" + i);
                woman_SanZhang[i] = Resources.Load<AudioClip>("sound/Landlords/woman/Woman_tuple" + i);

                if (i < 4)
                {
                    man_pass[i] = Resources.Load<AudioClip>("sound/Landlords/man/Man_BPass_" + i);
                    woman_pass[i] = Resources.Load<AudioClip>("sound/Landlords/woman/Woman_BPass_" + i);
                    if (i < 3)
                    {
                        man_gradLandlord[i] = Resources.Load<AudioClip>("sound/Landlords/man/Man_Q-qiangdizhu" + i);
                        woman_gradLandlord[i] = Resources.Load<AudioClip>("sound/Landlords/woman/Woman_Q-qiangdizhu" + i);
                    }
                }

            }
		}
        man_lianDui = Resources.Load<AudioClip>("sound/Landlords/man/Man_liandui");
        man_sanDaiYi = Resources.Load<AudioClip>("sound/Landlords/man/Man_sandaiyi");
        man_sanDaiDui = Resources.Load<AudioClip>("sound/Landlords/man/Man_sandaiyidui");
        man_shunZi = Resources.Load<AudioClip>("sound/Landlords/man/Man_shunzi");
        man_siDaiEr = Resources.Load<AudioClip>("sound/Landlords/man/Man_sidaier");
        man_siDaiLiangDui = Resources.Load<AudioClip>("sound/Landlords/man/Man_sidailiangdui");
        man_zhaDan = Resources.Load<AudioClip>("sound/Landlords/man/Man_zhadan");
        man_wangZha = Resources.Load<AudioClip>("sound/Landlords/man/Man_wangzha");
        man_feiJi = Resources.Load<AudioClip>("sound/Landlords/man/Man_Z-feiji");
        man_notDouble = Resources.Load<AudioClip>("sound/Landlords/man/Man_C-bujiabei");
        man_double = Resources.Load<AudioClip>("sound/Landlords/man/Man_C-jiabei");
        man_notCall = Resources.Load<AudioClip>("sound/Landlords/man/Man_F-bujiao");
        man_notGrad = Resources.Load<AudioClip>("sound/Landlords/man/Man_F-buqiang");
        man_callLandlord = Resources.Load<AudioClip>("sound/Landlords/man/Man_J-jiaodihzu");

        woman_lianDui = Resources.Load<AudioClip>("sound/Landlords/woman/Woman_liandui");
        woman_sanDaiYi = Resources.Load<AudioClip>("sound/Landlords/woman/Woman_sandaiyi");
        woman_sanDaiDui = Resources.Load<AudioClip>("sound/Landlords/woman/Woman_sandaiyidui");
        woman_shunZi = Resources.Load<AudioClip>("sound/Landlords/woman/Woman_shunzi");
        woman_siDaiEr = Resources.Load<AudioClip>("sound/Landlords/woman/Woman_sidaier");
        woman_siDaiLiangDui = Resources.Load<AudioClip>("sound/Landlords/woman/Woman_sidailiangdui");
        woman_wangZha = Resources.Load<AudioClip>("sound/Landlords/woman/Woman_wangzha");
        woman_zhaDan = Resources.Load<AudioClip>("sound/Landlords/woman/Woman_zhadan");
        woman_feiJi = Resources.Load<AudioClip>("sound/Landlords/woman/Woman_Z-feiji");
        woman_notDouble = Resources.Load<AudioClip>("sound/Landlords/woman/Woman_C-bujiabei");
        woman_double = Resources.Load<AudioClip>("sound/Landlords/woman/Woman_C-jiabei");
        woman_notCall = Resources.Load<AudioClip>("sound/Landlords/woman/Woman_F-bujiao");
        woman_notGrad = Resources.Load<AudioClip>("sound/Landlords/woman/Woman_F-buqiang");
        woman_callLandlord = Resources.Load<AudioClip>("sound/Landlords/woman/Woman_J-jiaodizhu");
    }
    #endregion

    // 数据初始化
    public static void InitData()
    {
        myHandCards = new byte[NORMAL_HANDCARDNUM];
        bankerHoleCards = new byte[3];
        isPlayerTrustee = new bool[GAME_NUM];
        playerInGame = new byte[GAME_NUM];  
        isPlayerCallLandlord = false;
        bankerChairdId = 65535;
        finallyOutCardType = 0; 
    }

    /// <summary>
    /// 初始化扑克牌信息
    /// </summary>
    public static void InitPoker()
    {
        pokers.Clear();
        pokers.Add(0, new Poker(0, "牛牛牌背", 0, 0));
        pokers.Add(78, new Poker(78, "小王", 21, 5));
        pokers.Add(79, new Poker(79, "大王", 22, 5));

        pokers.Add(1, new Poker(1, "方块1", 14, 1));
        pokers.Add(2, new Poker(2, "方块2", 15, 1));
        pokers.Add(3, new Poker(3, "方块3", 3, 1));
        pokers.Add(4, new Poker(4, "方块4", 4, 1));
        pokers.Add(5, new Poker(5, "方块5", 5, 1));
        pokers.Add(6, new Poker(6, "方块6", 6, 1));
        pokers.Add(7, new Poker(7, "方块7", 7, 1));
        pokers.Add(8, new Poker(8, "方块8", 8, 1));
        pokers.Add(9, new Poker(9, "方块9", 9, 1));
        pokers.Add(10, new Poker(10, "方块10", 10, 1));
        pokers.Add(11, new Poker(11, "方块J", 11, 1));
        pokers.Add(12, new Poker(12, "方块Q", 12, 1));
        pokers.Add(13, new Poker(13, "方块K", 13, 1));

        pokers.Add(17, new Poker(17, "梅花1", 14, 2));
        pokers.Add(18, new Poker(18, "梅花2", 15, 2));
        pokers.Add(19, new Poker(19, "梅花3", 3, 2));
        pokers.Add(20, new Poker(20, "梅花4", 4, 2));
        pokers.Add(21, new Poker(21, "梅花5", 5, 2));
        pokers.Add(22, new Poker(22, "梅花6", 6, 2));
        pokers.Add(23, new Poker(23, "梅花7", 7, 2));
        pokers.Add(24, new Poker(24, "梅花8", 8, 2));
        pokers.Add(25, new Poker(25, "梅花9", 9, 2));
        pokers.Add(26, new Poker(26, "梅花10", 10, 2));
        pokers.Add(27, new Poker(27, "梅花J", 11, 2));
        pokers.Add(28, new Poker(28, "梅花Q", 12, 2));
        pokers.Add(29, new Poker(29, "梅花K", 13, 2));

        pokers.Add(33, new Poker(33, "红桃1", 14, 3));
        pokers.Add(34, new Poker(34, "红桃2", 15, 3));
        pokers.Add(35, new Poker(35, "红桃3", 3, 3));
        pokers.Add(36, new Poker(36, "红桃4", 4, 3));
        pokers.Add(37, new Poker(37, "红桃5", 5, 3));
        pokers.Add(38, new Poker(38, "红桃6", 6, 3));
        pokers.Add(39, new Poker(39, "红桃7", 7, 3));
        pokers.Add(40, new Poker(40, "红桃8", 8, 3));
        pokers.Add(41, new Poker(41, "红桃9", 9, 3));
        pokers.Add(42, new Poker(42, "红桃10", 10, 3));
        pokers.Add(43, new Poker(43, "红桃J", 11, 3));
        pokers.Add(44, new Poker(44, "红桃Q", 12, 3));
        pokers.Add(45, new Poker(45, "红桃K", 13, 3));

        pokers.Add(49, new Poker(49, "黑桃1", 14, 4));
        pokers.Add(50, new Poker(50, "黑桃2", 15, 4));
        pokers.Add(51, new Poker(51, "黑桃3", 3, 4));
        pokers.Add(52, new Poker(52, "黑桃4", 4, 4));
        pokers.Add(53, new Poker(53, "黑桃5", 5, 4));
        pokers.Add(54, new Poker(54, "黑桃6", 6, 4));
        pokers.Add(55, new Poker(55, "黑桃7", 7, 4));
        pokers.Add(56, new Poker(56, "黑桃8", 8, 4));
        pokers.Add(57, new Poker(57, "黑桃9", 9, 4));
        pokers.Add(58, new Poker(58, "黑桃10", 10, 4));
        pokers.Add(59, new Poker(59, "黑桃J", 11, 4));
        pokers.Add(60, new Poker(60, "黑桃Q", 12, 4));
        pokers.Add(61, new Poker(61, "黑桃K", 13, 4));
    }

    /// <summary>
    /// 取得当前牌的名称
    /// </summary>
    public static string GetPokerName(int index)
    {
        if (pokers.Count == 0)
        {
            InitPoker();
        }
        if (pokers.ContainsKey(index))
        {
            return pokers[index].name;
        }
        else
        {
            Debug.LogError("取牌异常，当前牌型列表中没有包含此编码");
            return pokers[0].name;
        }
    }

    /// <summary>
    /// 取得当前牌对应的值
    /// </summary>
    public static int GetPokerValue(int index)
    {
        if (pokers.Count == 0)
        {
            InitPoker();
        }
        if (pokers.ContainsKey(index))
        {
            int value = pokers[index].value;
            return value;
        }
        else
        {
            Debug.LogError("取牌异常，当前牌型列表中没有包含此编码");
            return 0;
        }
    }

    /// <summary>
    /// 取得当前牌花色值
    /// </summary>
    public static int GetPokerTypeValue(int index)
    {
        if (pokers.Count == 0)
        {
            InitPoker();
        }
        if (pokers.ContainsKey(index))
        {
            int typeValue = pokers[index].typeValue;
            return typeValue;
        }
        else
        {
            Debug.LogError("取牌异常，当前牌型列表中没有包含此编码");
            return 0;
        }
    }

    //比较牌的大小
    public static int CompareCard(byte card1, byte card2)
    {
        if (GetPokerValue(card1) > GetPokerValue(card2))
        {
            return 1;
        }
        else if (GetPokerValue(card1) == GetPokerValue(card2))
        {
            if (GetPokerTypeValue(card1) > GetPokerTypeValue(card2))
            {
                return 1;
            }
            else if (GetPokerTypeValue(card1) == GetPokerTypeValue(card2))
            {
                return 0;
            }
            else
            {
                return -1;
            }
        }
        else
        {
            return -1;
        }
    }
    

    //比较牌大小，用于显示排序
    public static int CompareCard(int card1, int card2)
    {
        int v1, v2, t1, t2;
        v1 = GetPokerValue(card1);
        v2 = GetPokerValue(card2);
        t1 = GetPokerTypeValue(card1);
        t2 = GetPokerTypeValue(card2);
        if (v1 > v2)
        {
            return 1;
        }
        else if (v1 < v2)
        {
            return -1;
        }
        else
        {
            if (t1 > t2)
            {
                return 1;
            }
            else if (t1 < t2)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }

    #region  失败方法
    //出牌提示
    public static Dictionary<int, List<byte>> CardTip(List<byte> beforeOutCards, List<byte> myHandCards)
    {
        for (int i = 0; i < myHandCards.Count; i++)
        {
            Debug.Log(" myHandCards[" + i + "]:  " + myHandCards[i]);
        }
        Dictionary<int, List<byte>> myTipCardList = new Dictionary<int, List<byte>>();
        Debug.Log("JudgeCardType(beforeOutCards) : " + JudgeCardType(beforeOutCards));
        //先判断上家出牌的牌型
        switch (JudgeCardType(beforeOutCards))
        {                
            case 14://王炸
                myTipCardList = FindPromptCards(myHandCards, beforeOutCards, 14);
                break;
            case 13://炸弹
                myTipCardList = FindPromptCards(myHandCards, beforeOutCards, 13);
                break;
            case 12://四带双
                myTipCardList = FindPromptCards(myHandCards, beforeOutCards, 12);
                break;
            case 11://四带单
                myTipCardList = FindPromptCards(myHandCards, beforeOutCards, 11);
                break;
            case 10://飞机带双
                myTipCardList = FindPromptCards(myHandCards, beforeOutCards, 10);
                break;
            case 9://飞机带单
                myTipCardList = FindPromptCards(myHandCards, beforeOutCards, 9);
                break;
            case 8://飞机（三顺）
                myTipCardList = FindPromptCards(myHandCards, beforeOutCards, 8);
                break;
            case 7://连对（双顺）
                myTipCardList = FindPromptCards(myHandCards, beforeOutCards, 7);
                break;
            case 6://顺子（单顺）
                myTipCardList = FindPromptCards(myHandCards, beforeOutCards, 6);
                break;
            case 5://三带对
                myTipCardList = FindPromptCards(myHandCards, beforeOutCards, 5);
                break;
            case 4://三带一
                myTipCardList = FindPromptCards(myHandCards, beforeOutCards, 4);
                break;
            case 3://三张
                myTipCardList = FindPromptCards(myHandCards, beforeOutCards, 3);
                break;
            case 2://对子
                myTipCardList = FindPromptCards(myHandCards, beforeOutCards, 2);
                break;
            case 1://单牌
                myTipCardList = FindPromptCards(myHandCards, beforeOutCards, 1);
                break;
            default:
                break;
        }
        return myTipCardList;
    }

    //判断是什么牌型
    public static int JudgeCardType(List<byte> beforeOutCards)
    {
        int result = 0;
        if (beforeOutCards.Count > 0)
        {
            if (beforeOutCards.Count == 1)//单张
            {
                result = 1;
            }
            else if (beforeOutCards.Count == 2)//王炸、对子
            {
                if (IsWangZha(beforeOutCards))
                {
                    result = 14;
                }
                else if (IsDuiZi(beforeOutCards))
                {
                    result = 2;
                }
            }
            else if (beforeOutCards.Count == 3)//三张
            {
                if (IsSanZhang(beforeOutCards))
                {
                    result = 3;
                }
            }
            else if (beforeOutCards.Count == 4)//炸弹 、三带一
            {
                if (IsZhaDan(beforeOutCards))
                {
                    result = 13;
                }
                else if (IsSanDaiYiDan(beforeOutCards))
                {
                    result = 4;
                }
            }
            else if (beforeOutCards.Count == 5)//三带二、 顺子、四带一
            {
                if (IsSanDaiYiDui(beforeOutCards))
                {
                    result = 5;
                }
                else if (IsDanShun(beforeOutCards))
                {
                    result = 6;
                }
                else if (IsSiDaiLiangDan(beforeOutCards))
                {
                    result = 11;
                }
            }
            else if (beforeOutCards.Count == 6)//连对、飞机、顺子、四带二
            {
                if (IsShuangShun(beforeOutCards))
                {
                    result = 7;
                }
                else if (IsTripleStraight(beforeOutCards))
                {
                    result = 8;
                }
                else if (IsDanShun(beforeOutCards))
                {
                    result = 6;
                }
                else if (IsSiDaiLiangDui(beforeOutCards))
                {
                    result = 12;
                }
            }
            else
            {
                if (IsDanShun(beforeOutCards))
                {
                    result = 6;
                }
                else if (IsShuangShun(beforeOutCards))
                {
                    result = 7;
                }
                else if (IsTripleStraight(beforeOutCards))
                {
                    result = 8;
                }
                else if (IsPlaneWithSingle(beforeOutCards))
                {
                    result = 9;
                }
                else if (IsPlaneWithTwin(beforeOutCards))
                {
                    result = 10;
                }
            }            
        }

        return result;
    }

    //取出手牌中对应牌型
    public static List<List<byte>> GetFitCardGroup(int lastCardType, List<byte> lastCards, List<byte> myHandCards)
    {
        List<List<byte>> resultList = new List<List<byte>>();
        List<byte[]> tempList = new List<byte[]>();
        Range(ref myHandCards);//排序
        byte[] myHandCardArray = new byte[myHandCards.Count];
        for (int i = 0; i < myHandCards.Count; i++)
        {
            myHandCardArray[i] = myHandCards[i];
        }
        //排列组合手牌
        List<byte[]> list1 = PermutationAndCombination<byte>.GetCombination(myHandCardArray, lastCards.Count);

        int lastCardsCount = lastCards.Count;
        int myCardsCount = myHandCards.Count;

        if (lastCardsCount == 0 && myCardsCount != 0)//我先出牌，没有上家牌
        {
            
        }

        if (lastCardType == 14)//上家出王炸，自己要不起
        {
            Debug.Log("要不起******************");
        }

        int prevGrade = 0;//取上家出牌的第一张
        if (lastCardsCount > 0)
        {
            prevGrade = lastCards[0];
            Debug.Log("取上家出牌的第一张  prevGrade ： " + prevGrade);
        }

        if (lastCardType == 1)//单张
        {
            for (int i = myCardsCount-1; i <= 0 ; i--)
            {
                if (GetPokerValue(myHandCards[i]) > GetPokerValue(prevGrade))//如果我的单张大于上家出的单张
                {
                    resultList[i][0] = myHandCards[i];
                }
            }
        }
        else if (lastCardType == 2)//对子
        {
            //取自己手牌中对子，再和上家的牌作比较
            List<byte[]> tempCardList = new List<byte[]>();
            int duiZiDataCount = GetDuiZiData(myHandCards).Count;
            for (int i = 0; i < duiZiDataCount; i++)
            {
                tempCardList[i] = GetDuiZiData(myHandCards)[i];
            }
            for (int i = 0; i < tempCardList.Count; i++)
			{
                if (GetPokerValue(tempCardList[i][0]) > GetPokerValue(prevGrade))
                {
                    for (int j = 0; j < tempCardList[i].Length; j++)
                    {
                        resultList[i][j] = tempCardList[i][j];
                    }                    
                }
			}            
        }
        else if (lastCardType == 3)//三条
        {
            //取自己手牌中三条，再和上家的牌作比较
            List<List<byte>> tempCardList = new List<List<byte>>();
            int SanTiaoCount = GetSanTiaoData(myHandCards).Count;
            for (int i = 0; i < SanTiaoCount; i++)
            {
                tempCardList[i] = GetSanTiaoData(myHandCards)[i];
            }
            for (int i = 0; i < tempCardList.Count; i++)
			{
                if (GetPokerValue(tempCardList[i][0]) > GetPokerValue(prevGrade))
                {
                    for (int j = 0; j < tempCardList[i].Count; j++)
                    {
                        resultList[i][j] = tempCardList[i][j];
                    }                    
                }
			}     
        }
        else if (lastCardType == 4)//三带一
        {
            if (myCardsCount >= 4)
            {
                List<byte> tempCards = new List<byte>();
                for (int i = 0; i < myCardsCount; i++)
                {
                    tempCards[i] = myHandCards[i];
                }

                List<List<byte>> tempCardList = new List<List<byte>>();
                int SanTiaoDataCount = GetSanTiaoData(myHandCards).Count;
                for (int i = 0; i < SanTiaoDataCount; i++)
                {
                    tempCardList[i] = GetSanTiaoData(myHandCards)[i];
                }

                for (int i = 0; i < tempCardList.Count; i++)
                {
                    for (int j = 0; j < tempCardList[i].Count; j++)
                    {
                        tempCards.Remove(tempCardList[i][j]);
                    }
                }
                Range(ref tempCards);

                for (int i = 0; i < tempCards.Count; i++)
                {
                    tempCardList[i].Add(tempCards[i]);                   
                }

                for (int i = 0; i < tempCardList.Count; i++)
                {
                    if (GetPokerValue(tempCardList[i][0]) > GetPokerValue(prevGrade))
                    {
                        for (int j = 0; j < tempCardList[i].Count; j++)
                        {
                            resultList[i][j] = tempCardList[i][j];
                        }
                    }
                }
            }
        }
        else if (lastCardType == 5)//三带二
        {
            
        }



        return resultList;
    }

    #region 提示出牌
    public static Dictionary<int, List<byte>> FindPromptCards(List<byte> myCards, List<byte> lastCards, int lastCardType)
    {
        Debug.Log("   lastCardType:        " + lastCardType);
        Dictionary<int, List<byte>> PromptCards = new Dictionary<int, List<byte>>();
        Dictionary<byte, int> tempMyCardDic = new Dictionary<byte, int>();
        tempMyCardDic = SortCardUseDic1(myCards);
        List<byte> tempMyCardList = new List<byte>();
        tempMyCardList = myCards;
        Hashtable tempMyCardHash = SortCardUseHash1(myCards);

        // 上一首牌的个数
        int prevSize = lastCards.Count;
        int mySize = myCards.Count;

        // 我先出牌，上家没有牌
        if (prevSize == 0 && mySize != 0)
        {
            //把所有牌权重存入返回
            Debug.Log("上家没有牌");
            List<byte> myCardsHashKey = new List<byte>();
            foreach (byte key in tempMyCardDic.Keys)
            {
                myCardsHashKey.Add(key);
            }
            myCardsHashKey.Sort();
            for (int i = 0; i < myCardsHashKey.Count; i++)
            {
                List<byte> tempIntList = new List<byte>();
                tempIntList.Add(myCardsHashKey[i]);
                PromptCards.Add(i, tempIntList);
            }
        }

        // 集中判断是否王炸，免得多次判断王炸
        if (lastCardType == 14)
        {
            Debug.Log("上家王炸，肯定不能出。");
        }
        int prevGrade = 0;
        if (prevSize > 0)
        {
            prevGrade = lastCards[0];
            Debug.Log("上家出的牌：    prevGrade : " + prevGrade);
        }

        // 比较2家的牌，主要有2种情况，1.我出和上家一种类型的牌，即对子管对子；
        // 2.我出炸弹，此时，和上家的牌的类型可能不同
        // 王炸的情况已经排除

        // 上家出单
        if (lastCardType == 1)
        {
            int tempCount = 0;
            List<byte> myCardsHashKey = new List<byte>();
            foreach (byte key in tempMyCardDic.Keys)
            {
                myCardsHashKey.Add(key);
            }
            for (int i = 0; i < myCardsHashKey.Count; i++)
            {
                if (GetPokerValue(myCardsHashKey[i]) > GetPokerValue(prevGrade))
                {
                    List<byte> tempIntList = new List<byte>();
                    tempIntList.Add(myCardsHashKey[i]);
                    //Debug.Log(" myCardsHashKey[" + i + "]:    " + myCardsHashKey[i]);
                    PromptCards.Add(tempCount, tempIntList);
                    tempCount++;
                }
            }
        }
        // 上家出对子
        else if (lastCardType == 2)
        {
            Debug.Log("测试         " );
            int tempCount = 0;
            List<byte> myCardsHashKey = new List<byte>();
            foreach (byte key in tempMyCardDic.Keys)
            {
                myCardsHashKey.Add(key);
                Debug.Log("   key:      " + key);
            }
            for (int i = 0; i < myCardsHashKey.Count; i++)
            {
                Debug.Log("  myCardsHashKey[" + i + "] :       " + myCardsHashKey[i] + "   ,  tempMyCardDic[myCardsHashKey[" + i + "]]:    " + tempMyCardDic[myCardsHashKey[i]]);
                if (GetPokerValue(myCardsHashKey[i]) > GetPokerValue(prevGrade) && (int)tempMyCardDic[myCardsHashKey[i]] >= 2)
                {
                    List<byte> tempIntList = new List<byte>();                    
                    tempIntList.Add(myCardsHashKey[i]);
                    tempIntList.Add(myCardsHashKey[i + 1]);                  
                    PromptCards.Add(tempCount, tempIntList);
                    tempCount++;
                }                
            }
        }
        // 上家出3不带
        else if (lastCardType == 3)
        {
            int tempCount = 0;
            List<byte> myCardsHashKey = new List<byte>();
            foreach (byte key in tempMyCardDic.Keys)
            {
                myCardsHashKey.Add(key);
            }
            myCardsHashKey.Sort();
            for (int i = 0; i < myCardsHashKey.Count; i++)
            {
                if (myCardsHashKey[i] > GetPokerValue(prevGrade) && (int)tempMyCardDic[myCardsHashKey[i]] >= 3)
                {
                    List<byte> tempIntList = new List<byte>();
                    tempIntList.Add(tempMyCardList[i]);
                    tempIntList.Add(tempMyCardList[i]);
                    tempIntList.Add(tempMyCardList[i]);
                    PromptCards.Add(tempCount, tempIntList);
                    tempCount++;
                }
            }
        }
        // 上家出3带1
        else if (lastCardType == 4)
        {
            // 3带1 3不带 比较只多了一个判断条件
            if (mySize < 4)
            {

            }
            byte grade3 = 0;
            foreach (byte key in tempMyCardDic.Keys)
            {
                if (int.Parse(tempMyCardDic[key].ToString()) == 1)
                {
                    grade3 = key;
                    break;
                }
            }
            int tempCount = 0;
            List<byte> myCardsHashKey = new List<byte>();
            foreach (byte key in tempMyCardDic.Keys)
            {
                myCardsHashKey.Add(key);
            }
            myCardsHashKey.Sort();
            for (int i = 0; i < myCardsHashKey.Count; i++)
            {
                if (myCardsHashKey[i] > prevGrade && (int)tempMyCardDic[myCardsHashKey[i]] >= 3)
                {
                    List<byte> tempIntList = new List<byte>();
                    tempIntList.Add(myCardsHashKey[i]);
                    tempIntList.Add(myCardsHashKey[i]);
                    tempIntList.Add(myCardsHashKey[i]);
                    tempIntList.Add(grade3);
                    PromptCards.Add(tempCount, tempIntList);
                    tempCount++;
                }
            }
        }
        // 上家出3带2
        else if (lastCardType == 5)
        {
            // 3带1 3不带 比较只多了一个判断条件
            if (mySize < 5)
            {

            }
            byte grade3 = 0;
            byte grade4 = 0;
            foreach (byte key in tempMyCardDic.Keys)
            {
                if (int.Parse(tempMyCardDic[key].ToString()) == 2)
                {
                    grade3 = key;
                    grade4 = key;
                    break;
                }
            }
            int tempCount = 0;
            List<byte> myCardsHashKey = new List<byte>();
            foreach (byte key in tempMyCardDic.Keys)
            {
                myCardsHashKey.Add(key);
            }
            myCardsHashKey.Sort();
            for (int i = 0; i < myCardsHashKey.Count; i++)
            {
                if (myCardsHashKey[i] > prevGrade && (int)tempMyCardDic[myCardsHashKey[i]] >= 3)
                {
                    List<byte> tempIntList = new List<byte>();
                    tempIntList.Add(myCardsHashKey[i]);
                    tempIntList.Add(myCardsHashKey[i]);
                    tempIntList.Add(myCardsHashKey[i]);
                    tempIntList.Add(grade3);
                    tempIntList.Add(grade4);
                    PromptCards.Add(tempCount, tempIntList);
                    tempCount++;
                }
            }
        }
        // 上家出炸弹
        else if (lastCardType == 13)
        {
            int tempCount = 0;
            // 4张牌可以大过上家的牌
            for (int i = mySize - 1; i >= 3; i--)
            {
                int grade0 = myCards[i];
                int grade1 = myCards[i - 1];
                int grade2 = myCards[i - 2];
                int grade3 = myCards[i - 3];

                if (grade0 == grade1 && grade0 == grade2 && grade0 == grade3)
                {
                    if (grade0 > prevGrade)
                    {
                        // 把四张牌存进去
                        List<byte> tempIntList = new List<byte>();
                        tempIntList.Add((byte)grade0);
                        tempIntList.Add((byte)grade1);
                        tempIntList.Add((byte)grade2);
                        tempIntList.Add((byte)grade3);

                        PromptCards.Add(tempCount, tempIntList);
                        tempCount++;
                    }
                }
            }

        }
        // 上家出4带2 
        else if (lastCardType == 11)
        {
            // 4张牌可以大过上家的牌
            for (int i = mySize - 1; i >= 3; i--)
            {
                int grade0 = myCards[i];
                int grade1 = myCards[i - 1];
                int grade2 = myCards[i - 2];
                int grade3 = myCards[i - 3];

                if (grade0 == grade1 && grade0 == grade2 && grade0 == grade3)
                {
                    // 只要有炸弹，则返回true

                }
            }
        }
        // 上家出4带2 对子 
        else if (lastCardType == 12)
        {
            // 4张牌可以大过上家的牌
            for (int i = mySize - 1; i >= 3; i--)
            {
                int grade0 = myCards[i];
                int grade1 = myCards[i - 1];
                int grade2 = myCards[i - 2];
                int grade3 = myCards[i - 3];

                if (grade0 == grade1 && grade0 == grade2 && grade0 == grade3)
                {
                    // 只要有炸弹，则返回true

                }
            }
        }
        // 上家出顺子
        else if (lastCardType == 6)
        {
            if (mySize < prevSize)
            {

            }
            else
            {
                List<byte> tempMyCards = new List<byte>();
                tempMyCards = myCards;
                Dictionary<byte, int> myCardsHash = SortCardUseDic(tempMyCards);
                if (myCardsHash.Count < prevSize)
                {
                    Debug.Log("hash的总数小于顺子的count 肯定fales");

                }
                List<byte> myCardsHashKey = new List<byte>();
                foreach (byte key in myCardsHash.Keys)
                {
                    myCardsHashKey.Add(key);
                }
                myCardsHashKey.Sort();
                byte tempCount = 0;
                for (int i = myCardsHashKey.Count - 1; i >= prevSize - 1; i--)
                {
                    List<byte> cards = new List<byte>();
                    for (int j = 0; j < prevSize; j++)
                    {
                        cards.Add(myCardsHashKey[myCardsHashKey.Count - 1 - i + j]);
                    }
                    DDZ_POKER_TYPE myCardType = DDZ_POKER_TYPE.DDZ_PASS;
                    //bool isRule = DDZCardRule.PopEnable(cards, out myCardType);

                    if (myCardType == DDZ_POKER_TYPE.STRAIGHT_SINGLE)
                    {
                        int myGrade2 = cards[cards.Count - 1];// 最大的牌在最后
                        int prevGrade2 = lastCards[prevSize - 1];// 最大的牌在最后

                        if (myGrade2 > prevGrade2)
                        {
                            //存进去PromptCards
                            PromptCards.Add(tempCount, cards);
                            tempCount++;
                        }
                    }
                }

            }

        }
        // 上家出连对
        else if (lastCardType == 7)
        {
            if (mySize < prevSize)
            {

            }
            else
            {
                List<byte> tempMyCards = new List<byte>();
                tempMyCards = myCards;
                Dictionary<byte, int> myCardsHash = SortCardUseDic(tempMyCards);
                if (myCardsHash.Count < prevSize)
                {
                    Debug.Log("hash的总数小于顺子的count 肯定fales");

                }
                List<byte> myCardsHashKey = new List<byte>();
                foreach (byte key in myCardsHash.Keys)
                {
                    myCardsHashKey.Add(key);
                }
                myCardsHashKey.Sort();
                byte tempCount = 0;
                for (int i = myCardsHashKey.Count - 1; i >= prevSize - 1; i--)
                {

                    List<byte> cards = new List<byte>();
                    for (int j = 0; j < prevSize; j++)
                    {
                        cards.Add(myCardsHashKey[myCardsHashKey.Count - 1 - i + j]);
                    }
                    DDZ_POKER_TYPE myCardType = DDZ_POKER_TYPE.DDZ_PASS;
                    //bool isRule = DDZCardRule.PopEnable(cards, out myCardType);
                    if (myCardType == DDZ_POKER_TYPE.STRAIGHT_SINGLE)
                    {
                        int myGrade2 = cards[cards.Count - 1];// 最大的牌在最后
                        int prevGrade2 = lastCards[prevSize - 1];// 最大的牌在最后

                        if (myGrade2 > prevGrade2)
                        {
                            for (int ii = 0; ii < cards.Count; ii++)
                            {
                                if ((int)myCardsHash[cards[ii]] < 2)
                                {
                                    Debug.Log("是顺子但不是双顺");
                                    return PromptCards;
                                }
                                else
                                {
                                    for (int iii = 0; iii < cards.Count; iii++)
                                    {
                                        cards.Add(cards[iii]);
                                    }
                                    //存进去PromptCards
                                    PromptCards.Add(tempCount, cards);
                                    tempCount++;
                                }
                            }
                        }
                    }
                }
            }

        }
        //上家出飞机
        else if (lastCardType == 8)
        {
            if (mySize < prevSize)
            {

            }
            else
            {
                byte tempCount = 0;
                for (int i = 0; i <= mySize - prevSize; i++)
                {

                    List<byte> cards = new List<byte>();
                    for (int j = 0; j < prevSize; j++)
                    {
                        cards.Add(myCards[i + j]);
                    }

                    DDZ_POKER_TYPE myCardType = DDZ_POKER_TYPE.DDZ_PASS;
                    //bool isRule = DDZCardRule.PopEnable(cards, out myCardType);
                    if (myCardType == DDZ_POKER_TYPE.PLANE_PURE)
                    {
                        byte myGrade4 = cards[4];//
                        int prevGrade4 = lastCards[4];//

                        if (myGrade4 > prevGrade4)
                        {
                            //存进去PromptCards
                            PromptCards.Add(tempCount, cards);
                            tempCount++;
                        }
                    }
                }
            }
        }
        //上家出飞机带单
        else if (lastCardType == 9)
        {
            if (mySize < prevSize)
            {

            }
            else
            {
                byte tempCount = 0;
                for (int i = 0; i <= mySize - prevSize; i++)
                {

                    List<byte> cards = new List<byte>();
                    for (int j = 0; j < prevSize - prevSize / 4; j++)
                    {
                        cards.Add(myCards[i + j]);
                    }

                    DDZ_POKER_TYPE myCardType = DDZ_POKER_TYPE.DDZ_PASS;
                    //bool isRule = DDZCardRule.PopEnable(cards, out myCardType);
                    if (myCardType == DDZ_POKER_TYPE.PLANE_PURE)
                    {
                        int myGrade4 = cards[4];//
                        int prevGrade4 = lastCards[4];//

                        if (myGrade4 > prevGrade4)
                        {
                            int ii = 0;
                            //存进去PromptCards 然后再找一个最小的两个单
                            foreach (byte key in tempMyCardDic.Keys)
                            {
                                if (int.Parse(tempMyCardDic[key].ToString()) == 1)
                                {
                                    cards.Add(key);
                                    ii++;
                                    if (ii == prevSize / 4)
                                    {
                                        break;
                                    }
                                }
                            }
                            PromptCards.Add(tempCount, cards);
                            tempCount++;
                        }
                    }
                }
            }
        }

        //上家出飞机带双
        else if (lastCardType == 10)
        {
            if (mySize < prevSize)
            {

            }
            else
            {
                int tempCount = 0;
                for (int i = 0; i <= mySize - prevSize; i++)
                {

                    List<byte> cards = new List<byte>();
                    for (int j = 0; j < prevSize - prevSize / 5; j++)
                    {
                        cards.Add(myCards[i + j]);
                    }

                    DDZ_POKER_TYPE myCardType = DDZ_POKER_TYPE.DDZ_PASS;
                    //bool isRule = DDZCardRule.PopEnable(cards, out myCardType);
                    if (myCardType == DDZ_POKER_TYPE.PLANE_PURE)
                    {
                        int myGrade4 = cards[4];//
                        int prevGrade4 = lastCards[4];//

                        if (myGrade4 > prevGrade4)
                        {
                            List<int> tempTwoList = new List<int>();
                            for (int ii = 0; ii < cards.Count; ii++)
                            {
                                int tempInt = 0;
                                for (int j = 0; j < cards.Count; j++)
                                {

                                    if (cards[ii] == cards[j])
                                    {
                                        tempInt++;
                                    }

                                }
                                if (tempInt == 2)
                                {
                                    tempTwoList.Add(cards[ii]);
                                }

                            }
                            if (tempTwoList.Count / 2 < prevSize / 5)
                            {


                            }
                            else
                            {
                                //存进去
                                int iii = 0;
                                //存进去PromptCards 然后再找一个最小的两个单
                                foreach (byte key in tempMyCardDic.Keys)
                                {
                                    if (int.Parse(tempMyCardDic[key].ToString()) == 2)
                                    {
                                        cards.Add(key);
                                        cards.Add(key);
                                        iii++;
                                        if (iii == prevSize / 5)
                                        {
                                            break;
                                        }
                                    }
                                }
                                PromptCards.Add(tempCount, cards);
                                tempCount++;
                            }
                        }
                    }
                }
            }
        }

        // 集中判断对方不是炸弹，我出炸弹的情况
        if (lastCardType != 13)
        {
            List<byte> myCardsHashKey = new List<byte>();
            foreach (byte key in tempMyCardDic.Keys)
            {
                myCardsHashKey.Add(key);
            }
            myCardsHashKey.Sort();
            for (int i = 0; i < myCardsHashKey.Count; i++)
            {
                if ((int)tempMyCardDic[myCardsHashKey[i]] == 4)
                {
                    List<byte> tempIntList = new List<byte>();
                    tempIntList.Add(myCardsHashKey[i]);
                    tempIntList.Add(myCardsHashKey[i]);
                    tempIntList.Add(myCardsHashKey[i]);
                    tempIntList.Add(myCardsHashKey[i]);
                    Debug.Log("PromptCards.Count" + PromptCards.Count);
                    PromptCards.Add(PromptCards.Count, tempIntList);

                }
            }
        }
        if (mySize >= 2)
        {
            List<byte> myCardsHashKey = new List<byte>();
            foreach (byte key in tempMyCardDic.Keys)
            {
                myCardsHashKey.Add(key);
            }
            if (myCardsHashKey.Contains(53) && myCardsHashKey.Contains(54))
            {
                List<byte> tempIntList = new List<byte>();
                tempIntList.Add(53);
                tempIntList.Add(54);
                PromptCards.Add(PromptCards.Count, tempIntList);
            }
        }
        return PromptCards;
    }
    #endregion

    #endregion

    //数据排序
    private static void Range(ref List<byte> data)
    {
        for (int i = 0; i < data.Count; i++)
        {
            for (int j = i + 1; j < data.Count; j++)
            {
                if (CompareCard(data[i], data[j]) < 0)
                {
                    byte mid = data[i];
                    data[i] = data[j];
                    data[j] = mid;
                }
            }
        }
    }

    #region  判断牌型

    // 对牌（数值相同的两张牌）
    static bool IsDuiZi(List<byte> data)
    {
        Range(ref data);
        if (GetPokerValue(data[0]) == GetPokerValue(data[1]))
        {
            return true;
        }
        return false;
    }

    // 三张（数值相同的三张牌）
    static bool IsSanZhang(List<byte> data)
    {
        Range(ref data);
        if (GetPokerValue(data[0]) == GetPokerValue(data[1]) && GetPokerValue(data[1]) == GetPokerValue(data[2]))
        {
            return true;
        }
        return false;
    }

    // 三带一(4张牌 ： 数值相同的三张牌 + 一张单牌)
    static bool IsSanDaiYiDan(List<byte> data)
    {
        Range(ref data);
        if ((GetPokerValue(data[0]) == GetPokerValue(data[1]) && GetPokerValue(data[1]) == GetPokerValue(data[2])) ||
            (GetPokerValue(data[1]) == GetPokerValue(data[2]) && GetPokerValue(data[2]) == GetPokerValue(data[3])))
        {
            return true;
        }
        return false;
    }

    // 三带一对(5张牌 ： 数值相同的三张牌 + 一对牌)
    static bool IsSanDaiYiDui(List<byte> data)
    {
        Range(ref data);
        if (((GetPokerValue(data[0]) == GetPokerValue(data[1]) && GetPokerValue(data[1]) == GetPokerValue(data[2])) && (GetPokerValue(data[3]) == GetPokerValue(data[4]))) ||
            ((GetPokerValue(data[2]) == GetPokerValue(data[3]) && GetPokerValue(data[3]) == GetPokerValue(data[4])) && (GetPokerValue(data[0]) == GetPokerValue(data[1]))))
        {
            return true;
        }
        return false;
    }

    //顺子  单顺(五张或更多的连续单牌， 不包括2和双王 ： 如： 45678、78910JQKA)
    static bool IsDanShun(List<byte> data)
    {
        Range(ref data);
        int isAccordCount = 0;
        if (GetPokerValue(data[0]) != 15 && GetPokerValue(data[1]) != 15 && GetPokerValue(data[2]) != 15 && 
            GetPokerValue(data[0]) != 21 && GetPokerValue(data[1]) != 21 && GetPokerValue(data[0]) != 22)
        {
            if (data.Count >= 5)
            {
                for (int i = 0; i < data.Count; i++)
                {
                    if (i < data.Count - 1)
                    {
                        if (GetPokerValue(data[i]) - GetPokerValue(data[i + 1]) == 1)
                        {
                            isAccordCount += 1;
                        }
                    }
                }
                if (isAccordCount == data.Count - 1)
                {
                    return true;
                }
            }
        }        
        return false;
    }

    // 连对  双顺(三对或更多的连续对牌,不包括 2 点和双王如： 334455 、7788991010JJ )
    static bool IsShuangShun(List<byte> data)
    {
        Range(ref data);
        if (GetPokerValue(data[0]) != 15 && GetPokerValue(data[1]) != 15 && GetPokerValue(data[2]) != 15 && 
            GetPokerValue(data[0]) != 21 && GetPokerValue(data[1]) != 21 && GetPokerValue(data[0]) != 22)
        {
            if (data.Count >= 6 && data.Count % 2 == 0)
            {
                int ShunCount = 0;
                int DuiCout = 0;
                for (int i = 0; i < data.Count; i += 2)
                {
                    if (i < data.Count - 3)
                    {
                        if (GetPokerValue(data[i]) - GetPokerValue(data[i + 2]) == 1)
                        {
                            ShunCount += 1;
                        }
                    }
                }
                for (int i = 0; i < data.Count; i += 2)
                {
                    if (i < data.Count - 1)
                    {
                        if ((GetPokerValue(data[i]) == GetPokerValue(data[i + 1])))
                        {
                            DuiCout += 1;
                        }
                    }
                }
                if (ShunCount == (data.Count / 2 - 1) && DuiCout == data.Count / 2)
                {
                    return true;
                }
            }
        }
        return false;
    }

    // 三顺(二个或更多的连续三张牌，不包括 2 点和双王 如： 333444 、 555666777888 )
    static bool IsSanShun(List<byte> data)
    {
        Range(ref data);
        if (GetPokerValue(data[0]) != 15 && GetPokerValue(data[1]) != 15 && GetPokerValue(data[2]) != 15 &&
            GetPokerValue(data[0]) != 21 && GetPokerValue(data[1]) != 21 && GetPokerValue(data[0]) != 22)
        {
            int ShunCount = 0;
            int SanTiaoCout = 0;
            for (int i = 0; i < data.Count; i += 3)
            {
                if (i < data.Count - 5)
                {
                    if (GetPokerValue(data[i]) - GetPokerValue(data[i + 3]) == 1)
                    {
                        ShunCount += 1;
                    }
                }
            }
            for (int i = 0; i < data.Count; i += 3)
            {
                if (i < data.Count - 5)
                {
                    if (GetPokerValue(data[i]) == GetPokerValue(data[i + 1]) && GetPokerValue(data[i + 1]) == GetPokerValue(data[i + 2]))
                    {
                        SanTiaoCout += 1;
                    }
                }
            }
            if (ShunCount == (data.Count / 3 - 1) && SanTiaoCout == data.Count / 3)
            {
                return true;
            }
        }
        return false;
    }

    /// 飞机不带  （三顺）
    public static bool IsTripleStraight(List<byte> data)
    {
        Range(ref data);

        if (data.Count < 6 || data.Count % 3 != 0)
            return false;

        for (int i = 0; i < data.Count; i += 3)
        {
            if (GetPokerValue(data[i + 1]) != GetPokerValue(data[i]))
                return false;
            if (GetPokerValue(data[i + 2]) != GetPokerValue(data[i]))
                return false;
            if (GetPokerValue(data[i + 1]) != GetPokerValue(data[i + 2]))
                return false;

            if (i < data.Count - 3)
            {
                if (GetPokerValue(data[i]) - GetPokerValue(data[i + 3]) != 1)
                    return false;
 
                ////不能超过A
                //if (GetPokerValue(data[i]) > 14 || GetPokerValue(data[i + 3]) > 14)
                //    return false;
            }
        } 
        return true;
    }

    /// 飞机带单
    public static bool IsPlaneWithSingle(List<byte> data)
    {
        if (!HaveFour(data))
        {
            List<byte> tempThreeList = new List<byte>();
            for (int i = 0; i < data.Count; i++)
            {
                int tempInt = 0;
                for (int j = 0; j < data.Count; j++)
                {
                    if (GetPokerValue(data[i]) == GetPokerValue(data[j]))
                    {
                        tempInt++;
                    }                    
                }
                if (tempInt == 3)
                {
                    tempThreeList.Add(data[i]);
                }
            }
            if (tempThreeList.Count % 3 != data.Count % 4)
            {                
                return false;
            }
            else 
            {
                if (IsTripleStraight(tempThreeList))
                {
                    return true;
                }
                else 
                {                    
                    return false;
                }
            }
        }        
        return false;
    }

    /// 飞机带双
    public static bool IsPlaneWithTwin(List<byte> data)
    {
        if (!HaveFour(data))
        {
            List<byte> tempThreeList = new List<byte>();
            List<byte> tempTwoList = new List<byte>();
            for (int i = 0; i < data.Count; i++)
            {
                int tempInt = 0;
                for (int j = 0; j < data.Count; j++)
                {
                    if (GetPokerValue(data[i]) == GetPokerValue(data[j]))
                    {
                        tempInt++;
                    } 
                }
                if (tempInt == 3)
                {
                    tempThreeList.Add(data[i]);
                }
                else if (tempInt==2) 
                {
                    tempTwoList.Add(data[i]);
                }                
            }
            if (tempThreeList.Count % 3 != data.Count % 5 && tempTwoList.Count % 2 != data.Count % 5)
            { 
                return false;
            }
            else
            {
                if (IsTripleStraight(tempThreeList))
                {
                    if (IsAllDouble(tempTwoList))
                    {
                        return true;
                    }
                    else 
                    {
                        return false;
                    }
                }
                else
                { 
                    return false;
                }
            }
        }
        return false;
    }

    /// 判断牌里面是否是拥有4张牌
    public static bool HaveFour(List<byte> data)
    {
        for (int i = 0; i < data.Count; i++)
        {
            int tempInt = 0;
            for (int j = 0; j < data.Count; j++)
            {
                if (GetPokerValue(data[i]) == GetPokerValue(data[j]))
                {
                    tempInt++;
                }
            }
            if (tempInt == 4)
            {
                return true;
            }
        }
        return false;
    }

    /// 判断牌里面全是对子
    public static bool IsAllDouble(List<byte> data)
    {
        for (int i = 0; i < data.Count % 2; i += 2)
        {
            if (GetPokerValue(data[i]) != GetPokerValue(data[i + 1]))
            {
                return false;
            }
        }
        return true;
    }

    // 四带两单(6张牌)
    static bool IsSiDaiLiangDan(List<byte> data)
    {
        Range(ref data);
        if (data.Count == 6)
        {
            if ((GetPokerValue(data[0]) == GetPokerValue(data[1]) && GetPokerValue(data[1]) == GetPokerValue(data[2]) && GetPokerValue(data[2]) == GetPokerValue(data[3]))||
                GetPokerValue(data[1]) == GetPokerValue(data[2]) && GetPokerValue(data[2]) == GetPokerValue(data[3]) && GetPokerValue(data[3]) == GetPokerValue(data[4]) ||
                GetPokerValue(data[2]) == GetPokerValue(data[3]) && GetPokerValue(data[3]) == GetPokerValue(data[4]) && GetPokerValue(data[4]) == GetPokerValue(data[5]))
            {
                return true;
            }
        }
        return false;
    }

    // 四带两对(8张牌)
    static bool IsSiDaiLiangDui(List<byte> data)
    {
        Range(ref data);
        if (data.Count == 8)
        {
            if ((((GetPokerValue(data[0]) == GetPokerValue(data[1]) && GetPokerValue(data[1]) == GetPokerValue(data[2]) && GetPokerValue(data[2]) == GetPokerValue(data[3])) &&
                GetPokerValue(data[4]) == GetPokerValue(data[5]) && GetPokerValue(data[6]) == GetPokerValue(data[7]))) ||
                (((GetPokerValue(data[2]) == GetPokerValue(data[3]) && GetPokerValue(data[3]) == GetPokerValue(data[4]) && GetPokerValue(data[4]) == GetPokerValue(data[5])) &&
                GetPokerValue(data[0]) == GetPokerValue(data[1]) && GetPokerValue(data[6]) == GetPokerValue(data[7]))) ||
                (((GetPokerValue(data[4]) == GetPokerValue(data[5]) && GetPokerValue(data[5]) == GetPokerValue(data[6]) && GetPokerValue(data[6]) == GetPokerValue(data[7])) &&
                GetPokerValue(data[0]) == GetPokerValue(data[1]) && GetPokerValue(data[2]) == GetPokerValue(data[3]))))
            {
                return true;
            }
        }        
        return false;
    }

    // 炸弹(4张牌)
    static bool IsZhaDan(List<byte> data)
    {
        Range(ref data);
        if (GetPokerValue(data[0]) == GetPokerValue(data[1]) && GetPokerValue(data[1]) == GetPokerValue(data[2]) && GetPokerValue(data[2]) == GetPokerValue(data[3]))
        {
            return true;
        }
        return false;
    }

    // 王炸(两张牌)
    static bool IsWangZha(List<byte> data)
    {
        Range(ref data);
        if (GetPokerValue(data[0]) == 21 && GetPokerValue(data[1]) == 22)
        {
            return true;
        }
        return false;
    }
    #endregion

    #region 失败方法
    #region 取出对应牌型的牌
    //取 对子
    static List<byte[]> GetDuiZiData(List<byte> cards)
    {
        List<byte[]> resList = new List<byte[]>();
        byte[] tempArray = new byte[cards.Count];
        for (int i = 0; i < cards.Count; i++)
        {
            tempArray[i] = cards[i];
        }
        List<byte[]> list = PermutationAndCombination<byte>.GetCombination(tempArray, 2);
        if (list != null)
        {
            for (int i = 0; i < list.Count; i++)
            {
                //if (IsDuiZi(list[i]))
                //{
                //    resList.Add(list[i]);
                //}
            }
        }
        return resList;
    }

    //取 三条
    static List<List<byte>> GetSanTiaoData(List<byte> cards)
    {
        List<List<byte>> resList = new List<List<byte>>();
        byte[] tempArray = new byte[cards.Count];
        for (int i = 0; i < cards.Count; i++)
        {
            tempArray[i] = cards[i];
        }
        List<byte[]> list = PermutationAndCombination<byte>.GetCombination(tempArray, 3);
        List<List<byte>> tempList = new List<List<byte>>();
        for (int i = 0; i < list.Count; i++)
        {
            for (int j = 0; j < list[i].Length; j++)
            {
                tempList[i][j] = list[i][j];
            }
        }
        if (tempList != null)
        {
            for (int i = 0; i < tempList.Count; i++)
            {
                if (IsSanZhang(tempList[i]))
                {
                    resList.Add(tempList[i]);
                }
            }
        }
        return resList;
    }

    #endregion

    #region 使用哈希去存所有的牌  顺子连对用 不用存2 王
    /// 使用哈希去存所有的牌
    public static Hashtable SortCardUseHash(List<byte> cards)
    {
        Hashtable temp = new Hashtable();
        List<int> tempJoker = new List<int>();
        for (int i = 0; i < cards.Count; i++)
        {
            if (GetPokerValue(cards[i]) == 22)
            {
                cards.RemoveAt(i);
            }
        }
        for (int i = 0; i < cards.Count; i++)
        {
            if (GetPokerValue(cards[i]) == 21)
            {
                cards.RemoveAt(i);
            }
        }
        for (int i = 0; i < cards.Count; i++)
        {
            if (GetPokerValue(cards[i]) == 15)
            {
                cards.RemoveAt(i);
            }
        }
        for (int i = 0; i < cards.Count; i++)
        {
            if (temp.Contains(cards[i]))
            {
                temp[cards[i]] = (int)temp[cards[i]] + 1;
            }
            else {
                temp.Add(cards[i], 1);
            }
        }
        
        return temp;
    }
    #endregion

    #region 使用哈希去存所有的牌  顺子连对用 不用存2 王
    /// 使用哈希去存所有的牌
    public static Dictionary<byte, int> SortCardUseDic(List<byte> cards)
    {
        Dictionary<byte, int> temp = new Dictionary<byte, int>();
        List<int> tempJoker = new List<int>();
        for (int i = 0; i < cards.Count; i++)
        {
            if (GetPokerValue(cards[i]) == 22)
            {
                cards.RemoveAt(i);
            }
        }
        for (int i = 0; i < cards.Count; i++)
        {
            if (GetPokerValue(cards[i]) == 21)
            {
                cards.RemoveAt(i);
            }
        }
        for (int i = 0; i < cards.Count; i++)
        {
            if (GetPokerValue(cards[i]) == 15)
            {
                cards.RemoveAt(i);
            }
        }
        for (int i = 0; i < cards.Count; i++)
        {
            if (temp.ContainsKey(cards[i]))
            {
                temp[cards[i]] = (int)temp[cards[i]] + 1;
            }
            else
            {
                temp.Add(cards[i], 1);
            }
        }

        return temp;
    }
 
 
    #endregion

    #region 使用哈希存所有牌 key 权重  value个数
    public static Hashtable SortCardUseHash1(List<byte> cards)
    {
        Hashtable temp = new Hashtable();        
       
        for (int i = 0; i < cards.Count; i++)
        {
            if (temp.Contains(cards[i]))
            {
                temp[cards[i]] = (int)temp[cards[i]] + 1;
            }
            else
            {
                temp.Add(cards[i], 1);
            }
            
        }
        return temp;
    }
    #endregion

    #region 使用哈希存所有牌 key 权重  value个数
    public static Dictionary<byte, int> SortCardUseDic1(List<byte> cards)
    {
        Dictionary<byte, int> temp = new Dictionary<byte, int>();

        for (int i = 0; i < cards.Count; i++)
        {
            foreach (byte item in temp.Keys)
            {
                if (GetPokerValue(item) == GetPokerValue(cards[i]))
                {
                    temp[item] = temp[item] + 1;
                    temp.Add(cards[i], 1);
                }
                else
                {
                    temp.Add(cards[i], 1);
                }
            }
        }

        foreach (byte item in temp.Keys)
        {
            Debug.Log("   item:   " + item);
        }

        return temp;
    }
    #endregion

    #endregion

}
