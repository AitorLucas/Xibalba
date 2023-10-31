using UnityEngine;

public class BlurManager : MonoBehaviour {

    [SerializeField] private Camera blurCamera;
    [SerializeField] private Material blurMaterial;

    private void Start() {
         if (blurCamera.targetTexture != null) {
            Debug.Log("THIS RELEASE");
            blurCamera.targetTexture.Release();
        }
        blurCamera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32, 1);
        blurMaterial.SetTexture("_RenTex", blurCamera.targetTexture);
    }
}