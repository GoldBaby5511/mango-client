//
//  PKS_Utility.m
//  IAPPlugin
//
//  Created by preetminhas on 19/08/13.
//  Copyright (c) 2013 preetminhas. All rights reserved.
//

#import "PKS_Utility.h"

@implementation PKS_Utility

//to return a copy of the c string so that Unity handles the memory and gets a valid value.
+(char*) CStringCopy:(NSString*)input {
    const char *string = [input cStringUsingEncoding:NSUTF8StringEncoding];
    char *result = NULL;
    if (string != NULL) {
        result = (char*)malloc(strlen(string) + 1);
        strcpy(result, string);
    }
    return result;
}

// This takes a char* you get from Unity and converts it to an NSString*
+(NSString*) CreateNSString:(const char*) string {
    NSString *result = @"";
    if (string != NULL) {
        result = [NSString stringWithUTF8String:string];
    }
    return result;
}

@end
