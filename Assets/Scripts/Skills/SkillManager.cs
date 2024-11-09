using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;

    public Dash_skill dash {get;private set;}
    public Clone_skill clone {get;set;}
    public Sword_Skill sword {get;set;}
    public Blackhole_Skill blackhole {get;private set;}
    public Crystal_Skill crystal {get;set;}

    private void Awake() {
        if(instance != null)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start() {
        dash = GetComponent<Dash_skill>();
        clone = GetComponent<Clone_skill>();
        sword = GetComponent<Sword_Skill>();
        blackhole = GetComponent<Blackhole_Skill>();
        crystal = GetComponent<Crystal_Skill>();
    }
}
