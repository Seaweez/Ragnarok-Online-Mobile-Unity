//
//  CallbackAboutSDK.h
//  Unity-iPhone
//
//  Created by Felix on 16/5/16.
//
//

#import <XdComPlatform/XDCallback.h>

#ifndef CallbackAboutSDK_h
#define CallbackAboutSDK_h

@interface CallbackAboutSDK : NSObject<XDCallback>

//初始化成功
- (void)onInitSucceed;

//初始化失败
- (void)onInitFailed:(nullable NSString*)error_msg;

//登录成功
- (void)onLoginSucceed:(nonnull NSString*)access_token;

//登录失败
- (void)onLoginFailed:(nullable NSString*)error_msg;

//登录取消
- (void)onLoginCanceled;

//游客账号升级成功
- (void)onGuestBindSucceed:(nonnull NSString*)token;

//登出成功
- (void)onLogoutSucceed;

//支付完成
- (void)onPayCompleted;

//支付失败
- (void)onPayFailed:(nullable NSString*)error_msg;

//支付取消
- (void)onPayCanceled;

- (void)onRealNameSucceed;

- (void)onRealNameFailed:(nullable NSString*)error_msg;

@end

#endif /* CallbackAboutSDK_h */
