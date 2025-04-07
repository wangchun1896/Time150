//
//  NativeCallProxy.m
//  Unity-iPhone
//
//  Created by 罗平 on 2021/9/4.
//

#import <Foundation/Foundation.h>
#import "NativeCallProxy.h"


@implementation FrameworkLibAPI

id<TimeSDKNativeCallsProtocol> timeSDKApi = NULL;
+(void)registerAPIforTimeSDKNativeCalls:(id<TimeSDKNativeCallsProtocol>) aApi
{
    timeSDKApi = aApi;
}

@end


extern "C" {

void CalliOSFunction(const char * value);

}


void CalliOSFunction(const char * value){
    printf("-- 调用ios原生方法");
    return [timeSDKApi CalliOSFunction:[NSString stringWithUTF8String:value]];
    
}


