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

    float t = 0f;

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

        /*for (int i = 0; i < _carrotGolemController.lerpPositions.Count - 1; i++) {
            Debug.DrawLine(_carrotGolemController.lerpPositions[i], _carrotGolemController.lerpPositions[i + 1]);
        }*/
    }

    public void IgnoreCollisionWithTarget() {
        if (gameObject.TryGetComponent(out CharacterController characterController) && 
            _carrotGolemController.targetGameObject.TryGetComponent(out CharacterController playerCharacterController)) {            
            Physics.IgnoreCollision(characterController, playerCharacterController);
        }
    }

    public void UpdateMovementBehaviour(bool updateAngular, bool updateVelocity) {
        SteeringOutput result = AIHandler.GetInstance.GetBlendSteering.GetSteering(_carrotGolemController);
        
        if (updateAngular) {
            _carrotGolemController.orientation.y += _carrotGolemController.rotation.y * Time.deltaTime;
            _carrotGolemController.rotation.y += result.angular.y;
            _carrotGolemController.transform.eulerAngles = _carrotGolemController.orientation;
        }
        if (updateVelocity) {
            _carrotGolemController.transform.position += result.velocity * Time.deltaTime;
        }
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
        _carrotGolemController.lerpPositions = new List<Vector3>();
        _carrotGolemController.lerpLengthSegments = new List<Vector3>();

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

    public void SetTargetPlayer() {
        _carrotGolemController.targetGameObject = _targetObject;
        _carrotGolemController.targetPosition = _targetObject.transform.position;
    }

    private bool TargetIsHit(GameObject gameObject, GameObject targetGameObject, float attackRange) {
        Vector3 distance = targetGameObject.transform.position - gameObject.transform.position;
        return distance.magnitude < attackRange;
    }

    public bool PickAttack() {
        int attackChoice = Random.Range(0, 4);
        switch (attackChoice) {
            case 0: {
                    if (_carrotGolemController.latestUsedAttackUsed != _carrotGolemController.attackSlotOne &&
                        _carrotGolemController.secondLatestAttackUsed != _carrotGolemController.attackSlotOne) {
                        ChangeAttack(_carrotGolemController.attackSlotOne);
                        return true;
                    }
                    break;
                }
            case 1: {
                    if (_carrotGolemController.latestUsedAttackUsed != _carrotGolemController.attackSlotTwo &&
                        _carrotGolemController.secondLatestAttackUsed != _carrotGolemController.attackSlotTwo) {
                        ChangeAttack(_carrotGolemController.attackSlotTwo);
                        return true;
                    }
                    break;
                }
            case 2: {
                    if (_carrotGolemController.latestUsedAttackUsed != _carrotGolemController.attackSlotThree &&
                        _carrotGolemController.secondLatestAttackUsed != _carrotGolemController.attackSlotThree) {
                        ChangeAttack(_carrotGolemController.attackSlotThree);
                        return true;
                    }
                    break;
                }
            case 3: {
                    if (_carrotGolemController.latestUsedAttackUsed != _carrotGolemController.attackSlotFour &&
                        _carrotGolemController.secondLatestAttackUsed != _carrotGolemController.attackSlotFour) {
                        ChangeAttack(_carrotGolemController.attackSlotFour);
                        return true;
                    }
                    break;
                }
            default:
                break;
        }
        
        return false;
    }

    private Vector3 CreateLerp(Vector3 zero, Vector3 one, Vector3 two, Vector3 three) {
        Vector3 _a = Vector3.Lerp(zero, one, t);
        Vector3 _b = Vector3.Lerp(one, two, t);
        Vector3 _c = Vector3.Lerp(two, three, t);
        Vector3 _d = Vector3.Lerp(_a, _b, t);
        Vector3 _e = Vector3.Lerp(_b, _c, t);
        return Vector3.Lerp(_d, _e, t);
    }

    private void GenerateLerpSegments() {
        float distance = (_carrotGolemController.targetPosition - _carrotGolemController.transform.position).magnitude;

        _carrotGolemController.lerpPositions.Add(_carrotGolemController.transform.position);
        _carrotGolemController.lerpPositions.Add(_carrotGolemController.transform.position + _carrotGolemController.transform.up * distance);
        _carrotGolemController.lerpPositions.Add(_carrotGolemController.targetPosition + _carrotGolemController.transform.up * distance);
        _carrotGolemController.lerpPositions.Add(_carrotGolemController.targetPosition);

        _carrotGolemController.lerpLengthSegments.Add(_carrotGolemController.lerpPositions[0]);
        float t = 0.1f;
        for (int i = 0; i < 8; ++i) {
            _carrotGolemController.lerpLengthSegments.Add(CreateLerp(_carrotGolemController.lerpPositions[0], _carrotGolemController.lerpPositions[1],
                _carrotGolemController.lerpPositions[2], _carrotGolemController.lerpPositions[3]));
            t += 0.1f;
        }
        _carrotGolemController.lerpLengthSegments.Add(_carrotGolemController.lerpPositions[3]);

    }

    public void ChangeAttack(AttackMoves newAttack) {
        _carrotGolemController.secondLatestAttackUsed = _carrotGolemController.latestUsedAttackUsed;
        _carrotGolemController.latestUsedAttackUsed = newAttack;
        _carrotGolemController.currentAttack = newAttack;
        _carrotGolemController.attackTimer = _carrotGolemController.attackDuration;

        switch (newAttack) {
            case AttackMoves.GigaImpact:
                //AudioOverlord.GetInstance.PlayerOneShot(gameObject, _carrotGolemController.gigaImpactSound);
                _carrotGolemController.attackTimer = 5f;
                _carrotGolemController.targetPosition = _targetObject.transform.position;
                break;
            case AttackMoves.Earthquake:
                //AudioOverlord.GetInstance.PlayerOneShot(gameObject, _carrotGolemController.earthquakeSound);
                _carrotGolemController.targetPosition = _carrotGolemController.transform.position;
                break;
            case AttackMoves.BodySlam:
                //AudioOverlord.GetInstance.PlayerOneShot(gameObject, _carrotGolemController.bodySlamSound);
                _carrotGolemController.targetPosition = _targetObject.transform.position;
                GenerateLerpSegments();
                break;
            case AttackMoves.WoodHamemr:
                //AudioOverlord.GetInstance.PlayerOneShot(gameObject, _carrotGolemController.woodHammerSound);
                _carrotGolemController.targetPosition = _targetObject.transform.position;
                break;
            default:
                break;
        }
    }

    public void UseGigaImpact() {
        if (gameObject.TryGetComponent(out CharacterController characterController)) {
            if (_carrotGolemController.attackTimer > _carrotGolemController.attackDuration * 0.25f) {
                    _carrotGolemController.targetPosition = _carrotGolemController.targetGameObject.transform.position;
                    UpdateMovementBehaviour(true, false);

                    _carrotGolemController.direction = _carrotGolemController.targetPosition - _carrotGolemController.transform.position;
                    _carrotGolemController.direction.y = 0f;
                    _carrotGolemController.direction.Normalize();

            } else if (_carrotGolemController.attackTimer > 0f) {
                characterController.Move(_carrotGolemController.direction * 50f * Time.deltaTime);
                if (TargetIsHit(gameObject, _carrotGolemController.targetGameObject, _carrotGolemController.characterWidth) && !_carrotGolemController.hasHitOnce) {
                    ParticleOverlord.GetInstance.PlayAttackParticle(gameObject, _carrotGolemController.currentAttack);
                    _carrotGolemController.hasHitOnce = true;
                    Debug.Log("Target is hit by giga impact");
                }

            } else {
                _carrotGolemController.currentAttack = AttackMoves.None;
                _carrotGolemController.cooldownTimer = _carrotGolemController.attackCooldown;
                _carrotGolemController.hasHitOnce = false;
            }
            IgnoreCollisionWithTarget(); 
        }
    }

    public void UseEarthquake() {
        if (gameObject.TryGetComponent(out CharacterController characterController)) {            
            if (_carrotGolemController.attackTimer > _carrotGolemController.attackDuration * 0.05f) {
                characterController.Move(new Vector3(0f, (_carrotGolemController.jumpHeight + _carrotGolemController.gravity) * Time.deltaTime, 0f));
                if (_carrotGolemController.attackTimer <= _carrotGolemController.attackDuration * 0.05f + Time.deltaTime) {
                    _carrotGolemController.gravity = 0f;
                }

            } else if (_carrotGolemController.attackTimer <= _carrotGolemController.attackDuration * 0.05f && !characterController.isGrounded) {
                characterController.Move(new Vector3(0f, (_carrotGolemController.jumpHeight + (_carrotGolemController.gravity * 10)) * Time.deltaTime, 0f));

            } else {
                ParticleOverlord.GetInstance.PlayAttackParticle(gameObject, _carrotGolemController.currentAttack);
                if (_carrotGolemController.targetGameObject.TryGetComponent(out CharacterController targetCharacterController)) {
                    if (targetCharacterController.isGrounded) {
                        Debug.Log("Target is hit by earthquake");
                    }
                }
                _carrotGolemController.currentAttack = AttackMoves.None;
                _carrotGolemController.cooldownTimer = _carrotGolemController.attackCooldown;
                _carrotGolemController.gravity = 0;

            }
            _carrotGolemController.gravity -= Time.deltaTime;
            IgnoreCollisionWithTarget();
        }
    }

    public void UseBodySlam() {

        float curveLength = 0f;
        for (int i = 0; i < _carrotGolemController.lerpLengthSegments.Count - 1; ++i) {
            curveLength += (_carrotGolemController.lerpLengthSegments[i + 1] - _carrotGolemController.lerpLengthSegments[i]).magnitude;
        }
        t += curveLength * 0.025f * Time.deltaTime;

        _carrotGolemController.transform.position = CreateLerp(_carrotGolemController.lerpPositions[0], _carrotGolemController.lerpPositions[1],
            _carrotGolemController.lerpPositions[2], _carrotGolemController.lerpPositions[3]);
        
        if ((_carrotGolemController.targetPosition - _carrotGolemController.transform.position).magnitude <= 0.001f 
            //&&gameObject.GetComponent<CharacterController>().isGrounded
            ) {

            ParticleOverlord.GetInstance.PlayAttackParticle(gameObject, _carrotGolemController.currentAttack);
        
            if (TargetIsHit(gameObject, _carrotGolemController.targetGameObject, _carrotGolemController.characterWidth)) {
                Debug.Log("Target is hit by body slam");
            }
        
            t = 0f;
            _carrotGolemController.lerpPositions.Clear();
            _carrotGolemController.lerpLengthSegments.Clear();
            _carrotGolemController.currentAttack = AttackMoves.None;
            _carrotGolemController.cooldownTimer = _carrotGolemController.attackCooldown;
        }
        IgnoreCollisionWithTarget();
    }

    bool isAttacking = false;
    float woodHammerDuration = 1.0f;
    public void UseWoodHammer() {
        Vector3 distance = _carrotGolemController.targetPosition - _carrotGolemController.transform.position;
        _carrotGolemController.targetPosition = _carrotGolemController.targetGameObject.transform.position;

        if (distance.magnitude <= 5f && !isAttacking) {
            _carrotGolemController.attackTimer = woodHammerDuration;
            isAttacking = true;

        } else if (isAttacking) {
            if (_carrotGolemController.attackTimer > woodHammerDuration * 0.5f) {
                transform.Rotate(0f, 180.0f * Time.deltaTime, 0f);
                _carrotGolemController.orientation = transform.eulerAngles;

            } else {
                transform.Rotate(0f, -360.0f * Time.deltaTime, 0f);
                _carrotGolemController.orientation = transform.eulerAngles;
                UpdateMovementBehaviour(true, true);
                if (_carrotGolemController.attackTimer <= 0) {
                    if (TargetIsHit(gameObject, _carrotGolemController.targetGameObject, _carrotGolemController.characterWidth)) {
                        ParticleOverlord.GetInstance.PlayAttackParticle(gameObject, _carrotGolemController.currentAttack);
                        Debug.Log("Target is hit by body slam");
                    }
                    isAttacking = false;
                    _carrotGolemController.currentAttack = AttackMoves.None;
                    _carrotGolemController.cooldownTimer = _carrotGolemController.attackCooldown;
                }
            }

        } else {
            UpdateMovementBehaviour(true, true);

        }
        IgnoreCollisionWithTarget();
    }


}
