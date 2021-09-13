using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FluentBehaviourTree;


public class BehaviourTreeHandler : MonoBehaviour
{
    public IBehaviourTreeNode carrotGolemBehaviourTree;

    [SerializeField] GameObject _carrotChan;
    [SerializeField] GameObject _carrotGolem;

    private CarrotGolemController _carrotGolemController;

    private AttackHandler _attackHandler = new AttackHandler();

    public void CreateCarrotGolemTree() {
        BehaviourTreeBuilder behaviourTreeBuilder = new BehaviourTreeBuilder();
        carrotGolemBehaviourTree = behaviourTreeBuilder
            .Selector("Selector")
                .Selector("InAttackMode")
                    .Sequence("AttackInUse")
                        .Do("IsAttackInUse", t => {
                            if (IsAttackInUseDecorator()) {
                                return BehaviourTreeStatus.Success;

                            } else {
                                return BehaviourTreeStatus.Failure;
                            }
                        })
                        .Do("KeepOnAttacking", t => {
                            UseAttackAction();
                            return BehaviourTreeStatus.Success;
                        })
                    .End()
                    .Sequence("DecideAttack")
                        .Do("TargetInRange", t => {
                            if (TargetInRangeDecorator()) {
                                return BehaviourTreeStatus.Success;
                            } else {
                                return BehaviourTreeStatus.Failure;
                            }
                        })
                        .Do("PickAttack", t => {
                            PickAttackAction();
                            return BehaviourTreeStatus.Success;
                        })
                    .End()
                .End()
                .Selector("WalkToPlayer")
                    .Sequence("TargetIsPlayer")
                        .Do("IsTargetPlayer", t => {
                            if (TargetIsPlayer()) {
                                return BehaviourTreeStatus.Failure;
                            } else {
                                return BehaviourTreeStatus.Success;
                            }
                        })
                        .Do("TargetPlayer", t => {
                            SetTargetToPlayer();
                            return BehaviourTreeStatus.Success;
                        })
                    .End()
                    .Do("WalkTowardsPlayer", t => {
                        WalkTowardsPlayer();
                        return BehaviourTreeStatus.Success;
                    })
                .End()            
            .End()
        .Build();        
    }

    private bool IsAttackInUseDecorator() {
        if(_carrotGolem.TryGetComponent(out CarrotGolemBehaviour c)) {
            if(c.CarrotGolemController.currentAttack != AttackMoves.None) {
                return true;
            }
        }
        return false;
    }

    private bool TargetInRangeDecorator() {
        _carrotGolemController = _carrotGolem.GetComponent<CarrotGolemBehaviour>().CarrotGolemController;
        Vector3 distance = _carrotGolemController.targetPosition - _carrotGolemController.transform.position;
        
        if(distance.magnitude <= _carrotGolemController.attackRange) {
            return true;

        } else {
            return false;
        }
    }

    private bool TargetIsPlayer() {
        _carrotGolemController = _carrotGolem.GetComponent<CarrotGolemBehaviour>().CarrotGolemController;
        if (_carrotGolemController.targetGameobject.GetInstanceID() == _carrotChan.GetInstanceID()) {
            return true;

        } else {
            return false;
        }
    }

    private void UseAttackAction() {
        _carrotGolemController = _carrotGolem.GetComponent<CarrotGolemBehaviour>().CarrotGolemController;

        switch (_carrotGolemController.currentAttack) {
            case AttackMoves.GigaImpact:
                _attackHandler.UseGigaImpact();
                break;
            case AttackMoves.Earthquake:
                _attackHandler.UseEarthquake(_carrotGolem);
                break;
            case AttackMoves.BodySlam:
                _attackHandler.UseBodySlam();
                break;
            case AttackMoves.WoodHamemr:
                _attackHandler.UseWoodHammer();
                break;
            default:
                break;
        }

    }

    private void PickAttackAction() {
        _carrotGolemController = _carrotGolem.GetComponent<CarrotGolemBehaviour>().CarrotGolemController;

        while (true) {
            int attackChoice = Random.Range(0, 4);
            switch (attackChoice) {
                case 0: {
                    if(_carrotGolemController.latestUsedAttackUsed != _carrotGolemController.attackSlotOne &&
                        _carrotGolemController.secondLatestAttackUsed != _carrotGolemController.attackSlotOne) {
                        ChangeAttack(_carrotGolemController.attackSlotOne);
                        return;
                    }
                    break;
                }

                case 1: {
                    if (_carrotGolemController.latestUsedAttackUsed != _carrotGolemController.attackSlotTwo &&
                        _carrotGolemController.secondLatestAttackUsed != _carrotGolemController.attackSlotTwo) {
                        ChangeAttack(_carrotGolemController.attackSlotTwo);
                        return;
                    }
                    break;
                }
                case 2: {
                    if (_carrotGolemController.latestUsedAttackUsed != _carrotGolemController.attackSlotThree &&
                        _carrotGolemController.secondLatestAttackUsed != _carrotGolemController.attackSlotThree) {
                        ChangeAttack(_carrotGolemController.attackSlotThree);
                        return;
                    }
                    break;
                }
                case 3: { 
                    if (_carrotGolemController.latestUsedAttackUsed != _carrotGolemController.attackSlotFour &&
                        _carrotGolemController.secondLatestAttackUsed != _carrotGolemController.attackSlotFour) {
                        ChangeAttack(_carrotGolemController.attackSlotFour);
                        return;
                    }
                    break;
                }
                default:
                    ChangeAttack(AttackMoves.None);
                    break;
            }
        }
    }

    private void ChangeAttack(AttackMoves newAttack) {
        _carrotGolemController.secondLatestAttackUsed = _carrotGolemController.latestUsedAttackUsed;
        _carrotGolemController.latestUsedAttackUsed = newAttack;
        _carrotGolemController.currentAttack = newAttack;

        switch (newAttack) {
            case AttackMoves.GigaImpact:
                _carrotGolemController.targetPosition = _carrotChan.transform.position;
                break;
            case AttackMoves.Earthquake:
                _carrotGolemController.targetPosition = _carrotGolemController.transform.position;
                break;
            case AttackMoves.BodySlam:
                _carrotGolemController.targetPosition = _carrotChan.transform.position;
                break;
            case AttackMoves.WoodHamemr:
                _carrotGolemController.targetPosition = _carrotChan.transform.position;
                break;
            default:
                break;
        }
    }

    private void SetTargetToPlayer() {
        _carrotGolemController = _carrotGolem.GetComponent<CarrotGolemBehaviour>().CarrotGolemController;
        _carrotGolemController.targetGameobject = _carrotChan;
    }

    private void WalkTowardsPlayer() {


    }

}