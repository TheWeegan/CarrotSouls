using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestMathStuff : MonoBehaviour {
    public Mesh _mesh;
    public Canvas textCanvas;

    private void OnDrawGizmos() {
        Gizmos.color = Color.cyan;
        Gizmos.DrawMesh(_mesh);
    }
    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        CalculateMeshArea();

    }

    void CalculateMeshArea() {
        float meshesArea = 0;
        for (int i = 0; i < _mesh.triangles.Length; i += 3) {
            Vector3 firstVertice = _mesh.vertices[_mesh.triangles[i]];
            Vector3 secondVertice = _mesh.vertices[_mesh.triangles[i + 1]];
            Vector3 thirdVertice = _mesh.vertices[_mesh.triangles[i + 2]];

            Vector3 distance = thirdVertice - secondVertice;
            Vector3 distance2 = firstVertice - thirdVertice;
            Vector3 _cross = Vector3.Cross(distance, distance2);
            meshesArea += _cross.magnitude * 0.5f;
        }
        textCanvas.GetComponentInChildren<Text>().text = meshesArea.ToString() + "m^2";
    }


}