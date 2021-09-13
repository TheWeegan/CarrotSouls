using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BehaviourType {
	AlignBehaviour,
	ArriveBehaviour,
	CohesionBehaviour,
	FaceBehaviour,
	LookWhereYouAreGoingBehaviour,
	PursueBehaviour,
	SeekBehaviour,
	SeparationBehaviour,
	WanderBehaviour,
	Count
}

public struct BehaviourAndWeight {
	public BehaviourType _behaviourType;
	public float _behaviourWeight;

}
public class BlendSteering {	
	private AlignBehaviour _alignBehaviour = new AlignBehaviour();
	private FaceBehaviour _faceBehaviour = new FaceBehaviour();
	private PursueBehaviour _pursueBehaviour = new PursueBehaviour();
	private SeekBehaviour _seekBehaviour = new SeekBehaviour();
	
	public SteeringOutput GetSteering(CarrotGolemController carrotGolem) {
		SteeringOutput result = new SteeringOutput();
		SteeringOutput tempResult = result;

		foreach (var mappedBehaviour in AIManager.GetInstance.GetMappedBehaviours) {
            foreach (BehaviourAndWeight behaviour in mappedBehaviour.Value) {
				if(mappedBehaviour.Key != carrotGolem.id) {
					continue;
				}
                switch (behaviour._behaviourType) {
					case BehaviourType.AlignBehaviour: {
							tempResult = _alignBehaviour.GetSteering(carrotGolem);
							result.velocity += tempResult.velocity * behaviour._behaviourWeight;
							result.angular += tempResult.angular * behaviour._behaviourWeight;
							break;
					}
					case BehaviourType.FaceBehaviour: {
							tempResult = _faceBehaviour.GetSteering(carrotGolem);
							result.velocity += tempResult.velocity * behaviour._behaviourWeight;
							result.angular += tempResult.angular * behaviour._behaviourWeight;
							break;
					}
					case BehaviourType.PursueBehaviour: {
							tempResult = _pursueBehaviour.GetSteering(carrotGolem);
							result.velocity += tempResult.velocity * behaviour._behaviourWeight;
							result.angular += tempResult.angular * behaviour._behaviourWeight;
							break;
					}
					case BehaviourType.SeekBehaviour: {
							tempResult = _seekBehaviour.GetSteering(carrotGolem);
							result.velocity += tempResult.velocity * behaviour._behaviourWeight;
							result.angular += tempResult.angular * behaviour._behaviourWeight;
							break;
					}
                    default: {
							break;
                    }
				}
            }
        }
		return result;
    }

	public void AddMappedController(ref CarrotGolemController carrotGolem) {

		AIManager.GetInstance.GetMappedBehaviours.Add(carrotGolem.id, new List<BehaviourAndWeight>());
	}

	public void AddMappedBehaviour(ref CarrotGolemController carrotGolem, BehaviourAndWeight behaviourAndWeight) {

		AIManager.GetInstance.GetMappedBehaviours[carrotGolem.id].Add(behaviourAndWeight);
    }

	public void RemoveMappedBehaviour(ref CarrotGolemController carrotGolem, BehaviourType behaviourType) {
		int index = AIManager.GetInstance.GetMappedBehaviours[carrotGolem.id].FindIndex(beahviour => beahviour._behaviourType == behaviourType);		
		if(index >= 0) {
			AIManager.GetInstance.GetMappedBehaviours[carrotGolem.id].RemoveAt(index);
        }
	}
}
