using System;
using UnityEngine;
using System.Collections;
using RO;
using UnityEngine.Video;
namespace RenderHeads.Media.AVProVideo
{
    [SLua.CustomLuaClassAttribute]
    public class VideoPlayerNGUI : MonoBehaviour {
        public MediaPlayer MediaPlayer;
        public UITexture UITexture;
        private bool bStart = false;
        public Action finishPlaying;

        public bool OpenVideo(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                Debug.LogError("VideoPlayer: OpenVideo -- Get FilePath failed.");
                return false;
            }
            bool result = false;
            if (ApplicationHelper.AssetBundleLoadMode)
            {
                result=MediaPlayer.OpenVideoFromFile(MediaPlayer.FileLocation.RelativeToPeristentDataFolder,ApplicationHelper.platformFolder+"/Videos/"+filePath);
            }
            else
            {
                result=MediaPlayer.OpenVideoFromFile(MediaPlayer.FileLocation.RelativeToStreamingAssetsFolder,"Videos/"+filePath);
            }
            return result;
        }


        public void Play()
        {
//            Debug.Log("Play");
            MediaPlayer.Play ();
            bStart = true;
        }

        public void Pause()
        {
            MediaPlayer.Pause();
            bStart = false;
        }

        public void Close()
        {
            MediaPlayer.CloseVideo ();
            bStart = false;
        }

        public void Seek(float timeMS)
        {
            MediaPlayer.Control.Seek(timeMS);
        }

        public void SeekFast(float t)
        {
            MediaPlayer.Control.SeekFast(t);
        }

        public void Rewind()
        {
            MediaPlayer.Control.Rewind();
        }

        public void Stop()
        {
            MediaPlayer.Stop();
        }

        public void SetVolume(float v)
        {
            MediaPlayer.Control.SetVolume(v);
        }

        public float GetVolume()
        {
            return MediaPlayer.Control.GetVolume();
        }

        public float GetDurationMs()
        {
            return MediaPlayer.Info.GetDurationMs();
        }

        public float GetCurrentTimeMs()
        {
            return MediaPlayer.Control.GetCurrentTimeMs();
        }

        public float GetVideoDisplayRate()
        {
            return MediaPlayer.Info.GetVideoDisplayRate();
        }

        public void SetPlaybackRate(float rate)
        {
            MediaPlayer.Control.SetPlaybackRate(rate);
        }

        public bool IsPlaying()
        {
            return MediaPlayer.Control.IsPlaying();
        }

        public bool IsPaused()
        {
            return MediaPlayer.Control.IsPaused();
        }

        public bool IsMuted()
        {
            return MediaPlayer.Control.IsMuted();
        }

        public bool IsLooping()
        {
            return MediaPlayer.Control.IsLooping();
        }

        public void MuteAudio(bool bMute)
        {
            MediaPlayer.Control.MuteAudio(bMute);
        }

        public void SetTextureSize(float height,float width)
        {
            UITexture.height = (int)height;
            UITexture.width = (int)width;
        }

        public float GetVideoTextureRatio()
        {
            if (0 != MediaPlayer.Info.GetVideoHeight())
            {
                return (float)MediaPlayer.Info.GetVideoWidth() /(float)MediaPlayer.Info.GetVideoHeight();
            }
            return  (float)(1920f/1080f);
        }

        private Texture mediaTexture
        {
            get
            {
                Texture result = null;
                if (null != MediaPlayer && null != MediaPlayer.TextureProducer)
                {
                    result = MediaPlayer.TextureProducer.GetTexture();
                }
                return result;
            }
        }

        private bool bVerticalFlip
        {
            get
            {
                if (null != MediaPlayer && null != MediaPlayer.TextureProducer)
                    return MediaPlayer.TextureProducer.RequiresVerticalFlip ();
                return false;
            }
        }

        void Start()
        {
            if(null!=MediaPlayer)
                MediaPlayer.Events.AddListener(OnVideoEvent);
        }

        private void OnVideoEvent(MediaPlayer mp, MediaPlayerEvent.EventType et, ErrorCode errorCode)
        {
            switch (et)
            {
                case MediaPlayerEvent.EventType.ReadyToPlay:
                    break;
                case MediaPlayerEvent.EventType.Started:
                    break;
                case MediaPlayerEvent.EventType.FirstFrameReady:
                    break;
                case MediaPlayerEvent.EventType.FinishedPlaying:
                    finishPlaying();
                    break;
            }
        }

        private void applyMapping (Texture texture, bool requiresYFlip)
        {
            Vector2 scale = Vector2.one;
            Vector2 offset = Vector2.zero;
            if (requiresYFlip) {
                scale = new Vector2 (1.0f, -1.0f);
            }
            setTexture (texture, scale);
        }

        private void setTexture(Texture tex, Vector2 vec)
        {

            UITexture.mainTexture = tex;
            UITexture.transform.localScale = new Vector3 (vec.x, vec.y, 0f); 
            UITexture.enabled = false;
            UITexture.enabled = true;
        }

        void OnDestroy()
        {
            UITexture.mainTexture = null;
            UITexture = null;
            finishPlaying = null;
        }

        void Update()
        {
            if (!bStart)
                return;

            if (null != mediaTexture) {
                applyMapping (mediaTexture, bVerticalFlip);
            }
        }

    }
}

