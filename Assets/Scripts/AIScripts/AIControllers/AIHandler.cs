using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AIHandler  {
    private static AIHandler _instance = new AIHandler();
    public static AIHandler GetInstance { get => _instance; }

    private BlendSteering _blendSteering = new BlendSteering();
    private Dictionary<uint, List<BehaviourAndWeight>> _mappedBehaviours = new Dictionary<uint, List<BehaviourAndWeight>>();
    private uint _id = 0;


    public BlendSteering GetBlendSteering {
        get => _blendSteering;
    }

    public Dictionary<uint, List<BehaviourAndWeight>> GetMappedBehaviours {
        get => _mappedBehaviours;
    }

    public uint GetID { 
        get => _id;
        set { 
            _id = value; 
        }
        
    }

}