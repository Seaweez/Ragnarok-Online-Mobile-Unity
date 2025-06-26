using UnityEngine;
using RO.Test;
namespace RO
{

    [SLua.CustomLuaClassAttribute]
    public class PbManager 
    {
        static public ByteArray LoadPbFile(string Path)
        {
            TextAsset asset = null;
#if RESOURCE_LOAD
            if (null != ResourceManager.Me)
                asset = (TextAsset)ResourceManager.Me.SLoad(Path);
            else
                asset = (TextAsset)Resources.Load(Path);
#else
            string bundleFile = "assets/resources/" + Path + ".bytes";
            asset = ResourceManager.Me.SLoad<TextAsset> (LuaLoadOverrider.Me.GetCacheID (Path), bundleFile);
#endif
            if (asset != null)
            {
                ByteArray retVal = new ByteArray(asset.bytes, asset.bytes.Length);
                return retVal;
            }
            else
                return null;

            
        }
    }
}
