using UnityEngine;
using UnityEditor;

public class FXCommonShaderGUI : ShaderGUI
{
    private MaterialEditor materialEditor;
    private MaterialProperty[] properties;
    private Material material;

    // Properties
    private MaterialProperty mainTex;
    private MaterialProperty tintColor;
    private MaterialProperty mainTexST;
    private MaterialProperty mainSpeed;
    private MaterialProperty noiseTex;
    private MaterialProperty noiseTexST;
    private MaterialProperty noiseSpeed;
    private MaterialProperty noiseStrength;
    private MaterialProperty disTex;
    private MaterialProperty disTexST;
    private MaterialProperty disCol;
    private MaterialProperty disColB;
    private MaterialProperty dissolveAmount;
    private MaterialProperty edgeWidth;
    private MaterialProperty maskTex;
    private MaterialProperty maskTexST;
    private MaterialProperty maskSpeed;
    private MaterialProperty maskStrength;
    private MaterialProperty colorBoost;
    private MaterialProperty emissionGain;
    private MaterialProperty emissionColor;
    private MaterialProperty saturation;
    private MaterialProperty srcBlend;
    private MaterialProperty dstBlend;
    private MaterialProperty zWrite;
    private MaterialProperty cull;
    private MaterialProperty zTest;
    private MaterialProperty intensity;
    private MaterialProperty alphaPow;

    public override void OnGUI(MaterialEditor editor, MaterialProperty[] props)
    {
        materialEditor = editor;
        properties = props;
        material = editor.target as Material;

        FindProperties();
        DrawGUI();
    }

    private void FindProperties()
    {
        mainTex = FindProperty("_MainTex", properties);
        tintColor = FindProperty("_TintColor", properties);
        mainTexST = FindProperty("_MainTexST", properties);
        mainSpeed = FindProperty("_MainSpeed", properties);
        noiseTex = FindProperty("_NoiseTex", properties);
        noiseTexST = FindProperty("_NoiseTexST", properties);
        noiseSpeed = FindProperty("_NoiseSpeed", properties);
        noiseStrength = FindProperty("_NoiseStrength", properties);
        disTex = FindProperty("_DisTex", properties);
        disTexST = FindProperty("_DisTexST", properties);
        disCol = FindProperty("_DisCol", properties);
        disColB = FindProperty("_DisColB", properties);
        dissolveAmount = FindProperty("_DissolveAmount", properties);
        edgeWidth = FindProperty("_EdgeWidth", properties);
        maskTex = FindProperty("_MaskTex", properties);
        maskTexST = FindProperty("_MaskTexST", properties);
        maskSpeed = FindProperty("_MaskSpeed", properties);
        maskStrength = FindProperty("_MaskStrength", properties);
        colorBoost = FindProperty("_ColorBoost", properties);
        emissionGain = FindProperty("_EmissionGain", properties);
        emissionColor = FindProperty("_EmissionColor", properties);
        saturation = FindProperty("_Saturation", properties);
        srcBlend = FindProperty("_SrcBlend", properties);
        dstBlend = FindProperty("_DstBlend", properties);
        zWrite = FindProperty("_ZWrite", properties);
        cull = FindProperty("_Cull", properties);
        zTest = FindProperty("_ZTest", properties);
        intensity = FindProperty("_Intensity", properties);
        alphaPow = FindProperty("_AlphaPow", properties);
    }

    private void DrawGUI()
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);

        // Main Texture Settings
        EditorGUILayout.LabelField("Main Texture Settings", EditorStyles.boldLabel);
        materialEditor.TexturePropertySingleLine(new GUIContent("Main Texture"), mainTex, tintColor);
        materialEditor.VectorProperty(mainTexST, "Tiling & Offset");
        materialEditor.VectorProperty(mainSpeed, "UV Speed");

        EditorGUILayout.Space();

        // Noise Effect
        EditorGUILayout.LabelField("Noise Effect", EditorStyles.boldLabel);
        materialEditor.TexturePropertySingleLine(new GUIContent("Noise Texture"), noiseTex);
        materialEditor.VectorProperty(noiseTexST, "Noise Tiling & Offset");
        materialEditor.VectorProperty(noiseSpeed, "Noise Speed");
        materialEditor.ShaderProperty(noiseStrength, "Noise Strength");

        EditorGUILayout.Space();

        // Dissolve Effect
        EditorGUILayout.LabelField("Dissolve Effect", EditorStyles.boldLabel);
        materialEditor.TexturePropertySingleLine(new GUIContent("Dissolve Texture"), disTex);
        materialEditor.VectorProperty(disTexST, "Dissolve Tiling & Offset");
        materialEditor.ShaderProperty(dissolveAmount, "Dissolve Amount");
        materialEditor.ShaderProperty(edgeWidth, "Edge Width");
        materialEditor.ColorProperty(disCol, "Edge Color");
        materialEditor.ColorProperty(disColB, "Edge Color B");

        EditorGUILayout.Space();

        // Mask Settings
        EditorGUILayout.LabelField("Mask Settings", EditorStyles.boldLabel);
        materialEditor.TexturePropertySingleLine(new GUIContent("Mask Texture"), maskTex);
        materialEditor.VectorProperty(maskTexST, "Mask Tiling & Offset");
        materialEditor.VectorProperty(maskSpeed, "Mask Speed");
        materialEditor.ShaderProperty(maskStrength, "Mask Strength");

        EditorGUILayout.Space();

        // Color Enhancement
        EditorGUILayout.LabelField("Color Enhancement", EditorStyles.boldLabel);
        materialEditor.ShaderProperty(colorBoost, "Color Boost");
        materialEditor.ShaderProperty(emissionGain, "Emission Gain");
        materialEditor.ShaderProperty(emissionColor, "Emission Color");
        materialEditor.ShaderProperty(saturation, "Saturation");

        EditorGUILayout.Space();

        // Render Settings
        EditorGUILayout.LabelField("Render Settings", EditorStyles.boldLabel);
        materialEditor.ShaderProperty(srcBlend, "Source Blend");
        materialEditor.ShaderProperty(dstBlend, "Destination Blend");
        materialEditor.ShaderProperty(zWrite, "Z Write");
        materialEditor.ShaderProperty(cull, "Culling");
        materialEditor.ShaderProperty(zTest, "Z Test");

        EditorGUILayout.Space();

        // Effect Settings
        EditorGUILayout.LabelField("Effect Settings", EditorStyles.boldLabel);
        materialEditor.ShaderProperty(intensity, "Effect Intensity");
        materialEditor.ShaderProperty(alphaPow, "Alpha Power");

        EditorGUILayout.EndVertical();
    }
}