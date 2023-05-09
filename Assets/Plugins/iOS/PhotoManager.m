#import "PhotoManager.h"
@implementation PhotoManager
- ( void ) imageSaved: ( UIImage *) image didFinishSavingWithError:( NSError *)error
          contextInfo: ( void *) contextInfo
{
    NSLog(@"保存结束");
    if (error != nil) {
        NSLog(@"有错误");
    }
}
void _SavePhoto(char *readAddr)
{
    NSString *strReadAddr = [NSString stringWithUTF8String:readAddr];
    UIImage *img = [UIImage imageWithContentsOfFile:strReadAddr];
    NSLog([NSString stringWithFormat:@"w:%f, h:%f", img.size.width, img.size.height]);
    PhotoManager *instance = [PhotoManager alloc];
    UIImageWriteToSavedPhotosAlbum(img, instance,
                                   @selector(imageSaved:didFinishSavingWithError:contextInfo:), nil);
}
@end