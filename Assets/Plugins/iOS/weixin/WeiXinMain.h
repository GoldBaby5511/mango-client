//
//  WeiXinMain.h
//  Unity-iPhone
//
//  Created by 赵雄飞 on 2017/4/20.
//
//

#ifndef WeiXinMain_h
#define WeiXinMain_h

extern "C"
{
    void doWeiXinLogin();
    void doWeiXinShare(int _type,char* url, char* title,char* message,char* imagePath,char* extInfo);
    //void doWeiXinPay(char* payOrder);
}

#endif /* WeiXinMain_h */


//微信支付流程尚未测试


/*
 注意事项：
 1.开发者需要为工程添加以下链接库:
 SystemConfiguration.framework
 libz.tbd
 libsqlite3.0.tbd
 libc++.dylib
 Security.framework
 CoreTelephony.framework
 CFNetwork.framework
 
 2.在工程文件中选择Build Settings
 在"Other Linker Flags"中加入"-Objc -all_load"
 在Search Paths中添加 libWeChatSDK.a ，WXApi.h，WXApiObject.h，文件所在位置
 
 3.在“info”标签栏的“URL type“添加“URL scheme”为你所注册的应用程序id
 
 4.在Classes/Unity/UnityAppController.mm中添加以下内容
 <1>添加头文件    #import "WeiXinManager.h"
 <2>在openURL方法中添加如下调用    [[WeiXinManager getInstance] HandleOpenUrl:url];
 <3>添加方法
 -(BOOL)application:(UIApplication *)application handleOpenURL:(NSURL *)url
 {
 [[WeiXinManager getInstance] HandleOpenUrl:url];
 return YES;
 }
 <4>在didFinishLaunchingWithOptions方法中注册appId : [[WeiXinManager getInstance] InitApp];
 
 
 <微信appId配置信息在WeiXinManager.mm文件InitApp方法中进行配置>
 若出现提示微信未安装异常，可在info.plist里面添加LSApplicationQueriesSchemes(Array类型)，然后插入weixin, wechat, mqq的string类型子项
 
 */



/*
 微信回调：
    微信回调信息在WeiXinManager.mm中进行配置
    微信登陆、分享、支付结束后分别调用GameManager<GameObject名称>上的
        OnWeiXinLoginFinish(string result)
        OnWeiXinShareFinish(string result)
        OnWeiXinShareFinish(string result)
    方法
 */
