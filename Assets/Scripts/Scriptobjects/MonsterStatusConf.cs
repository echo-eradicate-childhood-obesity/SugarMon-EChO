using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="MonsterStatusConf",menuName ="Config/Monster Config")]
public class MonsterStatusConf:ScriptableObject
{
    //temp set. size of collider. .1f xyz works fine so far
    public Vector3 size;
    //where critical hit collider position info hold
    public List<Vector3> CritColPos;
    //where normal hit collider position info hold
    public List<Vector3> NormColPos;

}
