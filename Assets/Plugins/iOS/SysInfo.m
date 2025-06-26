
#import "SysInfo.h"

@implementation SysInfo

int getDeviceBatteryPct()
{
	UIDevice *device = [UIDevice currentDevice];  
    device.batteryMonitoringEnabled = YES; 
    return (int)(device.batteryLevel * 100);
}

bool getDeviceBatteryCharging()
{
	UIDevice *device = [UIDevice currentDevice];  
    device.batteryMonitoringEnabled = YES; 
    return device.batteryState == UIDeviceBatteryStateCharging;
}

float getSysScreenBrightness()
{
	return [UIScreen mainScreen].brightness;
}

void setScreenBrightness(float value)
{
	[[UIScreen mainScreen] setBrightness:value];
}

@end