using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AttackDurations {
    public const float gigaImpactDuration = 3f;
    public const float bodySlamDuration = 0.5f;
    public const float earthquakeDuration = 1f;
    public const float woodHammerDuration = 1f;
}

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
            carrotGolemBehaviour.UseGigaImpact();
        }
    }

    public void UseEarthquake(GameObject gameObject) {
        if (gameObject.TryGetComponent(out CarrotGolemBehaviour carrotGolemBehaviour)) {
            carrotGolemBehaviour.UseEarthquake();
        }
    }

    public void UseBodySlam(GameObject gameObject) {
        if (gameObject.TryGetComponent(out CarrotGolemBehaviour carrotGolemBehaviour)) {
            carrotGolemBehaviour.UseBodySlam();
        }
    }

    public void UseWoodHammer(GameObject gameObject) {
        if (gameObject.TryGetComponent(out CarrotGolemBehaviour carrotGolemBehaviour)) {
            carrotGolemBehaviour.UseWoodHammer();
        }
    }
}
