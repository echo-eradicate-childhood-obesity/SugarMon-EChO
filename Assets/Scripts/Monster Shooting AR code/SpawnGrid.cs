using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace ARMon
{
    public class SpawnGrid : IObersver
    {


        public float Z { get; private set; }

        public float Y { get; private set; }

        public float X { get; private set; }
        public bool IsOccupy { get; set; }

        //each grid have its neighbor which can be null, if there is no neighbor
        //each grid have six neighbor member, front, back, left, right, up and down
        public Dictionary<Direction, SpawnGrid> neighborDic;


        private GameManager gm;


        private Direction front = Direction.Front;
        private Direction back = Direction.Back;
        private Direction left = Direction.Left;
        private Direction right = Direction.Right;
        private Direction up = Direction.Up;
        private Direction down = Direction.Down;

        //define the position of grid
        public SpawnGrid(Vector3 pos)
        {
            this.X = pos.x;
            this.Y = pos.y;
            this.Z = pos.z;
            neighborDic = new Dictionary<Direction, SpawnGrid>();
            //sign key value pair
            neighborDic.Add(front, null);
            neighborDic.Add(back, null);
            neighborDic.Add(left, null);
            neighborDic.Add(right, null);
            neighborDic.Add(up, null);
            neighborDic.Add(down, null);
            gm = GameManager.Instance;
            //test

        }

        public Vector3 GetPos()
        {
            return new Vector3(X, Y, Z);
        }

        public void StatusUpdate(Vector3 pos, Direction dir, SpawngridConfig conf, List<IObersver> observers)
        {
            if (!CustomController.InRange(pos, GetPos(), conf,dir))
            {
                UpdatePos(dir, conf);
                UpdateNeigbor(this, observers, conf.gap);
                this.SignNeighbor(observers, conf.gap);
                this.IsOccupy = false;
            }
            #region foldcomment
            //todo refactro code
            //var neighborPos = new Vector3(this.X,this.Y,this.Z);

            //if (neighborDic[s] == null)
            //{
            //    if (s == front)
            //    {
            //        neighborPos += Vector3.forward*gap;
            //    }
            //    else if (s == back)
            //    {
            //        neighborPos += Vector3.back * gap;
            //    }
            //    else if (s == right)
            //    {
            //        neighborPos += Vector3.right * gap;
            //    }
            //    else if (s == left)
            //    {
            //        neighborPos += Vector3.left * gap;
            //    }
            //    else if (s == up)
            //    {
            //        neighborPos += Vector3.up * gap;
            //    }
            //    else if (down == s)
            //    {
            //        neighborPos += Vector3.down * gap;
            //    }
            //    neighborDic[s] = new SpawnGrid(neighborPos);
            //    if (gm.TempSGHoder == null)
            //    {
            //        gm.TempSGHoder = new List<IObersver>();
            //        gm.TempSGHoder.Add(neighborDic[s]);
            //    }
            //    else { gm.TempSGHoder.Add(neighborDic[s]); }
            //} 
            #endregion
            //force update all info
        }

        private void UpdatePos(Direction dir, SpawngridConfig conf)
        {
            //z front/back, x left/right, y up/down
            var newPos = GetPos();
            #region foldold
            //todo refactor
            //if (dir == front)
            //{
            //    newPos += Vector3.forward * conf.length * 2;
            //    newPos.z += conf.gap;
            //}
            //else if (dir == back)
            //{
            //    newPos += Vector3.back * conf.length * 2;
            //    newPos.z -= conf.gap;
            //}
            //else if (s == right)
            //{
            //    newPos.x += conf.gap;
            //    newPos += Vector3.right * conf.width * 2;
            //}
            //else if (s == left)
            //{
            //    newPos += Vector3.left * conf.width * 2;
            //    newPos.x -= conf.gap;
            //}
            //else if (s == up)
            //{
            //    newPos.y += conf.gap;
            //    newPos += Vector3.up * conf.height * 2;
            //}
            //else if (down == s)
            //{
            //    newPos += Vector3.down * conf.height * 2;
            //    newPos.y -= conf.gap;
            //} 
            #endregion
            switch (dir)
            {
                case Direction.Front:
                    newPos += Vector3.forward * conf.length * 2;
                    newPos.z += conf.gap;
                    break;
                case Direction.Back:
                    newPos += Vector3.back * conf.length * 2;
                    newPos.z -= conf.gap;
                    break;
                case Direction.Left:
                    newPos.x += conf.gap;
                    newPos += Vector3.right * conf.width * 2;
                    break;
                case Direction.Right:
                    newPos += Vector3.left * conf.width * 2;
                    newPos.x -= conf.gap;
                    break;
                case Direction.Up:
                    newPos.y += conf.gap;
                    newPos += Vector3.up * conf.height * 2;
                    break;
                case Direction.Down:
                    newPos += Vector3.down * conf.height * 2;
                    newPos.y -= conf.gap;
                    break;
            }
            this.X = newPos.x;
            this.Y = newPos.y;
            this.Z = newPos.z;
        }

        public void SignNeighbor(List<IObersver> obersvers, float gap)
        {
            foreach (SpawnGrid sg in obersvers)
            {
                //todo refactor
                if (DirectionTest(sg.X, sg.Y, sg.Z - gap, sg))
                {
                    NeighborUpate(front, sg);
                }
                if (DirectionTest(sg.X, sg.Y, sg.Z + gap, sg))
                {
                    NeighborUpate(back, sg);
                }
                if (DirectionTest(sg.X - gap, sg.Y, sg.Z, sg))
                {
                    NeighborUpate(right, sg);
                }
                if (DirectionTest(sg.X + gap, sg.Y, sg.Z, sg))
                {
                    NeighborUpate(left, sg);
                }
                if (DirectionTest(sg.X, sg.Y - gap, sg.Z, sg))
                {
                    NeighborUpate(up, sg);
                }
                if (DirectionTest(sg.X, sg.Y + gap, sg.Z, sg))
                {
                    NeighborUpate(down, sg);
                }
                #region foldcomment
                //if (sg.X == this.X && sg.Y == this.Y)
                //{
                //    if ((sg.Z - gap) == this.Z)
                //    {
                //        neighborDic[front] = sg;
                //    }
                //    if ((sg.Z + gap) == this.Z)
                //    {
                //        neighborDic[back] = sg;
                //    }
                //}
                //if (sg.X == this.X && sg.Z == this.Z)
                //{
                //    if ((sg.Y - gap) == this.Y)
                //    {
                //        neighborDic[up] = sg;

                //    }
                //    if ((sg.Y + gap) == this.Y)
                //    {
                //        neighborDic[down] = sg;
                //    }
                //}
                //if (sg.X == this.X && sg.Z == this.Z)
                //{
                //    if ((sg.X - gap) == this.X)
                //    {
                //        neighborDic[right] = sg;
                //    }
                //    if ((sg.X + gap) == this.X)
                //    {
                //        neighborDic[left] = sg;
                //    }
                //} 
                #endregion
            }
        }

        bool DirectionTest(float x, float y, float z, SpawnGrid sg)
        {
            if (x == this.X && y == this.Y && z == this.Z)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        protected void UpdateNeigbor(SpawnGrid sg, List<IObersver> observers, float gap)
        {
            foreach (Direction dir in this.neighborDic.Keys.ToList())
            {
                //set this ref to null
                if (neighborDic[dir] == sg)
                {
                    neighborDic[dir].SignNeighbor(observers, gap);
                }
            }
        }

        public void NeighborUpate(Direction dir, SpawnGrid sg)
        {
            neighborDic[dir] = sg;
        }
    }
}
