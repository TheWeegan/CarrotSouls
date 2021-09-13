using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekBehaviour
{
    public virtual SteeringOutput GetSteering(CarrotGolemController carrotGolemController) {

        SteeringOutput result = new SteeringOutput();
        result.velocity = carrotGolemController.targetPosition - carrotGolemController.transform.position;
        result.velocity.Normalize();
        result.velocity *= carrotGolemController.maxAcceleration;
        result.velocity.y = 0f;
        result.angular = new Vector3(0f, 0f, 0f);

        return result;
    }
}

public class PursueBehaviour : SeekBehaviour {

    public override SteeringOutput GetSteering(CarrotGolemController carrotGolemController) {

        carrotGolemController.direction = carrotGolemController.targetPosition - carrotGolemController.transform.position;
        carrotGolemController.distance = carrotGolemController.direction.magnitude;
        carrotGolemController.currentSpeed = carrotGolemController.velocity.magnitude;

        if(carrotGolemController.currentSpeed <= carrotGolemController.distance / carrotGolemController.maxPrediction) {
            carrotGolemController.currentPrediction = carrotGolemController.maxPrediction;

        } else {
            carrotGolemController.currentPrediction = carrotGolemController.distance / carrotGolemController.currentSpeed;
        }

        return base.GetSteering(carrotGolemController);
    }

}
