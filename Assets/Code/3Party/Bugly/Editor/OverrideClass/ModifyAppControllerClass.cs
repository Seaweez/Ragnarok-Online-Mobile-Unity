using UnityEngine;
using System.Collections;

public class ModifyAppControllerClass
{
	string _headerFilePath;
	string _filePath;

	public ModifyAppControllerClass (string headerFilePath, string filePath)
	{
		_headerFilePath = headerFilePath;
		_filePath = filePath;
	}

	public void MergeWithoutJPush ()
	{
		ROXClass appControllerMM = new ROXClass (_filePath);
        appControllerMM.WriteBelow ("extern \"C\" ScreenOrientation    UnityCurrentOrientation()   { return GetAppController().unityView.contentOrientation; }", 
			"extern \"C\"\n" +
			"{\n" +
			"    void _RegisterJPushNotification()\n" +
			"    {\n" +
			"    }\n" +
			"}");
	}

	public void OpenPush()
	{
		ROXClass appControllerHeader = new ROXClass (_headerFilePath);
		appControllerHeader.Replace ("#define UNITY_USES_REMOTE_NOTIFICATIONS 0", "#define UNITY_USES_REMOTE_NOTIFICATIONS 1");
	}

	//添加jpush，传入appkey ,是否是发布版
	public void MergeJPush (string appKey, bool release)
	{
		//header
		ROXClass appControllerHeader = new ROXClass (_headerFilePath);
		appControllerHeader.Replace ("@interface UnityAppController : NSObject<UIApplicationDelegate>", "@interface UnityAppController : NSObject<UIApplicationDelegate,JPUSHRegisterDelegate>");
		appControllerHeader.WriteBelow ("#include \"PluginBase/RenderPluginDelegate.h\"", "#import \"JPUSHService.h\"\n" +
		"// iOS10注册APNs所需头文件\n" +
		"#ifdef NSFoundationVersionNumber_iOS_9_x_Max\n" +
		"#import <UserNotifications/UserNotifications.h>\n" +
		"#endif");

		//class
		ROXClass appControllerMM = new ROXClass (_filePath);
		string apsForProduction = release ? "YES" : "NO";

		appControllerMM.WriteBelow ("#include <sys/sysctl.h>", 
		"\n"+
		"#import \"JPUSHService.h\"\n"+
		"#import \"JPushEventCache.h\"\n"+
		"#import <UserNotifications/UserNotifications.h>\n"

		);




		appControllerMM.WriteBelow ("[self preStartUnity];", 


			"\n"+
			"[[JPushEventCache sharedInstance] handFinishLaunchOption:launchOptions];\n"+
			"     /*\n" +
			"     不使用 IDFA 启动 SDK。\n" +
			"     参数说明：\n" +
			"     appKey: 极光官网控制台应用标识。\n" +
			"     channel: 频道，暂无可填任意。\n" +
			"     apsForProduction: YES: 发布环境；NO: 开发环境。\n" +
			"     */\n" +	
			string.Format ("    [JPUSHService setupWithOption:launchOptions appKey:@\"{0}\" channel:@\"\" apsForProduction:{1}];", appKey, apsForProduction)+
			"     [self registerJPushNotification];\n");
        appControllerMM.WriteBelow ("extern \"C\" ScreenOrientation    UnityCurrentOrientation()   { return GetAppController().unityView.contentOrientation; }", 
			"extern \"C\"\n" +
			"{\n" +
			"    void _RegisterJPushNotification()\n" +
			"    {\n" +
			"        UnityAppController* controller;\n" +
			"        controller = GetAppController();\n" +
			"        [controller registerJPushNotification];\n" +
			"    }\n" +
			"}");

		appControllerMM.WriteBelow ("- (void)application:(UIApplication*)application didRegisterForRemoteNotificationsWithDeviceToken:(NSData*)deviceToken\n{",
			" // Required.\n" +
			"    [JPUSHService registerDeviceToken:deviceToken];\n");

		appControllerMM.WriteBelow ("- (void)application:(UIApplication*)application didReceiveRemoteNotification:(NSDictionary*)userInfo\n{",
			" // Required.\n" +
			" if([UIApplication sharedApplication].applicationState==UIApplicationStateActive)\n"+
			"{}\n"+
			"else\n"+
			"{\n"+
			"[[JPushEventCache sharedInstance] sendEvent:userInfo withKey:@\"JPushPluginReceiveNotification\"];\n" +
			"    [JPUSHService handleRemoteNotification:userInfo];\n"+
			"}");

		appControllerMM.WriteBelow ("        handler(UIBackgroundFetchResultNoData);", 

			"\n"+
			"}\n"+
			"  [[JPushEventCache sharedInstance] sendEvent:userInfo withKey:@\"JPushPluginReceiveNotification\"];\n"+
			"if(false)\n{\n"
		);
		

		appControllerMM.WriteBelow ("- (void)applicationDidEnterBackground:(UIApplication*)application", 

			"{\n"+
			"::printf(\"-> applicationDidEnterBackground()\\n\");"+
			"}\n"+

			"// iOS 10 Support\n" +
			"- (void)jpushNotificationCenter:(UNUserNotificationCenter *)center willPresentNotification:(UNNotification *)notification withCompletionHandler:(void (^)(NSInteger))completionHandler {\n" +
			"    // Required\n" +

			"   if([UIApplication sharedApplication].applicationState==UIApplicationStateActive)\n"+
			"   {}\n"+
			"   else\n"+
			"   {\n"+

			"    NSDictionary * userInfo = notification.request.content.userInfo;\n" +
			"    if([notification.request.trigger isKindOfClass:[UNPushNotificationTrigger class]]) {\n" +
			"        [JPUSHService handleRemoteNotification:userInfo];\n" +

			"    [[NSNotificationCenter defaultCenter] postNotificationName:@\"JPushPluginReceiveNotification\" object:userInfo];\n" +
			"    completionHandler(UNNotificationPresentationOptionAlert | UNNotificationPresentationOptionBadge | UNNotificationPresentationOptionSound); // 需要执行这个方法，选择是否提醒用户，有Badge、Sound、Alert三种类型可以选择设置\n" +
			"    }\n" +
			"    }\n" +
			"    }\n"+
			"// iOS 10 Support\n" +
			"- (void)jpushNotificationCenter:(UNUserNotificationCenter *)center didReceiveNotificationResponse:(UNNotificationResponse *)response withCompletionHandler:(void (^)())completionHandler {\n" +
			"    // Required\n" +
			"    NSDictionary * userInfo = response.notification.request.content.userInfo;\n" +
			"    if([response.notification.request.trigger isKindOfClass:[UNPushNotificationTrigger class]]) {\n" +
			"        [JPUSHService handleRemoteNotification:userInfo];\n" +
			"    }\n" +
			"    completionHandler();  // 系统要求执行这个方法\n" +
			"}\n" +
			"- (void)registerJPushNotification\n" +
			"{\n" +
			"    if ([[UIDevice currentDevice].systemVersion floatValue] >= 10.0) {\n" +
			"#ifdef NSFoundationVersionNumber_iOS_9_x_Max\n" +
			"        JPUSHRegisterEntity * entity = [[JPUSHRegisterEntity alloc] init];\n" +
			"        entity.types = UNAuthorizationOptionAlert | UNAuthorizationOptionBadge | UNAuthorizationOptionSound;\n" +
			"        [JPUSHService registerForRemoteNotificationConfig:entity delegate:self];\n" +
			"#endif\n" +
			"    }\n" +
			"    \n" +
			"#if __IPHONE_OS_VERSION_MAX_ALLOWED > __IPHONE_7_1\n" +
			"    if ([[UIDevice currentDevice].systemVersion floatValue] >= 8.0) {\n" +
			"        //可以添加自定义categories\n" +
			"        [JPUSHService registerForRemoteNotificationTypes:(UIUserNotificationTypeBadge | UIUserNotificationTypeSound | UIUserNotificationTypeAlert) categories:nil];\n" +
			"    } else {\n" +
			"        //categories 必须为nil\n" +
			"        [JPUSHService registerForRemoteNotificationTypes:(UIRemoteNotificationTypeBadge | UIRemoteNotificationTypeSound |  UIRemoteNotificationTypeAlert) categories:nil];\n" +
			"    }\n" +
			"#else\n" +
			"    //categories 必须为nil\n" +
			"    [JPUSHService registerForRemoteNotificationTypes:(UIRemoteNotificationTypeBadge | UIRemoteNotificationTypeSound |UIRemoteNotificationTypeAlert) categories:nil];\n" +
			"#endif\n" +
			"}\n"+
			"- (void)applicationDidEnterBackground2:(UIApplication*)application"
		);


		appControllerMM.WriteBelow ("    ::printf(\"-> applicationWillEnterForeground()\\n\");", "\n     [UIApplication sharedApplication].applicationIconBadgeNumber= 0;\n"+
			" [JPUSHService resetBadge];\n"); 
	}
}
