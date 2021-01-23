using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class RoundedCube : MonoBehaviour {

	public int xSize, ySize,zSize;
	public int roundness;
    private Vector3[] vertices; //vertice 꼭짓점,정점
    private Mesh mesh;

    private void Awake(){
        Generate();
    }

    private void Generate() {
        //WaitForSeconds wait = new WaitForSeconds(0.05f);
        
        /*assign mesh to mesh filter, reference 'procedural grid' in component */
        GetComponent<MeshFilter>().mesh =mesh = new Mesh();
        mesh.name = "Procedural Cube";
        CreateVertices();
        CreateTriangles();
		CreateColliders();

    }

	private void CreateColliders(){
		AddBoxCollider(xSize, ySize - roundness * 2, zSize - roundness * 2);
		AddBoxCollider(xSize - roundness * 2, ySize, zSize - roundness * 2);
		AddBoxCollider(xSize - roundness * 2, ySize - roundness * 2, zSize);
	
		Vector3 min = Vector3.one * roundness;
		Vector3 half = new Vector3(xSize, ySize, zSize) * 0.5f; 
		Vector3 max = new Vector3(xSize, ySize, zSize) - min;

		AddCapsuleCollider(0, half.x, min.y, min.z);
		AddCapsuleCollider(0, half.x, min.y, max.z);
		AddCapsuleCollider(0, half.x, max.y, min.z);
		AddCapsuleCollider(0, half.x, max.y, max.z);
		
		AddCapsuleCollider(1, min.x, half.y, min.z);
		AddCapsuleCollider(1, min.x, half.y, max.z);
		AddCapsuleCollider(1, max.x, half.y, min.z);
		AddCapsuleCollider(1, max.x, half.y, max.z);
		
		AddCapsuleCollider(2, min.x, min.y, half.z);
		AddCapsuleCollider(2, min.x, max.y, half.z);
		AddCapsuleCollider(2, max.x, min.y, half.z);
		AddCapsuleCollider(2, max.x, max.y, half.z);
	}

	private void AddBoxCollider(float x, float y, float z){
		BoxCollider box = gameObject.AddComponent<BoxCollider>();
		box.size = new Vector3(x,y,z);
	}
	//----->>>>>>여기부터 다시 
	private void AddCapsuleCollider(int direction, float x, float y, float z){
		CapsuleCollider c = gameObject.AddComponent<CapsuleCollider>();
		c.center = new Vector3(x, y, z);
		c.direction = direction;
		c.radius = roundness;
		c.height = c.center[direction] * 2f;
	}

	private Vector3[] normals;

    private void CreateVertices(){
		//WaitForSeconds wait = new WaitForSeconds(0.05f);
        /*VERTICE 갯수*/		
		int cornerVertice = 8; //꼭짓점
		int edgeVertice = (xSize + ySize +zSize -3) * 4; 
		int faceVertice = ( (xSize-1)*(ySize-1) + (ySize-1)*(zSize-1) +(zSize-1)*(xSize-1) ) * 2;
		int numOfVertices = cornerVertice + edgeVertice + faceVertice;

        /*create grid of vertices*/
        vertices = new Vector3[numOfVertices];
		normals = new Vector3[vertices.Length];   

		int var = 0;
		for(int y=0; y<=ySize; y++){
			for(int x=0; x <= xSize; x++){
				//vertices[var++] = new Vector3(x,y,0);
				SetVertex(var++ , x, y, 0);
			}
			for(int z=1; z<=zSize; z++){
				//vertices[var++] =new Vector3(xSize,y,z);
				SetVertex(var++ , xSize, y, z); 			
			}

			for(int x= xSize-1; x>=0; x--){
				//vertices[var++] = new Vector3(x,y,zSize);
				SetVertex(var++ , x, y, zSize);
			}

			for(int z = zSize-1; z>0; z--){
				//vertices[var++] = new Vector3(0,y,z);
				SetVertex(var++ , 0, y, z);
			}
		}

		//TOP FACE
		for(int z=1; z<zSize; z++){
			for(int x=1; x<xSize; x++){
				vertices[var++]= new Vector3(x, ySize, z);
			}
		}

		//BOTTOM FACE
		for(int z=1; z<zSize; z++){
			for(int x=1; x<xSize; x++){
				vertices[var++]= new Vector3(x, 0, z);
			}
		}

        mesh.vertices = vertices;   
		mesh.normals =normals;

        }

    //quad 초기화하는 함수
    private static int SetQuad (int[] triangles, int index_tri, int v00, int v10, int v01, int v11) {
		triangles[index_tri] = v00;
		triangles[index_tri + 1] = triangles[index_tri + 4] = v01;
		triangles[index_tri + 2] = triangles[index_tri + 3] = v10;
		triangles[index_tri + 5] = v11;
		return index_tri + 6;
    }

	private void SetVertex(int i, int x, int y, int z){
		Vector3 inside = vertices[i] = new Vector3(x, y, z);

		if (x < roundness){
			inside.x = roundness;
		}
		else if (x> xSize - roundness){
			inside.x = xSize - roundness;
		}

		if (y < roundness){
			inside.y = roundness;
		}
		else if(y > ySize - roundness){
			inside.y = ySize - roundness;
		}

		if (z < roundness){
			inside.z = roundness;
		}
		else if(z > zSize - roundness){
			inside.z = zSize - roundness;
		}

		normals[i] = (vertices[i] - inside).normalized;
		vertices[i] = inside + normals[i] * roundness;
	}


    private void CreateTriangles(){
        //int numOfQuads = (xSize * ySize + xSize * zSize + ySize * zSize) * 2;

        int ring = (xSize + zSize) * 2;
		int tx =0, ty=0, tz=0;
		//int[] tSide = new int[2];
		//tSide[0] = tx;
		//tSide[1] = tz;
		//int tri = 0;

		int[] triangles_Zside = new int [(xSize * ySize) * 12];
		int[] triangles_Xside = new int [(zSize * ySize) * 12]; 
		int[] triangles_Yside = new int [(xSize * zSize) * 12];
		
		//int[] trianglesList = new int[numOfQuads * 6];
	
		//CreateSide(triangles_Xside,triangles_Zside, tSide, ring);

		int v=0;
		for (int y =0; y<ySize; y++, v++){
			//마지막 quad 전까지 한바퀴 돌리기
			for (int q = 0; q < xSize; q++, v++) {
				tz = SetQuad(triangles_Zside, tz, v, v + 1, v + ring, v +ring +1);
			}
			for (int q = 0; q < zSize; q++, v++){
				tx = SetQuad(triangles_Xside, tx, v, v + 1, v + ring, v +ring +1);
			}
			for (int q = 0; q < xSize; q++, v++) {
				tz = SetQuad(triangles_Zside, tz, v, v + 1, v + ring, v + ring + 1);
			}
			for (int q = 0; q < zSize - 1; q++, v++) {
				tx = SetQuad(triangles_Xside, tx, v, v + 1, v + ring, v + ring + 1);
			}
			//last quad
			tx = SetQuad(triangles_Xside, tx, v, v-ring +1, v+ring, v+1);
		}

		ty = CreateTop(triangles_Yside, ty, ring); 
		ty = CreateBottom(triangles_Yside, ty, ring); 


		mesh.subMeshCount = 3;
		mesh.SetTriangles(triangles_Zside, 0);
		mesh.SetTriangles(triangles_Xside, 1);
		mesh.SetTriangles(triangles_Yside, 2);
    }

	/*측면 삼각형
	private void CreateSide(int[] triangles_Xside,int[] triangles_Zside, int[] tSide, int ring){
		int v=0;
		//int tx =tSide[0];
		int tz =tSide[1];

		for (int y =0; y<ySize; y++, v++){
			//마지막 quad 전까지 한바퀴 돌리기
			for (int q = 0; q < xSize; q++, v++) {
				tz = SetQuad(triangles_Zside, tz, v, v + 1, v + ring, v +ring +1);
			}
			for (int q = 0; q < zSize; q++, v++){
				tx = SetQuad(triangles_Xside, tx, v, v + 1, v + ring, v +ring +1);
			}
			for (int q = 0; q < zSize - 1; q++, v++) {
				tx = SetQuad(triangles_Xside, tx, v, v + 1, v + ring, v + ring + 1);
			}
			//last quad
			tx = SetQuad(triangles_Xside, tx, v, v-ring +1, v+ring, v+1);
		}

		tSide[0] = tx;
		tSide[1] = tz;

		//return tSide;	
	}*/


	/*위쪽면 삼각형*/
	private int CreateTop(int[] trianglesList, int tri, int ring ){
		
		//위쪽면의 첫 줄
		int v = ring * ySize;
		for (int x=0; x < xSize -1; x++, v++){
			tri = SetQuad(trianglesList, tri, v, v+1, v + ring-1, v+ring);	
		}
		tri = SetQuad(trianglesList, tri, v, v+1, v+ring-1, v+2);

		//위쪽면의 중간부분
		int vMin = ring * (ySize+1) -1; 
		int vMid = vMin +1;
		int vMax = v + 2 ;

		for (int z =1; z< zSize -1; z++, vMin--, vMid++, vMax++){
			tri = SetQuad(trianglesList, tri, vMin, vMid, vMin -1, vMid + xSize -1);

			for(int x=1; x< xSize-1; x++, vMid++){
				tri = SetQuad(trianglesList, tri, 
				vMid, vMid +1, vMid + xSize -1, vMid +xSize);
			} 
			tri = SetQuad(trianglesList, tri, vMid, vMax, vMid + xSize -1, vMax +1);
		}
		
		//위 쪽면의 마지막 줄
		int vTop = vMin -2;
		tri = SetQuad(trianglesList, tri, vMin, vMid, vTop+1, vTop);
		
		for(int x=1; x< xSize -1; x++, vTop--, vMid++){
			tri = SetQuad(trianglesList, tri, vMid, vMid +1, vTop, vTop-1);
		}
		tri = SetQuad(trianglesList, tri, vMid, vTop -2, vTop, vTop-1);
		
		return tri;
	
	}

	/*바닥면 삼각형*/
	private int CreateBottom(int[] trianglesList, int tri, int ring ){
		
		//바닥면의 첫 줄
		int v = 1;
		int vMin = ring-2;
		int vMid = vertices.Length - (xSize-1)*(zSize-1);
		tri = SetQuad(trianglesList, tri, ring-1, vMid , 0,1);
		
		for (int x = 1; x < xSize - 1; x++, v++, vMid++) {
			tri = SetQuad(trianglesList, tri, vMid, vMid + 1, v, v + 1);
		}
		tri = SetQuad(trianglesList, tri, vMid, v + 2, v, v + 1);

		//바닥면의 중간부분
		vMid -= xSize - 2;
		int vMax = v + 2;

		for (int z = 1; z < zSize - 1; z++, vMin--, vMid++, vMax++) {
			tri = SetQuad(trianglesList, tri, vMin, vMid + xSize - 1, vMin + 1, vMid);
			for (int x = 1; x < xSize - 1; x++, vMid++) {
				tri = SetQuad(
					trianglesList, tri,
					vMid + xSize - 1, vMid + xSize, vMid, vMid + 1);
			}
			tri = SetQuad(trianglesList, tri, vMid + xSize - 1, vMax + 1, vMid, vMax);
		}
		
		//바닥면의 마지막
		int vBottom = vMin - 1;
		tri = SetQuad(trianglesList, tri, vBottom + 1, vBottom, vBottom + 2, vMid);
		for (int x = 1; x < xSize - 1; x++, vBottom--, vMid++) {
			tri = SetQuad(trianglesList, tri, vBottom, vBottom - 1, vMid, vMid + 1);
		}
		tri = SetQuad(trianglesList, tri, vBottom, vBottom - 1, vMid, vBottom - 2);
		

	return tri;
	}

    private void OnDrawGizmos(){
        //check if unity is in edit mode
        if (vertices == null) {
            return;
        }
    
        for (int i = 0; i < vertices.Length; i++) {
			Gizmos.color = Color.black;
            Gizmos.DrawSphere(vertices[i], 0.1f);
			Gizmos.color = Color.yellow;
			Gizmos.DrawRay(vertices[i], normals[i]);
        }
    }

}
