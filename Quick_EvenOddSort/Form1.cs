using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;

namespace Quick_EvenOddSort
{
  public partial class Form1 : Form
  {
    public Form1()
    {
      InitializeComponent();
    }
    int swapping = 0; int comprassion = 0; int swapping1 = 0; int comprassion2 = 0;
    public List<double> numb = new List<double>();
    public List<string> FilesPath = new List<string>();
    public PointPairList list = new PointPairList ();
    public PointPairList list2 = new PointPairList ();
    public PointPairList list3 = new PointPairList ();
    public PointPairList list4 = new PointPairList ();
    public PointPairList list5 = new PointPairList ();
    public PointPairList list6  = new PointPairList ();
    public List<double> sizes = new List<double>();
    public List<double> times1 = new List<double>();
    public List<double> times2 = new List<double>();
    public List<double> swapping_List = new List<double>();
    public List<double> swapping2_List = new List<double>();
    public List<double> comprassion_List = new List<double>();
    public List<double> comprassion2_List = new List<double>();

    public void DrawTimeSize(double size1, double time1, double time2, GraphPane pane, PointPairList x, PointPairList y, string text, string text1)
    {
        Random rnd = new Random ();
        pane.CurveList.Clear ();
        double ymax = ( (time1 * 1000) + 10 + (time2 * 1000) + 10) / 2;
        // Заполняем список точек
        x.Add (size1, time1 * 1000);
        y.Add (size1, time2 * 1000);
        // Создадим кривую
        pane.AddCurve (text, x, Color.DeepPink, SymbolType.Circle);
        pane.AddCurve (text1, y, Color.DarkBlue, SymbolType.Diamond);
        pane.Legend.IsVisible = true;

    }


    private void Load_Click(object sender, EventArgs e)
    {
      Cleaning();
      textBox2.Clear();
      button2.Enabled = true;
      OpenFileDialog openFileDialog1 = new OpenFileDialog();

      openFileDialog1.Filter = "Текстовый файл (*.txt)|*.txt|Все файлы (*.*)|*.*";
      openFileDialog1.FilterIndex = 1;
      openFileDialog1.RestoreDirectory = true;

      OpenFileDialog openFileDialog = new OpenFileDialog();
      openFileDialog.Multiselect = true;
      FilesPath.Clear();
            

      if (openFileDialog.ShowDialog() == DialogResult.OK)
      {
        foreach (string fileName in openFileDialog.FileNames)
        {
          FilesPath.Add(fileName);
          textBox2.Text += fileName + ":\r\n";
          string content = File.ReadAllText(fileName);
          string[] texfile = content.Split(new string[] { "\n", " ", "\0" }, StringSplitOptions.RemoveEmptyEntries);
          foreach (string n in texfile)
          {
            textBox2.Text += n.ToString() + "\r\n";
          } 
                    
        }
      }
    }

   private List<double> preStart(string FileName)
        {
            //
            string content = File.ReadAllText(FileName);
            string[] texfile = content.Split(new string[] { "\n", " ", "\0" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string Line in texfile)
            {
                double[] temp = Line.Split(new char[] { '\n', ' ', '\r' }).Select(double.Parse).ToArray();
                for (int i = 0; i < temp.Length; i++)
                {
                    numb.Add(temp[i]);
                }
            }

            return numb;
        }

        private void start_Click(object sender, EventArgs e)
        {
            Cleaning();
            foreach (var path in FilesPath)
            {
                List<double> numb = preStart(path);
                sizes.Add(numb.Count);
                textBox1.Text += path + ":\r\n";
                var Start = DateTime.Now;
                List<double> result = QuickSort(numb, 0, numb.Count - 1);
                var spendtime = DateTime.Now - Start;
                times1.Add(spendtime.TotalMilliseconds);
                textBox3.Text += path + ":\r\n";
                foreach (int n in result)
                {
                    textBox3.Text += n.ToString() + "\r\n";
                }
                swapping_List.Add(swapping);
                comprassion_List.Add(comprassion);
                numb.Clear();
                textBox1.Text += "Расческа:" + "\r\n";
                textBox1.Text += "Время:   " + spendtime.Milliseconds + " мс" + "\r\n";
                textBox1.Text += "Кол-во перестановок: " + swapping.ToString() + ":\r\n";
                textBox1.Text += "Кол-во сравнений: " + comprassion.ToString() + ":\r\n";
                
                
                
                numb= preStart(path);
                var spendTimeForOne = spendtime;
                Start = DateTime.Now;
                result = EvenOddSort(numb);
                spendtime = DateTime.Now - Start;
                textBox4.Text += path +":\r\n";
                foreach (int n in result)
                {
                    textBox4.Text += n.ToString() + "\r\n";
                }
                times2.Add(spendtime.TotalMilliseconds);
                swapping2_List.Add(swapping1);
                comprassion2_List.Add(comprassion2);
                numb.Clear();
                textBox1.Text += "Чет-нечет:" + "\r\n";
                textBox1.Text += "Время:   " + spendtime.Milliseconds + " мс" + "\r\n";
                textBox1.Text += "Кол-во перестановок: " + swapping1.ToString() + "\r\n";
                textBox1.Text += "Кол-во сравнений: " + comprassion2.ToString() + "\r\n";
  
            }
            MasterPane masterPane = zedGraphControl1.MasterPane;
            MasterPane masterPane1 = zedGraphControl2.MasterPane;
            MasterPane masterPane2 = zedGraphControl3.MasterPane;
            
            masterPane.PaneList.Clear ();
            masterPane1.PaneList.Clear ();
            masterPane2.PaneList.Clear ();
            
            GraphPane pane = new GraphPane ();
            GraphPane pane1 = new GraphPane ();
            GraphPane pane2 = new GraphPane ();
            
            for (int i = 0; i < sizes.Count; i++)
            {
                DrawTimeSize(sizes[i], times1[i], times2[i], pane, list, list2, "Быстрая", "Чет-Нечет");
                DrawTimeSize(sizes[i], swapping_List[i], swapping2_List[i], pane1,  list3, list4, "Быстрая", "Чет-Нечет");
                DrawTimeSize(sizes[i], comprassion_List[i], comprassion2_List[i], pane2,  list5, list6, "Быстрая", "Чет-Нечет");
            }
            masterPane.Add (pane);
            masterPane1.Add (pane1);
            masterPane2.Add (pane2);
            
            zedGraphControl1.AxisChange ();
            zedGraphControl2.AxisChange ();
            zedGraphControl3.AxisChange ();
            
            zedGraphControl1.Invalidate ();
            zedGraphControl2.Invalidate ();
            zedGraphControl3.Invalidate ();
        }
        
        private void Cleaning()
        {
            textBox1.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox1.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            swapping = 0;
            comprassion = 0; 
            swapping1 = 0;
            comprassion2 = 0;
            list.Clear(); list3.Clear(); list4.Clear(); list4.Clear(); list5.Clear(); list6.Clear();
            numb.Clear(); sizes.Clear(); times1.Clear(); times2.Clear(); swapping_List.Clear(); swapping2_List
                .Clear(); comprassion_List.Clear(); comprassion2_List.Clear();
            
        }
    
        public List<double> EvenOddSort(List<double> arr)
        {
            bool sorted = false;

            while (!sorted)
            {
                sorted = true;

                // Четные элементы
                for (int i = 0; i < arr.Count - 1; i += 2)
                {
                    if (arr[i] > arr[i + 1])
                    {
                        Swap(arr, i, i + 1);
                        sorted = false;
                    }
                }

                // Нечетные элементы
                for (int i = 1; i < arr.Count - 1; i += 2)
                {
                    if (arr[i] > arr[i + 1])
                    {
                        Swap(arr, i, i + 1);
                        sorted = false;
                    }
                }

                comprassion2 += 2;
            }

            return arr;
        }

        public void Swap(List<double> arr, int i, int j)
        {
            swapping1++;
            comprassion2++;
            double temp = arr[i];
            arr[i] = arr[j];
            arr[j] = temp;
        }

    private List<double> QuickSort(List<double> data, int left, int right)
    {
      int i, j;
      double pivot, temp;
      i = left;
      j = right;
      pivot = data[(left + right) / 2];

      do
      {
        while ((data[i].CompareTo(pivot) < 0) && (i < right)) i++;
        while ((pivot.CompareTo(data[j]) < 0) && (j > left)) j--;
        if (i <= j)
        {
          comprassion++;
          swapping++;
          temp = data[i];
          data[i] = data[j];
          data[j] = temp;
          i++;
          j--;
        }
      } while (i <= j);

      if (left < j){comprassion++; QuickSort(data, left, j);}
      if (i < right){comprassion++; QuickSort(data, i, right);}
      
      return data;
    }

  }
}
