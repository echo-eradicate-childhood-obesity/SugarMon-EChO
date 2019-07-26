using UnityEngine;

public interface IButtonAction  {

    //void ClickEventBoolean();


    void ClickEventTrigger();

    void DeselectAction();

    void Action(GameObject go);
    
}
