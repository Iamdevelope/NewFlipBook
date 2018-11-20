using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptTableTest : ScriptableObject {

    // 基本信息
    public string name; // 技能名称
    public string description; // 技能描述
    public AnimationClip animation;
    public Sprite icon; // 技能图标
                        // 伤害
    public float range = 2; // 攻击范围
    public float damage = 50; // 伤害值
    public LayerMask target; // 攻击对象layer
    public float damageDelay = 0.2f;// 计算伤害值时间延迟
                                    // 效果
    public GameObject startEffect; // 技能开始效果
    public Vector3 startEffectOffset; // 开始技能位置偏移
    public GameObject hitEffect; // 技能打击效果
    public float hitDelay = 0.5f; // 技能打击效果时间延迟
                                  // 冷却
    public int cd = 10;// 冷却时间
    public bool coding = false; // 是否处于冷却状态
}
