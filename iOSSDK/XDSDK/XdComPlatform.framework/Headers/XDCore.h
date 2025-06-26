//
//  XDCore.h
//  XdComPlatform
//
//  Created by w99wen on 14-11-10.
//  Copyright (c) 2014年 XINDONG Network. All rights reserved.
//



#import "XDCallback.h"


#ifndef XdComPlatform_XDCore_h

#define XdComPlatform_XDCore_h

/**
 心动SDK
 */
@interface XDCore : NSObject


# pragma mark - XDSDK3.0

/**
 获取SDK版本信息
 
 @return 版本信息
 */
+ (nonnull NSString*)getSDKVersion;


/**
 设置回调协议

 @param delegate <XDCallback>
 */
+ (void)setCallBack:(nonnull id<XDCallback>)delegate;


/**
 自定义登录入口。共五种，其中主要两种，次要两种。
 1、默认显示为：微信、TapTap、游客、QQ
 
 2、各登录方式对应名称如下：
 微信登录：WX_LOGIN，
 taptap登录：TAPTAP_LOGIN，
 QQ登录：QQ_LOGIN，
 游客登录：GUEST_LOGIN，
 心动登录：XD_LOGIN
 
 3、例，传入的数组。
 @[@"WX_LOGIN",@"TAPTAP_LOGIN",@"GUEST_LOGIN",@"QQ_LOGIN"]

 注：
    1）传入为空或nil，则当做隐藏所有按钮，直接显示心动登录。
    2）最多只能显示4种登录方式。
    3) 四个登录按钮TapTap和心动登录不能同时显示

 @param entries 入口类型，带顺序
 */
+ (void)setLoginEntries:(NSArray *)entries;


/**
 隐藏游客登录功能
 */
+ (void)hideGuest;


/**
 隐藏微信登录功能
 */
+ (void)hideWX;


/**
 隐藏QQ登录功能
 */
+ (void)hideQQ;


/**
 显示VeryCD登录功能
 */
+ (void)showVC;


/**
 强制QQ为Web登录方式
 */
+ (void)setQQWeb;


/**
 强制微信为web登录方式
 */
+ (void)setWXWeb;


/**
 隐藏TapTap登录
 */
+ (void)hideTapTap;


/**
 初始化SDK

 @param appid appid
 @param orientation 屏幕方向（0，横屏）（1，竖屏）
 */
+ (void)init:(nonnull NSString*)appid orientation:(int)orientation;


/**
 初始化sdk，包含心动SDK，TapDB统计SDK

 @param appid 心动appid
 @param orientation 屏幕方向
 @param channel 渠道号
 @param version 版本号
 @param enableTapdb 是否开启TapDB
 */
+ (void)init:(nonnull NSString*)appid orientation:(int)orientation
     channel:(nonnull NSString*)channel version:(nonnull NSString*)version
 enableTapdb:(BOOL)enableTapdb;


/**
 登录
 */
+ (void)login;


/**
 用户反馈
 */
+ (void)userFeedback;


/**
 获取Token

 @return Token
 */
+ (nullable NSString*)getAccessToken;


/**
 获取登录状态

 @return （YES，已登录）（NO，未登录）
 */
+ (BOOL)isLoggedIn;


/**
 打开用户中心

 @return （YES，打开成功）（NO，已经打开）
 */
+ (BOOL)openUserCenter;


/**
 打开实名认证窗口
 */
+ (void)openRealName;


/**
 进行支付

 @param prodectInfo 订单信息
 @return （YES，流程正常）（NO，尚未登录或重复调用）
 */
+ (BOOL)requestProduct:(nonnull NSDictionary*)prodectInfo;


/**
 登出用户
 */
+ (void)logout;


/**
 处理跳转app回调
 
 @param url 回调的URL
 @return TRUE-回调成功，FALSE-回调失败
 */
+ (BOOL)HandleXDOpenURL:(nonnull NSURL*)url;


#pragma mark - TapDB相关

/**
 设置玩家区服

 @param server server
 */
+ (void)setServer:(nonnull NSString *)server;


/**
 设置玩家等级

 @param level 等级
 */
+ (void)setLevel:(NSInteger)level;


@end

#endif
