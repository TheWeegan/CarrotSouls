using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FluentBehaviourTree;



public class CarrotGolemBehaviour : MonoBehaviour {
    [SerializeField] Vector3 angularSlowRadius = new Vector3(0f, 270f, 0f);
    [SerializeField] Vector3 maxAngularAcceleration = new Vector3(0f, 15f, 0f);
    [SerializeField] Vector3 maxRotation = new Vector3(0f, 360f, 0f);

    [SerializeField] float attackRange = 3f;
    [SerializeField] float jumpHeight = 5f;
    [SerializeField] float maxAcceleration = 5f;
    [SerializeField] float maxSpeed = 10f;
    [SerializeField] float maxPrediction = 5f;
    [SerializeField] float timeToTarget = 1f;
    [SerializeField] float targetRadius = 5f;

    [SerializeField] AttackMoves attackSlotOne;
    [SerializeField] AttackMoves attackSlotTwo;
    [SerializeField] AttackMoves attackSlotThree;
    [SerializeField] AttackMoves attackSlotFour;

    [SerializeField] GameObject target;
    private BehaviourTreeHandler _behaviourTreeCreator;    
    private CarrotGolemController _carrotGolemController = new CarrotGolemController();
    private TimeData _timeData = new TimeData();

    public CarrotGolemController CarrotGolemController { get => _carrotGolemController; set { _carrotGolemController = value; } }

    private void Awake() {
        _behaviourTreeCreator = new BehaviourTreeHandler();

    }

    void Start() {
        CarrotGolemInit();
        _behaviourTreeCreator.CreateCarrotGolemTree();
    }
    
    void Update() {
        _timeData.deltaTime = Time.deltaTime;
        _behaviourTreeCreator.carrotGolemBehaviourTree.Tick(_timeData);
        /*SteeringOutput result = AIManager.GetInstance.GetBlendSteering.GetSteering(_carrotGolem);
        _carrotGolem.transform.position += result.velocity * Time.deltaTime;

        _carrotGolem.orientation.y += _carrotGolem.rotation.y * Time.deltaTime;
        _carrotGolem.rotation.y += result.angular.y;

        _carrotGolem.transform.eulerAngles = _carrotGolem.orientation;*/

    }

    private void FixedUpdate() {        

    }

    void CarrotGolemInit() {
        _carrotGolemController.id = AIManager.GetInstance.GetID;
        ++AIManager.GetInstance.GetID;

        _carrotGolemController.transform = transform;
        _carrotGolemController.targetPosition = target.transform.position;
        _carrotGolemController.targetGameobject = target;

        _carrotGolemController.velocity = new Vector3(0f, 0f, 0f);
        _carrotGolemController.direction = new Vector3(0f, 0f, 0f);

        _carrotGolemController.orientation = new Vector3(0f, 0f, 0f);
        _carrotGolemController.rotation = new Vector3(0f, 0f, 0f);
        _carrotGolemController.angularSlowRadius = angularSlowRadius;
        _carrotGolemController.maxAngularAcceleration = maxAngularAcceleration;
        _carrotGolemController.maxRotation = maxRotation;

        _carrotGolemController.attackRange = attackRange;
        _carrotGolemController.jumpHeight = jumpHeight;
        _carrotGolemController.maxAcceleration = maxAcceleration;
        _carrotGolemController.maxSpeed = maxSpeed;
        _carrotGolemController.maxPrediction = maxPrediction;
        _carrotGolemController.timeToTarget = timeToTarget;
        _carrotGolemController.targetRadius = targetRadius;

        _carrotGolemController.currentSpeed = 0f;
        _carrotGolemController.distance = 0f;
        _carrotGolemController.currentPrediction = 0f;

        AIManager.GetInstance.GetBlendSteering.AddMappedController(ref _carrotGolemController);

        BehaviourAndWeight faceBehaviourAndWeight = new BehaviourAndWeight();
        faceBehaviourAndWeight._behaviourType = BehaviourType.FaceBehaviour;
        faceBehaviourAndWeight._behaviourWeight = 1f;
        AIManager.GetInstance.GetBlendSteering.AddMappedBehaviour(ref _carrotGolemController, faceBehaviourAndWeight);
        
        BehaviourAndWeight pursueBehaviourAndWeight = new BehaviourAndWeight();
        pursueBehaviourAndWeight._behaviourType = BehaviourType.PursueBehaviour;
        pursueBehaviourAndWeight._behaviourWeight = 1f;
        AIManager.GetInstance.GetBlendSteering.AddMappedBehaviour(ref _carrotGolemController, pursueBehaviourAndWeight);

        _carrotGolemController.currentAttack = AttackMoves.None;
        _carrotGolemController.attackSlotOne = attackSlotOne;
        _carrotGolemController.attackSlotTwo = attackSlotTwo;
        _carrotGolemController.attackSlotThree = attackSlotThree;
        _carrotGolemController.attackSlotFour = attackSlotFour;
    }
}
