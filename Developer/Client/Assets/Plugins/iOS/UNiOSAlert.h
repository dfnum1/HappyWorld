#import<Foundation/Foundation.h>
@interface UNAlertDialog : NSObject<UIAlertViewDelegate>{}
+ (void) showDialogWithTitle: (NSString *) title message:(NSString*) msg
      firstButtonTitle : (NSString *)actionFirstText
     secondButtonTitle : (NSString *)actionSecondText
     thirdButtonTitle : (NSString *)actionThirdText
    onCompletion : (void(*)(const char *)) func;
@end
