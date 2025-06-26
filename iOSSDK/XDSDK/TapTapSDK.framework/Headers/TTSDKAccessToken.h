//
//  TTSDKAccessToken.h
//  TapTapSDK
//
//  Created by TapTap on 2017/10/17.
//  Copyright © 2017年 易玩. All rights reserved.
//

#import <Foundation/Foundation.h>

/**
 *  @brief TapTap登录授权数据封装类
 *
 *  该类封装了所有授权提供的返回数据
 */
@interface TTSDKAccessToken : NSObject

/// 唯一标志
@property (nonatomic, copy, readonly) NSString * kid;

/// 认证码
@property (nonatomic, copy, readonly) NSString * accessToken;

/// 认证码类型
@property (nonatomic, copy, readonly) NSString * tokenType;

/// mac密钥
@property (nonatomic, copy, readonly) NSString * macKey;

/// mac密钥计算方式
@property (nonatomic, copy, readonly) NSString * macAlgorithm;

/// 用户授权的权限，多个时以逗号隔开
@property (nonatomic, copy, readonly) NSString * scope;

- (instancetype)initWithTokenString:(NSString *)tokenString;

/**
 *  @brief 获取当前认证
 *
 *  该认证会优先读取本地缓存，不存在时将会返回nil
 */
+ (TTSDKAccessToken *)currentAccessToken;

@end
