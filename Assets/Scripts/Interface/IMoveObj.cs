using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveObj
{
    float Speed { get; set; }
    void Movement();
    //void Hit(Collider col);
}
