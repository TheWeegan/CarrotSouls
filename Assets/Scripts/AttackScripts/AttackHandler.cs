using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackMoves {
    DazzlingGleam,
    Synthesis,
    ExtremeSpeed,
    SolarBeam,

    GigaImpact,
    Earthquake,
    BodySlam,
    WoodHamemr,

    None
}

public class AttackHandler {
    private CarrotGolemController _carrotGolemController;
    DebugLineOverlord debugLineOverlord = new DebugLineOverlord();

    private bool TargetIsHit(GameObject gameObject, GameObject targetGameObject, float attackRange) {
        Vector3 distance = targetGameObject.transform.position - gameObject.transform.position;        
        return distance.magnitude < attackRange;
    }

    public void UseDazzlingGleam() {

    }

    public void UseSynthesis() {

    }
    public void UseExtremeSpeed() {

    }
    public void UseSolarBeam() {

    }
    public void UseGigaImpact(GameObject gameObject) {
        if (gameObject.TryGetComponent(out CharacterController characterController)) {
            if (gameObject.TryGetComponent(out CarrotGolemBehaviour carrotGolemBehaviour)) {
                _carrotGolemController = carrotGolemBehaviour.CarrotGolemController;

                if (_carrotGolemController.attackTimer > _carrotGolemController.attackDuration * 0.25f) {
                    _carrotGolemController.targetPosition = _carrotGolemController.targetGameObject.transform.position;
                    carrotGolemBehaviour.UpdateMovementBehaviour(true, false, ref _carrotGolemController);
                    
                    _carrotGolemController.direction = _carrotGolemController.targetPosition - _carrotGolemController.transform.position;
                    _carrotGolemController.direction.y = 0f;
                    _carrotGolemController.direction.Normalize();

                } else if(_carrotGolemController.attackTimer > 0f) {
                    characterController.Move(_carrotGolemController.direction * 50f * Time.deltaTime);
                    if(TargetIsHit(gameObject, _carrotGolemController.targetGameObject, _carrotGolemController.characterWidth) && !_carrotGolemController.hasHitOnce) {
                        ParticleOverlord.GetInstance.PlayAttackParticle(gameObject, _carrotGolemController.currentAttack);
                        _carrotGolemController.hasHitOnce = true;
                    }

                } else {
                    _carrotGolemController.currentAttack = AttackMoves.None;
                    _carrotGolemController.cooldownTimer = _carrotGolemController.attackCooldown;
                    _carrotGolemController.hasHitOnce = false;
                }
                carrotGolemBehaviour.UpdateControllerValues(_carrotGolemController);
                carrotGolemBehaviour.IgnoreCollisionWithTarget();
            }
        }
    }

    public void UseEarthquake(GameObject gameObject) {

        if (gameObject.TryGetComponent(out CharacterController characterController)) {
            if (gameObject.TryGetComponent(out CarrotGolemBehaviour carrotGolemBehaviour)) {
                _carrotGolemController = carrotGolemBehaviour.CarrotGolemController;

                if(_carrotGolemController.attackTimer > _carrotGolemController.attackDuration * 0.05f) {
                    characterController.Move(new Vector3(0f, (_carrotGolemController.jumpHeight + _carrotGolemController.gravity) * Time.deltaTime, 0f));
                    if(_carrotGolemController.attackTimer <= _carrotGolemController.attackDuration * 0.05f + Time.deltaTime) {
                        _carrotGolemController.gravity = 0f;
                    }

                } else if(_carrotGolemController.attackTimer <= _carrotGolemController.attackDuration * 0.05f && !characterController.isGrounded) {
                    debugLineOverlord.DrawRectangle(gameObject, _carrotGolemController.targetPosition, 10f, 10f);
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
                carrotGolemBehaviour.UpdateControllerValues(_carrotGolemController);
                carrotGolemBehaviour.IgnoreCollisionWithTarget();

            }
        }
    }
    public void UseBodySlam(GameObject gameObject) {
        if (gameObject.TryGetComponent(out CarrotGolemBehaviour carrotGolemBehaviour)) {
            _carrotGolemController = carrotGolemBehaviour.CarrotGolemController;


            _carrotGolemController.currentAttack = AttackMoves.None;
            _carrotGolemController.cooldownTimer = _carrotGolemController.attackCooldown;
            carrotGolemBehaviour.UpdateControllerValues(_carrotGolemController);
            carrotGolemBehaviour.IgnoreCollisionWithTarget();
        }
    }

    bool isAttacking = false;
    float woodHammerDuration = 1.0f;
    public void UseWoodHammer(GameObject gameObject) {
        if (gameObject.TryGetComponent(out CarrotGolemBehaviour carrotGolemBehaviour)) {
            _carrotGolemController = carrotGolemBehaviour.CarrotGolemController;

            Vector3 distance = _carrotGolemController.targetPosition - _carrotGolemController.transform.position;
            _carrotGolemController.targetPosition = _carrotGolemController.targetGameObject.transform.position;

            if (distance.magnitude <= 5f && !isAttacking) {
                _carrotGolemController.attackTimer = woodHammerDuration;
                isAttacking = true;

            } else if (isAttacking) {
                if(_carrotGolemController.attackTimer > woodHammerDuration * 0.5f) {
                    carrotGolemBehaviour.transform.Rotate(0f, 180.0f * Time.deltaTime, 0f);
                    _carrotGolemController.orientation = carrotGolemBehaviour.transform.eulerAngles;

                } else {
                    carrotGolemBehaviour.transform.Rotate(0f, -360.0f * Time.deltaTime, 0f);
                    _carrotGolemController.orientation = carrotGolemBehaviour.transform.eulerAngles;
                    carrotGolemBehaviour.UpdateMovementBehaviour(true, true, ref _carrotGolemController);
                    if(_carrotGolemController.attackTimer <= 0) {
                        if(distance.magnitude <= 2f) {
                            Debug.Log("Got hammered by wood");
                        }
                        isAttacking = false;
                        _carrotGolemController.currentAttack = AttackMoves.None;
                        _carrotGolemController.cooldownTimer = _carrotGolemController.attackCooldown;
                    }
                }

            } else {
                carrotGolemBehaviour.UpdateMovementBehaviour(true, true, ref _carrotGolemController);

            }
          
            carrotGolemBehaviour.UpdateControllerValues(_carrotGolemController);
            carrotGolemBehaviour.IgnoreCollisionWithTarget();
        }
    }
}
