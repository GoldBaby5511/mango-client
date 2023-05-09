#import <Foundation/Foundation.h>

@interface Clipboard : NSObject

extern "C"
{
    /*  compare the namelist with system processes  */
    void _copyTextToClipboard(const char *textList);
}

@end

@implementation Clipboard
//将文本复制到IOS剪贴板
- (void)objc_copyTextToClipboard : (NSString*)text
{
    UIPasteboard *pasteboard = [UIPasteboard generalPasteboard];
    pasteboard.string = text;
}
@end

extern "C" {
    static Clipboard *iosClipboard;
    void _copyTextToClipboard(const char *textList)
    {
        NSString *text = [NSString stringWithUTF8String: textList] ;
        if(iosClipboard == NULL)
        {
            iosClipboard = [[Clipboard alloc] init];
        }
        [iosClipboard objc_copyTextToClipboard: text];
    }
}


extern "C"
{
    float getiOSBatteryLevel()
    {
        [[UIDevice currentDevice] setBatteryMonitoringEnabled:YES];
        return [[UIDevice currentDevice] batteryLevel];
    }
}




