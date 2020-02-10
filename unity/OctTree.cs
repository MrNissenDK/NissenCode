using System.Collections.Generic;
using UnityEngine;
namespace NissenCode
{
    public class OctTree<T>
    {
        public struct returnType
        {
            public T type;
            public Helper.Bound bound;
        }
        private int MaxDepth;
        private int depth;

        public Helper.Bound bound;
        private OctTree<T>[] childs = new OctTree<T>[8];
        private T myType;
        private bool hasChildren = false;

        public OctTree(T myType, Helper.Bound bound, int MaxDepth = 8, int MyDepth = 0)
        {
            this.MaxDepth = MaxDepth;
            depth = MyDepth;
            this.myType = myType;
            this.bound = bound;
        }
        public Messenger insert(T type, Vector3 possiton) {
            if (depth < MaxDepth)
            {
                if (!hasChildren)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        childs[i] = new OctTree<T>( myType, getBounds(i), MaxDepth, depth + 1);
                        hasChildren = true;
                    }
                }
                Messenger<int> msg = getChild(possiton);
                if (msg.value != -1)
                {
                    Messenger msgOut = childs[msg.value].insert(type, possiton);
                    bool ChildsSame = true;
                    for (int i = 0; i < 7; i++)
                    {
                        if (!childs[i].myType.Equals(childs[i+1].myType) || childs[i].hasChildren || childs[i+1].hasChildren)
                        {
                            ChildsSame = false;
                            break;
                        }
                    }
                    if (ChildsSame)
                    {
                        myType = type;
                        hasChildren = false;
                        childs = new OctTree<T>[8];
                    }
                    return msgOut;
                }
                else {
                    return msg;
                }
            }
            else
            {
                myType = type;
            }
            return new Messenger("Has successfully inserted \"type\"("+type+") into OctTree");
        }

        public returnType[] getDrawBounds(List<returnType> bounds = null) {
            if(bounds == null)
                bounds = new List<returnType>();
            if (hasChildren) {
                for (int i = 0; i < 8; i++)
                {
                    childs[i].getDrawBounds(bounds);
                }
            }
            else {
                bounds.Add(new returnType()
                {
                    bound = bound,
                    type = myType
                });
            }
            return bounds.ToArray();
        }

        public Helper.Bound getBounds(int i) {
            Vector3 p1 = bound.min;
            Vector3 p2 = new Vector3(bound.center.x, bound.min.y, bound.min.z);
            Vector3 p3 = new Vector3(bound.min.x, bound.min.y, bound.center.z);
            Vector3 p4 = new Vector3(bound.center.x, bound.min.y, bound.center.z);

            Vector3 p5 = new Vector3(bound.min.x, bound.center.y, bound.min.z);
            Vector3 p6 = new Vector3(bound.center.x, bound.center.y, bound.min.z);
            Vector3 p7 = new Vector3(bound.min.x, bound.center.y, bound.center.z);
            Vector3 p8 = bound.center;
            Vector3 p9 = new Vector3(bound.max.x, bound.center.y, bound.center.z);
            Vector3 p10 = new Vector3(bound.center.x, bound.center.y, bound.max.z);
            Vector3 p11 = new Vector3(bound.max.x, bound.center.y, bound.max.z);

            Vector3 p12 = new Vector3(bound.center.x, bound.max.y, bound.center.z);
            Vector3 p13 = new Vector3(bound.max.x, bound.max.y, bound.center.z);
            Vector3 p14 = new Vector3(bound.center.x, bound.max.y, bound.max.z);
            Vector3 p15 = bound.max;


            switch (i)
            {
                case 0:
                    return new Helper.Bound { min = p1, max = p8 };
                case 1:
                    return new Helper.Bound { min = p2, max = p9 };
                case 2:
                    return new Helper.Bound { min = p3, max = p10 };
                case 3:
                    return new Helper.Bound { min = p4, max = p11 };

                case 4:
                    return new Helper.Bound { min = p5, max = p12 };
                case 5:
                    return new Helper.Bound { min = p6, max = p13 };
                case 6:
                    return new Helper.Bound { min = p7, max = p14 };
                case 7:
                    return new Helper.Bound { min = p8, max = p15 };
            }
            throw new System.InvalidOperationException("index i is out of bound( "+i+" )");
        }
        private Messenger<int> getChild(Vector3 point) {

            for (int i = 0; i < 8; i++)
            {
                Helper.Bound bound = childs[i].bound;
                Vector3 min = bound.min;
                Vector3 max = bound.max;

                if (
                    point.x >= min.x && point.x < max.x &&
                    point.z >= min.z && point.z < max.z &&
                    point.y >= min.y && point.y < max.y
                    )
                {
                    return new Messenger<int>(i, "Found a index");
                }
            }
            return new Messenger<int>(-1, "Could't find a index probly out of bound", Level.Warning);
        }
    }
}
