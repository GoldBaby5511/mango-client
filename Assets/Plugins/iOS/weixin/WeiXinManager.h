//
//  WeiXinManager.h
//  Unity-iPhone
//
//  Created by 赵雄飞 on 17/4/7.
//
//

#import <Foundation/Foundation.h>

#import "WXApi.h"

@interface WeiXinManager : NSObject<WXApiDelegate,UIAlertViewDelegate>
{
    
}
+ (instancetype)getInstance;

//调用方法
- (void)InitApp;
- (void)HandleOpenUrl:(NSURL *)url;
- (void)WeiXinLogin;
- (void)WeiXinShare:(int)_type Url:(NSString*)url Title:(NSString*)title Message:(NSString*)msg ImagePath:(NSString*)imagePath ExtInfo:(NSString*)extinfo;
//- (void)WeiXinPay:(NSString*)payOrder;

//私有方法
- (void)doLoginFinish:(int)resultCode Result:(NSString*) result;
- (void)doShareFinish:(int)resultCode Result:(NSString*) result;
//- (void)doPayFinish:(int)resultCode Result:(NSString*) result;

@end


