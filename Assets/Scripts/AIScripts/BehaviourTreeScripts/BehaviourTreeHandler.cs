using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FluentBehaviourTree;


public class BehaviourTreeHandler
{
    private static BehaviourTreeHandler _instance = new BehaviourTreeHandler();
    public static BehaviourTreeHandler GetInstance { get => _instance; }

    public IBehaviourTreeNode carrotGolemBehaviourTree;

    private AttackHandler _attackHandler = new AttackHandler();
    private CarrotGolemController _carrotGolemController;

    public void CreateCarrotGolemTree(GameObject gameObject, GameObject targetObject) {
        BehaviourTreeBuilder behaviourTreeBuilder = new BehaviourTreeBuilder();
        carrotGolemBehaviourTree = behaviourTreeBuilder
            .Selector("Selector")
                .Selector("InAttackMode")
                    .Sequence("AttackInUse")
                        .Do("IsAttackInUse", t => {
                            if (IsAttackInUseDecorator(gameObject)) {
                                return BehaviourTreeStatus.Success;
                            
                            } else {
                                return BehaviourTreeStatus.Failure;
                            }
                        })
                        .Do("KeepOnAttacking", t => {
                            UseAttackAction(gameObject);
                            return BehaviourTreeStatus.Success;
                        })
                    .End()
                    .Sequence("InRange")
                        .Do("TargetInRange", t => {
                            if (TargetInRangeDecorator(gameObject)) {
                                return BehaviourTreeStatus.Success;
                            } else {
                                return BehaviourTreeStatus.Failure;
                            }
                        })
                        .Sequence("DecideAttack")
                            .Do("ReadyToAttack", t => {
                                if (ReadyToAttack(gameObject)) {
                                    return BehaviourTreeStatus.Success;
                                
                                } else {
                                    return BehaviourTreeStatus.Failure;

                                }
                            })
                            .Do("PickAttack", t => {
                                if(PickAttackAction(gameObject, targetObject)) {
                                    return BehaviourTreeStatus.Success;

                                } else {
                                    return BehaviourTreeStatus.Running;
                                }
                            })
                        .End()
                    .End()
                .End()
                .Selector("WalkToPlayer")
                    .Sequence("TargetIsPlayer")
                        .Do("IsTargetPlayer", t => {
                            if (TargetIsPlayer(gameObject, targetObject)) {
                                return BehaviourTreeStatus.Failure;
                            } else {
                                return BehaviourTreeStatus.Success;
                            }
                        })
                        .Do("TargetPlayer", t => {
                            SetTargetToPlayer(gameObject, targetObject);
                            return BehaviourTreeStatus.Success;
                        })
                    .End()
                    .Sequence("AttackReady")
                        .Do("ReadyToAttackOutsideRamge", t => {
                            if (ReadyToAttack(gameObject)) {
                                return BehaviourTreeStatus.Success;

                            } else {
                                return BehaviourTreeStatus.Failure;

                            }
                        })
                        .Do("WalkTowardsPlayer", t => {
                            WalkTowardsPlayer(gameObject);
                            return BehaviourTreeStatus.Success;
                        })                        
                    .End()

                .End()            
            .End()
        .Build();        
    }

    private bool IsAttackInUseDecorator(GameObject gameObject) {
        if(gameObject.TryGetComponent(out CarrotGolemBehaviour carrotGolemBehaviour)) {
            if (carrotGolemBehaviour.CarrotGolemController.currentAttack != AttackMoves.None) {
                return true;
            }
        }
        return false;
    }

    private bool TargetInRangeDecorator(GameObject gameObject) {
        if (gameObject.TryGetComponent(out CarrotGolemBehaviour carrotGolemBehaviour)) {
            _carrotGolemController = carrotGolemBehaviour.CarrotGolemController;
            Vector3 distance = _carrotGolemController.targetPosition - _carrotGolemController.transform.position;
            
            if(distance.magnitude <= _carrotGolemController.attackRange) {
                return true;

            } else {
                return false;
            }
        }
        return false;
    }

    private bool TargetIsPlayer(GameObject gameObject, GameObject targetObject) {
        if (gameObject.TryGetComponent(out CarrotGolemBehaviour carrotGolemBehaviour)) {
            _carrotGolemController = carrotGolemBehaviour.CarrotGolemController;

            if (_carrotGolemController.targetGameObject.GetInstanceID() == targetObject.GetInstanceID()) {
                return true;

            } else {
                return false;
            }
        }
        return false;
    }

    private void UseAttackAction(GameObject gameObject) {
        if (gameObject.TryGetComponent(out CarrotGolemBehaviour carrotGolemBehaviour)) {
            switch (_carrotGolemController.currentAttack) {
                case AttackMoves.GigaImpact:
                    _attackHandler.UseGigaImpact(gameObject);
                    break;
                case AttackMoves.Earthquake:
                    _attackHandler.UseEarthquake(gameObject);
                    break;
                case AttackMoves.BodySlam:
                    _attackHandler.UseBodySlam(gameObject);
                    break;
                case AttackMoves.WoodHamemr:
                    _attackHandler.UseWoodHammer(gameObject);
                    break;
                default:
                    break;
            }
        }

    }

    private bool ReadyToAttack(GameObject gameObject) {
        if (gameObject.TryGetComponent(out CarrotGolemBehaviour carrotGolemBehaviour)) {
            _carrotGolemController = carrotGolemBehaviour.CarrotGolemController;
            if(_carrotGolemController.cooldownTimer <= 0) {
                return true;

            } else {
                return false;
            }
        }
        return false;
    }

    private bool PickAttackAction(GameObject gameObject, GameObject targetObject) {
        if (gameObject.TryGetComponent(out CarrotGolemBehaviour carrotGolemBehaviour)) {
            _carrotGolemController = carrotGolemBehaviour.CarrotGolemController;

            int attackChoice = Random.Range(0, 4);
            switch (attackChoice) {
                case 0: {
                    if(_carrotGolemController.latestUsedAttackUsed != _carrotGolemController.attackSlotOne &&
                        _carrotGolemController.secondLatestAttackUsed != _carrotGolemController.attackSlotOne) {
                        ChangeAttack(gameObject, targetObject, _carrotGolemController.attackSlotOne);
                        return true;
                    }
                    break;
                }
                case 1: {
                    if (_carrotGolemController.latestUsedAttackUsed != _carrotGolemController.attackSlotTwo &&
                        _carrotGolemController.secondLatestAttackUsed != _carrotGolemController.attackSlotTwo) {
                        ChangeAttack(gameObject, targetObject, _carrotGolemController.attackSlotTwo);
                        return true;
                    }
                    break;
                }
                case 2: {
                    if (_carrotGolemController.latestUsedAttackUsed != _carrotGolemController.attackSlotThree &&
                        _carrotGolemController.secondLatestAttackUsed != _carrotGolemController.attackSlotThree) {
                        ChangeAttack(gameObject, targetObject, _carrotGolemController.attackSlotThree);
                            return true;
                    }
                    break;
                }
                case 3: { 
                    if (_carrotGolemController.latestUsedAttackUsed != _carrotGolemController.attackSlotFour &&
                        _carrotGolemController.secondLatestAttackUsed != _carrotGolemController.attackSlotFour) {
                        ChangeAttack(gameObject, targetObject, _carrotGolemController.attackSlotFour);
                            return true;
                    }
                    break;
                }
                default:
                    break;
            }
        }
        return false;
    }

    private void ChangeAttack(GameObject gameObject, GameObject targetObject, AttackMoves newAttack) {
        if (gameObject.TryGetComponent(out CarrotGolemBehaviour carrotGolemBehaviour)) {
            _carrotGolemController = carrotGolemBehaviour.CarrotGolemController;
            _carrotGolemController.secondLatestAttackUsed = _carrotGolemController.latestUsedAttackUsed;
            _carrotGolemController.latestUsedAttackUsed = newAttack;
            _carrotGolemController.currentAttack = newAttack;
            _carrotGolemController.attackTimer = _carrotGolemController.attackDuration;

            switch (newAttack) {
                case AttackMoves.GigaImpact:
                    //AudioOverlord.GetInstance.PlayerOneShot(gameObject, _carrotGolemController.gigaImpactSound);
                    Debug.Log("Using giga impact");
                    _carrotGolemController.attackTimer = 5f;
                    _carrotGolemController.targetPosition = targetObject.transform.position;
                    break;
                case AttackMoves.Earthquake:
                    AudioOverlord.GetInstance.PlayerOneShot(gameObject, _carrotGolemController.earthquakeSound);
                    _carrotGolemController.targetPosition = _carrotGolemController.transform.position;
                    break;
                case AttackMoves.BodySlam:
                    Debug.Log("Using body slam");
                    //AudioOverlord.GetInstance.PlayerOneShot(gameObject, _carrotGolemController.bodySlamSound);
                    _carrotGolemController.targetPosition = targetObject.transform.position;
                    break;
                case AttackMoves.WoodHamemr:
                    Debug.Log("Using wood hammer");
                    //AudioOverlord.GetInstance.PlayerOneShot(gameObject, _carrotGolemController.woodHammerSound);
                    _carrotGolemController.targetPosition = targetObject.transform.position;
                    break;
                default:
                    break;
            }
            carrotGolemBehaviour.UpdateControllerValues(_carrotGolemController);
        }
    }

    private void SetTargetToPlayer(GameObject gameObject, GameObject targetObject) {
        if (gameObject.TryGetComponent(out CarrotGolemBehaviour carrotGolemBehaviour)) {
            _carrotGolemController = carrotGolemBehaviour.CarrotGolemController;
            _carrotGolemController.targetGameObject = targetObject;
            _carrotGolemController.targetPosition = targetObject.transform.position;
            carrotGolemBehaviour.UpdateControllerValues(_carrotGolemController);
        }
    }

    private void WalkTowardsPlayer(GameObject gameObject) {
        if (gameObject.TryGetComponent(out CarrotGolemBehaviour carrotGolemBehaviour)) {
            _carrotGolemController = carrotGolemBehaviour.CarrotGolemController;
            _carrotGolemController.targetPosition = _carrotGolemController.targetGameObject.transform.position;
            carrotGolemBehaviour.UpdateMovementBehaviour(true, true, ref _carrotGolemController);
            carrotGolemBehaviour.IgnoreCollisionWithTarget();
        }

    }

}