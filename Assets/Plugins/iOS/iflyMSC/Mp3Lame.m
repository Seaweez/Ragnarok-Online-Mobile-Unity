//
//  Mp3Lame.m
//  Unity-iPhone
//
//  Created by kokunyo on 16/6/27.
//
//

#import "Mp3Lame.h"

@implementation Mp3Lame

+ (void)audio_PCMtoMP3
{
    NSArray * paths = NSSearchPathForDirectoriesInDomains(NSDocumentDirectory, NSUserDomainMask, YES);
    NSString *docPath = [paths objectAtIndex:0];
    NSString *pcmFilePath = [docPath stringByAppendingString:@"/asr.pcm"];
    NSString *mp3FilePath = [docPath stringByAppendingString:@"/asr.mp3"];
    
    int read, write;
        
    FILE *pcm = fopen([pcmFilePath cStringUsingEncoding:1], "rb");  //source 被转换的音频文件位置
    FILE *mp3 = fopen([mp3FilePath cStringUsingEncoding:1], "wb");  //output 输出生成的Mp3文件位置
    
    const int PCM_SIZE = 8192;
    const int MP3_SIZE = 8192;
    short int pcm_buffer[PCM_SIZE*2];
    unsigned char mp3_buffer[MP3_SIZE];
        
    lame_t lame = lame_init();
    lame_set_in_samplerate(lame, 8000);
    lame_set_out_samplerate(lame, 16000);// 16K采样率
    lame_set_brate(lame, 128);// 压缩的比特率为128K
    lame_set_VBR(lame, vbr_default);
    lame_init_params(lame);
        
    do {
        read = fread(pcm_buffer, 2*sizeof(short int), PCM_SIZE, pcm);
        if (read == 0)
            write = lame_encode_flush(lame, mp3_buffer, MP3_SIZE);
        else
            write = lame_encode_buffer_interleaved(lame, pcm_buffer, read, mp3_buffer, MP3_SIZE);
            
        fwrite(mp3_buffer, write, 1, mp3);
            
    } while (read != 0);
        
    lame_close(lame);
    fclose(mp3);
    fclose(pcm);
        
    NSLog(@"MP3生成成功: %@",mp3FilePath);
}

@end
