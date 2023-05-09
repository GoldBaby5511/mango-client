using System;
using System.Collections.Generic;
using System.Text;

//数据处理类都公共接口
public interface IHandler
{
    bool Handler(NetPacket packet);
}

