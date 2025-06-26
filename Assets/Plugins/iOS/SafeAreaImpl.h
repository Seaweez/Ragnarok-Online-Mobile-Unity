//
//  SafeAreaImpl.h
//  Unity-iPhone
//
//  Created by shuhang on 2017/10/25.
//
#import <Foundation/Foundation.h>

#ifndef SafeAreaImpl_h
#define SafeAreaImpl_h

@interface SafeAreaImpl : NSObject

@end

@interface SafeAreaImplInstnce : NSObject{
@private
}

+(bool)getEdgeProtect;

+(SafeAreaImplInstnce*)sharedInstance;
@end

#endif /* SafeAreaImpl_h */
