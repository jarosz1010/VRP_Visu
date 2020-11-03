using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VRP_Visu2
{
    public class Wyrzazanie
    {
        public List<List<double>> tablica;
        public List<List<double>> Ladunki;
        public List<int> permutacja_list;
        public List<int> Centralne = new List<int>();
        public List<double> Sumy = new List<double>();
        public int centralny;
        public int ilosc;
        public int blad = 0;

        public void SwapValues(int index1, int index2)
        {
            int temp = permutacja_list[index1];
            permutacja_list[index1] = permutacja_list[index2];
            permutacja_list[index2] = temp;
        }
        public double Cmax()
        {
            int miasta_na_pojazd = 0;
            double suma_dlugosci = 0;
            double suma_masy = 0;
            double Suma1 = 0;

            blad = 0;
            Centralne.Clear();
            Sumy.Clear();

            for (int i = 1; i < (permutacja_list.Count); i++)
            {


                //Ograniczenie 1 - maksymalnie 4 miasta na jeden pojazd
                /*
                if(permutacja_list[i] != centralny)
                {
                    miasta_na_pojazd++;
                }
                if (miasta_na_pojazd > 4) blad = 1;
                else if (miasta_na_pojazd < 5 && permutacja_list[i] == centralny) miasta_na_pojazd = 0;
                
           */

                /////// Ograniczenie 3 - ladunki
                ///
                if (permutacja_list[i] != centralny)
                {

                    suma_dlugosci = suma_dlugosci + Ladunki[permutacja_list[i]][1];
                    suma_masy = suma_masy + Ladunki[permutacja_list[i]][2];
                }
                if (permutacja_list[i] == centralny)
                {

                    if (suma_masy > 24000 || suma_dlugosci > 16.6)
                    {
                        blad = 3;
                    }
                    suma_dlugosci = 0;
                    suma_masy = 0;
                }

                // Liczenie sumy kilometrow
                int first = permutacja_list[i];
                int second = permutacja_list[i - 1];

                Suma1 = Suma1 + tablica[second][first];
                /*
                // Ograniczenie 2 - suma kilometrow nie wieksza niz 1000
                Sumy.Add(Suma1);
              
               if (permutacja_list[i] == centralny) Centralne.Add(i);

                */

            }
            /*
                 for (int k = 1; k < Centralne.Count; k++)
             {

                 double Roznica = Sumy[Centralne[k]-1] - Sumy[Centralne[k - 1]-1];
                 if (Roznica > 1800)  blad = 2;

             } */

            return Suma1;
        }

        public double Annealing()
        {
            double T = 1000;
            int i, j;
            double cmax_tmp = 0;
            double cmax_start;
            double r, error, diff;
            int L = ilosc;

            double a = 0.95;
            double Tend = 0.001;
            cmax_start = Cmax();
            while (T > Tend)
            {
                for (int k = 1; k < L; k++)
                {
                    Random rnd = new Random();
                    i = rnd.Next(0, ilosc);
                    j = rnd.Next(0, ilosc);

                    SwapValues(i, j);
                    cmax_tmp = Cmax();
                    if (blad == 0)
                    {
                        if (cmax_tmp > cmax_start)
                        {
                            r = rnd.Next(0, 100);
                            r = r / 100;
                            diff = cmax_start - cmax_tmp;
                            double podziel = diff / T;
                            error = Math.Exp(podziel);

                            if (r >= error) SwapValues(i, j);

                        }
                    }
                    // Tutaj zmienic kod bledu w zaleznosci od ograniczenia
                    else if (blad == 3) SwapValues(i, j);

                }

                T = a * T;
            }
            for (int it = 0; it < permutacja_list.Count; it++)
                Console.WriteLine(permutacja_list[it]);
            return cmax_tmp;
        }

    }
    static class Program
    {
        /// <summary>
        /// Główny punkt wejścia dla aplikacji.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
