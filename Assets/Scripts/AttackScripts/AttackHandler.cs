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

    public void UseDazzlingGleam() {

    }

    public void UseSynthesis() {

    }
    public void UseExtremeSpeed() {

    }
    public void UseSolarBeam() {

    }
    public void UseGigaImpact(GameObject gameObject) {
        if (gameObject.TryGetComponent(out CarrotGolemBehaviour carrotGolemBehaviour)) {
            _carrotGolemController = carrotGolemBehaviour.CarrotGolemController;
            _carrotGolemController.currentAttack = AttackMoves.None;
            _carrotGolemController.cooldownTimer = _carrotGolemController.attackCooldown;
            carrotGolemBehaviour.CarrotGolemController = _carrotGolemController;
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
                    _carrotGolemController.currentAttack = AttackMoves.None;
                    _carrotGolemController.cooldownTimer = _carrotGolemController.attackCooldown;
                    _carrotGolemController.gravity = 0;

                }
                _carrotGolemController.gravity -= Time.deltaTime;
                carrotGolemBehaviour.CarrotGolemController = _carrotGolemController;
            }
        }
    }
    public void UseBodySlam(GameObject gameObject) {
        if (gameObject.TryGetComponent(out CarrotGolemBehaviour carrotGolemBehaviour)) {
            _carrotGolemController = carrotGolemBehaviour.CarrotGolemController;
            _carrotGolemController.currentAttack = AttackMoves.None;
            _carrotGolemController.cooldownTimer = _carrotGolemController.attackCooldown;
            carrotGolemBehaviour.CarrotGolemController = _carrotGolemController;
        }
    }

    public void UseWoodHammer(GameObject gameObject) {
        if (gameObject.TryGetComponent(out CarrotGolemBehaviour carrotGolemBehaviour)) {
            _carrotGolemController = carrotGolemBehaviour.CarrotGolemController;
            _carrotGolemController.currentAttack = AttackMoves.None;
            _carrotGolemController.cooldownTimer = _carrotGolemController.attackCooldown;
            carrotGolemBehaviour.CarrotGolemController = _carrotGolemController;
        }
    }
}
