using UnityEditor;
using UnityEngine;

namespace EditorTool
{
    class AnimationClipOptimizer
    {
        static string _PrecisionSpecifierFormat = "F{0}";

        public static void Optimal(AnimationClip clip, int precision, bool bCutScale = true)
        {
            if (bCutScale)
                _CutScaleCurve(clip);
            _FixedPoint(clip, precision);
        }

        private static void _CutScaleCurve(AnimationClip clip)
        {
            if (clip != null)
            {
                //去除scale曲线
                foreach (EditorCurveBinding theCurveBinding in AnimationUtility.GetCurveBindings(clip))
                {
                    string name = theCurveBinding.propertyName.ToLower();
                    if (name.Contains("scale"))
                    {
                        AnimationUtility.SetEditorCurve(clip, theCurveBinding, null);
                        //Debug.LogFormat("关闭{0}的scale curve", clip.name);
                    }
                }
            }
        }

        private static void _FixedPoint(AnimationClip clip, int precision)
        {
            if (clip != null && precision > 0)
            {
                AnimationClipCurveData[] curves = null;
                curves = AnimationUtility.GetAllCurves(clip);
                Keyframe key;
                Keyframe[] keyFrames;
                string floatFormat = string.Format(_PrecisionSpecifierFormat, precision);
                if (curves != null && curves.Length > 0)
                {
                    for (int ii = 0; ii < curves.Length; ++ii)
                    {
                        AnimationClipCurveData curveDate = curves[ii];
                        if (curveDate.curve == null || curveDate.curve.keys == null)
                        {
                            //Debug.LogWarning(string.Format("AnimationClipCurveData {0} don't have curve; Animation name {1} ", curveDate, animationPath));
                            continue;
                        }
                        keyFrames = curveDate.curve.keys;
                        for (int i = 0; i < keyFrames.Length; i++)
                        {
                            key = keyFrames[i];
                            key.value = float.Parse(key.value.ToString(floatFormat));
                            key.inTangent = float.Parse(key.inTangent.ToString(floatFormat));
                            key.outTangent = float.Parse(key.outTangent.ToString(floatFormat));
                            keyFrames[i] = key;
                        }
                        curveDate.curve.keys = keyFrames;
                        clip.SetCurve(curveDate.path, curveDate.type, curveDate.propertyName, curveDate.curve);
                    }
                }
            }
            else
            {
                Debug.LogErrorFormat("目前不支持{0}位浮点", precision);
            }
        }
    }
}
