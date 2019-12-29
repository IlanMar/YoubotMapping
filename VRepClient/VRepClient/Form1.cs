//Margolin Ilan 2016
using System;
using System.Drawing;
using System.Windows.Forms;
using VRepAdapter;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace VRepClient
{
    public partial class Form1 : Form

    {
        public MapForm f2 = new MapForm();
        public Form1()
        {
            InitializeComponent();
            f1 = this;
            
            f2.Show();     
    

        }
        
        public RobotAdapter ra; //экземпляр класса ra - robot adapter
        public Drive RobDrive;
        public  SequencePoints SQ;//объект класса sequencePoints
        public Map map;//объект класса Map
        public SearchInGraph SiG;//объект класса поиска по графу        
        public KukaPotField KukaPotField = new KukaPotField();
        public int PotfieldButtonA = 0;//если кнопка нажате то методм PotField доступен
        public int KukaPotButtonB = 0;//если кнопка нажата то работает метод кука.
        public List<Point> ListPoints = new List<Point>();        
        
        private void button1_Click(object sender, EventArgs e)
        {
            ra.Init();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
        Graphics g;
        private void timer1_Tick(object sender, EventArgs e)
        {
            //pictureBox1.Invalidate();
            
                PaintEventArgs p = new PaintEventArgs(pictureBox1.CreateGraphics(), pictureBox1.Bounds); //Компонент на котором нужно рисовать и область на которой нужно рисовать
            pictureBox1_Paint(sender, p);
           // pictureBox1_Paint();
                 // this.Invalidate();
            f2.Invalidate();
            if(ra is VrepAdapter)
            {
                var vrep = ra as VrepAdapter;

                string Lidar = VRepFunctions.GetStringSignal(vrep.clientID, "Lidar");//str-данные с ледара
                string RobPos = VRepFunctions.GetStringSignal(vrep.clientID, "RobPos");//получение координат робота на сцене Врепа
                vrep.ReceiveLedData(Lidar);
                vrep.ReceiveOdomData(RobPos);
            }
            

            if (ra != null)
            {
                ra.Send(RobDrive);
            }         

            if (RobDrive != null & ra != null && SQ!=null )//отправка одометрии в экземпляр класса drive
            {
                map.GlobListToGraph(map.GlobalMapList, map.RobOdData);
                float GoalPointX = Convert.ToSingle(textBox8.Text);
                float GoalPointY = Convert.ToSingle(textBox9.Text);
             
                Point start = new Point((int)(ra.RobotOdomData[0] * 10 + map.Xmax / 2), (int)(ra.RobotOdomData[1] * 10 + map.Ymax / 2));
                Point goal = new Point((int)GoalPointX*10+map.Xmax / 2, (int)GoalPointY*10+map.Ymax / 2);
                 
               // ListPoints = null;
             
                    ListPoints = SiG.FindPath(map.graph, start, goal); //SearchInGraph.FindPath(map.graph, start, goal);             
              
                        
                if (ListPoints != null)
                {
                    SQ.GetNextPoint(ListPoints, ra.RobotOdomData[0], ra.RobotOdomData[1], ra.RobotOdomData[2], map.Xmax, map.Ymax);

                    RobDrive.GetDrive(ra.RobotOdomData[0], ra.RobotOdomData[1], ra.RobotOdomData[2], SQ.CurrentPointX, SQ.CurrentPointY, map.Xmax,map.Ymax);
                    ra.Send(RobDrive);
                }
                map.LedDataToList(ra.RobotLedData, ra.RobotOdomData);
                
            }
            if (ra != null & RobDrive != null)//вывод переменных из Робот Адаптера на форму
         {
             string OutLedData="";
             string OutOdomData = "";
             for (int i = 0; i < ra.RobotLedData.Length; i++) 
             {
                 OutLedData = OutLedData + ra.RobotLedData[i]+"; ";
             }
             for (int i = 0; i < ra.RobotOdomData.Length; i++)
             {
                 OutOdomData = OutOdomData + ra.RobotOdomData[i]+"; ";
             }

            // richTextBox1.Text = OutLedData;//закоменчен вывод данных одометрии
             richTextBox2.Text = OutOdomData;
             if (RobDrive != null)
             {
                 textBox2.Text = RobDrive.Phi.ToString();
                 textBox2.Invalidate();
                 textBox3.Text = RobDrive.RobotDirection.ToString();
                 textBox3.Invalidate();
                 textBox4.Text = RobDrive.TargetDirection.ToString();
                 textBox4.Invalidate();
                 textBox5.Text = RobDrive.DistToTarget.ToString();
                 textBox5.Invalidate();
             }
         }
                      
            
        }

    

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ra != null)
            {
                ra.Deactivate();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            PotfieldButtonA = 1;

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        void invoke(Action a) { Invoke(a); }
        public static Form1 f1;

        private void bt_tcp_test_Click(object sender, EventArgs e)
        {
            if (ra is YoubotAdapter)
            {
                var ya = ra as YoubotAdapter;
                ya.TCPconnect(tb_ip.Text);
            }
            else { MessageBox.Show("вы работаете с Врепом, а не с реальным роботом!"); }
        }

        public void ShowLedData(string s)
        {
            rtb_tcp.Invoke(new Action(() => rtb_tcp.Text = s));
        }
        public void ShowOdomData(string s)
        {
            rtb_tcp2.Invoke(new Action(() => rtb_tcp2.Text = s));
        }

        private void btsend_Click(object sender, EventArgs e)
        {
      
        }

        private void rtb_send_TextChanged(object sender, EventArgs e)
        {

        }

        private void KukaPotButton_Click(object sender, EventArgs e)
        {

        }

        private void VrepAdapter_Click(object sender, EventArgs e)
        {
             ra =  new VrepAdapter();
           
        }

        private void YoubotAdapter_Click(object sender, EventArgs e)
        {
            ra = new YoubotAdapter();
        }

        private void Drive_Click(object sender, EventArgs e)
        {
            SQ = new SequencePoints();
            RobDrive = new Drive();
            map = new Map();
            SiG = new SearchInGraph();

            int mapWidthPxls = map.Xmax * (CellSize + 1) + 1, mapHeightPxls = map.Ymax * (CellSize + 1) + 1;

            //invoke(() =>
            //{
                pictureBox1.Image = new Bitmap(mapWidthPxls, mapHeightPxls);
                g = Graphics.FromImage(pictureBox1.Image);
            //});
           
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void rtb_tcp_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }
        int CellSize = 4;
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (ra != null && SQ != null && Drive != null && map != null && map.graph != null)
         {         
                    
         

             int MapWidth = map.Xmax;
             int MapHeight = map.Ymax;
             // Размер карты в пискелях.
             int mapWidthPxls = MapWidth * (CellSize + 1) + 1,
                 mapHeightPxls = MapHeight * (CellSize + 1) + 1;
             

             // Заливаем весь битмап:
             g.Clear(Color.White);

             // Рисуем сетку:
             for (int x = 0; x <= MapWidth; x++)
                 g.DrawLine(Pens.LightGray, x * (CellSize + 1), 0, x * (CellSize + 1), mapHeightPxls);
             for (int y = 0; y <= MapHeight; y++)
                 g.DrawLine(Pens.LightGray, 0, y * (CellSize + 1), mapWidthPxls, y * (CellSize + 1));
            
            
            
         for (int i = 0; i < map.Xmax; i++) //закрашиваем ячейки с препядствиями
         {
             for (int k = 0; k < map.Ymax; k++)
             {
                 if (map.graph[i, k] ==2)
                 {
                     int H = CellSize+1;//(int)(pictureBox1.Height / map.Ymax);
                     int W = CellSize + 1;//(int)(pictureBox1.Width / map.Xmax);
                     
                     SolidBrush blueBrush = new SolidBrush(Color.Blue);                   
                     Rectangle rect = new Rectangle((i) * W, pictureBox1.Height + ((-1) * k * H) , W, H);                  
                     e.Graphics.FillRectangle(blueBrush, rect);                  
                 }
                
             }
         }
         if (ListPoints != null)//ресуем получившийся маршрут
         {
             for (int i = 0; i < ListPoints.Count; i++)
             {
                 int H =CellSize+1; //(int)(pictureBox1.Height / map.Ymax);
                 int W = CellSize + 1;//(int)(pictureBox1.Width / map.Xmax);

                 SolidBrush blueBrush = new SolidBrush(Color.Red);
                 SolidBrush greenBrush = new SolidBrush(Color.Green);
                 Rectangle rect = new Rectangle((ListPoints[i].X) * W, pictureBox1.Height + ((-1) * ListPoints[i].Y * H), W, H);
                 Rectangle rectCurrentPoint = new Rectangle(((int)SQ.CurrentPointX) * W, pictureBox1.Height + ((-1) * (int)SQ.CurrentPointY * H), W, H);
                 
                 e.Graphics.FillRectangle(blueBrush, rect);
                 e.Graphics.FillRectangle(greenBrush, rectCurrentPoint);
             }

         }
         int H2 =CellSize+1;// (int)(pictureBox1.Height / map.Ymax);
         int W2 = CellSize + 1; //(int)(pictureBox1.Width / map.Xmax);
         Point start = new Point((int)(ra.RobotOdomData[0] * 10 + map.Xmax / 2), (int)(ra.RobotOdomData[1] * 10 + map.Ymax / 2));
         e.Graphics.DrawEllipse(Pens.Chocolate, (int)start.X*W2-2*W2,pictureBox1.Height + ((-1) * (int)start.Y)*H2-2*W2, 20, 20);
         
     }
          
            if (map != null && map.invalidateform == true)//обновляем форму
            {
                pictureBox1.Invalidate();//вызов отрисовки на пикчербоксе перенести в более логичное мето
            }
            pictureBox1.Invalidate();

        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
         

        }

        public Image Rob { get; set; }
    }
}
