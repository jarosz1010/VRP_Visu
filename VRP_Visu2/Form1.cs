using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VRP_Visu2
{

   
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }



        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {



            List<string> Plik = new List<string>();
            List<List<double>> Odleglosci = new List<List<double>>();
            List<List<double>> Wspolrzedne = new List<List<double>>();
            int counter = 0;
            string line;
            int rozmiar;
            string b;
            System.IO.StreamReader file =
    new System.IO.StreamReader(@"C:\Users\Michał\source\repos\VRP\VRP\PL.csv");
            while ((line = file.ReadLine()) != null)
            {
                Plik.Add(line);
                counter++;
            }

            // Zapisujemy ile jest miast uprzednio pozbywajac sie srednikow
            String[] rozmiar_st = Plik[0].Split(';');
            rozmiar = Convert.ToInt32(rozmiar_st[0]);


            // Zapisanie odleglosci do tablicy
            for (int i = 2; i < rozmiar + 2; i++)
            {
                List<double> pom = new List<double>();
                String[] elements = Plik[i].Split(';');
                foreach (var element in elements)
                {

                    pom.Add(Convert.ToDouble(element));
                }
                Odleglosci.Add(pom);

            }

            for (int i = 27; i < rozmiar + 27; i++)
            {
                List<double> pom = new List<double>();
                String[] elements = Plik[i].Split(';');
                for(int j=2;j<4; j++)
                {

                    pom.Add(Convert.ToDouble(elements[j]));
                }
                Wspolrzedne.Add(pom);

            }
            file.Close();
           

            //////////////////////////
            ///
            List<string> Plik2 = new List<string>();
            List<List<double>> Ladunki_tmp = new List<List<double>>();
            int counter2 = 0;
            string line2;
            int rozmiar2;
            string b2;
            System.IO.StreamReader file2 =
    new System.IO.StreamReader(@"C:\Users\Michał\source\repos\VRP\VRP\PLdata.csv");
            while ((line2 = file2.ReadLine()) != null)
            {
                Plik2.Add(line2);
                counter2++;
            }

            // Zapisujemy ile jest miast uprzednio pozbywajac sie srednikow
            String[] rozmiar_st2 = Plik2[0].Split(';');
            rozmiar2 = Convert.ToInt32(rozmiar_st2[0]);


            // Zapisanie odleglosci do tablicy
            for (int i = 1; i < rozmiar2 + 1; i++)
            {
                List<double> pom = new List<double>();
                String[] elements = Plik2[i].Split(';');
                foreach (var element in elements)
                {

                    pom.Add(Convert.ToDouble(element));
                }
                Ladunki_tmp.Add(pom);

            }
            file.Close();
            /////////////////////


            double kilometry = 0;
            double kilometry_ann = 0;

            List<int> permutacja_list_tmp = new List<int>();
            Wyrzazanie test = new Wyrzazanie();

            test.tablica = Odleglosci;
            test.Ladunki = Ladunki_tmp;
            test.centralny = 2;
            test.ilosc = rozmiar;
            int do_dodawania = 0;


            for (int i = 0; i < (rozmiar - 1) / 3; i++)
            {
                permutacja_list_tmp.Add(test.centralny);
                for (int j = 0; j < 3; j++)
                {

                    if (do_dodawania == test.centralny)
                        do_dodawania++;
                    if (do_dodawania != test.centralny)
                    {
                        permutacja_list_tmp.Add(do_dodawania);

                        do_dodawania++;
                    }
                }

            }
            permutacja_list_tmp.Add(test.centralny);

            test.permutacja_list = permutacja_list_tmp;


            kilometry = test.Cmax();
            kilometry_ann = test.Annealing();
            textBox1.Text = "Ilosc kilometrow:" + kilometry;
            textBox2.Text = "Ilosc kilometrow po wyzarzaniu:" + kilometry_ann;

            for (int k = 0; k<test.permutacja_list.Count; k++)
           

            ///////////////////////////
            map.MapProvider = GMapProviders.GoogleMap;

            map.Position = new PointLatLng(50, 17);
            int kolor = 0;
            GMapOverlay markersOverlay = new GMapOverlay("markers");
            for (int k = 0; k<test.permutacja_list.Count - 1; k++)
            {
                textBox3.Text = textBox3.Text + test.permutacja_list[k] + " ";
                if (test.permutacja_list[k] == test.centralny && test.permutacja_list[k+1] != test.centralny) kolor++;
            if(kolor == 0)
                {
                    PointLatLng point = new PointLatLng(Wspolrzedne[test.permutacja_list[k]][0], Wspolrzedne[test.permutacja_list[k]][1]);
                    GMapMarker marker = new GMarkerGoogle(point, GMarkerGoogleType.red_pushpin);
                    markersOverlay.Markers.Add(marker);

                }
            else if(kolor == 1)
                {
                    PointLatLng point = new PointLatLng(Wspolrzedne[test.permutacja_list[k]][0], Wspolrzedne[test.permutacja_list[k]][1]);
                    GMapMarker marker = new GMarkerGoogle(point, GMarkerGoogleType.green_pushpin);
                    markersOverlay.Markers.Add(marker);

                }

                else if (kolor == 2)
                {
                    PointLatLng point = new PointLatLng(Wspolrzedne[test.permutacja_list[k]][0], Wspolrzedne[test.permutacja_list[k]][1]);
                    GMapMarker marker = new GMarkerGoogle(point, GMarkerGoogleType.blue_pushpin);
                    markersOverlay.Markers.Add(marker);

                }
                else if (kolor == 3)
                {
                    PointLatLng point = new PointLatLng(Wspolrzedne[test.permutacja_list[k]][0], Wspolrzedne[test.permutacja_list[k]][1]);
                    GMapMarker marker = new GMarkerGoogle(point, GMarkerGoogleType.yellow_pushpin);
                    markersOverlay.Markers.Add(marker);

                }
                else if (kolor == 4)
                {
                    PointLatLng point = new PointLatLng(Wspolrzedne[test.permutacja_list[k]][0], Wspolrzedne[test.permutacja_list[k]][1]);
                    GMapMarker marker = new GMarkerGoogle(point, GMarkerGoogleType.pink_pushpin);
                    markersOverlay.Markers.Add(marker);

                }
                else if (kolor == 5)
                {
                    PointLatLng point = new PointLatLng(Wspolrzedne[test.permutacja_list[k]][0], Wspolrzedne[test.permutacja_list[k]][1]);
                    GMapMarker marker = new GMarkerGoogle(point, GMarkerGoogleType.purple_pushpin);
                    markersOverlay.Markers.Add(marker);

                }
                else if (kolor == 6)
                {
                    PointLatLng point = new PointLatLng(Wspolrzedne[test.permutacja_list[k]][0], Wspolrzedne[test.permutacja_list[k]][1]);
                    GMapMarker marker = new GMarkerGoogle(point, GMarkerGoogleType.lightblue_pushpin);
                    markersOverlay.Markers.Add(marker);

                }
                else if (kolor == 7)
                {
                    PointLatLng point = new PointLatLng(Wspolrzedne[test.permutacja_list[k]][0], Wspolrzedne[test.permutacja_list[k]][1]);
                    GMapMarker marker = new GMarkerGoogle(point, GMarkerGoogleType.orange_dot);
                    markersOverlay.Markers.Add(marker);

                }
                else if (kolor == 8)
                {
                    PointLatLng point = new PointLatLng(Wspolrzedne[test.permutacja_list[k]][0], Wspolrzedne[test.permutacja_list[k]][1]);
                    GMapMarker marker = new GMarkerGoogle(point, GMarkerGoogleType.blue_dot);
                    markersOverlay.Markers.Add(marker);

                }
                else if (kolor == 9)
                {
                    PointLatLng point = new PointLatLng(Wspolrzedne[test.permutacja_list[k]][0], Wspolrzedne[test.permutacja_list[k]][1]);
                    GMapMarker marker = new GMarkerGoogle(point, GMarkerGoogleType.green_dot);
                    markersOverlay.Markers.Add(marker);

                }


            }
            
            
           
            map.Overlays.Add(markersOverlay);
            map.MinZoom = 5;
            map.MaxZoom = 100;
            map.Zoom = 5;
            map.MouseWheelZoomEnabled = true;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
