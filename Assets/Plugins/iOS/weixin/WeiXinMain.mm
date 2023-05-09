//
//  WeiXinMain.m
//  Unity-iPhone
//
//  Created by 赵雄飞 on 2017/4/20.
//
//

#import "WeiXinManager.h"

extern "C"
{
    //微信登陆
    void doWeiXinLogin()
    {
        [[WeiXinManager getInstance] WeiXinLogin];
    }
    
    //微信分享
    void doWeiXinShare(int _type,char* url, char* title,char* message,char* imagePath,char* extInfo)
    {
        [[WeiXinManager getInstance] WeiXinShare:_type Url:[NSString stringWithUTF8String:url] Title:[NSString stringWithUTF8String:title] Message:[NSString stringWithUTF8String:message] ImagePath:[NSString stringWithUTF8String:imagePath] ExtInfo:[NSString stringWithUTF8String:extInfo]];
    }
    
    //微信支付
//    void doWeiXinPay(char* payOrder)
//    {
//        [[WeiXinManager getInstance] WeiXinPay:[NSString stringWithUTF8String:payOrder]];
//    }
}
