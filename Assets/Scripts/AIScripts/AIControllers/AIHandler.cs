using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AIHandler  {
    private static AIHandler _instance = new AIHandler();
    public static AIHandler GetInstance { get => _instance; }

    private BlendSteering _blendSteering = new BlendSteering();
    private Dictionary<int, List<BehaviourAndWeight>> _mappedBehaviours = new Dictionary<int, List<BehaviourAndWeight>>();

    public BlendSteering GetBlendSteering {
        get => _blendSteering;
    }

    public Dictionary<int, List<BehaviourAndWeight>> GetMappedBehaviours {
        get => _mappedBehaviours;
    }
}