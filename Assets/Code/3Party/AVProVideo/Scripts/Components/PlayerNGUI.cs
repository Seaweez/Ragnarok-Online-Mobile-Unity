using System;
using UnityEngine;
using UnityEngine.Video; // Unity VideoPlayer namespace

namespace VideoNguis
{
    [SLua.CustomLuaClassAttribute]
    public class PlayerNGUI : MonoBehaviour
    {
        public VideoPlayer videoPlayer; // Unity's VideoPlayer component
        public UITexture uiTexture; // NGUI UITexture for rendering the video
        public string videoFileName; // The name of the video file to play
        public bool fitToScreen = false; // Flag to fit video to screen size
        public Action onFinishPlaying; // Delegate to call when video finishes

        private string lastLoadedVideo = ""; // To keep track of the last loaded video

        private void Start()
        {
            if (videoPlayer == null)
            {
                Debug.LogError("PlayerNGUI: VideoPlayer component is not assigned.");
                return;
            }

            videoPlayer.loopPointReached += OnVideoFinished;
            videoPlayer.prepareCompleted += OnVideoPrepared; // Added event handler for when the video is prepared

            if (!string.IsNullOrEmpty(videoFileName))
            {
                OpenVideo(videoFileName);
            }
        }

        public bool OpenVideo(string fileNameWithoutExtension)
        {
            if (string.IsNullOrEmpty(fileNameWithoutExtension))
            {
                Debug.LogError("PlayerNGUI: OpenVideo -- File name is empty.");
                return false;
            }

            // Check if the requested video is already loaded
            if (lastLoadedVideo == fileNameWithoutExtension)
            {
                Debug.Log("PlayerNGUI: OpenVideo -- Video is already loaded.");
                return true; // Early exit
            }

            string[] possibleExtensions = { ".mp4", ".avi", ".mov", ".mkv" }; // Add more extensions as needed
            foreach (var extension in possibleExtensions)
            {
                string fullPath = System.IO.Path.Combine(Application.streamingAssetsPath, "Videos", fileNameWithoutExtension + extension);
                if (System.IO.File.Exists(fullPath))
                {
                    videoPlayer.url = fullPath;
                    videoPlayer.Prepare(); // Asynchronously prepares the video
                    Debug.Log($"PlayerNGUI: OpenVideo -- Successfully found and preparing video: {fullPath}");
                    lastLoadedVideo = fileNameWithoutExtension; // Update the last loaded video
                    return true;
                }
            }

            Debug.LogError($"PlayerNGUI: OpenVideo -- File not found for any known extensions at path: {Application.streamingAssetsPath}/Videos/{fileNameWithoutExtension}");
            return false;
        }

        private void OnVideoPrepared(VideoPlayer source)
        {
            Debug.Log("PlayerNGUI: Video is prepared and ready to play.");
            // Here you can hide the loading indicator if you have one
        }

        public void Play()
        {
            if (videoPlayer.isPrepared)
            {
                videoPlayer.Play();
            }
            else
            {
                Debug.LogWarning("PlayerNGUI: Play -- Video is not prepared. Call OpenVideo and wait for preparation to complete.");
            }
        }

        public void Pause()
        {
            if (videoPlayer.isPlaying)
            {
                videoPlayer.Pause();
            }
        }

        public void Close()
        {
            videoPlayer.Stop();
        }

        private void OnVideoFinished(VideoPlayer source)
        {
            onFinishPlaying?.Invoke();
        }

        private void Update()
        {
            if (videoPlayer.isPlaying && videoPlayer.texture != null)
            {
                uiTexture.mainTexture = videoPlayer.texture;
                if (fitToScreen)
                {
                    AdjustUITextureToFitScreen();
                }
            }
        }

        private void AdjustUITextureToFitScreen()
        {
            // Adjust UITexture size to match the current screen size
            uiTexture.width = Screen.width;
            uiTexture.height = Screen.height;
        }

        private void OnDestroy()
        {
            videoPlayer.loopPointReached -= OnVideoFinished;
            if (uiTexture != null)
            {
                uiTexture.mainTexture = null;
            }
        }
    }
}
