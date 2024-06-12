using System;

namespace SharpCraft.Core.World.GameLevel.PathFinding
{
    public class BinaryHeap
    {
        private Node[] pathPoints = new Node[1024];
        private int count = 0;
        public virtual Node Insert(Node pathPoint1)
        {
            if (pathPoint1.index >= 0)
            {
                throw new InvalidOperationException("OW KNOWS!");
            }
            else
            {
                if (this.count == this.pathPoints.Length)
                {
                    Node[] pathPoint2 = new Node[this.count << 1];
                    Array.Copy(this.pathPoints, 0, pathPoint2, 0, this.count);
                    this.pathPoints = pathPoint2;
                }

                this.pathPoints[this.count] = pathPoint1;
                pathPoint1.index = this.count;
                this.UpHeap(this.count++);
                return pathPoint1;
            }
        }

        public virtual void Clear()
        {
            this.count = 0;
        }

        public virtual Node Pop()
        {
            Node pathPoint1 = this.pathPoints[0];
            this.pathPoints[0] = this.pathPoints[--this.count];
            this.pathPoints[this.count] = null;
            if (this.count > 0)
            {
                this.DownHeap(0);
            }

            pathPoint1.index = -1;
            return pathPoint1;
        }

        public virtual void ChangeCost(Node pathPoint1, float f2)
        {
            float f3 = pathPoint1.distanceToTarget;
            pathPoint1.distanceToTarget = f2;
            if (f2 < f3)
            {
                this.UpHeap(pathPoint1.index);
            }
            else
            {
                this.DownHeap(pathPoint1.index);
            }
        }

        private void UpHeap(int i1)
        {
            Node pathPoint2 = this.pathPoints[i1];
            int i4;
            for (float f3 = pathPoint2.distanceToTarget; i1 > 0; i1 = i4)
            {
                i4 = i1 - 1 >> 1;
                Node pathPoint5 = this.pathPoints[i4];
                if (f3 >= pathPoint5.distanceToTarget)
                {
                    break;
                }

                this.pathPoints[i1] = pathPoint5;
                pathPoint5.index = i1;
            }

            this.pathPoints[i1] = pathPoint2;
            pathPoint2.index = i1;
        }

        private void DownHeap(int i1)
        {
            Node pathPoint2 = this.pathPoints[i1];
            float f3 = pathPoint2.distanceToTarget;
            while (true)
            {
                int i4 = 1 + (i1 << 1);
                int i5 = i4 + 1;
                if (i4 >= this.count)
                {
                    break;
                }

                Node pathPoint6 = this.pathPoints[i4];
                float f7 = pathPoint6.distanceToTarget;
                Node pathPoint8;
                float f9;
                if (i5 >= this.count)
                {
                    pathPoint8 = null;
                    f9 = float.PositiveInfinity;
                }
                else
                {
                    pathPoint8 = this.pathPoints[i5];
                    f9 = pathPoint8.distanceToTarget;
                }

                if (f7 < f9)
                {
                    if (f7 >= f3)
                    {
                        break;
                    }

                    this.pathPoints[i1] = pathPoint6;
                    pathPoint6.index = i1;
                    i1 = i4;
                }
                else
                {
                    if (f9 >= f3)
                    {
                        break;
                    }

                    this.pathPoints[i1] = pathPoint8;
                    pathPoint8.index = i1;
                    i1 = i5;
                }
            }

            this.pathPoints[i1] = pathPoint2;
            pathPoint2.index = i1;
        }

        public virtual bool IsEmpty()
        {
            return this.count == 0;
        }
    }
}