using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTurretStuff : MonoBehaviour
{
    [SerializeField] List<GameObject> _gameObjects;
    [SerializeField] GameObject _player;

    public float _gunDistance = 10.0f;
    public float _barrelLength = 10.0f;
    public float _gunHeight = 10.0f;

    private void OnDrawGizmos() {
        

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
        }
    }

    void LookAtPlayer(GameObject turret) {
        Vector3 direction = _player.transform.position - turret.transform.position;
        if(direction.magnitude <= _gunDistance) {

        }

    }
}
