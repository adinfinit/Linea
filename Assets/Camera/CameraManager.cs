using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraManager : MonoBehaviour
{

	public Camera backgroundCamera, levelCamera, textCamera;
	public ColorizeEffect colorizer;

	void Start ()
	{
		
	}

	void OnPreRender ()
	{
		if (colorizer.backgroundCamera == null)
			colorizer.backgroundCamera = backgroundCamera;
		if (colorizer.levelCamera == null)
			colorizer.levelCamera = levelCamera;
		if (colorizer.textCamera == null)
			colorizer.textCamera = textCamera;
	}
}
