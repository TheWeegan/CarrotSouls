using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTurretStuff : MonoBehaviour
{
    [SerializeField] GameObject _gameObject;

    public Vector3 orientation = new Vector3();
    public Vector3 rotation = new Vector3();


    private void OnDrawGizmos() {
        

    }

    // Start is called before the first frame update
    void Start() {

        
    }

    // Update is called once per frame
    void Update() {
        Ray ray = new Ray(_gameObject.transform.position, -_gameObject.transform.up);
        Debug.DrawRay(_gameObject.transform.position, -_gameObject.transform.up, Color.green);

        if (Physics.Raycast(ray, out RaycastHit hit, 1.0f)) {
            _gameObject.transform.up = hit.normal;
            Debug.DrawRay(hit.point, hit.normal, Color.red);


        }


    }


}
