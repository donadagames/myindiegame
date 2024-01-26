using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName="SceneData")]
public class SceneData : ScriptableObject
{
    public bool isSafeZone;
    public Vector3 playerPosition = new Vector3();
    public Vector3 playerRotation = new Vector3();
    public AudioClip sceneAudioClip;
    public string builtName;
    public int virtualCameraIndex = 0;
}
