using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraManager : MonoBehaviour {

	public Camera backgroundCamera, levelCamera;
	public ColorizeEffect colorizer;
	
	private RenderTexture backgroundRT, levelRT;
	
	void Start(){
		backgroundRT = new RenderTexture(Screen.width, Screen.height, 0);
		//backgroundRT.format = RenderTextureFormat.RGB565;
		backgroundRT.Create();
		backgroundCamera.targetTexture = backgroundRT;
		//levelCamera.gameObject.GetComponent<ColorizeEffect>().backgroundTexture = backgroundRT;

		levelRT = new RenderTexture(Screen.width, Screen.height, 0);
		levelRT.Create();
		levelCamera.targetTexture = levelRT;

		colorizer.backgroundTexture = backgroundRT;
		colorizer.levelTexture = levelRT;
	}
}
