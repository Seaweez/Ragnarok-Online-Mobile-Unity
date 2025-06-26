//
//  CallbackAboutAppStore.m
//  XDSDK_Demo
//
//  Created by Felix on 2018/1/19.
//  Copyright © 2018年 XD. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "CallbackAboutAppStore.h"

@implementation CallbackAboutAppStore : NSObject

static CallbackAboutAppStore *_sharedInstance = nil;

+(instancetype) shareInstance
{
    static dispatch_once_t onceToken ;
    dispatch_once(&onceToken, ^{
        _sharedInstance = [[self alloc] init] ;
    }) ;
    
    return _sharedInstance ;
}

- (BOOL)paymentQueue:(SKPaymentQueue *)queue shouldAddStorePayment:(SKPayment *)payment forProduct:(SKProduct *)product{
    NSInteger count = payment.quantity;
    NSString *indentify = payment.productIdentifier;
    NSString *userName = payment.applicationUsername;
    NSString *price = product.price.stringValue;
    
    NSLog(@"count = %zd,indentity = %@,userName = %@,price = %@",count,indentify,userName,price);
    UnitySendMessage("XDSDKEventListener", "OnReceivePurchaseFromAppStore", [indentify UTF8String]);
    return NO;
}

@end
