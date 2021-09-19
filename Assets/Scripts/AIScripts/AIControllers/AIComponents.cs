using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct SteeringOutput {
    public Vector3 velocity;
    public Vector3 angular;
};

public struct CarrotGolemController {
    public uint id;
    public Transform transform;
    public GameObject targetGameObject;

    #region Variables for steering behaviour
    public Vector3 velocity;
    public Vector3 direction;

    public Vector3 orientation;
    public Vector3 rotation;
    public Vector3 angularSlowRadius;
    public Vector3 maxAngularAcceleration;
    public Vector3 maxRotation;
    public Vector3 targetPosition;

    public float maxAcceleration;
    public float maxSpeed;
    public float maxPrediction;
    public float timeToTarget;
    public float targetRadius;
    public float characterWidth;
    public float characterHeight;

    public float currentSpeed;
    public float distance;
    public float currentPrediction;

    public BehaviourAndWeight faceBehaviourAndWeight;
    public BehaviourAndWeight pursueBehaviourAndWeight;
    #endregion

    #region Variables for attacking
    public float attackRange;
    public float jumpHeight;

    public float attackCooldown;
    public float currentAttackDuration;
    public float attackTimer;
    public float cooldownTimer;
    public float gravity;

    public bool hasHitOnce;
    public bool hittingWithWoodHammer;

    public List<Vector3> lerpPositions;
    public List<Vector3> lerpLengthSegments;

    public AttackMoves currentAttack;
    public AttackMoves latestUsedAttackUsed;
    public AttackMoves secondLatestAttackUsed;

    public AttackMoves attackSlotOne;
    public AttackMoves attackSlotTwo;
    public AttackMoves attackSlotThree;
    public AttackMoves attackSlotFour;

    public AudioClip gigaImpactSound;
    public AudioClip earthquakeSound;
    public AudioClip bodySlamSound;
    public AudioClip woodHammerSound;

    public ParticleSystem gigaImpactParticleSystem;
    public ParticleSystem earthquakeParticleSystem;
    public ParticleSystem bodySlamParticleSystem;
    public ParticleSystem woodHammerParticleSystem;
    #endregion
}
