#import "UnityAppController.h"
#import <XdComPlatform/XDCore.h>
// #import "JPUSHService.h"
// // iOS10注册APNs所需头文件
// #ifdef NSFoundationVersionNumber_iOS_9_x_Max
// #import <UserNotifications/UserNotifications.h>
// #endif

#include "PluginBase/AppDelegateListener.h"

@interface ROAppController : UnityAppController//<JPUSHRegisterDelegate>
@end

IMPL_APP_CONTROLLER_SUBCLASS (ROAppController)

@implementation ROAppController

extern void UnityKeyboard_Hide ();

- (BOOL)application:(UIApplication*)application openURL:(NSURL*)url sourceApplication:(NSString*)sourceApplication annotation:(id)annotation
{
	NSLog(@"openURL");
    NSMutableArray* keys	= [NSMutableArray arrayWithCapacity:3];
	NSMutableArray* values	= [NSMutableArray arrayWithCapacity:3];

	#define ADD_ITEM(item)	do{ if(item) {[keys addObject:@#item]; [values addObject:item];} }while(0)

	ADD_ITEM(url);
	ADD_ITEM(sourceApplication);
	ADD_ITEM(annotation);

	#undef ADD_ITEM

	NSDictionary* notifData = [NSDictionary dictionaryWithObjects:values forKeys:keys];
	AppController_SendNotificationWithArg(kUnityOnOpenURL, notifData);
	return [XDCore HandleXDOpenURL:url];
}

- (BOOL)textView:(UITextView *)textView shouldChangeTextInRange:(NSRange)range replacementText:(NSString *)_text{
    if ([_text isEqualToString:@"\n"]){
        //判断输入的字是否是回车，即按下return
        //在这里做你响应return键的代码
        UnityKeyboard_Hide();
        return NO; //这里返回NO，就代表return键值失效，即页面上按下return，不会出现换行，如果为yes，则输入页面会换行
    }
    return YES;
}

- (BOOL)textViewShouldReturn:(UITextView*)textFieldObj
{
    UnityKeyboard_Hide();
    return YES;
}

- (BOOL)application:(UIApplication *)application handleOpenURL:(NSURL *)url
{
    NSLog(@"handleOpenURL");
    NSMutableArray* keys	= [NSMutableArray arrayWithCapacity:3];
    NSMutableArray* values	= [NSMutableArray arrayWithCapacity:3];
    
    #define ADD_ITEM(item)	do{ if(item) {[keys addObject:@#item]; [values addObject:item];} }while(0)
    
    ADD_ITEM(url);
    
    #undef ADD_ITEM
    
    NSDictionary* notifData = [NSDictionary dictionaryWithObjects:values forKeys:keys];
    AppController_SendNotificationWithArg(kUnityOnOpenURL, notifData);
    return [XDCore HandleXDOpenURL:url];
}

- (BOOL)application:(UIApplication *)app openURL:(NSURL *)url options:(NSDictionary<NSString*, id>*)options{
    NSLog(@"openURL1");
    NSMutableArray* keys	= [NSMutableArray arrayWithCapacity:3];
    NSMutableArray* values	= [NSMutableArray arrayWithCapacity:3];
    
#define ADD_ITEM(item)	do{ if(item) {[keys addObject:@#item]; [values addObject:item];} }while(0)
    
    ADD_ITEM(url);
    
#undef ADD_ITEM
    
    NSDictionary* notifData = [NSDictionary dictionaryWithObjects:values forKeys:keys];
    AppController_SendNotificationWithArg(kUnityOnOpenURL, notifData);
    return [XDCore HandleXDOpenURL:url];
}

// - (BOOL)application:(UIApplication *)application didFinishLaunchingWithOptions:(NSDictionary *)launchOptions
// {
//     [super application:application didFinishLaunchingWithOptions:launchOptions];
    
//     if ([[UIDevice currentDevice].systemVersion floatValue] >= 10.0) {
// #ifdef NSFoundationVersionNumber_iOS_9_x_Max
//         JPUSHRegisterEntity * entity = [[JPUSHRegisterEntity alloc] init];
//         entity.types = UNAuthorizationOptionAlert | UNAuthorizationOptionBadge | UNAuthorizationOptionSound;
//         [JPUSHService registerForRemoteNotificationConfig:entity delegate:self];
// #endif
//     }
    
// #if __IPHONE_OS_VERSION_MAX_ALLOWED > __IPHONE_7_1
//     if ([[UIDevice currentDevice].systemVersion floatValue] >= 8.0) {
//         //可以添加自定义categories
//         [JPUSHService registerForRemoteNotificationTypes:(UIUserNotificationTypeBadge | UIUserNotificationTypeSound | UIUserNotificationTypeAlert) categories:nil];
//     } else {
//         //categories 必须为nil
//         [JPUSHService registerForRemoteNotificationTypes:(UIRemoteNotificationTypeBadge | UIRemoteNotificationTypeSound |  UIRemoteNotificationTypeAlert) categories:nil];
//     }
// #else
//     //categories 必须为nil
//     [JPUSHService registerForRemoteNotificationTypes:(UIRemoteNotificationTypeBadge | UIRemoteNotificationTypeSound |UIRemoteNotificationTypeAlert) categories:nil];
// #endif
    
    
//     /*
//      不使用 IDFA 启动 SDK。
//      参数说明：
//      appKey: 极光官网控制台应用标识。
//      channel: 频道，暂无可填任意。
//      apsForProduction: YES: 发布环境；NO: 开发环境。
//      */
//     [JPUSHService setupWithOption:launchOptions appKey:@"1aecc9662adf1725e5db1a8f" channel:@"" apsForProduction:NO];
//     return YES;
// }

// - (void)application:(UIApplication *)application    didRegisterForRemoteNotificationsWithDeviceToken:(NSData *)deviceToken {
//     [super application:application didRegisterForRemoteNotificationsWithDeviceToken:deviceToken];
//     // Required.
//     [JPUSHService registerDeviceToken:deviceToken];
// }

// - (void)application:(UIApplication *)application    didReceiveRemoteNotification:(NSDictionary *)userInfo {
//     [super application:application didReceiveRemoteNotification:userInfo];
//     // Required.
//     [JPUSHService handleRemoteNotification:userInfo];
// }

// // iOS 10 Support
// - (void)jpushNotificationCenter:(UNUserNotificationCenter *)center willPresentNotification:(UNNotification *)notification withCompletionHandler:(void (^)(NSInteger))completionHandler {
//     // Required
//     NSDictionary * userInfo = notification.request.content.userInfo;
//     if([notification.request.trigger isKindOfClass:[UNPushNotificationTrigger class]]) {
//         [JPUSHService handleRemoteNotification:userInfo];
//     }
//     [[NSNotificationCenter defaultCenter] postNotificationName:@"JPushPluginReceiveNotification" object:userInfo];
//     completionHandler(UNNotificationPresentationOptionAlert | UNNotificationPresentationOptionBadge | UNNotificationPresentationOptionSound); // 需要执行这个方法，选择是否提醒用户，有Badge、Sound、Alert三种类型可以选择设置
// }

// // iOS 10 Support
// - (void)jpushNotificationCenter:(UNUserNotificationCenter *)center didReceiveNotificationResponse:(UNNotificationResponse *)response withCompletionHandler:(void (^)())completionHandler {
//     // Required
//     NSDictionary * userInfo = response.notification.request.content.userInfo;
//     if([response.notification.request.trigger isKindOfClass:[UNPushNotificationTrigger class]]) {
//         [JPUSHService handleRemoteNotification:userInfo];
//     }
//     completionHandler();  // 系统要求执行这个方法
// }


@end
