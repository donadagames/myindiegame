using Unity.VisualScripting;

public interface IState
{
    void Tick();
    void OnEnter();
    void OnExit();
}

public enum Animation
{
    Idle_Unarmed, Run_Unarmed, Land_Unarmed, 
    JumpInPlace_Unarmed, DoubleJump_Unarmed,
    Falling_Unarmed,
    Idle_Sword, Run_Sword, JumpInPlace_Sword, JumpRuning_Sword
}
