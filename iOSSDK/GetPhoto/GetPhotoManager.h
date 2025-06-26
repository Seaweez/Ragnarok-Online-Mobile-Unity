//
//  GetPhotoManager.h
//  Unity-iPhone
//
//  Created by shuhang on 2017/11/22.
//

#ifndef GetPhotoManager_h
#define GetPhotoManager_h

@interface GetPhotoManager : UIViewController<UIImagePickerControllerDelegate,UIActionSheetDelegate,UINavigationControllerDelegate>

- (void)GetPhoto:(NSInteger)ChooseType;

@end

#endif /* GetPhotoManager_h */
