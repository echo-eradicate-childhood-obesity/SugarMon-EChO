using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace ARMon
{
    public class Bullet : MonoBehaviour, IMoveObj
    {

        private Vector3 dir;
        private float speed;

        public float Speed { get { return speed; } set { } }
        public GameObject effectGO;
        // Start is called before the first frame update
        void Start()
        {
            speed = 2f;
            Invoke("SelfDestory", 3f);
        }

        // Update is called once per frame
        void Update()
        {
           // Movement();
        }

        //private void OnTriggerEnter(Collider col)
        //{
        //    col.transform.SendMessage("Hitted", col);
        //    CustomController.HitEffect(effectGO,col.transform.position);
        //    SelfDestory();
        //}

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log(collision.transform.gameObject.name);
            collision.transform.SendMessage("Hitted",collision);
            CustomController.HitEffect(effectGO,collision.contacts[0].point);
            SelfDestory();
        }

        public void SetDir(Vector3 dir)
        {
            this.dir = dir;
        }

        public void Movement()
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);
        }

        void SelfDestory()
        {
            Destroy(this.gameObject);
        }

        private void OnDisable()
        {
            GameManager.MoveHandler -= Movement;
        }

        private void OnEnable()
        {
            GameManager.MoveHandler+= this.Movement;
        }
    }
}
