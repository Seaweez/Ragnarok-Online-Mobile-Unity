//
//  iOSNativeAPI.m
//  Unity-iPhone
//
//  Created by Felix on 16/7/14.
//
//

#include "iOSNativeAPI.h"

@implementation iOSNativeAPI

+ (uint64_t) getTotalMemory
{
    NSArray *paths = NSSearchPathForDirectoriesInDomains(NSDocumentDirectory, NSUserDomainMask, YES);
    NSError* error;
    NSDictionary *dictionary = [[NSFileManager defaultManager] attributesOfFileSystemForPath:[paths lastObject] error: &error];
    if(error)
    {
        NSLog(@"%@", [error localizedDescription]);
        return 0;
    }
    if (dictionary)
    {
        NSNumber *fileSystemSizeInBytes = [dictionary objectForKey: NSFileSystemSize];
        return [fileSystemSizeInBytes unsignedLongLongValue];

    }
    return 0;
}

+ (uint64_t) getFreeMemory
{
    NSArray *paths = NSSearchPathForDirectoriesInDomains(NSDocumentDirectory, NSUserDomainMask, YES);
    NSError* error;
    NSDictionary *dictionary = [[NSFileManager defaultManager] attributesOfFileSystemForPath:[paths lastObject] error: &error];
    if(error)
    {
        NSLog(@"%@", [error localizedDescription]);
        return 0;
    }
    if (dictionary)
    {
        NSNumber *freeFileSystemSizeInBytes = [dictionary objectForKey:NSFileSystemFreeSize];
        return [freeFileSystemSizeInBytes unsignedLongLongValue];
    }
    return 0;
}

@end

extern "C"
{
    uint64_t _getTotalMemory()
    {
        return [iOSNativeAPI getTotalMemory];
    }

    uint64_t _getFreeMemory()
    {
        return [iOSNativeAPI getFreeMemory];
    }
}