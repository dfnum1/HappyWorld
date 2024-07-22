using UnityEngine;

[System.Serializable]
public struct TInfo
{
    public Vector3 LocalPostion;
    public Vector3 LocalRotation;
    public Vector3 LocalScale;
}

public class TransformInfo : MonoBehaviour
{
    public TInfo[] infos;
}