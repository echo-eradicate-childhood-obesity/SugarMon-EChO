using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ARMon
{
    public class Coin : MonoBehaviour, IMoveObj
    {
        private float speed;
        public float Speed { get { return speed; } set { } }
        public int BounsVal { get; set; }

        private GameManager gm;
        //target is ui go
        private GameObject targetGO;
        private Vector3 orgPos;
        private RectTransform thisRect;
        void Awake()
        {
            //hard coded speed;
            speed = 500f;
            thisRect = this.gameObject.GetComponent<RectTransform>();
            orgPos = thisRect.position;
            
            //test bounsval set
            BounsVal = 10;

        }
        public void Movement()
        {
            Debug.Log(speed);
            thisRect.position = Vector3.MoveTowards(thisRect.position,Vector3.zero,speed*Time.deltaTime);
        }

        // Update is called once per frame
        void Update()
        {
            if (thisRect.position.x <= 100&&thisRect.position.y<=100)
            {
                gm.CurrentScore += BounsVal;
                Destroy(this.gameObject);
            }
        }

        private void OnEnable()
        {
            gm = GameManager.Instance;
            GameManager.MoveHandler += this.Movement;
            Debug.Log("enabled");
        }

        private void OnDisable()
        {
            GameManager.MoveHandler -= Movement;
        }
    }
}
