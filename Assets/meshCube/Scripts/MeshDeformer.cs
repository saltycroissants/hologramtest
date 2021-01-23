using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshDeformer : MonoBehaviour
{
    Mesh deformingMesh;
    Vector3[] originalVertices, deformedVertices; 
    Vector3[] vertexVelocity; //vertex의 속도  

    // Start is called before the first frame update
    void Start(){
        deformingMesh = GetComponent<MeshFilter>().mesh;
        originalVertices = deformingMesh.vertices;
        deformedVertices = new Vector3[originalVertices.Length];
        for(int i=0; i<originalVertices.Length; i++){
            deformedVertices[i] = originalVertices[i];
        }
        vertexVelocity = new Vector3[originalVertices.Length];
    }

    // Update is called once per frame
    private float uniformScale =1f;
    void Update()
    {
        uniformScale = transform.localScale.x;
        for(int i=0; i<deformedVertices.Length; i++){
            UpdateVertex(i);
        }
        deformingMesh.vertices = deformedVertices;
        deformingMesh.RecalculateNormals();
    }

    public float springForce = 10f; //탄성정도 설정
    public float damping = 1f; //숫자가 클수록 덜 잘 변함

    void UpdateVertex (int index){
        Vector3 velocity = vertexVelocity[index];
        Vector3 displacement = deformedVertices[index] - originalVertices[index];
        displacement *= uniformScale;
        velocity -= displacement * springForce * Time.deltaTime;
        velocity *= 1f - (damping/10f) * Time.deltaTime;
        vertexVelocity[index] = velocity;
        deformedVertices[index] += velocity *(Time.deltaTime / uniformScale);

    }

    public void AddDeformingForce(Vector3 point, float force){
        //Debug.DrawLine(Camera.main.transform.position , point);
        point = transform.InverseTransformPoint(point);
        for(int i=0; i< deformedVertices.Length; i++){
            AddForceToVertex(i, point, force);
        }
    }

    
    void AddForceToVertex(int i, Vector3 point,float force){
        Vector3 pointToVertex = deformedVertices[i] - point;
        pointToVertex *= uniformScale;
        float attenuatedForce = force / (1f + pointToVertex.sqrMagnitude);
        float velocity = attenuatedForce * Time.deltaTime;
        vertexVelocity[i] += pointToVertex.normalized * velocity;
    }
    
}
