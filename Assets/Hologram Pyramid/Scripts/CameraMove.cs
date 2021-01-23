using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {

	
    public void cameraUP() {
        Debug.Log("CAM UP");
        transform.position += new Vector3(0, 0.1f, 0);
    }
    public void cameraDown()
    {
        Debug.Log("CAM Down");
        transform.position += new Vector3(0, -0.1f, 0);
    }
    public void cameraLeft()
    {
        Debug.Log("CAM Left");
        transform.position += new Vector3(-0.1f, 0, 0);
    }
    public void cameraRight()
    {
        Debug.Log("CAM Right");
        transform.position += new Vector3(0.1f, 0, 0);
    }
}
