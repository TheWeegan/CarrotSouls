using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignBehaviour {
    protected Vector3 _rotation = new Vector3(0, 0, 0);
    protected Vector3 _rotationSize = new Vector3(0, 0, 0);
    protected Vector3 _targetRotation = new Vector3(0, 0, 0);
    protected Vector3 _angularAcceleration = new Vector3(0, 0, 0);
    protected Vector3 _wantedOrientation = new Vector3(0, 0, 0);

    protected const float clampLimit = 180f;

    public virtual SteeringOutput GetSteering(CarrotGolemController carrotGolemController) {
        SteeringOutput result = new SteeringOutput();
        //_rotation.y = carrotGolemController._targetTransform.rotation.y - carrotGolemController._orientation.y;
        _rotation.y = _wantedOrientation.y - carrotGolemController.orientation.y;

        _rotation.y = Mathf.Clamp(_rotation.y, -clampLimit, clampLimit);
        _rotationSize.y = Mathf.Abs(_rotation.y);

        if(_rotationSize.y < carrotGolemController.targetRadius) {
            return new SteeringOutput();
        }

        if(_rotationSize.y > carrotGolemController.angularSlowRadius.y) {
            _targetRotation.y = carrotGolemController.maxRotation.y;

        } else {
            _targetRotation.y = carrotGolemController.maxRotation.y * _rotationSize.y / carrotGolemController.angularSlowRadius.y;
        }
        _targetRotation.y *= _rotation.y / _rotationSize.y;


        result.angular.y = _targetRotation.y - carrotGolemController.rotation.y;
        result.angular.y /= carrotGolemController.timeToTarget;
        
        _angularAcceleration.y = Mathf.Abs(result.angular.y);

        if (_angularAcceleration.y > carrotGolemController.maxAngularAcceleration.y) {
            result.angular.y /= _angularAcceleration.y;
            result.angular.y *= carrotGolemController.maxAngularAcceleration.y;
        }

        result.velocity = new Vector3(0,0,0);
        return result;
    }

}

public class FaceBehaviour : AlignBehaviour {
    private Vector3 _direction = new Vector3(0f, 0f, 0f);
    private float oldOrientation = 0;
    private float offset = 150f;

    public override SteeringOutput GetSteering(CarrotGolemController carrotGolemController) {
        _direction = carrotGolemController.targetPosition - carrotGolemController.transform.position;

        if (_direction.magnitude == 0f) {
            return new SteeringOutput();
        }
        _wantedOrientation.y = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg;

        if(_wantedOrientation.y < -offset && oldOrientation > offset) {
            if(carrotGolemController.orientation.y > -180) {
                carrotGolemController.orientation.y -= 360f;
            }
        } else if(_wantedOrientation.y > offset && oldOrientation < -offset) {
            if (carrotGolemController.orientation.y < 180) {
                carrotGolemController.orientation.y += 360f;
            }
        }

        oldOrientation = _wantedOrientation.y;
        return base.GetSteering(carrotGolemController);
    }

}

#region Quaternions attempt for the future
public class QuaternionAlignBehaviour {
    protected Quaternion _rotation = new Quaternion(0, 0, 0, 1);
    protected Quaternion _rotationSize = new Quaternion(0, 0, 0, 1);
    protected Quaternion _targetRotation = new Quaternion(0, 0, 0, 1);
    protected Quaternion _angularAcceleration = new Quaternion(0, 0, 0, 1);
    protected Quaternion _wantedOrientation = new Quaternion(0, 0, 0, 1);
    protected Quaternion _baseOrientation = new Quaternion(0, 0, 0, 1);

    protected const float clampLimit = 1f;

    public virtual SteeringOutput GetSteering(CarrotGolemController carrotGolemController) {
        SteeringOutput result = new SteeringOutput();



        result.velocity = new Vector3(0, 0, 0);
        return result;
    }
}

public class QuaternionFaceBehaviour : QuaternionAlignBehaviour {

    private Vector3 _direction = new Vector3(0f, 0f, 0f);

    public Quaternion CalculateOrientaion(Vector3 vector) {
        Vector3 zVector = new Vector3(0, 0, 1);
        Vector3 baseZVector = new Vector3(zVector.x * _baseOrientation.x, zVector.y * _baseOrientation.y, zVector.z * _baseOrientation.z);

        if (baseZVector == vector) {
            return _baseOrientation;
        } else if (baseZVector == -vector) {
            Quaternion negQuaternion = new Quaternion(-_baseOrientation.x, -_baseOrientation.y, -_baseOrientation.z, -_baseOrientation.w);
            return negQuaternion;
        }

        Vector3 axis = Vector3.Cross(baseZVector, vector);
        float angle = Mathf.Asin(axis.magnitude);
        axis.Normalize();

        float sinAngle = Mathf.Sin(angle / 2);

        return new Quaternion(Mathf.Cos(angle / 2), sinAngle * axis.x, sinAngle * axis.y, sinAngle * axis.z);
    }

    public override SteeringOutput GetSteering(CarrotGolemController carrotGolemController) {
        carrotGolemController.direction = carrotGolemController.targetPosition - carrotGolemController.transform.position;


        if (carrotGolemController.direction.magnitude == 0) {
            return new SteeringOutput();
        }

        _wantedOrientation = CalculateOrientaion(carrotGolemController.direction);
        return base.GetSteering(carrotGolemController);
    }
}
#endregion

