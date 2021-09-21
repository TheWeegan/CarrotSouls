using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMathStuff : MonoBehaviour
{
    [SerializeField] List<GameObject> _walls;

    [SerializeField] Vector2 velocity = new Vector2(7.5f, 6f);
    [SerializeField] float speed = 5f;
    [SerializeField] float _rayDistance = 20f;
    
    Vector2 position2D = new Vector2();

    /*private void OnDrawGizmos() {
        Vector2 position = gameObject.transform.position;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(_origo, position);

        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(_origo, _normal.normalized);
        //Gizmos.DrawLine(_b, _bNormal);

        Vector2 dotProduct = position * _normal.normalized;
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(position, dotProduct);

        Vector2 reflection = new Vector2(dotProduct.y, -dotProduct.x);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(position, reflection);

    }*/



    private void Start() {
        

    }

    private void FixedUpdate() {
        position2D = transform.position;
        //position2D += velocity * speed * Time.deltaTime;
        transform.position = position2D;

        Debug.DrawLine(position2D, position2D + (velocity.normalized * _rayDistance));

        RaycastHit2D hit2D = Physics2D.Raycast(position2D, velocity.normalized, _rayDistance);

        if (hit2D.collider != null) {
            //velocity = new Vector2(velocity.y, -velocity.x);
            float adj = (hit2D.point + hit2D.normal).magnitude;
            float hyp = (hit2D.point - velocity).magnitude;
            float cosIsh = adj / hyp;
            
            

            Debug.DrawLine(hit2D.point, hit2D.point + hit2D.normal, Color.cyan);
            Debug.DrawLine(hit2D.point, hit2D.point - velocity.normalized, Color.magenta);
            

            //Debug.DrawLine(position2D, hit2D.point, Color.green);            
            //float dot = Vector2.Dot(velocity, hit2D.normal);
            //Debug.DrawLine(hit2D.point, hit2D.point + (hit2D.normal * dot), Color.red);
        }
    }
}
