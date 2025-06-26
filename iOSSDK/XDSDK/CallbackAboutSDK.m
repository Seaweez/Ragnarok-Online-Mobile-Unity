//
//  CallbackAboutSDK.m
//  Unity-iPhone
//
//  Created by Felix on 16/5/16.
//
//
#import "CallbackAboutSDK.h"
#import <Foundation/Foundation.h>

@implementation CallbackAboutSDK

- (void) onInitSucceed
{
    NSLog(@"init success");
    UnitySendMessage("XDSDKEventListener", "OnReveiveInitializeSuccess", "");
}

- (void) onInitFailed:(NSString *)error_msg
{
    NSLog(@"init fail");
    const char* cStringErrorMsg = [error_msg cStringUsingEncoding:NSUTF8StringEncoding];
    char* copyCStringErrorMsg = (char*)malloc(strlen(cStringErrorMsg) + 1);
    if (copyCStringErrorMsg)
    {
        strcpy(copyCStringErrorMsg, cStringErrorMsg);
    }
    UnitySendMessage("XDSDKEventListener", "OnReceiveInitializeFail", copyCStringErrorMsg);
}

- (void) onLoginSucceed:(NSString *)access_token
{
    NSLog(@"login success");
    const char* cStringAccessToken = [access_token cStringUsingEncoding:NSUTF8StringEncoding];
    char* copyCStringAccessToken = (char*)malloc(strlen(cStringAccessToken) + 1);
    if (copyCStringAccessToken)
    {
        strcpy(copyCStringAccessToken, cStringAccessToken);
    }
    UnitySendMessage("XDSDKEventListener", "OnReceiveLoginSuccess", copyCStringAccessToken);
}

- (void) onLoginFailed:(NSString *)error_msg
{
    NSLog(@"login fail");
    const char* cStringErrorMsg = [error_msg cStringUsingEncoding:NSUTF8StringEncoding];
    char* copyCStringErrorMsg = (char*)malloc(strlen(cStringErrorMsg) + 1);
    if (copyCStringErrorMsg)
    {
        strcpy(copyCStringErrorMsg, cStringErrorMsg);
    }
    UnitySendMessage("XDSDKEventListener", "OnReceiveLoginFail", copyCStringErrorMsg);
}

- (void) onLoginCanceled
{
    NSLog(@"login cancel");
    UnitySendMessage("XDSDKEventListener", "OnReceiveLoginCancel", "");
}

- (void)onGuestBindSucceed:(NSString *)token
{
    NSLog(@"bind success");
    const char* cStringToken = [token cStringUsingEncoding:NSUTF8StringEncoding];
    char* copyCStringToken = (char*)malloc(strlen(cStringToken) + 1);
    if (copyCStringToken)
    {
        strcpy(copyCStringToken, cStringToken);
    }
    UnitySendMessage("XDSDKEventListener", "", copyCStringToken);
}

- (void)onLogoutSucceed
{
    NSLog(@"logout success");
    UnitySendMessage("XDSDKEventListener", "OnReceiveLogoutSuccess", "");
}

- (void) onPayCompleted
{
    NSLog(@"pay success");
    UnitySendMessage("XDSDKEventListener", "OnReceivePaySuccess", "");
}

- (void) onPayFailed:(NSString *)error_msg
{
    NSLog(@"pay fail");
    UnitySendMessage("XDSDKEventListener", "OnReceivePayFail", "");
}

- (void) onPayCanceled
{
    NSLog(@"pay cancel");
    UnitySendMessage("XDSDKEventListener", "OnReceivePayCancel", "");
}

- (void)onRealNameSucceed
{
    NSLog(@"real name success");
    UnitySendMessage("XDSDKEventListener", "OnReceiveRealNameSuccess", "");
}

- (void)onRealNameFailed:(nullable NSString*)error_msg
{
    NSLog(@"real name fail");
    UnitySendMessage("XDSDKEventListener", "OnReceiveRealNameFail", "");
}

@end
