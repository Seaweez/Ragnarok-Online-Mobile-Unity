using System.Collections.Generic;

namespace EditorTool
{
    public class AudioTable : TableBase<AudioCfgCell, AudioCfg>
    {
        static List<string> _AudioExts = new List<string>(){
             ".wav",
             ".mp3"
        };
        static List<string> _AudioFolder = new List<string>
        {
            "Assets/Resources/Public/Audio"
        };
        static List<string> _FileList = new List<string>();
        public AudioTable(string name) : base(name) { }

        public override void InitData()
        {
            _FileList.Clear();
            _FileList = CollectAudioFile();
            InitCfgList();
        }

        List<string> CollectAudioFile()
        {
            return ConfigEditorUtil.CollectAllFolders(_AudioFolder, _AudioExts);
        }

        public override void InitCfgList()
        {
            cfgList.Clear();
            for(int i=0; i < _FileList.Count; i++)
            {
                AudioCfg cfg = new AudioCfg();
                cfg.SpecialInit(i, _FileList[i]);
                cfgList.Add(cfg);
            }
        }
    }
}
