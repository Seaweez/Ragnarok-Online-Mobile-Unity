//
//  ROOCClass.m
//  Unity-iPhone
//
//  Created by Yuan YiFei on 2018/7/12.
//

#import "ROOCClass.h"
#import <AVFoundation/AVFoundation.h>

extern "C"
{
    static ROOCClass *roocclass;
     char* GetIOSVersion()
    {
        NSString *version = [UIDevice currentDevice].systemVersion;
        char* a = (char*)malloc(strlen([version UTF8String]) + 1);
        strcpy(a, [version UTF8String]);
        return a;
    }
    
    bool isUserNotificationEnable()
    { // 判断用户是否允许接收通知
        BOOL isEnable = NO;
        if ([[UIDevice currentDevice].systemVersion floatValue] >= 8.0f) { // iOS版本 >=8.0 处理逻辑
            UIUserNotificationSettings *setting = [[UIApplication sharedApplication] currentUserNotificationSettings];
            isEnable = (UIUserNotificationTypeNone == setting.types) ? NO : YES;
        } else { // iOS版本 <8.0 处理逻辑
            UIRemoteNotificationType type = [[UIApplication sharedApplication] enabledRemoteNotificationTypes];
            isEnable = (UIRemoteNotificationTypeNone == type) ? NO : YES;
        }
        return isEnable;
    }
    
    void ShowHintOpenPushView(const char* title,const char* message,const char* cancelButtonTitle,const char* otherButtonTitles)
    {
        if(roocclass==nil)
        {
            roocclass = [[ROOCClass alloc] init];
            roocclass.alert=[[UIAlertView alloc]initWithTitle:[NSString stringWithUTF8String:title]
                            
                                                     message:[NSString stringWithUTF8String:message]
                            
                                                    delegate:roocclass
                            
                                           cancelButtonTitle:[NSString stringWithUTF8String:cancelButtonTitle]
                            
                                           otherButtonTitles:[NSString stringWithUTF8String:otherButtonTitles], nil];
        }
        [roocclass.alert show];
    }
    
    
    bool canRecord()
    {
        return [ROOCClass canRecord];
    }
    
    
}

@implementation ROOCClass
#pragma marks -- UIAlertViewDelegate --
//根据被点击按钮的索引处理点击事件
-(void)alertView:(UIAlertView *)alertView clickedButtonAtIndex:(NSInteger)buttonIndex
{
    NSLog(@"clickButtonAtIndex:%ld",buttonIndex);
    if( 0 == buttonIndex)
    {
        [_alert setHidden:true];
    }
    else if( 1 == buttonIndex)
    {
        [_alert setHidden:true];
    }
}

//AlertView已经消失时执行的事件
-(void)alertView:(UIAlertView *)alertView didDismissWithButtonIndex:(NSInteger)buttonIndex
{
    NSLog(@"didDismissWithButtonIndex clickButtonAtIndex:%ld",buttonIndex);
}

//ALertView即将消失时的事件
-(void)alertView:(UIAlertView *)alertView willDismissWithButtonIndex:(NSInteger)buttonIndex
{
    NSLog(@"willDismissWithButtonIndex:%ld",buttonIndex);
}

+ (BOOL) canRecord
{
    __block BOOL bCanRecord = YES;
    if ([[[UIDevice currentDevice] systemVersion] compare:@"7.0"] != NSOrderedAscending)
    {
        AVAudioSession *audioSession = [AVAudioSession sharedInstance];
        if ([audioSession respondsToSelector:@selector(requestRecordPermission:)]) {
            [audioSession performSelector:@selector(requestRecordPermission:) withObject:^(BOOL granted) {
                bCanRecord = granted;
            }];
        }
    }
    return bCanRecord;
}

@end
