using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTurretStuff : MonoBehaviour
{
    [SerializeField] List<GameObject> _gameObjects;
    [SerializeField] GameObject _player;

    public float _gunHeight = 1.3f;
    public float _barrelLength = 3f;
    public float _gunSeparation = 0.3f;

    Vector3 _gun1 = new Vector3();
    Vector3 _gun2 = new Vector3();

    Vector3[] _corners;

    private void OnDrawGizmos() {

        foreach (GameObject turret in _gameObjects) {
            _corners = new Vector3[] {
                // bottom 4 positions:
	            turret.transform.right + turret.transform.forward,
                -turret.transform.right + turret.transform.forward,
                -turret.transform.right + -turret.transform.forward,
                turret.transform.right + -turret.transform.forward,
	            // top 4 positions:
	            turret.transform.position + turret.transform.right + (turret.transform.up * 2) + turret.transform.forward,
                turret.transform.position + -turret.transform.right + (turret.transform.up * 2) + turret.transform.forward,
                turret.transform.position + -turret.transform.right + (turret.transform.up * 2) + -turret.transform.forward,
                turret.transform.position + turret.transform.right + (turret.transform.up * 2) + -turret.transform.forward
            };

            Matrix4x4 bottonCorners = new Matrix4x4(_corners[0], _corners[1], _corners[2], _corners[3]);

            Vector3 matrixPos1 = new Vector3(bottonCorners.m00, bottonCorners.m01, bottonCorners.m02);
            Vector3 matrixPos2 = new Vector3(bottonCorners.m10, bottonCorners.m11, bottonCorners.m12);


            Gizmos.DrawLine(matrixPos1 + turret.transform.position, matrixPos2 + turret.transform.position);

            Gizmos.DrawLine(_corners[0] + turret.transform.position, _corners[1] + turret.transform.position);


            break;
            Gizmos.DrawLine(_corners[0], _corners[1]);
            Gizmos.DrawLine(_corners[1], _corners[2]);
            Gizmos.DrawLine(_corners[2], _corners[3]);
            Gizmos.DrawLine(_corners[3], _corners[0]);

            Gizmos.DrawLine(_corners[0], _corners[4]);
            Gizmos.DrawLine(_corners[1], _corners[5]);
            Gizmos.DrawLine(_corners[2], _corners[6]);
            Gizmos.DrawLine(_corners[3], _corners[7]);

            Gizmos.DrawLine(_corners[4], _corners[5]);
            Gizmos.DrawLine(_corners[5], _corners[6]);
            Gizmos.DrawLine(_corners[6], _corners[7]);
            Gizmos.DrawLine(_corners[7], _corners[4]);


            _gun1 = turret.transform.position + turret.transform.forward * _barrelLength;
            _gun1.y = turret.transform.position.y + turret.transform.up.y * _gunHeight;

            _gun2 = turret.transform.position + turret.transform.forward * _barrelLength;
            _gun2.y = turret.transform.position.y + turret.transform.up.y * _gunHeight;

            _gun1.x += _gunSeparation * 0.5f;
            _gun2.x -= _gunSeparation * 0.5f;

            Vector3 barrelOffset = new Vector3(_gunSeparation * 0.5f, 0f, 0f);

            Gizmos.DrawLine(turret.transform.position + barrelOffset, _gun1);
            Gizmos.DrawLine(turret.transform.position - barrelOffset, _gun2);
            
            break;
        }
    }

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        foreach(GameObject turret in _gameObjects) {
            Ray ray = new Ray(turret.transform.position, -turret.transform.up);
            Debug.DrawRay(turret.transform.position, -turret.transform.up, Color.cyan);

            if (Physics.Raycast(ray, out RaycastHit hit, 1.0f)) {
                turret.transform.up = hit.normal;
                Debug.DrawRay(hit.point, hit.normal, Color.red);
            }

            Debug.DrawRay(turret.transform.position, turret.transform.right, Color.red);
            Debug.DrawRay(turret.transform.position, turret.transform.up, Color.green);
            Debug.DrawRay(turret.transform.position, turret.transform.forward, Color.blue);
            /*
            _corners = new Vector3[] {
                // bottom 4 positions:
	            turret.transform.position + turret.transform.right + turret.transform.forward,
                turret.transform.position + -turret.transform.right + turret.transform.forward,
                turret.transform.position + -turret.transform.right + -turret.transform.forward,
                turret.transform.position + turret.transform.right + -turret.transform.forward,
	            // top 4 positions:
	            turret.transform.position + turret.transform.right + (turret.transform.up * 2) + turret.transform.forward,
                turret.transform.position + -turret.transform.right + (turret.transform.up * 2) + turret.transform.forward,
                turret.transform.position + -turret.transform.right + (turret.transform.up * 2) + -turret.transform.forward,
                turret.transform.position + turret.transform.right + (turret.transform.up * 2) + -turret.transform.forward
            };*/

            Debug.DrawLine(_corners[0], _corners[1]);
            Debug.DrawLine(_corners[1], _corners[2]);
            Debug.DrawLine(_corners[2], _corners[3]);
            Debug.DrawLine(_corners[3], _corners[0]);

            Debug.DrawLine(_corners[0], _corners[4]);
            Debug.DrawLine(_corners[1], _corners[5]);
            Debug.DrawLine(_corners[2], _corners[6]);
            Debug.DrawLine(_corners[3], _corners[7]);

            Debug.DrawLine(_corners[4], _corners[5]);
            Debug.DrawLine(_corners[5], _corners[6]);
            Debug.DrawLine(_corners[6], _corners[7]);
            Debug.DrawLine(_corners[7], _corners[4]);

            break;
        }
    }

    void LookAtPlayer(GameObject turret) {
        
    }
}
