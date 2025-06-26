//
//  FunctionInTyrantdb.mm
//  Unity-iPhone
//
//  Created by Felix on 16/6/13.
//
//

#import <Foundation/Foundation.h>
#import "TyrantdbGameTracker.h"

NSString* CreateNSStringFromStringForTyrantdb(const char* str)
{
    if (str)
    {
        return [NSString stringWithUTF8String:str];
    }
    return NULL;
}

TGTUserType UserTypeFromInt(int i_type)
{
    TGTUserType userType;
    if (i_type == 0)
    {
        userType = TGTTypeAnonymous;
    }
    else if (i_type == 1)
    {
        userType = TGTTypeRegistered;
    }
    return userType;
}

TGTUserSex UserSexFromInt(int i_sex)
{
    TGTUserSex userSex;
    if (i_sex == 0)
    {
        userSex = TGTSexMale;
    }
    else if (i_sex == 1)
    {
        userSex = TGTSexFemale;
    }
    else if (i_sex == 2)
    {
        userSex = TGTSexUnknown;
    }
    return userSex;
}

extern "C"
{
    void _InitializeForTyrantdb(const char* app_id, const char* channel, const char* version)
    {
        NSLog(@"_InitializeForTyrantdb");
        NSString* nsstringAppID = CreateNSStringFromStringForTyrantdb(app_id);
        NSString* nsstringChannel = CreateNSStringFromStringForTyrantdb(channel);
        NSString* nsstringVersion = CreateNSStringFromStringForTyrantdb(version);
        NSLog(@"appid = %@", nsstringAppID);
        NSLog(@"channel = %@", nsstringChannel);
        NSLog(@"version = %@", nsstringVersion);
        [TyrantdbGameTracker onStart:nsstringAppID channel:nsstringChannel version:nsstringVersion];
    }
    
    void _SetUserForTyrantdb(const char* user_id, int user_type, int user_sex, int user_age, const char* user_name)
    {
        NSLog(@"_SetUserForTyrantdb");
        NSString* nsstringUserID = CreateNSStringFromStringForTyrantdb(user_id);
        NSString* nsstringUserName = CreateNSStringFromStringForTyrantdb(user_name);
        NSLog(@"userid = %@", nsstringUserID);
        NSLog(@"usertype = %d", user_type);
        NSLog(@"usersex = %d", user_sex);
        NSLog(@"userage = %d", user_age);
        NSLog(@"username = %@", nsstringUserName);
        [TyrantdbGameTracker setUser:nsstringUserID userType:UserTypeFromInt(user_type) userSex:UserSexFromInt(user_sex) userAge:user_age userName:nsstringUserName];
    }
    
    void _SetLevelForTyrantdb(int level)
    {
        NSLog(@"_SetLevelForTyrantdb");
        NSLog(@"level = %d", level);
        [TyrantdbGameTracker setLevel:level];
    }
    
    void _SetServerForTyrantdb(const char* server)
    {
        NSLog(@"_SetServerForTyrantdb");
        NSString* nsstringServer = CreateNSStringFromStringForTyrantdb(server);
        NSLog(@"server = %@", nsstringServer);
        [TyrantdbGameTracker setServer:nsstringServer];
    }
    
    void _OnChargeRequestForTyrantdb(const char* order_id, const char* product_name, int amount, const char* currency_type, int virtual_currency_amount, const char* payment)
    {
        NSLog(@"_OnChargeRequestForTyrantdb");
        NSString* nsstringOrderID = CreateNSStringFromStringForTyrantdb(order_id);
        NSString* nsstringProductName = CreateNSStringFromStringForTyrantdb(product_name);
        NSString* nsstringCurrencyType = CreateNSStringFromStringForTyrantdb(currency_type);
        NSString* nsstringPayment = CreateNSStringFromStringForTyrantdb(payment);
        [TyrantdbGameTracker onChargeRequest:nsstringOrderID product:nsstringProductName amount:amount currencyType:nsstringCurrencyType virtualCurrencyAmount:virtual_currency_amount payment:nsstringPayment];
    }
    
    void _OnChargeSuccessForTyrantdb(const char* order_id)
    {
        NSLog(@"_OnChargeSuccessForTyrantdb");
        NSString* nsstringOrderID = CreateNSStringFromStringForTyrantdb(order_id);
        [TyrantdbGameTracker onChargeSuccess:nsstringOrderID];
    }
    
    void _OnChargeFailForTyrantdb(const char* order_id, const char* reason)
    {
        NSLog(@"_OnChargeFailForTyrantdb");
        NSString* nsstringOrderID = CreateNSStringFromStringForTyrantdb(order_id);
        NSString* nsstringReason = CreateNSStringFromStringForTyrantdb(reason);
        [TyrantdbGameTracker onChargeFail:nsstringOrderID reason:nsstringReason];
    }
    
    void _OnChargeOnlySuccessForTyrantdb(const char* order_id, const char* product_name, int amount, const char* currency_type, int virtual_currency_amount, const char* payment)
    {
        NSLog(@"_OnChargeOnlySuccessForTyrantdb");
        NSString* nsstringOrderID = CreateNSStringFromStringForTyrantdb(order_id);
        NSString* nsstringProductName = CreateNSStringFromStringForTyrantdb(product_name);
        NSString* nsstringCurrencyType = CreateNSStringFromStringForTyrantdb(currency_type);
        NSString* nsstringPayment = CreateNSStringFromStringForTyrantdb(payment);
        [TyrantdbGameTracker onChargeOnlySuccess:nsstringOrderID product:nsstringProductName amount:amount currencyType:nsstringCurrencyType virtualCurrencyAmount:virtual_currency_amount payment:nsstringPayment];
    }
    
    void _OnCreateRole(const char* str)
    {

    }
    
    void _SetUser(const char* userId,int userType,int userSex,int userAge,const char* userName)
    {

    }
    
    void _ChargeTo3rd(const char* order_id, int amount, const char* currency_type, const char* payment)
    {
        NSLog(@"_ChargeTo3rd");
        NSString* nsstringOrderID = CreateNSStringFromStringForTyrantdb(order_id);
        NSString* nsstringCurrencyType = CreateNSStringFromStringForTyrantdb(currency_type);
        NSString* nsstringPayment = CreateNSStringFromStringForTyrantdb(payment);
        
        [TyrantdbGameTracker onChargeRequest:nsstringOrderID product:nsstringOrderID amount:0 currencyType:nsstringCurrencyType virtualCurrencyAmount:0 payment:nsstringPayment];
    }
    
    void _OnChargeRequest(const char* orderId,const char* product,int amount,const char* currencyType,int virtualCurrencyAmount,const char* payment)
    {
        NSString* orderIdstr = CreateNSStringFromStringForTyrantdb(orderId);
        NSString* productstr = CreateNSStringFromStringForTyrantdb(product);
        NSString* currencyTypestr = CreateNSStringFromStringForTyrantdb(currencyType);
        NSString* paymentstr = CreateNSStringFromStringForTyrantdb(payment);
        
        [TyrantdbGameTracker onChargeRequest:orderIdstr product:productstr amount:amount currencyType:currencyTypestr virtualCurrencyAmount:virtualCurrencyAmount payment:paymentstr];
    }
    
    const char* _GetAppVersion()
    {
        NSString * version = [[NSBundle mainBundle] objectForInfoDictionaryKey: @"CFBundleShortVersionString"];
        const char* cStringVersion = [version cStringUsingEncoding:NSUTF8StringEncoding];
        char* copyCStringVersion = (char*)malloc(strlen(cStringVersion) + 1);
        if (copyCStringVersion)
        {
            strcpy(copyCStringVersion, cStringVersion);
        }
        return copyCStringVersion;
    }
}
