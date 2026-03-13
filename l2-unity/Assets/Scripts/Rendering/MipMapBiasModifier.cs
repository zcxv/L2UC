using System.Collections.Generic;
using UnityEngine;

[Tooltip("Adjusts the mipmap bias for textures on this GameObject and all its children. " +
         "Warning: This modifies the shared texture assets, so any other objects using " +
         "the same textures in the current scene will also be affected!")]
public class MipMapBiasModifier : MonoBehaviour {
    
    // default unity PBR shader properties
    private static readonly string[] TEX_PROPERTIES = {
        "_MainTex", "_BaseMap", "_BumpMap", "_NormalMap", 
        "_MetallicGlossMap", "_OcclusionMap", "_EmissionMap"
    };
    
    [Tooltip("Negative values increase sharpness by biasing towards higher resolution mip levels. Positive values increase blur.")] 
    [Range(-2f, 2f)]
    public float Bias;

    private readonly HashSet<Texture> textures = new ();
    
    private void ApplyBias() {
        foreach (var r in GetComponentsInChildren<Renderer>(true)) {
            foreach (var material in r.sharedMaterials) {
                if (material == null) {
                    continue;
                }

                Shader shader = material.shader;
                foreach (var property in TEX_PROPERTIES) {
                    if (!material.HasProperty(property)) {
                        continue;
                    }
                    
                    Texture tex = material.GetTexture(property);
                    if (tex != null && textures.Add(tex)) {
                        tex.mipMapBias = Bias;
                    }
                }
            }
        }
    }

    private void ResetBias() {
        foreach (var tex in textures) {
            tex.mipMapBias = 0f;
        }
        textures.Clear();
    }
    
    private void OnEnable() => ApplyBias();

    private void OnDisable() => ResetBias();
    private void OnDestroy() => ResetBias();
    
    // Unity Editor Hack: preventing mipmap bias stickiness
    private void OnApplicationQuit() => ResetBias();
    
#if UNITY_EDITOR
    private void OnValidate() {
        if (Application.isPlaying) {
            ResetBias();
            ApplyBias();
        }
    }
#endif
    
}