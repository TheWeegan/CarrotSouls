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
                                if(PickAttackAction(gameObject)) {
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
            Vector3 distance = carrotGolemBehaviour.CarrotGolemController.targetPosition - carrotGolemBehaviour.CarrotGolemController.transform.position;
            
            if(distance.magnitude <= carrotGolemBehaviour.CarrotGolemController.attackRange) {
                return true;

            } else {
                return false;
            }
        }
        return false;
    }

    private bool TargetIsPlayer(GameObject gameObject, GameObject targetObject) {
        if (gameObject.TryGetComponent(out CarrotGolemBehaviour carrotGolemBehaviour)) {
            if (carrotGolemBehaviour.CarrotGolemController.targetGameObject.GetInstanceID() == targetObject.GetInstanceID()) {
                return true;

            } else {
                return false;
            }
        }
        return false;
    }

    private void UseAttackAction(GameObject gameObject) {
        if (gameObject.TryGetComponent(out CarrotGolemBehaviour carrotGolemBehaviour)) {
            switch (carrotGolemBehaviour.CarrotGolemController.currentAttack) {
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
            carrotGolemBehaviour.CarrotGolemController = carrotGolemBehaviour.CarrotGolemController;
            if(carrotGolemBehaviour.CarrotGolemController.cooldownTimer <= 0) {
                return true;

            } else {
                return false;
            }
        }
        return false;
    }

    private bool PickAttackAction(GameObject gameObject) {
        if (gameObject.TryGetComponent(out CarrotGolemBehaviour carrotGolemBehaviour)) {
            return carrotGolemBehaviour.PickAttack();
        }
        return false;
    }

    private void SetTargetToPlayer(GameObject gameObject, GameObject targetObject) {
        if (gameObject.TryGetComponent(out CarrotGolemBehaviour carrotGolemBehaviour)) {
            carrotGolemBehaviour.SetTargetPlayer();
        }
    }

    private void WalkTowardsPlayer(GameObject gameObject) {
        if (gameObject.TryGetComponent(out CarrotGolemBehaviour carrotGolemBehaviour)) {
            carrotGolemBehaviour.SetTargetPlayer();
            carrotGolemBehaviour.UpdateMovementBehaviour(true, true);
            carrotGolemBehaviour.IgnoreCollisionWithTarget();
        }

    }

}