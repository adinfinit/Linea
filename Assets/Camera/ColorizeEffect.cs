using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ColorizeEffect : MonoBehaviour
{
    public Color line = new Color(0f, 0f, 0f);
	public Color background = new Color(1f, 1f, 1f);
	public Texture paperTexture;
	public float parallaxSpeed;
	public float fuzzyness = 0.1f;
	public bool isBackgroundStatic;

	public RenderTexture backgroundTexture, levelTexture, textTexture;
	public Material blendMaterial;

	private Shader shader;
	private Material material;

	// Creates a private material used to the effect
	void Awake ()
	{
		shader = Shader.Find ("Hidden/ColorizeEffect");
		material = new Material (shader);
	}

	// Postprocess the image
	void OnRenderImage (RenderTexture source, RenderTexture destination)
	{
		if (material == null) {
			material = new Material (shader);
		}

		material.SetColor ("_LineColor", line);
		material.SetColor ("_BackgroundColor", background);
		
		material.SetTexture("_BackgroundTex", paperTexture);

		material.SetFloat("_ParallaxSpeed", parallaxSpeed);
		material.SetFloat("_DisplacementMultiplier", fuzzyness);

		material.SetInt("_IsTexStatic", isBackgroundStatic ? 1 : 0);

		material.SetVector("_ScreenSize", new Vector4(
			2 * Camera.main.orthographicSize * Camera.main.aspect, 
			2 * Camera.main.orthographicSize));

		//Graphics.Blit (source, levelTexture);
		Graphics.Blit (backgroundTexture, source);
		Graphics.Blit (levelTexture, source, blendMaterial);
		Graphics.Blit (source, destination, material);
		Graphics.Blit (textTexture, destination, blendMaterial);
		//Graphics.Blit (source, destination, material);
	}
}
