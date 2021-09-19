using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FluentBehaviourTree;



public class CarrotGolemBehaviour : MonoBehaviour {

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

    float lerpT = 0f;
    float curveLength = 0f;

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
        _carrotGolemController.angularSlowRadius = new Vector3(0f, 150f, 0f);
        _carrotGolemController.maxAngularAcceleration = new Vector3(0f, 120f, 0f);
        _carrotGolemController.maxRotation = new Vector3(0f, 180f, 0f);
        _carrotGolemController.characterWidth = 2.0f;
        _carrotGolemController.lerpPositions = new List<Vector3>();
        _carrotGolemController.lerpLengthSegments = new List<Vector3>();

        _carrotGolemController.attackRange = 20f;
        _carrotGolemController.attackCooldown = 2f;
        _carrotGolemController.currentAttackDuration = 0f;
        _carrotGolemController.jumpHeight = 5f;
        _carrotGolemController.maxAcceleration = 5f;
        _carrotGolemController.maxSpeed = 5f;
        _carrotGolemController.maxPrediction = 5f;
        _carrotGolemController.timeToTarget = 1f;
        _carrotGolemController.targetRadius = 1f;

        _carrotGolemController.currentSpeed = 0f;
        _carrotGolemController.distance = 0f;
        _carrotGolemController.currentPrediction = 0f;
        _carrotGolemController.gravity = 0f;
        _carrotGolemController.cooldownTimer = 0f;
        _carrotGolemController.attackTimer = 0f;

        _carrotGolemController.hasHitOnce = false;
        _carrotGolemController.hittingWithWoodHammer = false;

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

        _carrotGolemController.gigaImpactParticleSystem = gigaImpactParticleSystem;
        _carrotGolemController.earthquakeParticleSystem = earthquakeParticleSystem;
        _carrotGolemController.bodySlamParticleSystem = bodySlamParticleSystem;
        _carrotGolemController.woodHammerParticleSystem = woodHammerParticleSystem;
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
        Vector3 _a = Vector3.Lerp(zero, one, lerpT);
        Vector3 _b = Vector3.Lerp(one, two, lerpT);
        Vector3 _c = Vector3.Lerp(two, three, lerpT);
        Vector3 _d = Vector3.Lerp(_a, _b, lerpT);
        Vector3 _e = Vector3.Lerp(_b, _c, lerpT);
        return Vector3.Lerp(_d, _e, lerpT);
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
        
        curveLength = 0f;
        for (int i = 0; i < _carrotGolemController.lerpLengthSegments.Count - 1; ++i) {
            curveLength += (_carrotGolemController.lerpLengthSegments[i + 1] - _carrotGolemController.lerpLengthSegments[i]).magnitude;
        }
    }

    public void ChangeAttack(AttackMoves newAttack) {
        _carrotGolemController.secondLatestAttackUsed = _carrotGolemController.latestUsedAttackUsed;
        _carrotGolemController.latestUsedAttackUsed = newAttack;
        _carrotGolemController.currentAttack = newAttack;
        _carrotGolemController.attackTimer = _carrotGolemController.currentAttackDuration;

        switch (newAttack) {
            case AttackMoves.GigaImpact:
                //AudioOverlord.GetInstance.PlayerOneShot(gameObject, _carrotGolemController.gigaImpactSound);
                _carrotGolemController.currentAttackDuration = AttackDurations.gigaImpactDuration;
                _carrotGolemController.attackTimer = 5f;
                _carrotGolemController.targetPosition = _targetObject.transform.position;
                break;
            case AttackMoves.Earthquake:
                //AudioOverlord.GetInstance.PlayerOneShot(gameObject, _carrotGolemController.earthquakeSound);
                _carrotGolemController.currentAttackDuration = AttackDurations.earthquakeDuration;
                _carrotGolemController.attackTimer = 1f;
                _carrotGolemController.targetPosition = _carrotGolemController.transform.position;
                break;
            case AttackMoves.BodySlam:
                //AudioOverlord.GetInstance.PlayerOneShot(gameObject, _carrotGolemController.bodySlamSound);
                _carrotGolemController.currentAttackDuration = AttackDurations.bodySlamDuration;
                _carrotGolemController.attackTimer = 1f;
                _carrotGolemController.targetPosition = _targetObject.transform.position;
                GenerateLerpSegments();
                break;
            case AttackMoves.WoodHamemr:
                //AudioOverlord.GetInstance.PlayerOneShot(gameObject, _carrotGolemController.woodHammerSound);
                _carrotGolemController.currentAttackDuration = AttackDurations.woodHammerDuration;
                _carrotGolemController.attackTimer = 0.5f;
                _carrotGolemController.targetPosition = _targetObject.transform.position;
                break;
            default:
                break;
        }
    }

    public void UseGigaImpact() {
        if (gameObject.TryGetComponent(out CharacterController characterController)) {
            if (_carrotGolemController.attackTimer > _carrotGolemController.currentAttackDuration * 0.25f) {
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
            if (_carrotGolemController.attackTimer > _carrotGolemController.currentAttackDuration * 0.05f) {
                characterController.Move(new Vector3(0f, (_carrotGolemController.jumpHeight + _carrotGolemController.gravity) * Time.deltaTime, 0f));
                if (_carrotGolemController.attackTimer <= _carrotGolemController.currentAttackDuration * 0.05f + Time.deltaTime) {
                    _carrotGolemController.gravity = 0f;
                }

            } else if (_carrotGolemController.attackTimer <= _carrotGolemController.currentAttackDuration * 0.05f && !characterController.isGrounded) {
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
        lerpT += curveLength * 0.025f * Time.deltaTime;

        _carrotGolemController.transform.position = CreateLerp(_carrotGolemController.lerpPositions[0], _carrotGolemController.lerpPositions[1],
            _carrotGolemController.lerpPositions[2], _carrotGolemController.lerpPositions[3]);
        
        if ((_carrotGolemController.targetPosition - _carrotGolemController.transform.position).magnitude <= 0.001f) {

            ParticleOverlord.GetInstance.PlayAttackParticle(gameObject, _carrotGolemController.currentAttack);
        
            if (TargetIsHit(gameObject, _carrotGolemController.targetGameObject, _carrotGolemController.characterWidth)) {
                Debug.Log("Target is hit by body slam");
            }
        
            lerpT = 0f;
            _carrotGolemController.lerpPositions.Clear();
            _carrotGolemController.lerpLengthSegments.Clear();
            _carrotGolemController.currentAttack = AttackMoves.None;
            _carrotGolemController.cooldownTimer = _carrotGolemController.attackCooldown;
        }
        IgnoreCollisionWithTarget();
    }

    public void UseWoodHammer() {
        Vector3 distance = _carrotGolemController.targetPosition - _carrotGolemController.transform.position;
        _carrotGolemController.targetPosition = _carrotGolemController.targetGameObject.transform.position;

        if (distance.magnitude <= 5f && !_carrotGolemController.hittingWithWoodHammer) {
            _carrotGolemController.attackTimer = AttackDurations.woodHammerDuration;
            _carrotGolemController.hittingWithWoodHammer = true;

        } else if (_carrotGolemController.hittingWithWoodHammer) {
            if (_carrotGolemController.attackTimer > AttackDurations.woodHammerDuration * 0.5f) {
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
                    _carrotGolemController.hittingWithWoodHammer = false;
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
