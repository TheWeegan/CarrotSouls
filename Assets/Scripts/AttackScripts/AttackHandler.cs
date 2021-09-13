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
    public void UseDazzlingGleam() {

    }

    public void UseSynthesis() {

    }
    public void UseExtremeSpeed() {

    }
    public void UseSolarBeam() {

    }
    public void UseGigaImpact() {

    }
    public void UseEarthquake(GameObject gameObject) {

        if (gameObject.TryGetComponent(out CharacterController characterController)) {
            if (gameObject.TryGetComponent(out CarrotGolemController carrotGolemController)) {
                
                characterController.Move(new Vector3(0f, carrotGolemController.jumpHeight * Time.deltaTime, 0f));
                
                //characterController.Move(new Vector3(0f, -carrotGolemController.jumpHeight * Time.deltaTime, 0f));


            }
        }
    }
    public void UseBodySlam() {

    }
    public void UseWoodHammer() {

    }
}
