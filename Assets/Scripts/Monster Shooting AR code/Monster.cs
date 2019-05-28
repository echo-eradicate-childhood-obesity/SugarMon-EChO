using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ARMon {
    [ExecuteInEditMode]
    public class Monster : MonoBehaviour
    {

        SpawnGrid sg;
        [SerializeField]
        List<Collider> critialCol = new List<Collider>();
        [SerializeField]
        List<Collider> normalCol = new List<Collider>();

        //hit value here
        int critCount;
        int normCount;
        public int BoundsVal { get { return (critCount * 10 + normCount * 5); } }

        //when hit, start countdown;
        bool hitted;
        bool crRunning;

        public MonsterStatusConf msc;

        GameManager gm;

        //test field
        public GameObject norm;
        public GameObject crit;
        void Hitted(Collision col)
        {
            if (!hitted)
            {
                hitted = true;
            }
            Debug.LogFormat("{0} got hit", this.transform.gameObject.name);
            try
            {
                ReleaseGrid();
            }
            finally { Debug.Log("no gs occupied"); }

            if (critialCol.Contains(col.collider))
            {
                ++critCount;
            }
            else if (normalCol.Contains(col.collider))
            {
                ++normCount;
            }
            else { return; }
            
        }

        void ReleaseGrid()
        {
            if (sg != null)
            {
                sg.IsOccupy = false;
            }
        }

        void OccupyGrid(SpawnGrid sg)
        {
            this.sg = sg;
        }

        private void Start()
        {
            foreach (Vector3 v3 in msc.CritColPos)
            {
                var col = transform.gameObject.AddComponent<BoxCollider>();
                col.size = msc.size;
                col.center = v3;
                critialCol.Add(col);
            }
            foreach (Vector3 v3 in msc.NormColPos)
            {
                var col = transform.gameObject.AddComponent<BoxCollider>();
                col.size = msc.size;
                col.center = v3;
                normalCol.Add(col);
            }
            critCount = 0;
            normCount = 0;
            hitted = false;
            gm = GameManager.Instance;
        }

        private void Update()
        {
            if (hitted)
            {
                StartCoroutine(Die());
            }

        }


        private void LateUpdate()
        {
            TestFollowSG();
        }
        IEnumerator Die()
        {
            if (!crRunning)
            {
                crRunning = true;
                //1s seems to fast, change it to 2
                yield return new WaitForSeconds(2);

                //todo currently, send the value to currentscore directly
                //will send this info to coin(?), then when coin touches player, val increase.
                //GameManager.Instance.CurrentScore += BoundsVal;
                var canv = GameObject.Find("Canv");
                CustomController.SpawnCoin(gm.coin, canv,this.gameObject, BoundsVal);
                gm.MonsterDie(this.gameObject);
                Destroy(this.gameObject);
            }
            else { yield return null; }
        }

        void TestFollowSG()
        {
            transform.position = sg.GetPos();
        }
    }
}
