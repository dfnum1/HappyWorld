#import "UNiOSAlert.h"
extern "C"
{
    typedef void (*OnCompletionCallback)(const char * str);
    void _showDialog(const char *title, const char *msg,
              const char * actionFirstStr,
const char * actionSecondStr,
const char * actionThirdStr,
              OnCompletionCallback testCallback)
    {
        [UNAlertDialog showDialogWithTitle:[NSString stringWithUTF8String : title]
          message:[NSString stringWithUTF8String : msg]
          firstButtonTitle: (actionFirstStr != nil) ?  [NSString stringWithUTF8String : actionFirstStr] : nil
          secondButtonTitle:(actionSecondStr != nil) ?  [NSString stringWithUTF8String : actionSecondStr] : nil
          thirdButtonTitle:(actionThirdStr != nil) ?  [NSString stringWithUTF8String : actionThirdStr] : nil
          onCompletion: testCallback];
    }
}
 
@implementation UNAlertDialog
+ (void) showDialogWithTitle:(NSString *) title
           message:(NSString*) msg
           firstButtonTitle : (NSString *)actionFirstText
           secondButtonTitle : (NSString *)actionSecondText
           thirdButtonTitle : (NSString *)actionThirdText
           onCompletion : (void(*)(const char *)) func
{
    UIAlertController* alert = [UIAlertController
               alertControllerWithTitle: title
               message:msg
               preferredStyle:UIAlertControllerStyleAlert];
    [UNAlertDialog setAlertWith : alert actionText : actionFirstText callType : @"yes" onCompletion : func];
    [UNAlertDialog setAlertWith : alert actionText : actionSecondText callType : @"no" onCompletion : func];
    [UNAlertDialog setAlertWith : alert actionText : actionThirdText callType : @"other" onCompletion : func];
 
    UIViewController * viewCtrl = UnityGetGLViewController();
    [viewCtrl presentViewController:alert animated:YES completion:nil];
 
}
+(void)setAlertWith : (UIAlertController * ) alert actionText: (NSString*)text callType: (NSString*)call
   onCompletion : (void(*)(const char *)) func;
{
    if(text != nil )
    {
        UIAlertAction* cancelAction = [UIAlertAction
                          actionWithTitle:text
                          style:UIAlertActionStyleDefault
                          handler:^(UIAlertAction * action) {
                             if(func != nil){
                                func([call UTF8String ]);
                             }
                          }];
        [alert addAction:cancelAction];
    }
}
@end
