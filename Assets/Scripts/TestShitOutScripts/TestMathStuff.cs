using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestMathStuff : MonoBehaviour {
    public Mesh _mesh;
    public Canvas textCanvas;

    Vector3 _cross = new Vector3();

    private void OnDrawGizmos() {
        Gizmos.color = Color.cyan;
        //Gizmos.DrawMesh(_mesh);


        Gizmos.color = Color.green;
        for (int i = 0; i < 1; i++) {
            Vector3 firstVertice = _mesh.vertices[_mesh.triangles[0]];
            Vector3 secondVertice = _mesh.vertices[_mesh.triangles[1]];
            Vector3 thirdVertice = _mesh.vertices[_mesh.triangles[2]];

            //Gizmos.DrawLine(firstVertice, secondVertice);
            Gizmos.DrawLine(secondVertice, thirdVertice);
            Gizmos.DrawLine(thirdVertice, firstVertice);


            //Gizmos.DrawLine(cross,cross.normalized);
            //float verticecArea = cross.magnitude * 0.5f;
            //meshesArea += verticecArea;

        }
    }
    // Start is called before the first frame update
    void Start() {
        float meshesArea = 0;
        for (int i = 0; i < _mesh.triangles.Length; i += 3) {
            Vector3 firstVertice = _mesh.vertices[_mesh.triangles[i]];
            Vector3 secondVertice = _mesh.vertices[_mesh.triangles[i + 1]];
            Vector3 thirdVertice = _mesh.vertices[_mesh.triangles[i + 2]];




            //Vector3 cross = Vector3.Cross(distance1, distance2);
            //float verticecArea = cross.magnitude * 0.5f;
            //meshesArea += verticecArea;

        }
        textCanvas.GetComponentInChildren<Text>().text = meshesArea.ToString();

    }

    // Update is called once per frame
    void Update() {


    }



}