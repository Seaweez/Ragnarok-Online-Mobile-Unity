//
//  FunctionInXDSDK.mm
//  Unity-iPhone
//
//  Created by Felix on 16/5/17.
//
//

#import <Foundation/Foundation.h>
#import <XdComPlatform/XDCore.h>

#include "CallbackAboutSDK.h"

#import <StoreKit/StoreKit.h>
#import "CallbackAboutAppStore.h"

NSString* CreateNSStringFromString(const char* str)
{
    if (str)
    {
        return [NSString stringWithUTF8String:str];
    }
    return NULL;
}

CallbackAboutSDK* callbackAboutSDK;

extern "C"
{
    void _Start()
    {
        printf("_Start\n");
        callbackAboutSDK = [[CallbackAboutSDK alloc] init];
    }
    
    void _SetCallback()
    {
        printf("_SetCallback\n");
        [XDCore setCallBack:callbackAboutSDK];
        NSString* version = [[UIDevice currentDevice] systemVersion];
        if ([version compare:@"11.0" options:NSNumericSearch] != NSOrderedAscending)
        {
            [[SKPaymentQueue defaultQueue] addTransactionObserver:[CallbackAboutAppStore shareInstance]];
        }
    }
    
    void _Initialize(const char* app_id, const char* app_key, const char* secret_key, int orientation)
    {
        [XDCore setLoginEntries:[NSArray arrayWithObjects:@"WX_LOGIN",@"TAPTAP_LOGIN",@"GUEST_LOGIN",@"QQ_LOGIN",nil]];
        printf("_Initialize\n");
        NSString* strAppID = CreateNSStringFromString(app_id);
        NSString* strAppKey = CreateNSStringFromString(app_key);
        NSString* strSecretKey = CreateNSStringFromString(secret_key);
        [XDCore init:strAppID orientation:orientation channel:@"" version:@"" enableTapdb:FALSE];
    }
    
    void _Login(const char* key)
    {
        printf("_Login\n");
        NSString* strKey = CreateNSStringFromString(key);
    
        
    }
    
//    void _QQLogin(const char* open_id, const char* access_token)
//    {
//        NSString* openID = [NSString stringWithUTF8String:open_id];
//        NSString* accessToken = [NSString stringWithUTF8String:access_token];
//        [xdCore qqlogin:openID token:accessToken];
//    }
    
    void _Logout(int key)
    {
        [XDCore logout];
    }
    
    bool _IsLogined()
    {
        return [XDCore isLoggedIn];
    }
    
    const char* _GetUserID()
    {
//        NSString* userID = [xdCore getUserID];
//        return [userID cStringUsingEncoding:NSUTF8StringEncoding];
        return "function GetUserID invalid";
    }
    
    void _EnterPlatform()
    {
        [XDCore openUserCenter];
    }
    
    const char* _GetAccessToken()
    {
        NSString* accessToken = [XDCore getAccessToken];
        const char* cStringAccessToken = [accessToken cStringUsingEncoding:NSUTF8StringEncoding];
        char* copyCStringAccessToken = (char*)malloc(strlen(cStringAccessToken) + 1);
        if (copyCStringAccessToken)
        {
            strcpy(copyCStringAccessToken, cStringAccessToken);
        }
        return copyCStringAccessToken;
    }
    
    // void _ShowToolBar(int tool_bar_place)
    // {
    //     [xdCore showToolBar:tool_bar_place];
    // }
    
    // void _HideToolBar()
    // {
    //     [xdCore hideToolBar];
    // }
    
//    void _AccountSwitch()
//    {
//        [XDCore switch];
//    }
    
//    void _DoExit()
//    {
//        [xdCore exit];
//    }
    
    // void _Pause()
    // {
    //     [xdCore pause];
    // }
    
//    void _SetScreenOrientation(int orientation)
//    {
//        [xdCore SetScreenOrientation:orientation];
//    }
    
    bool _PayForProduct(const char* products)
    {
        NSString* strProducts = CreateNSStringFromString(products);
        if (strProducts)
        {
            NSArray* strProductsSeparateByComma = [strProducts componentsSeparatedByString:@","];
            if (strProductsSeparateByComma && strProductsSeparateByComma.count > 0)
            {
                NSMutableDictionary* dicProducts = [[NSMutableDictionary alloc] init];
                int indicator = 0;
                while (indicator < strProductsSeparateByComma.count - 1)
                {
                    int productID = [[strProductsSeparateByComma objectAtIndex:indicator] intValue];
                    int productCount = [[strProductsSeparateByComma objectAtIndex:indicator + 1] intValue];
                    [dicProducts setObject:@(productCount) forKey:@(productID)];
                    indicator += 2;
                }
                [XDCore requestProduct:dicProducts];
                return true;
            }
        }
        return false;
    }
    
    const char* _GetOrderID()
    {
//        NSString* orderID = [XDCore getOrderId];
//        return [orderID cStringUsingEncoding:NSUTF8StringEncoding];
        return "function GetOrderID invalid";
    }
    
    void _PayForProductWithParams(int price, const char* s_id, const char* product_id, const char* product_name, const char* role_id, const char* p_ext, int product_count, const char* order_id)
    {
        if (product_count > 0)
        {
            NSString* sID = CreateNSStringFromString(s_id);
            NSString* productID = CreateNSStringFromString(product_id);
            if (productID && productID.length > 0)
            {
                NSString* productName = CreateNSStringFromString(product_name);
                NSString* roleID = CreateNSStringFromString(role_id);
                if (roleID && roleID.length > 0)
                {
                    NSString* nsstringOrderID = CreateNSStringFromString(order_id);
                    NSString* ext = CreateNSStringFromString(p_ext);
                    NSLog(@"price = %@", @(price));
                    NSLog(@"sID = %@", sID);
                    NSLog(@"productID = %@", productID);
                    NSLog(@"productName = %@", productName);
                    NSLog(@"roleID = %@", roleID);
                    NSLog(@"ext = %@", ext);
                    NSLog(@"product_count = %@", @(product_count));
                    NSLog(@"orderID = %@", nsstringOrderID);
                    NSArray* keys = [[NSArray alloc] initWithObjects:@"Product_Price", @"Sid", @"Product_Id", @"Product_Name", @"Role_Id", @"EXT", @"Product_Count", @"OrderId", nil];
                    NSArray* values = [[NSArray alloc] initWithObjects:@(price), sID, productID, productName, roleID, ext, @(product_count), nsstringOrderID, nil];
                    NSDictionary* dict = [[NSDictionary alloc] initWithObjects:values forKeys:keys];
                    [XDCore requestProduct:dict];
                }
            }
        }
    }
    
    const char* _GetSDKVersion()
    {
        NSString* sdkVersion = [XDCore getSDKVersion];
        return [sdkVersion cStringUsingEncoding:NSUTF8StringEncoding];
    }
    
//    bool _IsGuester()
//    {
//        return [xdCore isGuester];
//    }
    
    bool _HandleXDOpenURL(const char* url)
    {
        NSString* strURL = CreateNSStringFromString(url);
        if (strURL)
        {
            NSURL* nsURL = [[NSURL alloc] initWithString:strURL];
            return [XDCore HandleXDOpenURL:nsURL];
        }
        return 0;
    }
    
//    void _OpenXDWebView(const char* url)
//    {
//        NSString* strURL = CreateNSStringFromString(url);
//        if (strURL)
//        {
//            [xdCore openXDWebView:strURL];
//        }
//    }
    
//    bool _IsSupportWX()
//    {
//        return [XDCore isSupportWX];
//    }
    
    bool _IsInstalledWX()
    {
//        return [XDCore isInstalledWX];
        return false;
    }
    
//    bool _IsInstalledQQ()
//    {
//        return [XDCore isInstalledQQ];
//    }
    
    void _SetCustomLogin()
    {
//        [xdCore setCustomLogin];
    }
    
    void _LoginWithWX()
    {
//        [xdCore loginWithWX];
    }
    
//    void _LoginWithQQ()
//    {
//        [xdCore loginWithQQ];
//    }
    
//    void _LoginWithGuester()
//    {
//        [xdCore loginWithGuester];
//    }
    
//    void _LoginWithXD()
//    {
//        [xdCore loginWithXD];
//    }
    
    void _HideWechat()
    {
        [XDCore hideWX];
    }

    void _Log(const char* str)
    {
        NSString* nsstringStr = CreateNSStringFromString(str);
        NSLog(@"%@", nsstringStr);
    }

    void _HideGuest(bool b)
    {
        [XDCore hideGuest];
    }
    
    void _OpenRealName()
    {
        [XDCore openRealName];
    }

    void _HideTapTap()
    {
        [XDCore hideTapTap];
    }
    
    void storeReviewCall()
    {
        
    }
}
