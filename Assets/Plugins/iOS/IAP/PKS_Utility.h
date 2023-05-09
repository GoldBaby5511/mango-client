//
//  PKS_Utility.h
//  IAPPlugin
//
//  Created by preetminhas on 19/08/13.
//  Copyright (c) 2013 preetminhas. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface PKS_Utility : NSObject
+(char*) CStringCopy:(NSString*)input;
+(NSString*) CreateNSString:(const char*) string;
@end
