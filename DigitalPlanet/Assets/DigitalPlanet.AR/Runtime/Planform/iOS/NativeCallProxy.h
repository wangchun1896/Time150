//
//  NativeCallProxy.h
//  Unity-iPhone
//
//  Created by 罗平 on 2021/9/4.
//

#import <Foundation/Foundation.h>
@protocol TimeSDKNativeCallsProtocol

@required

- (void)CalliOSFunction:(NSString *)value;


@end

__attribute__ ((visibility("default")))

@interface FrameworkLibAPI : NSObject
// call it any time after UnityFrameworkLoad to set object implementing NativeCallsProtocol methods
+(void) registerAPIforTimeSDKNativeCalls:(id<TimeSDKNativeCallsProtocol>) aApi;


@end


