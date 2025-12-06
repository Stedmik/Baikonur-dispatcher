using UnityEngine;
using Unity.Cinemachine;

public class Cinemachine_tranzition : MonoBehaviour
{
    public CinemachineCamera cinCamera;
    public Transform target;

    public void ChangeTarget()
    {
        cinCamera.Target.TrackingTarget = target;
    }

}
