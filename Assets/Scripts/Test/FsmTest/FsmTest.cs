using PJW.FSM;
using System;
using UnityEngine;


public class FsmTest : MonoBehaviour
{
    private FsmManager fsmManager;
    private FsmBaseTest baseTest;
    private IFsm<FsmBaseTest> fsm;
    private FsmIdle fsmIdle;
    private FsmStateTest stateTest;
    private void Start()
    {
        fsmManager = new FsmManager();
        baseTest = new FsmBaseTest();
        fsmIdle = new FsmIdle();
        stateTest = new FsmStateTest();
        fsm = fsmManager.CreateFsm<FsmBaseTest>("Test", baseTest, new FsmState<FsmBaseTest>[] { fsmIdle, stateTest });
        fsm.Start(typeof(FsmIdle));
        
        Debug.Log(" the count of baseTest " + fsm.FsmStateCount);
    }

    private void Test()
    {

    }
    private void Update()
    {
        fsmManager.Update(1, Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            fsm.FireEvent(this, 0);
        }
    }
}

public class FsmBaseTest : FsmBase
{
    public override Type OwnerType
    {
        get
        {
            return this.GetType();
        }
    }

    public override int FsmStateCount { get { return 0; } }

    public override bool IsRunning { get { return false; } }

    public override bool IsDestroyed { get { return false; } }

    public override string CurrentStateName { get { return "DefaultState"; } }

    public override float CurrentStateTime { get { return 1; } }

    public override void ShutDown()
    {
        throw new NotImplementedException();
    }

    public override void Update(float elapseSeconds, float realElapseSeconds)
    {
        Debug.Log(" the fsm is fsmbasetest ");
    }
}
public class DefaultState : FsmState<FsmBaseTest>
{
    public string _Name = "DefaultState";
    protected internal override void OnUpdate(IFsm<FsmBaseTest> fsm, float elapseSeconds, float realElapseSeconds)
    {
        Debug.Log(" the default state ");
    }
}
public class FsmStateTest : FsmState<FsmBaseTest>
{
    protected internal override void OnInit(IFsm<FsmBaseTest> fsm)
    {
        Debug.Log(fsm.ToString() + "***********");
    }
    protected internal override void OnUpdate(IFsm<FsmBaseTest> fsm, float elapseSeconds, float realElapseSeconds)
    {
        Debug.Log(" the fsmstatetest ");
    }
}
/// <summary>
/// idle
/// </summary>
public class FsmIdle : FsmState<FsmBaseTest>
{
    protected internal override void OnInit(IFsm<FsmBaseTest> fsm)
    {
        Debug.Log(" the idle oninit ");
        AddListernEvent(0, OnInitIdleEvent);
    }

    private void OnInitIdleEvent(IFsm<FsmBaseTest> fsm, object sender, object userData)
    {
        ChangeState(fsm, typeof(FsmStateTest));
    }

    protected internal override void OnUpdate(IFsm<FsmBaseTest> fsm, float elapseSeconds, float realElapseSeconds)
    {
        Debug.Log(" the idle state ");
    }
    
}