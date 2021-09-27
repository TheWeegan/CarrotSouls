using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestMathStuff : MonoBehaviour {
    public Mesh _mesh;
    public Canvas textCanvas;

    public int maxBounceCount = 32;

    void OnDrawGizmos() {
        // ray origin
        // ray direction
        Ray ray = new Ray(transform.position, transform.right);

        for (int i = 0; i < maxBounceCount; i++) {
            if (Physics.Raycast(ray, out RaycastHit hit)) {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(ray.origin, hit.point);
                // Gizmos.color = Color.cyan;
                // Gizmos.DrawRay( hit.point, hit.normal );
                // Gizmos.color = Color.white;
                Vector3 reflected = Reflect(ray.direction, hit.normal);
                // Gizmos.DrawRay( hit.point, reflected );

                // move ray to the new bounce location + direction
                ray.origin = hit.point + hit.normal * 0.001f; // tiny offset margin to avoid starting inside colliders
                ray.direction = reflected;
            } else {
                break; // no more bounces
            }
        }
    }




    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update() {


    }

    void CreateLaser() {
        // ray origin
        // ray direction
        Ray ray = new Ray(transform.position, transform.right);

        for (int i = 0; i < maxBounceCount; i++) {
            if (Physics.Raycast(ray, out RaycastHit hit)) {
                Vector3 reflected = Reflect(ray.direction, hit.normal);

                // move ray to the new bounce location + direction
                ray.origin = hit.point + hit.normal * 0.001f; // tiny offset margin to avoid starting inside colliders
                ray.direction = reflected;
            } else {
                break; // no more bounces
            }
        }
    }
    Vector3 Reflect(Vector3 inDir, Vector3 normal) {
        return inDir - 2 * normal * Vector3.Dot(normal, inDir);
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

            float verticeArea = _cross.magnitude;
            
            meshesArea += verticeArea * 0.5f;
            
        }

        textCanvas.GetComponentInChildren<Text>().text = meshesArea.ToString() + "m^2";
    }


}