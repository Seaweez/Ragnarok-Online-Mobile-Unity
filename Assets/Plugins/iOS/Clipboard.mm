//
//  CopyToClipBoard.m
//  Unity-iPhone
//
//  Created by shuhang on 2017/12/4.
//

extern "C" int _CopyToClipBoard(const char* contents)
{
    UIPasteboard *pasteboard = [UIPasteboard generalPasteboard];
    if(contents != NULL && pasteboard != NULL)
    {
        pasteboard.string = [NSString stringWithUTF8String: contents];
        return 0;
    }
    else
        return -1;
}
