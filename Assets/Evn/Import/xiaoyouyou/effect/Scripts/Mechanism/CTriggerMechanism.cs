using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
public class CTriggerMechanism : CBaseMechanism {

    public CBaseMechanism m_caMechanism;

    void Awake()
    {
        m_type = 4;
    }

    public override void Init(string[] args)
    {
        base.Init(args);
    }

    protected override bool OnTrigger(Collider other, CBaseMechanism mechanism)
    {
        if (!base.OnTrigger(other, mechanism))
            return false;
        if (!m_caMechanism.TriggerOther(m_args))
        {
            this.enabled = false;
            return false;
        }

        return true;
    }
}
