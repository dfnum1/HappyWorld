#import "GetUUID.h"
#import "KeyChainStore.h"

@implementation getUUID

+(NSString *)getUUID
{
    NSString *bundleId = [[NSBundle mainBundle] bundleIdentifier];
    NSString *strUUID = (NSString *)[KeyChainStore load:bundleId];
    //首次执行该方法时，uuid为空
    if ([strUUID isEqualToString:@""] || !strUUID)
    {
        //生成一个uuid的方法
        CFUUIDRef uuidRef = CFUUIDCreate(kCFAllocatorDefault);
        
        strUUID = (NSString *)CFBridgingRelease(CFUUIDCreateString (kCFAllocatorDefault,uuidRef));
        
        //将该uuid保存到keychain
        [KeyChainStore save:bundleId data:strUUID];
        
    }
    return strUUID;
}

@end
