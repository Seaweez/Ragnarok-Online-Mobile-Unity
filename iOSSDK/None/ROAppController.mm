#import "UnityAppController.h"

@interface ROAppController : UnityAppController
@end

IMPL_APP_CONTROLLER_SUBCLASS (ROAppController)

@implementation ROAppController

extern void UnityKeyboard_Hide ();

- (BOOL)textView:(UITextView *)textView shouldChangeTextInRange:(NSRange)range replacementText:(NSString *)_text{
    if ([_text isEqualToString:@"\n"]){
        //判断输入的字是否是回车，即按下return
        //在这里做你响应return键的代码
        UnityKeyboard_Hide();
        return NO; //这里返回NO，就代表return键值失效，即页面上按下return，不会出现换行，如果为yes，则输入页面会换行
    }
    return YES;
}

- (BOOL)textViewShouldReturn:(UITextView*)textFieldObj
{
    UnityKeyboard_Hide();
    return YES;
}



@end
