using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleOverlord
{
    private static ParticleOverlord _instance = new ParticleOverlord();
    public static ParticleOverlord GetInstance { get => _instance; }

    public void PlayAttackParticle(GameObject gameObject, AttackMoves attackMove) {
        if(gameObject.TryGetComponent(out CarrotGolemBehaviour carrotGolemBehaviour)) {
            switch (attackMove) {
                case AttackMoves.DazzlingGleam:
                    break;
                case AttackMoves.Synthesis:
                    break;
                case AttackMoves.ExtremeSpeed:
                    break;
                case AttackMoves.SolarBeam:
                    break;
                case AttackMoves.GigaImpact:
                    carrotGolemBehaviour.CarrotGolemController.gigaImpactParticleSystem.Play();
                    break;
                case AttackMoves.Earthquake:
                    carrotGolemBehaviour.CarrotGolemController.earthquakeParticleSystem.Play();
                    break;
                case AttackMoves.BodySlam:
                    carrotGolemBehaviour.CarrotGolemController.bodySlamParticleSystem.Play();
                    break;
                case AttackMoves.WoodHamemr:
                    carrotGolemBehaviour.CarrotGolemController.woodHammerParticleSystem.Play();
                    break;
                case AttackMoves.None:
                    break;
                default:
                    break;
            }


            
        }


    }



}
