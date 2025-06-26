#import "SafeAreaImpl.h"
#include "UnityAppController.h"
#include "UI/UnityView.h"

// Macros
#define isEdgeProtectOn(n) (n > 1)
#define validEdgeValue(n) (n >= 1 && n <= 2)
#define EdgeOff 1
#define EdgeOn 2
#define EdgeDefault EdgeOn

#if defined(__cplusplus)
extern "C" {
#endif
    extern void UnitySendMessage(const char* obj, const char* method, const char* msg);
#if defined(__cplusplus)
}
#endif

extern "C" void GetSafeAreaImpl(float* l, float* r, float* b, float* t, float* w, float* h)
{
    UIView* view = GetAppController().unityView;
    CGSize screenSize = view.bounds.size;
    float scale = view.contentScaleFactor;

    UIEdgeInsets insets = view.safeAreaInsets;

    *l = insets.left * scale;
    *r = insets.right * scale;
    *b = insets.bottom * scale;
    *t = insets.top * scale;
    *w = (screenSize.width - insets.left - insets.right) * scale;
    *h = (screenSize.height - insets.top - insets.bottom) * scale;
}

extern "C" void AddChangeOrientationListener()
{
    [[NSNotificationCenter defaultCenter] addObserver:[SafeAreaImplInstance sharedInstance] selector:@selector(changeRotate:) name:UIDeviceOrientationDidChangeNotification object:nil];
}

extern "C" void RemoveChangeOrientationListener()
{
    [[NSNotificationCenter defaultCenter] removeObserver:[SafeAreaImplInstance sharedInstance] name:UIDeviceOrientationDidChangeNotification object:nil];
}

extern "C" void SwitchIPhoneXEdgeProtect(bool bSwitch)
{
    [[NSUserDefaults standardUserDefaults] setInteger:(bSwitch ? EdgeOn : EdgeOff) forKey:@"EdgeProtect"];
    [[NSUserDefaults standardUserDefaults] synchronize];
}

@implementation SafeAreaImpl : NSObject
@end

@implementation SafeAreaImplInstance

static SafeAreaImplInstance * _sharedInstance = nil;

+ (SafeAreaImplInstance *)sharedInstance {
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        _sharedInstance = [[SafeAreaImplInstance alloc] init];
    });
    return _sharedInstance;
}

+ (bool)getEdgeProtect
{
    NSInteger nEdgeProtect = [[NSUserDefaults standardUserDefaults] integerForKey:@"EdgeProtect"];
    if (!validEdgeValue(nEdgeProtect)) {
        nEdgeProtect = EdgeDefault;
        SwitchIPhoneXEdgeProtect(isEdgeProtectOn(nEdgeProtect));
    }
    return isEdgeProtectOn(nEdgeProtect);
}

- (void)changeRotate:(NSNotification*)notification
{
    UIDeviceOrientation orientation = [[UIDevice currentDevice] orientation];
    if (orientation == UIDeviceOrientationLandscapeLeft || orientation == UIDeviceOrientationLandscapeRight) {
        UnitySendMessage("SafeArea", "OnChangeOrientation", orientation == UIDeviceOrientationLandscapeLeft ? "true" : "false");
    }
}

@end
