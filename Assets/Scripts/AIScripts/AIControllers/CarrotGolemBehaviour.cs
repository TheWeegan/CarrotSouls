using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FluentBehaviourTree;



public class CarrotGolemBehaviour : MonoBehaviour {
    [SerializeField] Vector3 angularSlowRadius = new Vector3(0f, 270f, 0f);
    [SerializeField] Vector3 maxAngularAcceleration = new Vector3(0f, 15f, 0f);
    [SerializeField] Vector3 maxRotation = new Vector3(0f, 360f, 0f);

    [SerializeField] float attackRange = 20f;
    [SerializeField] float attackCooldown = 2f;
    [SerializeField] float attackDuration = 3f;
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

    [SerializeField] AudioClip gigaImpactSound;
    [SerializeField] AudioClip earthquakeSound;
    [SerializeField] AudioClip bodySlamSound;
    [SerializeField] AudioClip woodHammerSound;

    [SerializeField] ParticleSystem gigaImpactParticleSystem;
    [SerializeField] ParticleSystem earthquakeParticleSystem;
    [SerializeField] ParticleSystem bodySlamParticleSystem;
    [SerializeField] ParticleSystem woodHammerParticleSystem;

    [SerializeField] GameObject _targetObject;
    private BehaviourTreeHandler _behaviourTreeCreator;

    private CarrotGolemController _carrotGolemController = new CarrotGolemController();
    private TimeData _timeData = new TimeData();

    public CarrotGolemController CarrotGolemController { get => _carrotGolemController; set { _carrotGolemController = value; } }

    private void Awake() {
        _behaviourTreeCreator = BehaviourTreeHandler.GetInstance;

    }

    void Start() {
        CarrotGolemInit();

        _behaviourTreeCreator.CreateCarrotGolemTree(gameObject, _targetObject);
    }
    
    void Update() {
        _timeData.deltaTime = Time.deltaTime;
        _behaviourTreeCreator.carrotGolemBehaviourTree.Tick(_timeData);

        if(_carrotGolemController.cooldownTimer > 0) {
            _carrotGolemController.cooldownTimer -= Time.deltaTime;
        }

        if (_carrotGolemController.attackTimer > 0) {
            _carrotGolemController.attackTimer -= Time.deltaTime;
        }
    }

    public void IgnoreCollisionWithTarget() {
        if (gameObject.TryGetComponent(out CharacterController characterController) && 
            _carrotGolemController.targetGameObject.TryGetComponent(out CharacterController playerCharacterController)) {            
            Physics.IgnoreCollision(characterController, playerCharacterController);
        }
    }

    public void UpdateMovementBehaviour(bool updateAngular, bool updateVelocity, ref CarrotGolemController carrotGolemController) {
        SteeringOutput result = AIHandler.GetInstance.GetBlendSteering.GetSteering(_carrotGolemController);
        
        if (updateAngular) {
            carrotGolemController.orientation.y += _carrotGolemController.rotation.y * Time.deltaTime;
            carrotGolemController.rotation.y += result.angular.y;
            carrotGolemController.transform.eulerAngles = _carrotGolemController.orientation;
        }
        if (updateVelocity) {
            carrotGolemController.transform.position += result.velocity * Time.deltaTime;
        }

        _carrotGolemController = carrotGolemController;
    }

    public void UpdateControllerValues(CarrotGolemController controllerValues) {
        _carrotGolemController = controllerValues;
    }

    void CarrotGolemInit() {
        _carrotGolemController.id = AIHandler.GetInstance.GetID;
        ++AIHandler.GetInstance.GetID;

        _carrotGolemController.transform = transform;
        _carrotGolemController.targetPosition = _targetObject.transform.position;
        _carrotGolemController.targetGameObject = _targetObject;

        _carrotGolemController.velocity = new Vector3(0f, 0f, 0f);
        _carrotGolemController.direction = new Vector3(0f, 0f, 0f);

        _carrotGolemController.orientation = new Vector3(0f, 0f, 0f);
        _carrotGolemController.rotation = new Vector3(0f, 0f, 0f);
        _carrotGolemController.angularSlowRadius = angularSlowRadius;
        _carrotGolemController.maxAngularAcceleration = maxAngularAcceleration;
        _carrotGolemController.maxRotation = maxRotation;
        _carrotGolemController.characterWidth = 2.0f;

        _carrotGolemController.attackRange = attackRange;
        _carrotGolemController.attackCooldown = attackCooldown;
        _carrotGolemController.attackDuration = attackDuration;
        _carrotGolemController.jumpHeight = jumpHeight;
        _carrotGolemController.maxAcceleration = maxAcceleration;
        _carrotGolemController.maxSpeed = maxSpeed;
        _carrotGolemController.maxPrediction = maxPrediction;
        _carrotGolemController.timeToTarget = timeToTarget;
        _carrotGolemController.targetRadius = targetRadius;

        _carrotGolemController.currentSpeed = 0f;
        _carrotGolemController.distance = 0f;
        _carrotGolemController.currentPrediction = 0f;
        _carrotGolemController.gravity = 0f;
        _carrotGolemController.cooldownTimer = 0f;
        _carrotGolemController.attackTimer = 0f;

        _carrotGolemController.hasHitOnce = false;

        AIHandler.GetInstance.GetBlendSteering.AddMappedController(ref _carrotGolemController);

        BehaviourAndWeight faceBehaviourAndWeight = new BehaviourAndWeight();
        faceBehaviourAndWeight._behaviourType = BehaviourType.FaceBehaviour;
        faceBehaviourAndWeight._behaviourWeight = 1f;
        AIHandler.GetInstance.GetBlendSteering.AddMappedBehaviour(ref _carrotGolemController, faceBehaviourAndWeight);
        
        BehaviourAndWeight pursueBehaviourAndWeight = new BehaviourAndWeight();
        pursueBehaviourAndWeight._behaviourType = BehaviourType.PursueBehaviour;
        pursueBehaviourAndWeight._behaviourWeight = 1f;
        AIHandler.GetInstance.GetBlendSteering.AddMappedBehaviour(ref _carrotGolemController, pursueBehaviourAndWeight);

        _carrotGolemController.currentAttack = AttackMoves.None;
        _carrotGolemController.attackSlotOne = attackSlotOne;
        _carrotGolemController.attackSlotTwo = attackSlotTwo;
        _carrotGolemController.attackSlotThree = attackSlotThree;
        _carrotGolemController.attackSlotFour = attackSlotFour;

        _carrotGolemController.gigaImpactSound = gigaImpactSound;
        _carrotGolemController.earthquakeSound = earthquakeSound;
        _carrotGolemController.bodySlamSound = bodySlamSound;
        _carrotGolemController.woodHammerSound = woodHammerSound;

        _carrotGolemController.gigaImpactParticleSystem = earthquakeParticleSystem;
        _carrotGolemController.earthquakeParticleSystem = earthquakeParticleSystem;
        _carrotGolemController.bodySlamParticleSystem = earthquakeParticleSystem;
        _carrotGolemController.woodHammerParticleSystem = earthquakeParticleSystem;
    }
}
