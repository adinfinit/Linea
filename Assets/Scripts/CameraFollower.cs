using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour {
    // TODO: get better camera follwer
    public GameObject player;
    public Vector3 offset;

    //Default zoom level of the camera
    public float DefaultCameraZoom = 10;
    private float ZoomChangePerUpdate = 0.5f;
    private Camera textCamera, backgroundCamera, levelCamera;

    public LayerMask cameraZoomMask;

    void Start() {
        offset = transform.position - player.transform.position;
        CameraManager cameraManager = GetComponent<CameraManager>();
        textCamera = GameObject.Find("TextCamera").GetComponent<Camera>();
        backgroundCamera = cameraManager.backgroundCamera;
        levelCamera = cameraManager.levelCamera;
    }

    void Update() {
        transform.position = player.transform.position + offset;

        //update camera zoom depending on the desired zoom (if in a 'zone').
        Collider2D zoomCollider = Physics2D.OverlapPoint(player.transform.position, cameraZoomMask);

        float desiredValue;
        if (zoomCollider == null) {
            desiredValue = DefaultCameraZoom;
        } else {
            CameraZoom cameraZoom = zoomCollider.gameObject.GetComponent<CameraZoom>();
            desiredValue = cameraZoom.ZoomValue;
        }
        float nextSize = 0;
        if (desiredValue != backgroundCamera.orthographicSize) {
            //Avoid stutter:
            if (Mathf.Abs(desiredValue - backgroundCamera.orthographicSize) < ZoomChangePerUpdate) {
                nextSize = desiredValue;
                //zoom in:
            } else if (desiredValue < backgroundCamera.orthographicSize) {
                nextSize = backgroundCamera.orthographicSize - ZoomChangePerUpdate;
            }  //zoom out: 
            else {
                nextSize = backgroundCamera.orthographicSize + ZoomChangePerUpdate;
            }

            backgroundCamera.orthographicSize = nextSize;
            levelCamera.orthographicSize = nextSize;
            textCamera.orthographicSize = nextSize;
        }
    }
}
