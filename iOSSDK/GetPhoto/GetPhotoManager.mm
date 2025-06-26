//
//  GetPhotoManager.mm
//  Unity-iPhone
//
//  Created by shuhang on 2017/11/22.
//

#include <sys/param.h>
#include <sys/mount.h>
#import <Photos/Photos.h>
#import <AssetsLibrary/AssetsLibrary.h>
#import <AVFoundation/AVCaptureDevice.h>
#import <AVFoundation/AVMediaFormat.h>
#import "GetPhotoManager.h"

#define camera 0
#define album  1

static int width = 128;
static int height = 128;
static GetPhotoManager *getPhotoControl;

extern "C" void OpenAlbum(int w, int h)
{
    width = w;
    height = h;
    if(getPhotoControl == NULL)
    {
        getPhotoControl = [[GetPhotoManager alloc] init];
    }
    [getPhotoControl GetPhoto:album];
}


@implementation GetPhotoManager

- (void)GetPhoto:(NSInteger)ChooseType
{
    UIImagePickerControllerSourceType sourceType = UIImagePickerControllerSourceTypePhotoLibrary;
    // 是否支持相机
    if([UIImagePickerController isSourceTypeAvailable:UIImagePickerControllerSourceTypeCamera]) {
        switch (ChooseType) {
            case camera:
                if([self GetCameraPermission] == -1)
                {
                    return;
                }
                sourceType = UIImagePickerControllerSourceTypeCamera;
                break;
            case album:
                if([self GetPickPermission] == -1)
                {
                    return;
                }
                sourceType = UIImagePickerControllerSourceTypePhotoLibrary;
                break;
            default:
                return;
        }
        // 跳转到相机或相册
        UIImagePickerController *picker = [[UIImagePickerController alloc] init];
        picker.delegate = self;
        picker.allowsEditing = YES;
        picker.sourceType = sourceType;
        [UnityGetGLViewController() presentViewController:picker animated:YES completion:nil];
    }
    else
    {
        UIAlertView *alert =[[UIAlertView alloc] initWithTitle:nil message:@"该设备不支持相机" delegate:nil cancelButtonTitle:@"关闭" otherButtonTitles:nil];
        [alert show];
    }
}

- (void)imagePickerController:(UIImagePickerController *)picker didFinishPickingMediaWithInfo:(NSDictionary *)info
{
    UIImage *image = [info objectForKey:UIImagePickerControllerEditedImage];
    [picker dismissViewControllerAnimated:YES completion:^{ }];
    NSData *fData = [self imageCompressForWidth:image targetWidth:width targetHeight:height];
    NSString *stringPhoto = [fData base64EncodedStringWithOptions:0];
    UnitySendMessage("ImageCrop", "ChooseComplete", [stringPhoto UTF8String]);
}

- (void)imagePickerControllerDidCancel:(UIImagePickerController *)picker
{
    UIAlertView *alert =[[UIAlertView alloc] initWithTitle:nil message:@"您取消了选择图片" delegate:nil cancelButtonTitle:@"关闭" otherButtonTitles:nil];
    [alert show];
    [picker dismissViewControllerAnimated:YES completion:^{ }];
}

- (NSData *) imageCompressForWidth:(UIImage *)sourceImage targetWidth:(CGFloat)defineWidth targetHeight:(CGFloat)defineHeight
{
    CGSize imageSize = sourceImage.size;
    CGFloat width = imageSize.width;
    CGFloat height = imageSize.height;
    CGFloat targetWidth = defineWidth;
    CGFloat targetHeight = (targetWidth / width) * height;
    if(targetHeight > defineHeight)
    {
        targetHeight = defineHeight;
        targetWidth = (targetHeight / height) * width;
    }
    UIGraphicsBeginImageContext(CGSizeMake(targetWidth, targetHeight));

    CGContextRef context = UIGraphicsGetCurrentContext();
    CGContextSetFillColorWithColor(context, [UIColor clearColor].CGColor);

    [sourceImage drawInRect:CGRectMake(0,0,targetWidth,  targetHeight)];
    UIImage* newImage = UIGraphicsGetImageFromCurrentImageContext();
    UIGraphicsEndImageContext();
    NSData *fData = UIImagePNGRepresentation(newImage);
    return fData;
}

-(int) GetCameraPermission
{
    NSString *mediaType = AVMediaTypeVideo;
    AVAuthorizationStatus authStatus = [AVCaptureDevice authorizationStatusForMediaType:mediaType];
    
    if(authStatus == AVAuthorizationStatusRestricted|| authStatus == AVAuthorizationStatusDenied){
        UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"提示"
                                                        message:@"请在设备的设置-隐私-相机中允许访问相机。"
                                                       delegate:self
                                              cancelButtonTitle:@"确定"
                                              otherButtonTitles:nil];
        [alert show];
        return -1;
    }
    else if(authStatus == AVAuthorizationStatusAuthorized){//允许访问
        return 0;
    }
    return 0;
}

//ALAuthorizationStatusNotDetermined = 0, // 用户尚未做出选择这个应用程序的问候
//ALAuthorizationStatusRestricted,        // 此应用程序没有被授权访问的照片数据。可能是家长控制权限
//ALAuthorizationStatusDenied,            // 用户已经明确否认了这一照片数据的应用程序访问
//ALAuthorizationStatusAuthorized         // 用户已经授权应用访问照片数据 authorizationStatus;
-(int) GetPickPermission
{
    if ([UIDevice currentDevice].systemVersion.floatValue < 8.0)
    {
        ALAuthorizationStatus author = [ALAssetsLibrary authorizationStatus];
        if (author == ALAuthorizationStatusRestricted || author == ALAuthorizationStatusDenied){
            UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"提示"
                                                            message:@"请在设备的设置-隐私-照片中允许访问照片。"
                                                           delegate:self
                                                  cancelButtonTitle:@"确定"
                                                  otherButtonTitles:nil];
            [alert show];
            return -1;
        }
    }
    else
    {
        PHAuthorizationStatus status = [PHPhotoLibrary authorizationStatus];
        if (status == PHAuthorizationStatusRestricted || status == PHAuthorizationStatusDenied) {
            UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"提示"
                                                            message:@"请在设备的设置-隐私-照片中允许访问照片。"
                                                           delegate:self
                                                  cancelButtonTitle:@"确定"
                                                  otherButtonTitles:nil];
            [alert show];
            return -1;
        }
    }
    return 0;
}

@end
