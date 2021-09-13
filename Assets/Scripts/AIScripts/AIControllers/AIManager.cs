using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AIManager  {
    private static AIManager _instance = new AIManager();
    private BlendSteering _blendSteering = new BlendSteering();
    private Dictionary<uint, List<BehaviourAndWeight>> _mappedBehaviours = new Dictionary<uint, List<BehaviourAndWeight>>();
    private uint _id = 0;

    public static AIManager GetInstance {
        get => _instance;
    }

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