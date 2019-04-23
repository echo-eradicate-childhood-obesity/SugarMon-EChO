using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ARMon
{

    public enum Direction {
        Front,
        Back,
        Left,
        Right,
        Up,
        Down
    }

    public class CustomController
    {
        public static List<Direction> PositionCompair(Vector3 original, Vector3 current, float offset)
        {
            Direction front = Direction.Front, back = Direction.Back, left = Direction.Left, right = Direction.Right, up = Direction.Up, down = Direction.Down;
            List<Direction> output = new List<Direction>();
            if ((Mathf.Abs(current.x - original.x) >= offset))
            {
                output.Add(PairSelect(current.x, original.x, right, left));
            }
            if ((Mathf.Abs(current.y - original.y) >= offset))
            {
                output.Add(PairSelect(current.y, original.y, up, down));
            }
            if ((Mathf.Abs(current.z - original.z) >= offset))
            {
                output.Add(PairSelect(current.z, original.z, front, back));
            }
            return output;
        }

        internal static void HitEffect(GameObject go,Vector3 position)
        {
            UnityEngine.Object.Instantiate(go,position,Quaternion.identity);
        }

        static t PairSelect<t>(float f1, float f2, t d1, t d2)
        {
            if (f1 > f2)
            {
                return d1;
            }
            else return d2;
        }

        public static Vector3 RoundToClose(Vector3 inputPos, float gap)
        {
            
            var ox = inputPos.x % gap;
            var oy = inputPos.y % gap;
            var oz = inputPos.z % gap;
            ox=Mathf.Abs(ox)<0.5f*gap?ox:-(Mathf.Abs(ox)/ox)*(gap-Mathf.Abs(ox));
            oy = Mathf.Abs( oy) < 0.5f * gap ? oy : -(Mathf.Abs(oy) / oy)*(gap-Mathf.Abs(oy));
            oz = Mathf.Abs(oz) < 0.5f * gap ? oz : -(Mathf.Abs(oz) / oz)*(gap-Mathf.Abs(oz));
            var output = inputPos - new Vector3(ox, oy, oz);
            return output;
        }

        public static bool InRange(Vector3 v1, Vector3 v2, SpawngridConfig conf, Direction dir)
        {
            switch (dir) {
                #region foldold
                //as the less method include the equal as true, use some trick here.
                //case "Front":
                //    return !Less(fVal,Mathf.Abs(v1.z-v2.z));
                //case "Back":
                //    return !Less(fVal,Mathf.Abs(v1.z - v2.z) );
                //case "Left":
                //    return !Less(fVal,Mathf.Abs(v1.x - v2.x));
                //case "Right":
                //    return !Less(fVal,Mathf.Abs(v1.x - v2.x));
                //case "Up":
                //    return !Less(fVal,Mathf.Abs(v1.y - v2.y));
                //case "Down":
                //    return !Less(fVal,Mathf.Abs(v1.y - v2.y)); 
                #endregion
                case Direction.Front:
                    return Less( Mathf.Abs(v1.z - v2.z),conf.length);
                case Direction.Back:
                    return Less( Mathf.Abs(v1.z - v2.z),conf.length);
                case Direction.Left:
                    return Less( Mathf.Abs(v1.x - v2.x),conf.width);
                case Direction.Right:
                    return Less( Mathf.Abs(v1.x - v2.x),conf.width);
                case Direction.Up:
                    return Less( Mathf.Abs(v1.y - v2.y),conf.height);
                case Direction.Down:
                    return Less( Mathf.Abs(v1.y - v2.y),conf.height);
                default:
                    throw new System.ArgumentException("No this direction");
            }
            //if (Mathf.Abs(f1-f2)<fVal) {
            //    return true;
            //}
            return false;
        }

        public static bool InRange(Vector3 v1, Vector3 v2, float fVal)
        {
            if (Vector3.Distance(v1, v2) <= fVal)
            {
                return true;
            }
            return false;
        }

        //two decimal value compair, when f1 is less/equal than f2, then return true
        public static bool Less(float f1,float f2)
        {
            if (f1 <= f2)
            {
                return true;
            }
            return false;
        }

        public static int GetScore()
        {
            if (!PlayerPrefs.HasKey("EchoCoin"))
            {
                PlayerPrefs.SetInt("EchoCoin",0);
            }
            return PlayerPrefs.GetInt("EchoCoin");
        }
        
        public static void SetCoin(int currentPoint)
        {
            var val = PlayerPrefs.GetInt("EchoCoin");
            PlayerPrefs.SetInt("EchoCoin", (val + currentPoint));
        }


        //spawn coin go in canvas
        public static void SpawnCoin(GameObject spawnGO, GameObject canv,GameObject posGO, int val)
        {
            var pos = Camera.main.WorldToScreenPoint(posGO.transform.position);
            var coin = UnityEngine.Object.Instantiate(spawnGO,canv.transform);
            coin.GetComponent<RectTransform>().position = pos;
            coin.transform.GetComponent<Coin>().BounsVal = val;
            //coins.Add(coin);
        }
    }
}