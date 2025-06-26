//
//  TapTapSDK.h
//  TapTapSDK
//
//  Created by TapTap on 2017/10/17.
//  Copyright © 2017年 易玩. All rights reserved.
//

#import <Foundation/Foundation.h>

#define SDK_VERSION @"1.2"  //!< SDK Version

#import "TTSDKApplicationDelegate.h"
#import "TTSDKLoginResult.h"
#import "TTSDKAccessToken.h"
#import "TTSDKLoginManager.h"
#import "TTSDKProfile.h"
#import "TTSDKProfileManager.h"
#import "TTSDKCertify.h"

@interface TapTapSDK: NSObject

/**
 *  @brief SDK初始化方法
 *
 *  请尽量在程序启动的时候初始化
 *  @param clientID TapTap开发者ID
 */
+ (void)sdkInitialize:(NSString *)clientID;

/**
 *  @brief 判断当前的TapTap版本是否支持该SDK调用
 *
 *  @return 支持返回YES，不支持返回NO
 */
+ (BOOL)isTapTapClientSupport;

/**
 *  @brief 打开TapTap论坛
 *  @param appid 游戏论坛ID，与TapTap开发者ID不同
 */
+ (void)openTapTapForum:(NSString *)appid;

@end
