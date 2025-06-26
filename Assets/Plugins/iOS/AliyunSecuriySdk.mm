//
//  AliyunSecuriySdk.cpp
//  Unity-iPhone
//
//  Created by Tacus on 16/8/2.
//
//

//#include "AliyunSecuriySdk.h"
//#include "
#include <string>
#include <sys/types.h>
#include <sys/socket.h>
#include <netdb.h>
extern "C"
{
  extern int YunCeng_GetNextIPByGroupName(const char *group_name, const char *ip);
  extern int YunCeng_Init(const char *app_key);
  
  const char* ALSDK_GetNextIPByGroupName(const char * groupName,const char * domain)
  {
    char* str = new char[200];
    
    int code =  YunCeng_GetNextIPByGroupName(groupName,str);
    printf("code:%d,ip:%s,end \n",code,str);
    return str;
  }

  const char* GetLanguage()
  {
     int size = 200;
     char* str = new char[size];
     NSArray *languages = [NSLocale preferredLanguages];
     NSString *currentLanguage = [languages objectAtIndex:0];
     [currentLanguage getCString:str maxLength:size encoding:(NSUTF8StringEncoding)];
     return str;
  }
  
  
  int ALSDK_init(const char* appkey)
  {
//    printf("ALSDK_init appkey:%s",appkey);
    return YunCeng_Init(appkey);
  }
//  
//
}