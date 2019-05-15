using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDragable {
    float Distance { get;  }
    Vector3 Pos { get;  }
    void OnMouseDrag();
    void OnMouseUp();
}
