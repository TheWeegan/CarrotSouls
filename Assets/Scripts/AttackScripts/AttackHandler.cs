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
        if (gameObject.TryGetComponent(out CarrotGolemBehaviour carrotGolemBehaviour)) {
            carrotGolemBehaviour.UseGigaImpact();
        }
    }

    public void UseEarthquake(GameObject gameObject) {
        if (gameObject.TryGetComponent(out CarrotGolemBehaviour carrotGolemBehaviour)) {
            carrotGolemBehaviour.UseEarthquake();
        }
    }

    float t = 0f;
    public void UseBodySlam(GameObject gameObject) {
        if (gameObject.TryGetComponent(out CarrotGolemBehaviour carrotGolemBehaviour)) {
            carrotGolemBehaviour.UseBodySlam();
        }
    }

    bool isAttacking = false;
    float woodHammerDuration = 1.0f;
    public void UseWoodHammer(GameObject gameObject) {
        if (gameObject.TryGetComponent(out CarrotGolemBehaviour carrotGolemBehaviour)) {
            carrotGolemBehaviour.UseWoodHammer();
        }
    }
}
