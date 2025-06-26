//
//  ROOCClass.h
//  Unity-iPhone
//
//  Created by Yuan YiFei on 2018/7/16.
//

#import <Foundation/Foundation.h>

@interface ROOCClass : NSObject<UIAlertViewDelegate>

@property (nonatomic, strong) UIAlertView *alert;

+ (BOOL) canRecord;

@end
