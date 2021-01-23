using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeadTacking2 : MonoBehaviour {

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    public Vector3 offset = Vector3.zero;

    public GameObject item;

    public Vector3 correction = Vector3.one;
    public Text textUI;

    void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }


    void Update()
    {

        uint playerID = KinectManager.Instance != null ? KinectManager.Instance.GetPlayer1ID() : 0;

        if (playerID <= 0)
        {
            if (transform.position != initialPosition)
                transform.position = initialPosition;

            if (transform.rotation != initialRotation)
                transform.rotation = initialRotation;

            return;
        }

        // set the position in space
        Vector3 posCamera = KinectManager.Instance.GetUserPosition(playerID);

        if (KinectManager.Instance.IsJointTracked(playerID, 3))
        {
            Vector3 posJoint = KinectManager.Instance.GetJointPosition(playerID, 3);
            Quaternion rotJoint = KinectManager.Instance.GetJointOrientation(playerID, 3, false);

            posJoint -= posCamera;
            posJoint.z = -posJoint.z;

            //mirror
            //posJoint.x = -posJoint.x;
            //posJoint.z = -posJoint.z;
            //rotJoint.x = -rotJoint.x;
            //rotJoint.z = -rotJoint.z;
            
            transform.localPosition = new Vector3(-posJoint.x*2, -0.5f, 1f);




            //transform.localRotation = Quaternion.Euler(0, 180 + -posJoint.x * 75f,0);
  //          transform.localRotation = transform.LookAt(item.transform);


            //transform.localEulerAngles = new Vector3(0,180+posJoint.x*300,0);
            //transform.RotateAround(item.transform.position, item.transform.forward, posJoint.x*10f);



            /*
            transform.localPosition = posJoint;// * 10 + offset;
            transform.localPosition = new Vector3(transform.localPosition.x * correction.x,
                transform.localPosition.y * correction.y,
                transform.localPosition.z * correction.z);
            //transform.localRotation = rotJoint;
            */
            Debug.Log("HEAD TRACKING" + posJoint + " " + rotJoint+" PosCamera :"+posCamera);
            Debug.Log(transform.localPosition);

            textUI.text = posJoint + " " + rotJoint;
        }
        else
        {
            textUI.text = "NULL";

        }
    }
}
