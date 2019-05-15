using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="Spawngrid Config",menuName ="Config/Session Config")]
public class SpawngridConfig : ScriptableObject
{
    public float height;
    public float width ;
    public float length ;
    public float gap ;
    public float radius { get { return gap * 2/*Mathf.Sqrt(2)*/; } }
}
