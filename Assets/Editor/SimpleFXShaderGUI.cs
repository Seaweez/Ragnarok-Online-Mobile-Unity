using UnityEngine;
using UnityEditor;

public class SimpleFXShaderGUI : ShaderGUI
{
    private bool showMainSettings = true;
    private bool showNoiseSettings = true;
    private bool showDissolveSettings = true;
    private bool showMaskSettings = true;
    private bool showFresnelSettings = true;
    private bool showRenderSettings = true;

    private MaterialProperty FindProp(string name, MaterialProperty[] props)
    {
        return FindProperty(name, props, false);
    }

    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        Material material = materialEditor.target as Material;

        // Main Settings
        showMainSettings = EditorGUILayout.Foldout(showMainSettings, "Main Settings", true);
        if (showMainSettings)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            {
                MaterialProperty mainTex = FindProp("_MainTex", properties);
                MaterialProperty tintColor = FindProp("_TintColor", properties);
                MaterialProperty colParam = FindProp("_ColParam", properties);
                MaterialProperty colParamB = FindProp("_ColParamB", properties);

                // Main Texture with UV Settings
                materialEditor.TexturePropertySingleLine(new GUIContent("Main Texture"), mainTex);
                if (mainTex.textureValue != null)
                {
                    EditorGUI.indentLevel++;
                    Vector4 uvSpeed = colParam.vectorValue;
                    Vector4 uvScale = colParamB.vectorValue;

                    EditorGUI.BeginChangeCheck();
                    Vector2 speed = EditorGUILayout.Vector2Field("UV Speed", new Vector2(uvSpeed.x, uvSpeed.y));
                    Vector2 scale = EditorGUILayout.Vector2Field("UV Scale", new Vector2(uvScale.x, uvScale.y));
                    if (EditorGUI.EndChangeCheck())
                    {
                        uvSpeed.x = speed.x;
                        uvSpeed.y = speed.y;
                        uvScale.x = scale.x;
                        uvScale.y = scale.y;
                        colParam.vectorValue = uvSpeed;
                        colParamB.vectorValue = uvScale;
                    }
                    EditorGUI.indentLevel--;
                }

                materialEditor.ShaderProperty(tintColor, "Tint Color");
            }
            EditorGUILayout.EndVertical();
        }

        // Noise Settings
        showNoiseSettings = EditorGUILayout.Foldout(showNoiseSettings, "Noise Settings", true);
        if (showNoiseSettings)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            {
                MaterialProperty noiseTex = FindProp("_NoiseTex", properties);
                MaterialProperty noiseParam = FindProp("_NoiseParam", properties);

                materialEditor.TexturePropertySingleLine(new GUIContent("Noise Texture"), noiseTex);
                if (noiseTex.textureValue != null)
                {
                    EditorGUI.indentLevel++;
                    Vector4 noiseValues = noiseParam.vectorValue;

                    EditorGUI.BeginChangeCheck();
                    Vector2 speed = EditorGUILayout.Vector2Field("Noise Speed", new Vector2(noiseValues.x, noiseValues.y));
                    float strength = EditorGUILayout.Slider("Noise Strength", noiseValues.z, 0f, 1f);
                    if (EditorGUI.EndChangeCheck())
                    {
                        noiseValues.x = speed.x;
                        noiseValues.y = speed.y;
                        noiseValues.z = strength;
                        noiseParam.vectorValue = noiseValues;
                    }
                    EditorGUI.indentLevel--;
                }
            }
            EditorGUILayout.EndVertical();
        }

        // Dissolve Settings
        showDissolveSettings = EditorGUILayout.Foldout(showDissolveSettings, "Dissolve Settings", true);
        if (showDissolveSettings)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            {
                MaterialProperty disTex = FindProp("_DisTex", properties);
                MaterialProperty disCol = FindProp("_DisCol", properties);
                MaterialProperty disColB = FindProp("_DisColB", properties);
                MaterialProperty disParam = FindProp("_DisParam", properties);
                MaterialProperty disParamB = FindProp("_DisParamB", properties);

                materialEditor.TexturePropertySingleLine(new GUIContent("Dissolve Texture"), disTex);
                if (disTex.textureValue != null)
                {
                    EditorGUI.indentLevel++;
                    materialEditor.ShaderProperty(disCol, "Edge Color A");
                    materialEditor.ShaderProperty(disColB, "Edge Color B");

                    Vector4 dissolveValues = disParam.vectorValue;
                    EditorGUI.BeginChangeCheck();
                    Vector2 speed = EditorGUILayout.Vector2Field("Dissolve Speed", new Vector2(dissolveValues.x, dissolveValues.y));
                    float amount = EditorGUILayout.Slider("Dissolve Amount", dissolveValues.z, 0f, 1f);
                    if (EditorGUI.EndChangeCheck())
                    {
                        dissolveValues.x = speed.x;
                        dissolveValues.y = speed.y;
                        dissolveValues.z = amount;
                        disParam.vectorValue = dissolveValues;
                    }
                    EditorGUI.indentLevel--;
                }
            }
            EditorGUILayout.EndVertical();
        }

        // Mask Settings
        showMaskSettings = EditorGUILayout.Foldout(showMaskSettings, "Mask Settings", true);
        if (showMaskSettings)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            {
                MaterialProperty maskTex = FindProp("_MaskTex", properties);
                MaterialProperty maskParam = FindProp("_MaskParam", properties);
                MaterialProperty maskParamB = FindProp("_MaskParamB", properties);

                materialEditor.TexturePropertySingleLine(new GUIContent("Mask Texture"), maskTex);
                if (maskTex.textureValue != null)
                {
                    EditorGUI.indentLevel++;
                    Vector4 maskValues = maskParam.vectorValue;
                    EditorGUI.BeginChangeCheck();
                    Vector2 speed = EditorGUILayout.Vector2Field("Mask Speed", new Vector2(maskValues.x, maskValues.y));
                    float power = EditorGUILayout.Slider("Mask Power", maskValues.z, 0f, 1f);
                    if (EditorGUI.EndChangeCheck())
                    {
                        maskValues.x = speed.x;
                        maskValues.y = speed.y;
                        maskValues.z = power;
                        maskParam.vectorValue = maskValues;
                    }
                    EditorGUI.indentLevel--;
                }
            }
            EditorGUILayout.EndVertical();
        }

        // Render Settings
        showRenderSettings = EditorGUILayout.Foldout(showRenderSettings, "Render Settings", true);
        if (showRenderSettings)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            {
                int queue = EditorGUILayout.IntField("Render Queue", material.renderQueue);
                if (queue != material.renderQueue)
                {
                    material.renderQueue = queue;
                }
                MaterialProperty srcBlend = FindProp("_SrcBlend", properties);
                MaterialProperty dstBlend = FindProp("_DstBlend", properties);
                MaterialProperty zWrite = FindProp("_ZWrite", properties);
                MaterialProperty cull = FindProp("_Cull", properties);
                MaterialProperty zTest = FindProp("_ZTest", properties);
                MaterialProperty fogOn = FindProp("_FogOn", properties);

                materialEditor.ShaderProperty(srcBlend, "Source Blend");
                materialEditor.ShaderProperty(dstBlend, "Destination Blend");
                materialEditor.ShaderProperty(zWrite, "Z Write");
                materialEditor.ShaderProperty(cull, "Cull Mode");
                materialEditor.ShaderProperty(zTest, "Z Test");
                materialEditor.ShaderProperty(fogOn, "Fog Enable");

                EditorGUILayout.Space();

                if (GUILayout.Button("Set to Additive"))
                {
                    material.SetFloat("_SrcBlend", (float)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    material.SetFloat("_DstBlend", (float)UnityEngine.Rendering.BlendMode.One);
                    material.SetFloat("_ZWrite", 0f);
                    material.renderQueue = 3023; // Transparent+23
                }

                if (GUILayout.Button("Set to Alpha Blend"))
                {
                    material.SetFloat("_SrcBlend", (float)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    material.SetFloat("_DstBlend", (float)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    material.SetFloat("_ZWrite", 0f);
                    material.renderQueue = 3023; // Transparent+23
                }
            }
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.Space();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(material);
        }
    }
}