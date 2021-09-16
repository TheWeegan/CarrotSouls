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
                        Debug.Log("Target is hit by giga impact");
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

    float t = 0f;
    public void UseBodySlam(GameObject gameObject) {
        if (gameObject.TryGetComponent(out CarrotGolemBehaviour carrotGolemBehaviour)) {
            _carrotGolemController = carrotGolemBehaviour.CarrotGolemController;

            float curveLength = 0f;
            for (int i = 0; i < _carrotGolemController.lerpLengthSegments.Count - 1; ++i) {
                curveLength += (_carrotGolemController.lerpLengthSegments[i + 1] - _carrotGolemController.lerpLengthSegments[i]).magnitude;
            }
            t += curveLength * 0.025f * Time.deltaTime;
            
            Vector3 _a = Vector3.Lerp(_carrotGolemController.lerpPositions[0], _carrotGolemController.lerpPositions[1], t);
            Vector3 _b = Vector3.Lerp(_carrotGolemController.lerpPositions[1], _carrotGolemController.lerpPositions[2], t);
            Vector3 _c = Vector3.Lerp(_carrotGolemController.lerpPositions[2], _carrotGolemController.lerpPositions[3], t);
            Vector3 _d = Vector3.Lerp(_a, _b, t);
            Vector3 _e = Vector3.Lerp(_b, _c, t);
            _carrotGolemController.transform.position = Vector3.Lerp(_d, _e, t);

            if ((_carrotGolemController.targetPosition - _carrotGolemController.transform.position).magnitude <= 0.001f) {
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
                carrotGolemBehaviour.UpdateMovementBehaviour(true, true, ref _carrotGolemController);

            }
          
            carrotGolemBehaviour.UpdateControllerValues(_carrotGolemController);
            carrotGolemBehaviour.IgnoreCollisionWithTarget();
        }
    }
}
