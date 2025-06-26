//
//  SpeechRecognizer.m
//  Unity-iPhone
//
//  Created by kokunyo on 16/2/15.
//
//

#import "SpeechRecognizer.h"
#import "ISRDataHelper.h"
#import "Mp3Lame.h"

SpeechRecognizer *instance;

@implementation SpeechRecognizer

void InitSpeechRecognizer()
{
    //设置sdk的工作路径
    NSArray * paths = NSSearchPathForDirectoriesInDomains(NSDocumentDirectory, NSUserDomainMask, YES);
    NSString *docPath = [paths objectAtIndex:0];
    [IFlySetting setLogFilePath:docPath];
    
    //创建语音配置,appid必须要传入，仅执行一次则可
    NSString *initString = [[NSString alloc] initWithFormat:@"appid=%@",@"56bffdb9"];
    //所有服务启动前，需要确保执行createUtility
    [IFlySpeechUtility createUtility:initString];
    
    instance = [SpeechRecognizer alloc];
}

/**
 启动听写
 *****/
void startRecognizerHandler() {
 
    NSLog(@"%s[IN]",__func__);
    
    instance.isCanceled = NO;
    
    if(instance.iFlySpeechRecognizer == nil)
    {
        [instance initRecognizer];
    }
    
    [instance.iFlySpeechRecognizer cancel];
    
    //设置音频来源为麦克风
    [instance.iFlySpeechRecognizer setParameter:IFLY_AUDIO_SOURCE_MIC forKey:@"audio_source"];
    
    //设置听写结果格式为json
    [instance.iFlySpeechRecognizer setParameter:@"json" forKey:[IFlySpeechConstant RESULT_TYPE]];
    
    //保存录音文件，保存在sdk工作路径中，如未设置工作路径，则默认保存在library/cache下
    [instance.iFlySpeechRecognizer setParameter:@"asr.pcm" forKey:[IFlySpeechConstant ASR_AUDIO_PATH]];
    
    [instance.iFlySpeechRecognizer setDelegate:instance];
    
    [instance.iFlySpeechRecognizer startListening];
}

/**
 停止录音
 *****/
void stopRecognizerHandler() {
    
    [instance.iFlySpeechRecognizer stopListening];
}

/**
 设置识别参数
 ****/
-(void)initRecognizer
{
    NSLog(@"%s",__func__);
    
    //单例模式，无UI的实例
    if (_iFlySpeechRecognizer == nil) {
        _iFlySpeechRecognizer = [IFlySpeechRecognizer sharedInstance];
        NSLog(@"initRecognizer  初始化");
        [_iFlySpeechRecognizer setParameter:@"" forKey:[IFlySpeechConstant PARAMS]];
        
        //设置听写模式
        [_iFlySpeechRecognizer setParameter:@"iat" forKey:[IFlySpeechConstant IFLY_DOMAIN]];
    }
    _iFlySpeechRecognizer.delegate = self;
    
    if (_iFlySpeechRecognizer != nil) {
        
        //设置最长录音时间
        [_iFlySpeechRecognizer setParameter:@"30000" forKey:[IFlySpeechConstant SPEECH_TIMEOUT]];
        //设置后端点
        [_iFlySpeechRecognizer setParameter:@"3000" forKey:[IFlySpeechConstant VAD_EOS]];
        //设置前端点
        [_iFlySpeechRecognizer setParameter:@"3000" forKey:[IFlySpeechConstant VAD_BOS]];
        //网络等待时间
        [_iFlySpeechRecognizer setParameter:@"20000" forKey:[IFlySpeechConstant NET_TIMEOUT]];
        
        //设置采样率，推荐使用16K
        [_iFlySpeechRecognizer setParameter:@"16000" forKey:[IFlySpeechConstant SAMPLE_RATE]];
        
        //设置语言
        [_iFlySpeechRecognizer setParameter:@"zh_cn" forKey:[IFlySpeechConstant LANGUAGE]];
        //设置方言
        [_iFlySpeechRecognizer setParameter:@"mandarin" forKey:[IFlySpeechConstant ACCENT]];

        //设置是否返回标点符号
        [_iFlySpeechRecognizer setParameter:@"1" forKey:[IFlySpeechConstant ASR_PTT]];
        
    }
}

/**
 音量回调函数
 volume 0－30
 ****/
- (void) onVolumeChanged: (int)volume
{
    if (self.isCanceled) {
        return;
    }
    
    NSString *stringInt = [NSString stringWithFormat:@"%d",volume];
    const char *s = [stringInt UTF8String];
    UnitySendMessage("SpeechRecognizer","VolumeChanged",s);
}

/**
 开始识别回调
 ****/
- (void) onBeginOfSpeech
{
    NSLog(@"onBeginOfSpeech");
    
    self.result = @"";
}

/**
 停止录音回调
 ****/
- (void) onEndOfSpeech
{
    NSLog(@"onEndOfSpeech");
}

/**
 听写结束回调（注：无论听写是否正确都会回调）
 error.errorCode =
 0     听写正确
 other 听写出错
 ****/
- (void) onError:(IFlySpeechError *) error
{
    NSLog(@"%s",__func__);
    
    NSString *text ;
        
    if (self.isCanceled) {
        text = @"识别取消";
            
    } else if (error.errorCode == 0 ) {
        if (_result.length == 0) {
            text = @"无识别结果";
        }else {
            text = @"识别成功";
        }
    }else {
        text = [NSString stringWithFormat:@"发生错误：%d %@", error.errorCode,error.errorDesc];
        NSLog(@"%@",text);
    }
}

/**
 无界面，听写结果回调
 results：听写结果
 isLast：表示最后一次
 ****/
- (void) onResults:(NSArray *) results isLast:(BOOL)isLast
{
    
    NSMutableString *resultString = [[NSMutableString alloc] init];
    NSDictionary *dic = results[0];
    for (NSString *key in dic) {
        [resultString appendFormat:@"%@",key];
    }

    NSString * resultFromJson =  [ISRDataHelper stringFromJson:resultString];
    
    if (isLast){
        NSLog(@"听写结果(json)：%@测试",  self.result);
        
        [Mp3Lame audio_PCMtoMP3];
        
        const char *s = [self.result UTF8String];
        
        UnitySendMessage("SpeechRecognizer","SetResult",s);
    }
    NSLog(@"_result=%@",_result);
    NSLog(@"resultFromJson=%@",resultFromJson);
    
    self.result = [NSString stringWithFormat:@"%@%@", self.result,resultFromJson];
}



/**
 有界面，听写结果回调
 resultArray：听写结果
 isLast：表示最后一次
 ****/
- (void)onResult:(NSArray *)resultArray isLast:(BOOL)isLast
{
    NSMutableString *result = [[NSMutableString alloc] init];
    NSDictionary *dic = [resultArray objectAtIndex:0];
    
    for (NSString *key in dic) {
        [result appendFormat:@"%@",key];
    }
}



/**
 听写取消回调
 ****/
- (void) onCancel
{
    NSLog(@"识别取消");
}

@end
