//Margolin Ilan 2016
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VRepClient
{
    public class SequencePoints
    {
        public float CurrentPointX=0;
        public float CurrentPointY=0;

        public float GoalPointX;
        public float GoalPointY;
        public float GoalPointX2;
        public float GoalPointY2; 
        public float GoalPointX3;
        public float GoalPointY3;
        float DistToTarget;
        int NumberOfGoalPoint=0;
        float LastDist = 1;
        public bool KeyForSearchInGraph=true;
        public void GetNextPoint(List<Point> ListPoints, float RobX, float RobY, float RobA, int Xmax, int Ymax)
        {
                  
            int RobLocX = (int)(RobX * 10)+Xmax/2; //положение робота в клетках графа
            int RobLocY = (int)(RobY * 10)+Ymax/2;     
            
            for(int i=1;i<ListPoints.Count-4;i++)
            {
                if (Math.Abs( ListPoints[i].X-RobLocX)<3 && Math.Abs(ListPoints[i].Y-RobLocY)<3) 
                {
                    CurrentPointX = ListPoints[i+4].X;
                    CurrentPointY = ListPoints[i+4].Y;
                    KeyForSearchInGraph = true;
                  //  break;
                }
               // else
                  //  KeyForSearchInGraph = false;
                
            }
            
        }
    }
}
