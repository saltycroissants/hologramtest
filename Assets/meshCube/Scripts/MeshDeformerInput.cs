using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
>>>>>>READ ME<<<<<<<
MeshDeformerInput Script는 CAMERA에 연결해야함!! 
 */

public class MeshDeformerInput : MonoBehaviour
{
    public float force = 10f;
    public float forceOffset =0.1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0)){
            HandleInput();
        }
    }

    //user가 가르키는 곳을 알기 위해
    void HandleInput(){
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(inputRay, out hit)){
            MeshDeformer deformer = hit.collider.GetComponent<MeshDeformer>();
            if(deformer){
                Vector3 point = hit.point;
                point += hit.normal * forceOffset;
                deformer.AddDeformingForce(point, force);
            }
        }
    }
}
