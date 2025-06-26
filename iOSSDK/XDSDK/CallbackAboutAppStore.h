//
//  CallbackAboutAppStore.h
//  XDSDK_Demo
//
//  Created by Felix on 2018/1/19.
//  Copyright © 2018年 XD. All rights reserved.
//

#import <StoreKit/StoreKit.h>

#ifndef CallbackAboutAppStore_h
#define CallbackAboutAppStore_h

@interface CallbackAboutAppStore : NSObject<SKPaymentTransactionObserver>

+(instancetype) shareInstance;

@end

#endif /* CallbackAboutAppStore_h */
