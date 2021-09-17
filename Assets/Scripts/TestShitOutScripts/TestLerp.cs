using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLerp : MonoBehaviour
{
    [SerializeField] GameObject _gameObject0;
    [SerializeField] GameObject _gameObject1;
    [SerializeField] GameObject _gameObject2;
    [SerializeField] GameObject _gameObject3;

    private Vector3 _p0;
    private Vector3 _p1;
    private Vector3 _p2;
    private Vector3 _p3;

    private Vector3 _a = new Vector3();
    private Vector3 _b = new Vector3();
    private Vector3 _c = new Vector3();
    private Vector3 _d = new Vector3();
    private Vector3 _e = new Vector3();

    private Vector3 _currentPosition;

    private List<Vector3> _lengthSegments = new List<Vector3>();

    private float _t = 0.01f;

    private void Awake() {
        _currentPosition = gameObject.transform.position;

        _p0 = _gameObject0.transform.position;
        _p1 = _gameObject1.transform.position;
        _p2 = _gameObject2.transform.position;
        _p3 = _gameObject3.transform.position;

        _lengthSegments.Add(_p0);

        float t = 0.1f;
        for (int i = 0; i < 8; ++i) {
            _a = Vector3.Lerp(_p0, _p1, t);
            _b = Vector3.Lerp(_p1, _p2, t);
            _c = Vector3.Lerp(_p2, _p3, t);
            _d = Vector3.Lerp(_a, _b, t);
            _e = Vector3.Lerp(_b, _c, t);
            _lengthSegments.Add(Vector3.Lerp(_d, _e, t));
            t += 0.1f;
        }
        _lengthSegments.Add(_p3);
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    float _length = 0f;
    float speed = 2500f;
    float distance = 0f;
    // Update is called once per frame
    void Update() {
        for (int i = 0; i < _lengthSegments.Count - 1; ++i) {
            _length += (_lengthSegments[i + 1] - _lengthSegments[i]).magnitude; 
        }

        if(_t >= 1f && distance > 0) {
             distance = 0f;
        
        } else if(_t <= 0f && distance > 0) {
            distance = 0f;
        }
        distance += (speed / _length) * Time.deltaTime;


        _p0 = _gameObject0.transform.position;
        _p1 = _gameObject1.transform.position;
        _p2 = _gameObject2.transform.position;
        _p3 = _gameObject3.transform.position;



        _t = distance / _length;


        Vector3 oldPosition = gameObject.transform.position;

        _a = Vector3.Lerp(_p0, _p1, _t);
        _b = Vector3.Lerp(_p1, _p2, _t);
        _c = Vector3.Lerp(_p2, _p3, _t);
        _d = Vector3.Lerp(_a, _b, _t);
        _e = Vector3.Lerp(_b, _c, _t);
        _currentPosition = Vector3.Lerp(_d, _e, _t);
        gameObject.transform.position = _currentPosition;
        //distance += (_currentPosition - oldPosition).magnitude;
        
        _length = 0f;
    }

    float DistToT(float[] LUT, float distance) {
        float arcLength = 0f;
        int n = LUT.Length;

        if(distance > 0 && distance < arcLength) {
            for (int i = 0; i < n - 1; ++i) {
                if(distance > LUT[i] && distance < LUT[i + 1]) {
                    distance = Remap(distance, 
                        LUT[i], 
                        LUT[i + 1], 
                        i / (n - 1),
                        (i + 1f) / (n - 1f));
                    

                }

            }
        }
        return distance / arcLength;
    }

    public float Remap(float value, float from1, float to1, float from2, float to2) {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
