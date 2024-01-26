using UnityEngine;

public class LevelUpWings : MonoBehaviour
{

    [SerializeField] Transform leftWing;
    [SerializeField] Transform rightWing;


    private void OnEnable()
    {
        leftWing.LeanRotateAroundLocal(Vector3.up, 30, .5f).setLoopPingPong();
        rightWing.LeanRotateAroundLocal(Vector3.up, 30, .5f).setLoopPingPong();
    }

    private void OnDisable()
    {
        leftWing.localEulerAngles = Vector3.zero;
        rightWing.localEulerAngles = Vector3.zero;

        LeanTween.cancelAll(false);
    }
}
