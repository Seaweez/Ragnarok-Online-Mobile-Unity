//
//  TTSDKCertify.h
//  TapTapSDK
//
//  Created by 任龙 on 2018/5/9.
//  Copyright © 2018年 易玩. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "TTSDKProfile.h"

/**
 * @brief 进行实名认证的回调方法
 *
 * @param profile   实名认证后的用户信息
 * @param error     错误类型（自定义错误会在error_msg中提供）
 */
typedef void (^TTSDKCertifyResultHandler)(TTSDKProfile *profile, NSError *error);

@interface TTSDKCertify: NSObject

+ (void)certify:(NSString *)realName idCard:(NSString *)idCard handler:(TTSDKCertifyResultHandler)handler;

@end
