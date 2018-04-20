using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class LineEffect : MonoBehaviour
{
	[Range(0.5f, 1.0f)]
	public float lineWidth = 1f;
	[Range(0, 5)]
	public int iterations = 3;

	private Shader shader;
	private Material material;

	// Creates a private material used to the effect
	void Awake ()
	{
		shader = Shader.Find ("Hidden/LineEffect");
		material = new Material (shader);
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