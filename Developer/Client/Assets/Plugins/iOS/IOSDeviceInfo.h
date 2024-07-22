//
//  IOSGetDeviceInfo.h
//  Unity-iPhone
//
//
//

#ifndef IOSDeviceInfo_h
#define IOSDeviceInfo_h


#endif /* IOSGetDeviceInfo_h */

@interface IOSDeviceInfo : NSObject
{}



@property(nonatomic,retain) NSString* mCallBackObjectName;
@property(nonatomic,retain) NSString* mDevicesInfo;
- (void)init:(NSString*)callBackObjectName;
- (NSString*)startGetDevicesInfo;
@end

