//
//  XDWXShare.h
//  XdComPlatform
//
//  Created by 杜凯强 on 2017/8/9.
//  Copyright © 2017年 dyy. All rights reserved.
//

#import <Foundation/Foundation.h>

#import <UIKit/UIKit.h>

# pragma mark - XDWXShareCallback

@protocol XDWXShareCallback <NSObject>

@optional


/**
 分享成功
 */
- (void)onWXShareSucceed;


/**
 分享失败

 @param error_msg 错误信息(0成功,-1普通错误,-2用户取消,-3发送失败,-4授权失败，-5微信不支持,) 未知错误(error_msg:"") 详情见微信官方API
 */
- (void)onWXShareFailed:(NSString*)error_msg;

@end

# pragma mark - XDWXShareObject

enum  XDWXShareType {
    XDWXShareTypeText     = 0,   /**< 文本分享 >**/
    XDWXShareTypeImage    = 1,   /**< 图片分享 >**/
    XDWXShareTypeMusic    = 2,   /**< 音乐分享 >**/
    XDWXShareTypeVideo    = 3,   /**< 视频分享 >**/
    XDWXShareTypeWebPage  = 4,   /**< 网页分享 >**/
};

@interface XDWXShareObject : NSObject

+ (instancetype)shareObject;

//################ 基础信息 #########################

@property (nonatomic,retain) NSString *text;//文字内容

@property (nonatomic,assign) BOOL bText;//(YES文本分享,NO多媒体数据分享)

@property (nonatomic,assign) int scene;//分享场景（0聊天界面，1朋友圈，2收藏）

@property (nonatomic,assign) enum XDWXShareType type;//分享类型

@property (nonatomic,retain) NSString *title;//标题

@property (nonatomic,retain) NSString *descriptionStr;//内容描述

@property (nonatomic,retain) NSData *thumbData;//缩略图数据 大小不能超过32K

@property (nonatomic,retain) NSString *thumbPath;//缩略图路径

@property (nonatomic,retain) UIImage *thumbImage;//缩略图对象 thumbData、thumbPath、thumbImage同时为空或者数据不符合要求时，将无法显示缩略图。 优先级thumbImage > thumbData > thumbPath。

@property (nonatomic,retain) NSString *mediaTagName;//长度不能超过64字节

@property (nonatomic,retain) NSString *messageExt;

@property (nonatomic,retain) NSString *messageAction;

//################ 图片分享 ################################


/** 图片真实数据内容
 * @note 大小不能超过10M
 * @note imageData和imageUrl成员不能同时为空
 */
@property (nonatomic,retain) NSData *imageData;


/** 图片url
 * @note 长度不能超过10K
 */
@property (nonatomic,retain) NSString *imageUrl;




//################ 音乐分享 ################################

/** 音乐网页的url地址
 * @note 长度不能超过10K
 */
@property (nonatomic, retain) NSString *musicUrl;

/** 音乐lowband网页的url地址
 * @note 长度不能超过10K
 */
@property (nonatomic, retain) NSString *musicLowBandUrl;

/** 音乐数据url地址
 * @note 长度不能超过10K
 */
@property (nonatomic, retain) NSString *musicDataUrl;

/**音乐lowband数据url地址
 * @note 长度不能超过10K
 */
@property (nonatomic, retain) NSString *musicLowBandDataUrl;




//################ 视频分享 ################################

/** 视频网页的url地址
 * @note 长度不能超过10K
 */
@property (nonatomic, retain) NSString *videoUrl;

/** 视频lowband网页的url地址
 * @note 长度不能超过10K
 */
@property (nonatomic, retain) NSString *videoLowBandUrl;




//################  网页分享 ################################


/** 网页的url地址
 * @note 不能为空且长度不能超过10K
 */
@property (nonatomic, retain) NSString *webpageUrl;


@end


# pragma mark - XDWXShare


@interface XDWXShare : NSObject


/**
 设置分享回调

 @param callback XDWXShareCallback
 */
+ (void)setWXShareCallBack:(id<XDWXShareCallback>) callback;



/**
 分享

 @param message XDWXShareObject，请根据具体分享需求填写参数
 */
+ (void)WXShareWithObject:(XDWXShareObject*)message;


@end
