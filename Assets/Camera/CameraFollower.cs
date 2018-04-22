using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour {
    // TODO: get better camera follwer
    public GameObject player;
    public Vector3 offset;

    //Default zoom level of the camera
    public float DefaultCameraZoom = 10;
    private float ZoomChangePerUpdate = 0.5f, OffsetChangePerUpdate = 0.5f;
    private Camera textCamera, backgroundCamera, levelCamera;

    public LayerMask cameraZoomMask;

    void Start() {
        CameraManager cameraManager = GetComponent<CameraManager>();
        textCamera = GameObject.Find("TextCamera").GetComponent<Camera>();
        backgroundCamera = cameraManager.backgroundCamera;
        levelCamera = cameraManager.levelCamera;
    }

    void Update() {
        ZoomAndMove();
        transform.position = player.transform.position + offset;
    }

    //update camera zoom depending on the desired zoom (if in a 'zone').
    private void ZoomAndMove() {
        Collider2D zoomCollider = Physics2D.OverlapPoint(player.transform.position, cameraZoomMask);

        float desiredZoom = DefaultCameraZoom;
        if (zoomCollider == null) {
            desiredZoom = DefaultCameraZoom;
            offset.Set(0, 0, 0);
        } else {
            CameraZoom cameraZoom = zoomCollider.gameObject.GetComponent<CameraZoom>();
            //Ignore zoom value from 'zone' when not a positive number.
            if (cameraZoom.ZoomValue > 0) {
                desiredZoom = cameraZoom.ZoomValue;
            }
            offset = cameraZoom.Offset;
        }

        float nextZoom = 0;
        if (desiredZoom != backgroundCamera.orthographicSize) {
            //Avoid stutter:
            if (Mathf.Abs(desiredZoom - backgroundCamera.orthographicSize) < ZoomChangePerUpdate) {
                nextZoom = desiredZoom;
                //zoom in:
            } else if (desiredZoom < backgroundCamera.orthographicSize) {
                nextZoom = backgroundCamera.orthographicSize - ZoomChangePerUpdate;
            }  //zoom out: 
            else {
                nextZoom = backgroundCamera.orthographicSize + ZoomChangePerUpdate;
            }

            backgroundCamera.orthographicSize = nextZoom;
            levelCamera.orthographicSize = nextZoom;
            textCamera.orthographicSize = nextZoom;
        }
    }
}
