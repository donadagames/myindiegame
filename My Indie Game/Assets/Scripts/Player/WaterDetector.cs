using UnityEngine;

public class WaterDetector : MonoBehaviour
{
    [SerializeField] LayerMask waterLayer;

    public bool isRunningOnWater => Physics.CheckSphere(transform.position, .1f, waterLayer);
}
