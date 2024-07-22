#import <Foundation/Foundation.h>
#import "IOSDeviceInfo.h"
#import "sys/utsname.h"
#include <sys/sysctl.h>
#include <mach/mach.h>
#include <net/if.h>
#include <net/if_dl.h>
#import <netinet/in.h>
#include "GetUUID.h"
#include <SystemConfiguration/SCNetworkReachability.h>
#include <CoreTelephony/CTTelephonyNetworkInfo.h>
#include "Reachability.h"
extern NSString* const GET_DEVICEES_INFO_FINISH = @"MSG_GET_DEVICES_INFO_FINISH";

#if defined(__cplusplus)
extern "C"
{
#endif
    extern void   UnitySendMessage(const char* obj,const char* method,const char*msg);
    extern NSString* CreateNSString(const char* string);
    extern   char* makeStringCopy(const char* string);
#if defined(__cplusplus)
}
#endif

static IOSDeviceInfo* _instance = nil;

@implementation IOSDeviceInfo

@synthesize  mCallBackObjectName;
@synthesize mDevicesInfo;

+ (IOSDeviceInfo*)instance
{
    @synchronized (self) {
        if (_instance == nil) {
            _instance = [[IOSDeviceInfo alloc] init];
        }
    }
    return _instance;
}

- (id)init
{
    self = [super init];
    if (self) {
        self.mCallBackObjectName = @"";
        self.mDevicesInfo = @"";
    }
    return self;
}

- (void)dealloc
{
    //如果要开启，则要设置‘Objective-C Automatic Reference Counting’ 为NO
   // [super dealloc];
}

-(void)init:(NSString *)callBackObjectName
{
    NSLog(@"DeviceInfo::init : %@",callBackObjectName);
    self.mCallBackObjectName = callBackObjectName;
}

/* 获取 设备制造商 设备型号 操作系统版本 mac地址 udid network */
-(NSString *)startGetDevicesInfo
{
    NSString *strSysVersion = [[UIDevice currentDevice] systemVersion]; // 操作系统版本
    NSString *strMacAddress = @""; // mac 地址
    NSString *strUuid = [getUUID getUUID];
    NSNumber *groupid = [NSNumber numberWithInt:1];
    NSString *strOSName = @"ios";

    if([self isJailBreak])
    {
    	groupid = @3;
    }
    else
    {
		groupid = @1;
    }

    NSDictionary *info = [NSDictionary dictionaryWithObjectsAndKeys: strSysVersion,@"os_ver",strUuid,@"udid", strMacAddress,@"mac_addr",groupid,@"group_id",strOSName,@"os_name",nil];
    NSString *infoStr = [self dictionaryToJson:info];
   
    return infoStr;
}

//Change Dictionary to Json
- (NSString*)dictionaryToJson:(NSDictionary *)dic
{
    NSError *parseError = nil;
    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:dic options:0 error:&parseError];
    return [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
}

char* printEnv(void)  
{  
    char *env = getenv("DYLD_INSERT_LIBRARIES");  
    NSLog(@"%s", env);  
    return env;  
}  
  
- (BOOL)isJailBreak  
{  
    if (printEnv()) {  
        NSLog(@"The device is jail broken!");  
        return YES;  
    }  
    NSLog(@"The device is NOT jail broken!");  
    return NO;  
}

// Return the local MAC addy
// Courtesy of FreeBSD hackers email list
// Accidentally munged during previous update. Fixed thanks to mlamb.
- (NSString *) macAddress
{
    
    int                 mib[6];
    size_t              len;
    char                *buf;
    unsigned char       *ptr;
    struct if_msghdr    *ifm;
    struct sockaddr_dl  *sdl;
    
    mib[0] = CTL_NET;
    mib[1] = AF_ROUTE;
    mib[2] = 0;
    mib[3] = AF_LINK;
    mib[4] = NET_RT_IFLIST;
    
    if ((mib[5] = if_nametoindex("en0")) == 0) {
        printf("Error: if_nametoindex error/n");
        return NULL;
    }
    
    if (sysctl(mib, 6, NULL, &len, NULL, 0) < 0) {
        printf("Error: sysctl, take 1/n");
        return NULL;
    }
    
    buf = (char*) malloc(len);
    
    if (buf == NULL) {
        printf("Could not allocate memory. error!/n");
        return NULL;
    }
    
    if (sysctl(mib, 6, buf, &len, NULL, 0) < 0) {
        printf("Error: sysctl, take 2");
        return NULL;
    }
    
    ifm = (struct if_msghdr *)buf;
    sdl = (struct sockaddr_dl *)(ifm + 1);
    ptr = (unsigned char *)LLADDR(sdl);
    NSString *outstring = [NSString stringWithFormat:@"%02x:%02x:%02x:%02x:%02x:%02x", *ptr, *(ptr+1), *(ptr+2), *(ptr+3), *(ptr+4), *(ptr+5)];
    
    //    NSString *outstring = [NSString stringWithFormat:@"%02x%02x%02x%02x%02x%02x", *ptr, *(ptr+1), *(ptr+2), *(ptr+3), *(ptr+4), *(ptr+5)];
    
    NSLog(@"outString:%@", outstring);
    
    free(buf);
    
    return [outstring uppercaseString];
}

- (NSString *)getCurrentNetworkState {
    // 状态栏是由当前app控制的，首先获取当前app
    UIApplication *app = [UIApplication sharedApplication];
    
    NSArray *children = [[[app valueForKeyPath:@"statusBar"] valueForKeyPath:@"foregroundView"] subviews];
    
    int type = 0;
    for (id child in children) {
        if ([child isKindOfClass:[NSClassFromString(@"UIStatusBarDataNetworkItemView") class]]) {
            type = [[child valueForKeyPath:@"dataNetworkType"] intValue];
        }
    }
    
    NSString *stateString = @"wifi";
    
    switch (type) {
        case 0:
            stateString = @"notReachable";
            break;
            
        case 1:
            stateString = @"2G";
            break;
            
        case 2:
            stateString = @"3G";
            break;
            
        case 3:
            stateString = @"4G";
            break;
            
        case 4:
            stateString = @"LTE";
            break;
            
        case 5:
            stateString = @"wifi";
            break;
            
        default:
            break;
    }
    
    return stateString;
}

-(NSString *)getNetworkType
{
    struct sockaddr_in zeroAddress;
    bzero(&zeroAddress, sizeof(zeroAddress));
    zeroAddress.sin_len = sizeof(zeroAddress);
    zeroAddress.sin_family = AF_INET;
    SCNetworkReachabilityRef defaultRouteReachability = SCNetworkReachabilityCreateWithAddress(NULL, (struct sockaddr *)&zeroAddress);
    SCNetworkReachabilityFlags flags;
    SCNetworkReachabilityGetFlags(defaultRouteReachability, &flags);
  
    if ((flags & kSCNetworkReachabilityFlagsReachable) == 0)
    {
        return @"Not Reachable";
    }
    
    if ((((flags & kSCNetworkReachabilityFlagsConnectionOnDemand ) != 0) ||
         (flags & kSCNetworkReachabilityFlagsConnectionOnTraffic) != 0))
    {
        if ((flags & kSCNetworkReachabilityFlagsInterventionRequired) == 0)
        {
            return @"wifi";
        }
    }
    
    if((flags & kSCNetworkReachabilityFlagsReachable) == kSCNetworkReachabilityFlagsReachable){
        
        if ((flags & kSCNetworkReachabilityFlagsIsWWAN) == kSCNetworkReachabilityFlagsIsWWAN){
//
//            if ((flags & kSCNetworkReachabilityFlagsTransientConnection) == kSCNetworkReachabilityFlagsTransientConnection){
//                
//                if((flags & kSCNetworkReachabilityFlagsConnectionRequired) == kSCNetworkReachabilityFlagsConnectionRequired){
//                    
//                    return @"2g";
//                }
//                return @"3g";
//            }
               return @"mobile";
        }
    
    }
    
    if ((flags & kSCNetworkReachabilityFlagsConnectionRequired) == 0)
    {
        return @"wifi";
    }
    
    return @"Unknown";
}

- (NSString *)getNetconnType{
    
    NSString *netconnType = @"";
    
    Reachability *reach = [Reachability reachabilityWithHostName:@"www.apple.com"];
    
    switch ([reach currentReachabilityStatus]) {
        case NotReachable:// 没有网络
        {
            
            netconnType = @"no network";
        }
            break;
            
        case ReachableViaWiFi:// Wifi
        {
            netconnType = @"wifi";
        }
            break;
            
        case ReachableViaWWAN:// 手机自带网络
        {
            // 获取手机网络类型
            CTTelephonyNetworkInfo *info = [[CTTelephonyNetworkInfo alloc] init];
            
            NSString *currentStatus = info.currentRadioAccessTechnology;
            
            if ([currentStatus isEqualToString:@"CTRadioAccessTechnologyGPRS"]) {
                
                netconnType = @"GPRS";
            }else if ([currentStatus isEqualToString:@"CTRadioAccessTechnologyEdge"]) {
                
                netconnType = @"2.75G EDGE";
            }else if ([currentStatus isEqualToString:@"CTRadioAccessTechnologyWCDMA"]){
                
                netconnType = @"3G";
            }else if ([currentStatus isEqualToString:@"CTRadioAccessTechnologyHSDPA"]){
                
                netconnType = @"3.5G HSDPA";
            }else if ([currentStatus isEqualToString:@"CTRadioAccessTechnologyHSUPA"]){
                
                netconnType = @"3.5G HSUPA";
            }else if ([currentStatus isEqualToString:@"CTRadioAccessTechnologyCDMA1x"]){
                
                netconnType = @"2G";
            }else if ([currentStatus isEqualToString:@"CTRadioAccessTechnologyCDMAEVDORev0"]){
                
                netconnType = @"3G";
            }else if ([currentStatus isEqualToString:@"CTRadioAccessTechnologyCDMAEVDORevA"]){
                
                netconnType = @"3G";
            }else if ([currentStatus isEqualToString:@"CTRadioAccessTechnologyCDMAEVDORevB"]){
                
                netconnType = @"3G";
            }else if ([currentStatus isEqualToString:@"CTRadioAccessTechnologyeHRPD"]){
                
                netconnType = @"HRPD";
            }else if ([currentStatus isEqualToString:@"CTRadioAccessTechnologyLTE"]){
                
                netconnType = @"4G";
            }
        }
            break;
            
        default:
            break;
    }
    
    return netconnType;
}

- (long)availableMemory{
  vm_statistics_data_t vmStats;
  mach_msg_type_number_t infoCount = HOST_VM_INFO_COUNT;
  kern_return_t kernReturn = host_statistics(mach_host_self(), HOST_VM_INFO, (host_info_t)&vmStats, &infoCount);
  if (kernReturn != KERN_SUCCESS) 
  {
    return -1;
  }
  return vm_page_size *vmStats.free_count;
}

#if defined(__cplusplus)
extern "C"{
#endif
    NSString* CteateNSString(const char* string)
    {
        return [NSString stringWithUTF8String:(string?string:"")];
    }
	
	char* makeStringCopy(const char* string)
    {
        if(string == NULL)
            return NULL;
        
        char *res = (char*)malloc(strlen(string) +1);
        
        strcpy(res,string);
        return res;
    }	
    
    void _InitDeviceInfo(const char* callBackObjectName)
    {
        [[IOSDeviceInfo instance] init:CteateNSString(callBackObjectName)];
    }
    
    const char* _GetCurrentNetworkState()
    {
        return makeStringCopy([[[IOSDeviceInfo instance] getNetconnType] UTF8String]);
    }
	
	long _GetRemainMemory()
    {
        return [[IOSDeviceInfo instance] availableMemory];
    }
    
    bool _IsJailBreak()
    {
        return [[IOSDeviceInfo instance] isJailBreak];
    }
	
	const char* _GetUUID()
    {
        return makeStringCopy([[getUUID getUUID] UTF8String]);
    }
    
#if defined(__cplusplus)
}
#endif



@end
