using UnityEngine;
using System.Collections;
using System;

//SCORE-Int64   WORD-UInt16     DWORD-UInt32    TCHAR-string   LONG - int LONGLONG-Int64

#region 接收

//空闲状态
public class CMD_Landlords_S_StatusFree : DataBase
{
    //游戏属性
    public int lCellScore;    //底分
    public UInt16[] wUserTimes = new UInt16[LandlordsModel.GAME_NUM]; //当前倍数

    //时间信息
    public byte cbTimeCallScore;   //叫分时间
    public byte cbTimeAddTime;		// 加倍时间
    public byte cbTimeHeadOutCard;    //首出时间
    public byte cbTimeOutCard;    //出牌时间   
    public byte cbTimePassCard;			// 要不起时间
    
    public byte[] cbPlayStatus = new byte[LandlordsModel.GAME_NUM];      // 用户状态

    //历史积分
    public Int64[] lTurnScore = new Int64[LandlordsModel.GAME_NUM];		//积分信息
    public Int64[] lCollectScore = new Int64[LandlordsModel.GAME_NUM];     //积分信息
    protected override void UnPackBody()
    {
        lCellScore = ReadInt();
        for (int i = 0; i < LandlordsModel.GAME_NUM; i++)
        {
            wUserTimes[i] = ReadUInt16();
        }
        cbTimeCallScore = ReadByte();
        cbTimeAddTime = ReadByte();
        cbTimeHeadOutCard = ReadByte();
        cbTimeOutCard = ReadByte();
        cbTimePassCard = ReadByte();
        for (int i = 0; i < LandlordsModel.GAME_NUM; i++)
        {
            cbPlayStatus[i] = ReadByte();
        }

        for (int i = 0; i < LandlordsModel.GAME_NUM; i++)
        {
            lTurnScore[i] = ReadInt64();
        }
        for (int i = 0; i < LandlordsModel.GAME_NUM; i++)
        {
            lCollectScore[i] = ReadInt64();
        }
    }
}

//叫分状态
public class CMD_Landlords_S_StatusCall : DataBase
{
    //时间信息
    public byte cbTimeCallScore;   //叫分时间
    public byte cbTimeAddTime;		// 加倍时间
    public byte cbTimeHeadOutCard;    //首出时间
    public byte cbTimeOutCard;    //出牌时间   
    public byte cbTimePassCard;			// 要不起时间
    public byte[] cbPlayStatus = new byte[LandlordsModel.GAME_NUM];      // 用户状态

    //游戏信息
    public int lCellScore;    //底分
    public UInt16[] wUserTimes = new UInt16[LandlordsModel.GAME_NUM]; //当前倍数
    public UInt16 wCurrentUser;   //当前玩家
    public byte[] cbScoreInfo = new byte[LandlordsModel.GAME_NUM];    //叫分信息
    public byte[] cbHandCardData = new byte[LandlordsModel.NORMAL_HANDCARDNUM];   //手上扑克
    public byte[] cbUserTrustee = new byte[LandlordsModel.GAME_NUM];  //托管标志
    //历史积分
    public Int64[] lTurnScore = new Int64[LandlordsModel.GAME_NUM];		//积分信息
    public Int64[] lCollectScore = new Int64[LandlordsModel.GAME_NUM];     //积分信息
    
    protected override void UnPackBody()
    {
        cbTimeCallScore = ReadByte();
        cbTimeAddTime = ReadByte();
        cbTimeHeadOutCard = ReadByte();
        cbTimeOutCard = ReadByte();
        cbTimePassCard = ReadByte();
        for (int i = 0; i < LandlordsModel.GAME_NUM; i++)
        {
            cbPlayStatus[i] = ReadByte();
        }

        lCellScore = ReadInt();
        for (int i = 0; i < LandlordsModel.GAME_NUM; i++)
        {
            wUserTimes[i] = ReadUInt16();
        }
        wCurrentUser = ReadUInt16();

        for (int i = 0; i < LandlordsModel.GAME_NUM; i++)
        {
            cbScoreInfo[i] = ReadByte();
        }

        for (int i = 0; i < LandlordsModel.NORMAL_HANDCARDNUM; i++)
        {
            cbHandCardData[i] = ReadByte();
        }

        for (int i = 0; i < LandlordsModel.GAME_NUM; i++)
        {
            cbUserTrustee[i] = ReadByte();
        }

        for (int i = 0; i < LandlordsModel.GAME_NUM; i++)
        {
            lTurnScore[i] = ReadInt64();
        }
        for (int i = 0; i < LandlordsModel.GAME_NUM; i++)
        {
            lCollectScore[i] = ReadInt64();
        }
    }
}

//加倍状态
public class CMD_Landlords_S_StatusAddTime : DataBase
{
   //时间信息
    public byte cbTimeCallScore;   //叫分时间
    public byte cbTimeAddTime;		// 加倍时间
    public byte cbTimeHeadOutCard;    //首出时间
    public byte cbTimeOutCard;    //出牌时间   
    public byte cbTimePassCard;			// 要不起时间
    public byte[] cbPlayStatus = new byte[LandlordsModel.GAME_NUM];      // 用户状态

	//游戏信息
    public int lCellScore;    //底分
    public UInt16[] wUserTimes = new UInt16[LandlordsModel.GAME_NUM]; //当前倍数
    public UInt16 wCurrentUser;   //当前玩家
    public UInt16 wBankerChairId;	// 地主

    public byte[] cbScoreInfo = new byte[LandlordsModel.GAME_NUM];    //叫分信息
    public byte[] cbAddTimes = new byte[LandlordsModel.GAME_NUM];	// 加倍信息(0,不加倍；1，加倍；255，还没发送加倍消息)
    public byte[] cbBankerCard = new byte[3];		// 游戏底牌
    public byte[] cbHandCardData = new byte[LandlordsModel.MAX_COUNT];   //手上扑克
    public byte[] cbUserTrustee = new byte[LandlordsModel.GAME_NUM];  //托管标志
    //历史积分
    public Int64[] lTurnScore = new Int64[LandlordsModel.GAME_NUM];		//积分信息
    public Int64[] lCollectScore = new Int64[LandlordsModel.GAME_NUM];     //积分信息
	
    protected override void UnPackBody()
    {
        cbTimeCallScore = ReadByte();
        cbTimeAddTime = ReadByte();
        cbTimeHeadOutCard = ReadByte();
        cbTimeOutCard = ReadByte();
        cbTimePassCard = ReadByte();
        for (int i = 0; i < LandlordsModel.GAME_NUM; i++)
        {
            cbPlayStatus[i] = ReadByte();
        }

        lCellScore = ReadInt();
        for (int i = 0; i < LandlordsModel.GAME_NUM; i++)
        {
            wUserTimes[i] = ReadUInt16();
        }
        wCurrentUser = ReadUInt16();
        wBankerChairId = ReadUInt16();

        for (int i = 0; i < LandlordsModel.GAME_NUM; i++)
        {
            cbScoreInfo[i] = ReadByte();
        }

        for (int i = 0; i < LandlordsModel.GAME_NUM; i++)
        {
            cbAddTimes[i] = ReadByte();
        }

        for (int i = 0; i < 3; i++)
        {
            cbBankerCard[i] = ReadByte();
        }

        for (int i = 0; i < LandlordsModel.MAX_COUNT; i++)
        {
            cbHandCardData[i] = ReadByte();
        }

        for (int i = 0; i < LandlordsModel.GAME_NUM; i++)
        {
            cbUserTrustee[i] = ReadByte();
        }

        for (int i = 0; i < LandlordsModel.GAME_NUM; i++)
        {
            lTurnScore[i] = ReadInt64();
        }
        for (int i = 0; i < LandlordsModel.GAME_NUM; i++)
        {
            lCollectScore[i] = ReadInt64();
        }
    }
}

//游戏状态
public class CMD_Landlords_S_StatusPlay : DataBase
{
    //时间信息
    public byte cbTimeCallScore;   //叫分时间
    public byte cbTimeAddTime;		// 加倍时间
    public byte cbTimeHeadOutCard;    //首出时间
    public byte cbTimeOutCard;    //出牌时间   
    public byte cbTimePassCard;			// 要不起时间
    public byte[] cbPlayStatus = new byte[LandlordsModel.GAME_NUM];      // 用户状态

    //游戏变量
    public int lCellScore;    //底分
    public UInt16[] wUserTimes = new UInt16[LandlordsModel.GAME_NUM]; //当前倍数
    public byte cbBombCount;  //炸弹次数
    public UInt16 wBankerUser;    //庄家用户
    public UInt16 wCurrentUser;   //当前玩家

    //出牌信息
    public UInt16 wTurnWiner;   //胜利玩家
    public byte cbTurnCardCount;    //出牌数目
    public byte[] cbTurnCardData = new byte[LandlordsModel.MAX_COUNT];  //出牌数据

    //扑克信息
    public byte[] cbBankerCard = new byte[3];   //游戏底牌
    public byte[] cbHandCardData = new byte[LandlordsModel.MAX_COUNT];  //手上扑克
    public byte[] cbHandCardCount = new byte[LandlordsModel.GAME_NUM];  //三个玩家各自扑克数目
    public byte[] cbEpicycleOutState = new byte[LandlordsModel.GAME_NUM];	// 上一次出牌态(255 初始态   0 不出    1 出）
    public byte[] cbEpicycleCardCount = new byte[LandlordsModel.GAME_NUM]; 	// 本轮出牌玩家数目
    public byte[,] cbEpicycleCardData = new byte[LandlordsModel.GAME_NUM, LandlordsModel.MAX_COUNT];	// 本轮出牌数据
    public byte cbActive;						// 是否主动出牌(0,被动；1，主动)
    public byte bySearchCount;					// 解的个数
    public byte[] cbEachResultCount = new byte[LandlordsModel.MAX_COUNT];	// 每个解的牌数
    public byte[,] cbResultCard = new byte[LandlordsModel.MAX_COUNT, LandlordsModel.MAX_COUNT];	// 结果扑克

    //历史积分
    public Int64[] lTurnScore = new Int64[LandlordsModel.GAME_NUM];     //积分信息
    public Int64[] lCollectScore = new Int64[LandlordsModel.GAME_NUM];  //积分信息
    public byte[] cbUserTrustee = new byte[LandlordsModel.GAME_NUM];    //托管标志
    protected override void UnPackBody()
    {
        cbTimeCallScore = ReadByte();
        cbTimeAddTime = ReadByte();
        cbTimeHeadOutCard = ReadByte();
        cbTimeOutCard = ReadByte();
        cbTimePassCard = ReadByte();
        for (int i = 0; i < LandlordsModel.GAME_NUM; i++)
        {
            cbPlayStatus[i] = ReadByte();
        }

        lCellScore = ReadInt();
        for (int i = 0; i < LandlordsModel.GAME_NUM; i++)
        {
            wUserTimes[i] = ReadUInt16();
        }
        cbBombCount = ReadByte();
        wBankerUser = ReadUInt16();
        wCurrentUser = ReadUInt16();

        wTurnWiner = ReadUInt16();
        cbTurnCardCount = ReadByte();
        for (int i = 0; i < LandlordsModel.MAX_COUNT; i++)
        {
            cbTurnCardData[i] = ReadByte();
        }

        for (int i = 0; i < 3; i++)
        {
            cbBankerCard[i] = ReadByte();
        }
        for (int i = 0; i < LandlordsModel.MAX_COUNT; i++)
        {
            cbHandCardData[i] = ReadByte();
        }
        for (int i = 0; i < LandlordsModel.GAME_NUM; i++)
        {
            cbHandCardCount[i] = ReadByte();
        }
        for (int i = 0; i < LandlordsModel.GAME_NUM; i++)
        {
            cbEpicycleOutState[i] = ReadByte();
        }
        for (int i = 0; i < LandlordsModel.GAME_NUM; i++)
        {
            cbEpicycleCardCount[i] = ReadByte();
        }
        for (int i = 0; i < LandlordsModel.GAME_NUM; i++)
        {
            for (int j = 0; j < LandlordsModel.MAX_COUNT; j++)
            {
                cbEpicycleCardData[i, j] = ReadByte();
            }
        }
        cbActive = ReadByte();
        bySearchCount = ReadByte();
        for (int i = 0; i < LandlordsModel.MAX_COUNT; i++)
        {
            cbEachResultCount[i] = ReadByte();
        }
        for (int i = 0; i < LandlordsModel.MAX_COUNT; i++)
        {
            for (int j = 0; j < LandlordsModel.MAX_COUNT; j++)
            {
                cbResultCard[i, j] = ReadByte();
            }
        }
        for (int i = 0; i < LandlordsModel.GAME_NUM; i++)
        {
            lTurnScore[i] = ReadInt64();
        }
        for (int i = 0; i < LandlordsModel.GAME_NUM; i++)
        {
            lCollectScore[i] = ReadInt64();
        }
        for (int i = 0; i < LandlordsModel.GAME_NUM; i++)
        {
            cbUserTrustee[i] = ReadByte();
        }
    }
}

//游戏开始
public class CMD_Landlords_S_GameStart : DataBase
{
    public UInt16 wStartUser; //开始玩家
    public byte cbValidCardData;  //明牌扑克
    public byte cbValidCardIndex; //明牌位置
    public byte[] cbCardData = new byte[LandlordsModel.NORMAL_HANDCARDNUM];   //扑克列表
    public byte[] cbPlayStatus = new byte[LandlordsModel.GAME_NUM];      // 用户状态

    protected override void UnPackBody()
    {
        wStartUser = ReadUInt16();
        cbValidCardData = ReadByte();
        cbValidCardIndex = ReadByte();
        for (int i = 0; i < LandlordsModel.NORMAL_HANDCARDNUM; i++)
        {
            cbCardData[i] = ReadByte();
        }
        for (int i = 0; i < LandlordsModel.GAME_NUM; i++)
        {
            cbPlayStatus[i] = ReadByte();
        }
    }
}

//重新发牌
public class CMD_Landlords_S_ReSendCard : DataBase
{
    public UInt16 wStartUser; //开始玩家
    public byte[] cbCardData = new byte[LandlordsModel.NORMAL_HANDCARDNUM];   //扑克列表

    protected override void UnPackBody()
    {
        wStartUser = ReadUInt16();
        for (int i = 0; i < LandlordsModel.NORMAL_HANDCARDNUM; i++)
        {
            cbCardData[i] = ReadByte();
        }
    }
}

//机器人扑克
public class CMD_Landlords_S_AndroidCard : DataBase
{
    public byte[,] cbHandCard = new byte[LandlordsModel.GAME_NUM, LandlordsModel.NORMAL_HANDCARDNUM];//手上扑克
    public byte[] cbBankerCard =new byte[3];    //庄家底牌
    public UInt16 wCurrentUser;   //当前玩家

    protected override void UnPackBody()
    {
        for (int i = 0; i < LandlordsModel.GAME_NUM; i++)
        {
            for (int j = 0; j < LandlordsModel.NORMAL_HANDCARDNUM; j++)
            {
                cbHandCard[i, j] = ReadByte();
            }            
        }
        for (int i = 0; i < 3; i++)
        {
            cbBankerCard[i] = ReadByte();
        }        
        wCurrentUser = ReadUInt16();        
    }
}

//作弊扑克
public class CMD_Landlords_S_CheatCard : DataBase
{
    public UInt16[] wCardUser = new UInt16[LandlordsModel.GAME_NUM];   //作弊玩家
    public byte cbUserCount;  //作弊数量
    public byte[,] cbCardData = new Byte[LandlordsModel.GAME_NUM ,LandlordsModel.MAX_COUNT];	//扑克列表
    public byte[] cbCardCount = new byte[LandlordsModel.GAME_NUM];    //扑克数量

    protected override void UnPackBody()
    {
        for (int i = 0; i < LandlordsModel.GAME_NUM; i++)
        {
            wCardUser[i] = ReadUInt16();
        }
        cbUserCount = ReadByte();
        for (int i = 0; i < LandlordsModel.GAME_NUM; i++)
        {
            for (int j = 0; j < LandlordsModel.MAX_COUNT; j++)
            {
                cbCardData[i, j] = ReadByte();
            }            
        }
        for (int i = 0; i < LandlordsModel.GAME_NUM; i++)
        {
            cbCardCount[i] = ReadByte();
        }
    }
}

//用户叫分
public class CMD_Landlords_S_CallScore : DataBase
{
    public UInt16 wCurrentUser; //当前叫分玩家
    public UInt16 wCallScoreUser;   //下个叫分玩家
    public byte cbCurrentScore; //当前叫分 (0 不叫， 1 不抢，2 叫地主 ，3 抢地主 )
    public UInt16[] wUserTimes = new UInt16[LandlordsModel.GAME_NUM];    // 抢完地主之后的倍数

    protected override void UnPackBody()
    {
        wCurrentUser = ReadUInt16();
        wCallScoreUser = ReadUInt16();
        cbCurrentScore = ReadByte();
        for (int i = 0; i < LandlordsModel.GAME_NUM; i++)
        {
            wUserTimes[i] = ReadUInt16();
        }
    }
}

//用户加倍
public class CMD_Landlords_S_AddTimes : DataBase
{
    public UInt16 wAddUser; //加倍玩家
    public byte cbAddTimes; //倍数（0，不加倍;1,加倍）
    public UInt16[] wUserTimes = new UInt16[LandlordsModel.GAME_NUM];  //此次加倍之后的总的倍数
    public byte byCanOutCard;	//是否可以出牌了		

    protected override void UnPackBody()
    {
        wAddUser = ReadUInt16();
        cbAddTimes = ReadByte();
        for (int i = 0; i < LandlordsModel.GAME_NUM; i++)
        {
            wUserTimes[i] = ReadUInt16();
        }
        byCanOutCard = ReadByte();
    }
}

//庄家信息
public class CMD_Landlords_S_BankerInfo : DataBase
{
    public UInt16 wBankerUser;				//庄家玩家
    public UInt16[] wUserTimes = new UInt16[LandlordsModel.GAME_NUM];				//当前总倍数
    public byte[] cbBankerCard = new byte[3];			//庄家扑克
    public byte bHasAddTime;			// 是否有加倍的流程(0 没有加倍  1 有加倍)

    protected override void UnPackBody()
    {
        wBankerUser = ReadUInt16();
        for (int i = 0; i < LandlordsModel.GAME_NUM; i++)
        {
            wUserTimes[i] = ReadUInt16();
        }
        for (int i = 0; i < 3; i++)
        {
            cbBankerCard[i] = ReadByte();
        }
        bHasAddTime = ReadByte();
    }
}

//用户出牌
public class CMD_Landlords_S_OutCard : DataBase
{
    public UInt16 wOutCardUser;				//出牌玩家
    public byte byCardType;						// 出牌类型
    public byte cbCardCount;				//出牌数目   
    public byte[] cbCardData = new byte[LandlordsModel.MAX_COUNT];      //扑克列表
    public UInt16[] wUserTimes = new UInt16[LandlordsModel.GAME_NUM];   //此次出牌之后各个玩家的倍数
    public UInt16 wNextUser;				//下个出牌玩家
    public byte bNextUserCanOutCard;			// 下一家是否要得起	(0,要不起；1，要得起)
    public byte[] cbRestCardCount = new byte[LandlordsModel.GAME_NUM];  //所有玩家余下扑克数目
    public byte bySearchCount;					// 解的个数
	public byte[] cbEachResultCount = new byte[LandlordsModel.MAX_COUNT];			// 每个解的牌数
    public byte[,] cbResultCard = new byte[LandlordsModel.MAX_COUNT, LandlordsModel.MAX_COUNT];	// 结果扑克

    protected override void UnPackBody()
    {
        wOutCardUser = ReadUInt16();
        byCardType = ReadByte();
        cbCardCount = ReadByte();
        for (int i = 0; i < LandlordsModel.MAX_COUNT; i++)
        {
            cbCardData[i] = ReadByte();
        }
        for (int i = 0; i < LandlordsModel.GAME_NUM; i++)
        {
            wUserTimes[i] = ReadUInt16();
        }
        wNextUser = ReadUInt16();
        bNextUserCanOutCard = ReadByte();
        for (int i = 0; i < LandlordsModel.GAME_NUM; i++)
        {
            cbRestCardCount[i] = ReadByte();
        }
        bySearchCount = ReadByte();
        for (int i = 0; i < LandlordsModel.MAX_COUNT; i++)
        {
            cbEachResultCount[i] = ReadByte();
        }
        for (int i = 0; i < LandlordsModel.MAX_COUNT; i++)
        {
            for (int j = 0; j < LandlordsModel.MAX_COUNT; j++)
            {
                cbResultCard[i, j] = ReadByte();
            }
        }
    }
}

//出牌失败
public class CMD_Landlords_S_OutCardFail : DataBase
{
    public string szDescribeString;		// 失败原因

    protected override void UnPackBody()
    {
        szDescribeString = ReadString(80);
    }
};


//放弃出牌
public class CMD_Landlords_S_PassCard : DataBase
{
    public byte cbTurnOver; //一轮结束
    public UInt16 wPassCardUser;    //放弃玩家
    public UInt16 wCurrentUser; //当前出牌玩家   
    public byte bNextUserCanOutCard;		// 下一家是否要得起(0,要不起；1，要得起)
    public byte bySearchCount;					// 解的个数
    public byte[] cbEachResultCount = new byte[LandlordsModel.MAX_COUNT];			// 每个解的牌数
    public byte[,] cbResultCard = new byte[LandlordsModel.MAX_COUNT, LandlordsModel.MAX_COUNT];	// 结果扑克

    protected override void UnPackBody()
    {
        cbTurnOver = ReadByte();
        wPassCardUser = ReadUInt16();
        wCurrentUser = ReadUInt16();
        bNextUserCanOutCard = ReadByte();
        bySearchCount = ReadByte();
        for (int i = 0; i < LandlordsModel.MAX_COUNT; i++)
        {
            cbEachResultCount[i] = ReadByte();
        }
        for (int i = 0; i < LandlordsModel.MAX_COUNT; i++)
        {
            for (int j = 0; j < LandlordsModel.MAX_COUNT; j++)
            {
                cbResultCard[i, j] = ReadByte();
            }
        }
    }
}

//游戏结束
public class CMD_Landlords_S_GameEnd : DataBase
{
    //积分变量
    public int lCellScore;					//单元积分
    public Int64[] lGameScore = new Int64[LandlordsModel.GAME_NUM];    //游戏积分

    //春天标志
    public byte bChunTian;					//春天标志
    public byte bFanChunTian;				//春天标志

    //炸弹信息
    public byte cbBombCount;				//炸弹个数
    public byte[] cbEachBombCount = new byte[LandlordsModel.GAME_NUM];   //炸弹个数

    //游戏信息
    public byte[] cbCardCount = new byte[LandlordsModel.GAME_NUM];   //扑克数目
    public byte[,] cbHandCardData = new byte[LandlordsModel.GAME_NUM, LandlordsModel.MAX_COUNT]; //扑克列表

    protected override void UnPackBody()
    {
        lCellScore = ReadInt();
        for (int i = 0; i < LandlordsModel.GAME_NUM; i++)
        {
            lGameScore[i] = ReadInt64();
        }

        bChunTian = ReadByte();
        bFanChunTian = ReadByte();

        cbBombCount = ReadByte();
        for (int i = 0; i < LandlordsModel.GAME_NUM; i++)
        {
            cbEachBombCount[i] = ReadByte();
        }

        for (int i = 0; i < LandlordsModel.GAME_NUM; i++)
        {
            cbCardCount[i] = ReadByte();
        }
        for (int i = 0; i < LandlordsModel.GAME_NUM; i++)
        {
            for (int j = 0; j < LandlordsModel.MAX_COUNT; j++)
            {
                cbHandCardData[i, j] = ReadByte();
            }            
        }
    }
}

//托管
public class CMD_Landlords_S_Trustee : DataBase
{
    public UInt16 wTrusteeUser; //托管玩家
    public byte bTrustee;   //托管标志

    protected override void UnPackBody()
    {
        wTrusteeUser = ReadUInt16();
        bTrustee = ReadByte();
    }
}


#endregion

#region 发送
//用户叫分
public class CMD_Landlords_C_CallScore : DataBase
{
    public byte cbCallScore;    //叫分数目  0 不叫、不抢   1 叫地主、抢地主

    public CMD_Landlords_C_CallScore()
    {
        header.wMainCmdID = 200;
        header.wSubCmdID = 1;
    }

    protected override void PackBody()
    {
        WriteByte(cbCallScore);
    }
}

//用户出牌
public class CMD_Landlords_C_OutCard : DataBase
{
    public byte cbCardCount;    //出牌数目
    public byte[] cbCardData = new byte[LandlordsModel.MAX_COUNT];  //扑克数据

    public CMD_Landlords_C_OutCard()
    {
        header.wMainCmdID = 200;
        header.wSubCmdID = 2;
    }

    protected override void PackBody()
    {
        WriteByte(cbCardCount);
        for (int i = 0; i < LandlordsModel.MAX_COUNT; i++)
        {
            WriteByte(cbCardData[i]);
        }
    }
}

//用户放弃
public class CMD_Landlords_C_PassCard : DataBase
{
    public CMD_Landlords_C_PassCard()
    {
        header.wMainCmdID = 200;
        header.wSubCmdID = 3;
    }
}

//托管
public class CMD_Landlords_C_Trustee : DataBase
{
    public byte bTrustee;             //托管标志

    public CMD_Landlords_C_Trustee()
    {
        header.wMainCmdID = 200;
        header.wSubCmdID = 4;
    }

    protected override void PackBody()
    {
        WriteByte(bTrustee);
    }
}

//用户加倍
public class CMD_Landlords_C_AddTimes : DataBase
{
    // 是否加倍(0,不加倍;1,加倍)
    public byte cbAddTimes;

    public CMD_Landlords_C_AddTimes()
    {
        header.wMainCmdID = 200;
        header.wSubCmdID = 5;
    }

    protected override void PackBody()
    {
        WriteByte(cbAddTimes);
    }
}
#endregion