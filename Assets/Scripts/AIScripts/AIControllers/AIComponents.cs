using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct SteeringOutput {
    public Vector3 velocity;
    public Vector3 angular;
};

public struct CarrotGolemController {
    public uint id;

    public Vector3 velocity;
    public Vector3 direction;

    public Vector3 orientation;
    public Vector3 rotation;
    public Vector3 angularSlowRadius;
    public Vector3 maxAngularAcceleration;
    public Vector3 maxRotation;
    public Vector3 targetPosition;

    public Transform transform;
    public GameObject targetGameObject;

    public float maxAcceleration;
    public float maxSpeed;
    public float maxPrediction;
    public float timeToTarget;
    public float targetRadius;

    public float currentSpeed;
    public float distance;
    public float currentPrediction;

    public float attackRange;
    public float attackCooldown;
    public float attackDuration;
    public float attackTimer;
    public float cooldownTimer;
    public float jumpHeight;
    public float gravity;

    public BehaviourAndWeight faceBehaviourAndWeight;
    public BehaviourAndWeight pursueBehaviourAndWeight;

    public AttackMoves currentAttack;
    public AttackMoves latestUsedAttackUsed;
    public AttackMoves secondLatestAttackUsed;

    public AttackMoves attackSlotOne;
    public AttackMoves attackSlotTwo;
    public AttackMoves attackSlotThree;
    public AttackMoves attackSlotFour;
}
