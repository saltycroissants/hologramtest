using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class grid2D_mesh : MonoBehaviour {

	public int xSize, ySize;
    private Vector3[] vertices;
    private Mesh mesh;

    private void Awake(){
        StartCoroutine(Generate());
    }

    private IEnumerator Generate() {
        WaitForSeconds wait = new WaitForSeconds(0.05f);
        
        /*assign mesh to mesh filter, reference 'procedural grid' in component */
        GetComponent<MeshFilter>().mesh =mesh = new Mesh();
        mesh.name = "Procedural Grid";

        /*create grid of vertices*/
        vertices = new Vector3[(xSize + 1) * (ySize + 1)];
        for (int i = 0, y = 0; y <= ySize; y++) {
            for (int x = 0; x <= xSize; x++, i++){
                vertices[i] = new Vector3(x, y);
                yield return wait;
            }
        }
        mesh.vertices = vertices;

        /*give mesh to triangles*/
        
        /* 삼각형 처음 절반
        int [] triangles = new int[6];
        //1st triangle
        triangles[0] = 0;
        triangles[1] = xSize +1; //1st vertex of next row
        triangles[2] = 1 
        //2nd triangle
        triangles[3] = 1 ;
        triangles[4] = xSize +1;
        triangles[5] = xSize +2;
        */

        int[] triangles = new int[xSize*ySize*6];
        for (int t =0, v=0, y=0; y<ySize; y++, v++){
            for(int x=0; x < xSize; x++, t+=6, v++){
                triangles[t] = v;
                triangles[t+1] = triangles[t+4] = v + xSize +1;
                triangles[t+2] = triangles[t+3] = v + 1;
                triangles[t+5] = v + xSize +2;
            //mesh.triangles = triangles;
            }
        }

        mesh.triangles = triangles;


    }

    private void OnDrawGizmos(){
        //check if unity is in edit mode
        if (vertices == null) {
            return;
        }

        Gizmos.color = Color.black;
        for (int i = 0; i < vertices.Length; i++) {
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }

}
