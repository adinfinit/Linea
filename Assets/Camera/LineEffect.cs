using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class LineEffect : MonoBehaviour
{
	[Range (0.5f, 1.0f)]
	public float lineWidth = 1f;
	[Range (0, 5)]
	public int iterations = 3;

	private Shader shader;
	private Material material;

	private Camera effectCamera;

	// Creates a private material used to the effect
	void Awake ()
	{
		shader = Shader.Find ("Hidden/LineEffect");
		material = new Material (shader);
		effectCamera = GetComponent<Camera> ();
	}

	public void OnPreRender ()
	{
		if (effectCamera == null) {
			effectCamera = GetComponent<Camera> ();
		}
		if (effectCamera.targetTexture == null ||
		    effectCamera.targetTexture.width != Screen.width ||
		    effectCamera.targetTexture.height != Screen.height) {

			if (effectCamera.targetTexture != null) {
				effectCamera.targetTexture.Release ();
				effectCamera.targetTexture = null;
			}

			effectCamera.targetTexture = new RenderTexture (Screen.width, Screen.height, 0);
		}
	}

	// Postprocess the image
	void OnRenderImage (RenderTexture source, RenderTexture destination)
	{
		if (material == null) {
			material = new Material (shader);
		}

		material.SetFloat ("_LineWidth", lineWidth);
		material.SetInt ("_Iterations", iterations);

		Graphics.Blit (source, destination, material);
	}
}