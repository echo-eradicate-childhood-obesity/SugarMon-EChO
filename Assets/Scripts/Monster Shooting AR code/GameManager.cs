/*
 * This file has been editted by Mark Botaish  
 */

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
        [System.Serializable]
        public struct AttackTypes {
            public string name;
            public Sprite sprite;
        }
        private static GameManager instance;

        public static GameManager Instance
        {
            get { return instance; }
        }

        public int CurrentScore { get; set; }

        public delegate void Move();

        public GameObject camera;
        
        public float time;
        public float shotTimer;
        private bool buttonHit;
        bool shot;

        public GameObject backToMainButton;

        public SpawnspotHandler ssHandler = new SpawnspotHandler();

        [Header("Attack Settings ----------------------------------")]
        public GameObject _sparks;
        public GameObject projectile;
        public GameObject _rapidFireBullet;
        public float _rapidSpeed;
        public List<AttackTypes> _attacks;
        private int _currentAttack = 0;

        private List<GameObject> monsters;
        public static event Move MoveHandler;
        //-------------------------------------------------------------


        [Header("Monster Spawn Settings ----------------------------------")]
        public List<GameObject> monsterPrefabs;
        public float minSpawnDistance = 2.0f;
        public float maxSpawnDistance = 5.0f;

        //Varaibles get set in the GameManagerEditor Assets/Editor/GameManagerEditor
        [SerializeField, HideInInspector] public float commonDropRate;
        [SerializeField, HideInInspector] public float uncommonDropRate;
        [SerializeField, HideInInspector] public float rareDropRate;

        //-------------------------------------------------------------


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

            previousPos = Vector3.zero;
            monsters = new List<GameObject>();
            //set currentscore to 0 for test
            CurrentScore = 0;
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
                        if (CustomController.InRange(camera.transform.position, targetPos, r))
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
            Shoot();
        }

        
        private void LateUpdate()
        {
            MoveHandler?.Invoke();
        }

        public void Test()
        {
            monsters.Clear();
        }    

        private void Shoot()
        {
            FireEvent();
            if (shot&&buttonHit)
            {
                shotTimer += Time.deltaTime;
            }
            else { shotTimer = 0f; }
            switch (_currentAttack)
            {
                case 0:
                    ShootProjectile();
                    break;
                case 1:
                    ShootLaser();     
                    break;
                case 2:
                    ShootRapidFire();
                    break;
                default:
                    break;
            }

            
        }

        void ShootRapidFire()
        {
            if (buttonHit)
            {
                GameObject temp = Instantiate(_rapidFireBullet, camera.transform.position - camera.transform.up * 0.05f, Quaternion.identity);
                temp.GetComponent<Rigidbody>().velocity = camera.transform.forward * 5f;
                Destroy(temp, 3);
            }
        }

        void ShootProjectile()
        {
            if (shotTimer >= time)
            {


                GameObject temp = Instantiate(projectile, camera.transform.position, camera.transform.rotation);
                temp.GetComponent<ProjectileScript>().SetProjectile(camera.transform.forward);

                //var bul = Instantiate(bulletGO, deviceGO.transform.position, Quaternion.identity);
                //bul.SendMessage("SetDir", deviceGO.transform.forward);
                shotTimer = 0f;
            }
        }

        void ShootLaser()
        {
           

            if (buttonHit)
            {
                RaycastHit hit;
                // Does the ray intersect any objects excluding the player layer
                if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, Mathf.Infinity))
                {
                    if (!_sparks.activeSelf)
                        _sparks.SetActive(true);
                    _sparks.transform.position = hit.point;
                    camera.GetComponent<LineRenderer>().SetPosition(0, camera.transform.position - Vector3.up*0.2f);
                    camera.GetComponent<LineRenderer>().SetPosition(1, hit.point);
                }
                else
                {
                    if (_sparks.activeSelf)
                        _sparks.SetActive(false);
                    camera.GetComponent<LineRenderer>().SetPosition(0, camera.transform.position - Vector3.up * 0.2f);
                    camera.GetComponent<LineRenderer>().SetPosition(1, camera.transform.position + camera.transform.forward * 100);
                }
                camera.GetComponent<LineRenderer>().enabled = true;
            }
            else
            {
                if (_sparks.activeSelf)
                    _sparks.SetActive(false);
                camera.GetComponent<LineRenderer>().SetPosition(0, camera.transform.position);
                camera.GetComponent<LineRenderer>().SetPosition(1, camera.transform.position);
                camera.GetComponent<LineRenderer>().enabled = false;
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
                Debug.DrawRay(camera.transform.position, Camera.main.transform.forward, Color.red, 10f);
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
            try
            {
                if(monsterPrefabs.Count > 0)
                {
                    //Random position
                    float x = Random.Range(-1.0f, 1.0f);
                    float y = Random.Range(-1.0f, 1.0f);
                    float z = Random.Range(-1.0f, 1.0f);

                    //Random distance
                    float dis = Random.Range(minSpawnDistance, maxSpawnDistance);

                    //Random rarity 
                    float rarity = Random.Range(0.0f, 100f);

                    //Random monster
                    int index = Random.Range(0, monsterPrefabs.Count);

                    Color col = Color.white ;

                    //Which rarity should spawn 
                    if (rarity <= commonDropRate) col = Color.white;
                    else if (rarity <= commonDropRate + uncommonDropRate) col = Color.blue;
                    else col = Color.yellow;

                    //Get the position around the player
                    Vector3 pos = new Vector3(x, y, z).normalized * dis + camera.transform.position;

                    //Get the monster
                    GameObject monster = Instantiate(monsterPrefabs[index], pos, Quaternion.identity);

                    //If the color is not white (a rarity other than common), change the color
                    if(col != Color.white)
                    {
                        MeshRenderer[] matRends = monster.GetComponentsInChildren<MeshRenderer>();

                        foreach (MeshRenderer matRend in matRends)
                        {
                            foreach (Material mat in matRend.materials)
                            {
                                mat.color = col;
                            }
                        }
                    }                   
                    
                    //Monster looking at the player
                    monster.transform.LookAt(camera.transform.position);
                    monsters.Add(monster);
                }
                else
                {
                    Debug.LogError("NO MONSTERS IN LIST TO SPAWN");
                }        
                
                                                             
            }
            catch (System.Exception e)
            {
                Debug.Log(e.StackTrace);
            }
            
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

        public void ChangeWeapon(Image image)
        {
            _currentAttack = (_currentAttack + 1) % _attacks.Count;
            image.sprite = _attacks[_currentAttack].sprite;
        }

        public void ReturnToMainMenu()
        {
            SceneManager.LoadScene("Menu");
        }
    }
}