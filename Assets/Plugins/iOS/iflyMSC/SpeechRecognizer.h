//
//  SpeechRecognizer.h
//  Unity-iPhone
//
//  Created by kokunyo on 16/2/15.
//
//

#import "iflyMSC/iflyMSC.h"

@class IFlySpeechRecognizer;

/**
 语音听写demo
 使用该功能仅仅需要四步
 1.创建识别对象；
 2.设置识别参数；
 3.有选择的实现识别回调；
 4.启动识别
 */
@interface SpeechRecognizer : UIViewController<IFlySpeechRecognizerDelegate>

@property (nonatomic, strong) IFlySpeechRecognizer *iFlySpeechRecognizer;//不带界面的识别对象

@property (nonatomic, strong) NSString * result;
@property (nonatomic, assign) BOOL isCanceled;

@end
