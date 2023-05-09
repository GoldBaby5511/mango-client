//
//  WeiXinManager.m
//  Unity-iPhone
//
//  Created by 赵雄飞 on 17/4/7.
//
//

#import "WeiXinManager.h"

#define kWXNotInstallOrTooOld   2222

@implementation WeiXinManager

+ (instancetype)getInstance
{
    static WeiXinManager *instance;
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        instance = [[WeiXinManager alloc] init];
    });
    return instance;
}

//- (void)dealloc
//{
//    [super dealloc];
//}

- (void)InitApp
{
    //向微信注册
    [WXApi registerApp:@"wxcd3747df8ce6b4f7" enableMTA:NO];
    
    //向微信注册支持的文件类型
    UInt64 typeFlag = MMAPP_SUPPORT_TEXT | MMAPP_SUPPORT_PICTURE | MMAPP_SUPPORT_LOCATION | MMAPP_SUPPORT_VIDEO |MMAPP_SUPPORT_AUDIO | MMAPP_SUPPORT_WEBPAGE | MMAPP_SUPPORT_DOC | MMAPP_SUPPORT_DOCX | MMAPP_SUPPORT_PPT | MMAPP_SUPPORT_PPTX | MMAPP_SUPPORT_XLS | MMAPP_SUPPORT_XLSX | MMAPP_SUPPORT_PDF;
    
    NSLog(@"微信初始化！");
    [WXApi registerAppSupportContentFlag:typeFlag];
}

- (void)HandleOpenUrl:(NSURL *)url
{
    [WXApi handleOpenURL:url delegate:self];
}

- (void)WeiXinLogin
{
    //if(![WXApi isWXAppInstalled] || ![WXApi isWXAppSupportApi])
    if(![WXApi isWXAppInstalled])
    {
        [self ShowWXAlert];
        [self doLoginFinish:2 Result:@"您还未安装微信"];
        return;
    }
    
//    SendAuthReq* req = [[[SendAuthReq alloc] init] autorelease];
    SendAuthReq* req = [[SendAuthReq alloc] init];
    req.scope = @"snsapi_userinfo";
    req.state = @"123";
    [WXApi sendReq:req];
}

//type,0-文本，1-图片，2-网址，extinfo，0-好友，1-朋友圈
- (void)WeiXinShare:(int)_type Url:(NSString*)url Title:(NSString*)title Message:(NSString*)msg ImagePath:(NSString*)imagePath ExtInfo:(NSString*)extinfo
{
    //if(![WXApi isWXAppInstalled] || ![WXApi isWXAppSupportApi])
    if(![WXApi isWXAppInstalled])
    {
        [self ShowWXAlert];
        [self doShareFinish:2 Result:@"您还未安装微信"];
        return;
    }
    int shareStyle = [extinfo intValue];
    switch (_type) {
        case 0:
            [self WeiXinShareText:shareStyle Message:msg];
            break;
        case 1:
            [self WeiXinShareImage:shareStyle ImagePath:imagePath];
            break;
        case 2:
            [self WeiXinShareUrl:shareStyle Url:url Title:title Message:msg];
            break;
        default:
            NSLog(@"没有这种方式:%d",_type);
            break;
    }
}

//- (void)WeiXinPay:(NSString*)payOrder
//{
//    //if(![WXApi isWXAppInstalled] || ![WXApi isWXAppSupportApi])
//    if(![WXApi isWXAppInstalled])
//    {
//        [self ShowWXAlert];
//        [self doPayFinish:2 Result:@"您还未安装微信"];
//        return;
//    }
//    
//    NSError *error;
//    NSData *response = [payOrder dataUsingEncoding:NSUTF8StringEncoding];
//    if(response != nil)
//    {
//        NSMutableDictionary *dict = [NSJSONSerialization JSONObjectWithData:response options:NSJSONReadingMutableLeaves error:&error];
//        if(dict != nil)
//        {
//            NSMutableString *retcode = [dict objectForKey:@"retcode"];
//            if(retcode.intValue == 0)
//            {
//                NSMutableString *stamp = [dict objectForKey:@"timestamp"];
//                // 调起微信支付
//                PayReq* req = [[PayReq alloc] init];
//                req.partnerId = [dict objectForKey:@"partnerid"];
//                req.prepayId = [dict objectForKey:@"prepayid"];
//                req.nonceStr = [dict objectForKey:@"noncestr"];
//                req.timeStamp = stamp.intValue;
//                req.package = [dict objectForKey:@"package"];
//                req.sign = [dict objectForKey:@"sign"];
//                
//                [WXApi sendReq:req];
//            }
//            else
//            {
//                [self WeixinPayAlert:[dict objectForKey:@"retmsg"]];
//                [self doPayFinish:2 Result:[dict objectForKey:@"retmsg"]];
//            }
//        }
//        else
//        {
//            [self WeixinPayAlert:@"订单错误"];
//            [self doPayFinish:2 Result:@"订单错误"];
//        }
//    }
//    else
//    {
//        [self WeixinPayAlert:@"订单错误"];
//        [self doPayFinish:2 Result:@"订单错误"];
//    }
//}

#pragma mark 私有方法

- (void)WeiXinShareText:(int)_type Message:(NSString*)msg
{
    SendMessageToWXReq* req = [[SendMessageToWXReq alloc] init];
    req.text = msg;
    req.bText = YES;
    if(_type==0)
    {
        req.scene = WXSceneSession;
    }
    else
    {
        req.scene = WXSceneTimeline;
    }
    [WXApi sendReq:req];
}

- (void)WeiXinShareImage:(int)_type ImagePath:(NSString*)imagePath
{
    NSData *imageData = [NSData dataWithContentsOfFile:imagePath];
    NSData *compImg = [self checkThumbImageSize:imageData];
    //压缩资源
    UIImage *image = [[UIImage alloc] initWithData:imageData];
    NSData *pressData = UIImageJPEGRepresentation(image, 0.25);
    
    WXMediaMessage *message = [WXMediaMessage message];
    [message setThumbData:compImg];
    
    WXImageObject *imageObj = [WXImageObject object];
    imageObj.imageData = pressData;
    message.mediaObject = imageObj;
    
    SendMessageToWXReq* req = [[SendMessageToWXReq alloc] init];
    req.bText = NO;
    req.message = message;
    if(_type==0)
    {
        req.scene = WXSceneSession;
    }
    else
    {
        req.scene = WXSceneTimeline;
    }
    [WXApi sendReq:req];
}

- (void)WeiXinShareUrl:(int)_type Url:(NSString*)url Title:(NSString*)title Message:(NSString*)msg
{
    WXMediaMessage *message = [WXMediaMessage message];
    message.title = title;
    message.description = msg;
    [message setThumbImage:[UIImage imageNamed:@"AppIcon57x57.png"]];
    
    WXWebpageObject *webpageObj = [WXWebpageObject object];
    webpageObj.webpageUrl = url;
    message.mediaObject = webpageObj;
    
    SendMessageToWXReq* req = [[SendMessageToWXReq alloc] init];
    req.bText = NO;
    req.message = message;
    if(_type==0)
    {
        req.scene = WXSceneSession;
    }
    else
    {
        req.scene = WXSceneTimeline;
    }
    [WXApi sendReq:req];
}

- (void)ShowWXAlert
{
    UIAlertView *alertView = [[UIAlertView alloc] initWithTitle:@"提示"
                                                        message:@"您还未安装微信，是否前去安装？"
                                                       delegate:self
                                              cancelButtonTitle:@"确定"
                                              otherButtonTitles:@"取消", nil];
    [alertView setTag:kWXNotInstallOrTooOld];
    [alertView show];
//    [alertView release];
}

//- (void)WeixinPayAlert:(NSString*)message
//{
//    UIAlertView *weixinPayAlert = [[UIAlertView alloc] initWithTitle:@"提示"
//                                                             message:message
//                                                            delegate:nil
//                                                   cancelButtonTitle:@"取消"
//                                                   otherButtonTitles:nil];
//    [weixinPayAlert show];
////    [weixinPayAlert release];
//}

#pragma mark Application
- (void)alertView:(UIAlertView *)alertView clickedButtonAtIndex:(NSInteger)buttonIndex
{
    if(alertView.tag == kWXNotInstallOrTooOld)
    {
        if(buttonIndex==0)
        {
            NSString *urlStr = [WXApi getWXAppInstallUrl];
            [[UIApplication sharedApplication] openURL:[NSURL URLWithString:urlStr]];
        }
    }
}

- (void)onReq:(BaseReq *)req
{
    
}

- (void)onResp:(BaseResp *)resp
{
    if([resp isKindOfClass:[SendMessageToWXResp class]])
    {
        switch (resp.errCode)
        {
            case WXSuccess:
                [self doShareFinish:0 Result:@"share_ok"];
                break;
            case WXErrCodeUserCancel:
                [self doShareFinish:1 Result:@""];
                break;
            default:
                [self doShareFinish:2 Result:[resp errStr]];
                break;
        }
    }
    else if ([resp isKindOfClass:[SendAuthResp class]])
    {
        switch (resp.errCode)
        {
            case WXSuccess:
            {
                SendAuthResp *tmpResp = (SendAuthResp *)resp;
                [self doLoginFinish:0 Result:[tmpResp code]];
            }
                break;
            case WXErrCodeUserCancel:
                [self doLoginFinish:1 Result:@""];
                break;
            default:
                [self doLoginFinish:2 Result:[resp errStr]];
                break;
        }
    }
//    else if ([resp isKindOfClass:[PayResp class]])
//    {
//        switch (resp.errCode)
//        {
//            case WXSuccess:
//                [self doPayFinish:0 Result:@""];
//                break;
//            case WXErrCodeUserCancel:
//                [self doPayFinish:1 Result:@""];
//                break;
//            default:
//                [self doPayFinish:2 Result:[resp errStr]];
//                break;
//        }
//    }
}

#pragma mark 回调信息
//0-成功，1-取消，2-失败
- (void)doLoginFinish:(int)resultCode Result:(NSString*) result
{
    NSString* _result = [NSString stringWithFormat:@"%d|%@",resultCode,result];
    UnitySendMessage("PluginManager", "OnWeiXinLoginFinish", [_result UTF8String]);
}

- (void)doShareFinish:(int)resultCode Result:(NSString*) result
{
    NSString* _result = [NSString stringWithFormat:@"%d|%@",resultCode,result];
    UnitySendMessage("PluginManager", "OnWeiXinShareFinish", [_result UTF8String]);
}

//- (void)doPayFinish:(int)resultCode Result:(NSString*) result
//{
//    NSString* _result = [NSString stringWithFormat:@"%d|%@",resultCode,result];
//    UnitySendMessage("GameManager", "OnWeiXinPayFinish", [_result UTF8String]);
//}

#pragma mark 图片处理
-(UIImage*) OriginImage:(UIImage *)image scaleToSize:(CGSize)size
{
    UIGraphicsBeginImageContext(size);  //size 为CGSize类型，即你所需要的图片尺寸
    
    [image drawInRect:CGRectMake(0, 0, size.width, size.height)];
    
    UIImage* scaledImage = UIGraphicsGetImageFromCurrentImageContext();
    
    UIGraphicsEndImageContext();
    
    return scaledImage;   //返回的就是已经改变的图片
}
- (NSData *)checkThumbImageSize:(NSData *)thumbImageData
{
    static NSInteger maxThumbImageDataLen = 32 * 1024;
    static CGFloat thumbImageCompressionQuality = 0.75;
    
    if (thumbImageData.length > maxThumbImageDataLen)
    {
        UIImage *image = [[UIImage alloc] initWithData:thumbImageData];
        NSData *data = UIImageJPEGRepresentation(image, 0.5);
        
        if (data.length > maxThumbImageDataLen)
        {
            if(image.size.width > 400 || image.size.height > 400)
            {
                //尺寸减到400＊400
                image = [self OriginImage:image scaleToSize:CGSizeMake(667, 375)];
                data = UIImageJPEGRepresentation(image, 0.5);
            }
            
            if (data.length > maxThumbImageDataLen)
            {
                //尺寸减到250＊250
                if (image.size.width > 250 || image.size.height > 250)
                {
                    image = [self OriginImage:image scaleToSize:CGSizeMake(334, 188)];
                    data = UIImageJPEGRepresentation(image, thumbImageCompressionQuality);
                }
                
                if(data.length > maxThumbImageDataLen)
                {
                    //尺寸减到150＊150
                    if (image.size.width > 150 || image.size.height > 150)
                    {
                        image = [self OriginImage:image scaleToSize:CGSizeMake(167, 94)];
                        data = UIImageJPEGRepresentation(image, thumbImageCompressionQuality);
                    }
                    
                    if (data.length > maxThumbImageDataLen)
                    {
                        if (image.size.width > 100 || image.size.height > 100)
                        {
                            //尺寸减到100*100
                            image = [self OriginImage:image scaleToSize:CGSizeMake(84, 47)];
                            data = UIImageJPEGRepresentation(image, thumbImageCompressionQuality);
                        }
                    }
                }
            }
        }
        
        thumbImageData = data;
    }
    
    return thumbImageData;
}

@end

