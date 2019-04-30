using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleARCore;
using System.Linq;
using UnityEngine.SceneManagement;
namespace ARMon
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager instance;

        public static GameManager Instance
        {
            get { return instance; }
        }

        public int CurrentScore { get; set; }

        public delegate void Move();


        public GameObject textCamPos;

        public GameObject camPosText;

        public GameObject deviceGO;

        public float time;
        public float shotTimer;
        private bool buttonHit;
        bool shot;

        public GameObject bulletGO;
        public GameObject coin;
        public GameObject monsterGo;
        public GameObject canvas, arCoreDevice, planeDiscovery, backToMainButton;
        [SerializeField]
        private SpawngridConfig sconfig;

        public SpawngridConfig Sconfig { get { return sconfig; } }

        public SpawnspotHandler ssHandler = new SpawnspotHandler();

        //detect radius of player where grid spawn
        public float radius;

        private List<GameObject> monsters;
        public static event Move MoveHandler;


        //test field
        private Vector3 currentRoundPos = Vector3.zero;
        public Vector3 CurrentRoundPos { get { return currentRoundPos; } }

        private Vector3 previousPos;
                
        public Vector3 PreviousPos
        {
            get { return previousPos; }
        }


        //test ui 
        //public List<GameObject> zeroObjText;
        //public GameObject currentObjText;
        //public GameObject deviceObjText;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else { Destroy(this.gameObject); }

            var height = sconfig.height;
            var width = sconfig.width;
            var length = sconfig.length;
            var gap = sconfig.gap;
            radius = sconfig.radius;
            previousPos = Vector3.zero;
            SpwanGridCreater(width, height, length, gap, radius);
            monsters = new List<GameObject>();
            //set currentscore to 0 for test
            CurrentScore = 0;
            
            foreach (SpawnGrid sg in ssHandler.Obersvers)
            {
                sg.SignNeighbor(ssHandler.Obersvers, sconfig.gap);
            }

        }

        //create grid
        //witht the raduis, height/width/length is meanless, specially width & length
        private void SpwanGridCreater(float width, float height, float length, float gap, float r)
        {
            var localHeight = -height;
            while (CustomController.Less(localHeight, height))
            {
                var localLength = -length;
                while (CustomController.Less(localLength, length))
                {
                    var localWidth = -width;
                    while (CustomController.Less(localWidth, width))
                    {
                        var targetPos = new Vector3(localWidth, localHeight, localLength);
                        if (CustomController.InRange(deviceGO.transform.position, targetPos, r))
                        {
                            var newGrid = new SpawnGrid(targetPos);
                            ssHandler.Subscribe(newGrid);
                        }
                        localWidth += gap;
                    }
                    localLength += gap;
                }
                localHeight += gap;
            }
        }

        private void Start()
        {
            time = 0.3f;
            shotTimer = 0f;
            shot = false;

        }
        private void Update()
        {
            Shoot(bulletGO);
        }

        
        private void LateUpdate()
        {
            MoveHandler?.Invoke();
            //current scene score
            textCamPos.GetComponent<Text>().text = CurrentScore.ToString();
            
            List<Direction> moveDir = CustomController.PositionCompair( previousPos,currentRoundPos, sconfig.gap);
            foreach (Direction s in moveDir)
            {
                previousPos = currentRoundPos;
                ObserverUpate(s);
            }
        }

        public void Test()
        {
            monsters.Clear();
        }
        private void FixedUpdate()
        {
            if (!canvas.activeSelf && monsters.Count <= 0)
            {
                backToMainButton.gameObject.SetActive(true);
            }
        }

        //this is use for the touch/mouse event
        


        private void Shoot(GameObject bulletGO)
        {
            FireEvent();
            if (shot&&buttonHit)
            {
                shotTimer += Time.deltaTime;
            }
            else { shotTimer = 0f; }
            if (shotTimer >= time)
            {
                var bul = Instantiate(bulletGO, deviceGO.transform.position, Quaternion.identity);
                bul.SendMessage("SetDir", deviceGO.transform.forward);
                shotTimer = 0f;
            }
        }
        private void FireEvent()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                shot = true;
                #region this part mainly for test
                //new mechanic do not relay on spanw andy and hit andy to create monster, so comment this
                //only hit treasure layer
                //LayerMask layermask = 1 << 12 | 1 << 10;
                //var touchPos = new Vector2(0.2f,0.2f) ;
                //Ray ray = new Ray(deviceGO.transform.position, Camera.main.transform.forward);
                Debug.DrawRay(deviceGO.transform.position, Camera.main.transform.forward, Color.red, 10f);
                //textCamPos.GetComponent<Text>().text = "I shoot somthing";
                //RaycastHit hit;
                //if (Physics.Raycast(ray, out hit, Mathf.Infinity, layermask))
                //{
                //    if (hit.transform.gameObject.layer == 12)
                //    {
                //        Instantiate(treasureBox, hit.point, Quaternion.identity);

                //    }
                //    if (hit.transform.gameObject.layer == 10)
                //    {
                //        Destroy(hit.transform.gameObject);
                //        //camPosText.GetComponent<Text>().text = "Hit treasure";
                //    }
                //} 
                #endregion
            }
            else if (Input.GetButtonUp("Fire1"))
            {
                buttonHit = false;
                shot = false;
            }
        }
        //this used for the fire button
        public void TriggerFire()
        {
            buttonHit = true;
        }


        
        public void Summon(string s)
        {
            //GameObject deviceGO = GameManager.Instance.deviceGO;
            //get a random pos from gird;
            try
            {
                SpawnGrid sg = GetGrid(ssHandler.Obersvers);
                Vector3 pos = sg.GetPos();
                Vector3 offsetPos = deviceGO.transform.position;
                //instantiate and set the value of bullet obj
                var spawnGO = Instantiate(monsterGo, pos, Quaternion.identity);
                spawnGO.transform.localScale = new Vector3(1, 1, 1);
                spawnGO.SendMessage("OccupyGrid", sg);
                monsters.Add(spawnGO);
                
            }
            catch (System.Exception e)
            {
                Debug.Log(e.StackTrace);
            }
            
        }

        
        private SpawnGrid GetGrid(List<IObersver> list)
        {
            SpawnGrid output;
            List<IObersver> coll = list.Where(l => !l.IsOccupy).ToList();
            output = (SpawnGrid)coll[Random.Range(0, coll.Count())];
            output.IsOccupy = true;
            return output;
        }

        public void ObserverUpate(Direction dir)
        {
            ssHandler.Notfiy(currentRoundPos, dir, sconfig);
        }

        
        public void ChangeMode()
        {
            bool canvasStatus = canvas.activeSelf;
            canvas.gameObject.SetActive(!canvasStatus);
            arCoreDevice.gameObject.SetActive(canvasStatus);
            planeDiscovery.gameObject.SetActive(canvasStatus);
        }
            

        public void LoadTreeScene()
        {
            CustomController.SetCoin(CurrentScore);
            Debug.Log(CurrentScore);
            Debug.Log(CustomController.GetScore());
            SceneManager.LoadScene("Tree_Avatar");
        }

        public void MonsterDie(GameObject go)
        {
            monsters.Remove(go);
        }
    }
}