//
//  HandleIP.m
//  Unity-iPhone
//
//  Created by 赵雄飞 on 16/7/26.
//
//

#import <Foundation/Foundation.h>

@interface BundleId : NSObject

extern "C"
{
    char* getIPv6(char* mHost, char* mPort);
}
@end