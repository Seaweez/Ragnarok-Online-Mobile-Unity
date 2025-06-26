#import "PhotoManager.h"

@implementation PhotoManager

- ( void ) imageSaved: ( UIImage *) image didFinishSavingWithError:( NSError *)error 
    contextInfo: ( void *) contextInfo
{
    NSLog(@"[PhotoManager] 保存相片结束: %@", 
        ((nil != error ? [error localizedDescription] : @"成功")));
}

// C-type interface begin
void RO_SavePhoto(char* readAddr)
{
    NSString *strReadAddr = [NSString stringWithUTF8String:readAddr];
    UIImage *img = [UIImage imageWithContentsOfFile:strReadAddr];
    // NSLog([NSString stringWithFormat:@"w:%f, h:%f", img.size.width, img.size.height]);
    PhotoManager *instance = [PhotoManager alloc];
    UIImageWriteToSavedPhotosAlbum(img, instance, 
        @selector(imageSaved:didFinishSavingWithError:contextInfo:), nil);
}
// C-type interface end

@end