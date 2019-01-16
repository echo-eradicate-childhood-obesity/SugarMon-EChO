using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController {

    //adjust the ui size, some wild code
	public static void MonsterIMGInst(RectTransform rt)
    {
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = new Vector2(0, 40);
        rt.sizeDelta = new Vector2(122, 150);
    }

}
