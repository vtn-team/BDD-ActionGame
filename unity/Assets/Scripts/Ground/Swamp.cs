using UnityEngine;

public class Swamp : GroundBase
{
    [SerializeField] private float _inhibitionTime = 2f;
    
    public float InhibitionTime => _inhibitionTime;
}