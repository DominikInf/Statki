using BattleShipConsole.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Console;

namespace BattleShipConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            bool pokazstatki = false;//pokazuje pozycje wrogie

            zasoby mzasoby = new zasoby();
            zasoby pzasoby = new zasoby();



            Dictionary<char, int> kordy = PopulateDictionary();
            naglowek();
            for (int h = 0; h < 19; h++)
            {
                Write(" ");
            }


            PrintMap(mzasoby.pozstrzalu, mzasoby, pzasoby, pokazstatki);

            int gra;
            for (gra = 1; gra < 101; gra++)
            {
                mzasoby.StepsTaken++;

                Position position = new Position();

                ForegroundColor = ConsoleColor.White;
                WriteLine("Enter firing position (e.g. A3).");
                string input = ReadLine();
                position = AnalyzeInput(input, kordy);

                if (position.x == -1 || position.y == -1)
                {
                    WriteLine("nieprawidłowe koordynaty!");
                    gra--;
                    continue;
                }

                if (mzasoby.pozstrzalu.Any(EFP => EFP.x == position.x && EFP.y == position.y))
                {
                    WriteLine("Juz tu strzeliles.");
                    gra--;
                    continue;
                }


                pzasoby.Fire();


                var index = mzasoby.pozognia.FindIndex(p => p.x == position.x && p.y == position.y);

                if (index == -1)
                    mzasoby.pozstrzalu.Add(position);

                Clear();



                mzasoby.AllShipsPosition.OrderBy(o => o.x).ThenBy(n => n.y).ToList();
                mzasoby.CheckShipStatus(pzasoby.pozstrzalu);

                pzasoby.AllShipsPosition.OrderBy(o => o.x).ThenBy(n => n.y).ToList();
                pzasoby.CheckShipStatus(mzasoby.pozstrzalu);

                naglowek();
                for (int h = 0; h < 19; h++)
                {
                    Write(" ");
                }



                PrintMap(mzasoby.pozstrzalu, mzasoby, pzasoby, pokazstatki);

                Commentator(mzasoby, true);
                Commentator(pzasoby, false);
                if (pzasoby.IsObliteratedAll || mzasoby.IsObliteratedAll) { break; }


            }

            ForegroundColor = ConsoleColor.White;

            if (pzasoby.IsObliteratedAll && !mzasoby.IsObliteratedAll)
            {
                WriteLine("Koniec gry, Wygrales");
            }
            else if (!pzasoby.IsObliteratedAll && mzasoby.IsObliteratedAll)
            {
                WriteLine("Koniec gry, Przegrales");
            }
            else
            {
                WriteLine("Koniec gry, remis");
            }

            WriteLine("Total steps taken:{0} ", gra);
            ReadLine();


        }

        static void PrintStatistic(int x, int y, zasoby zasoby)
        {
            if (x == 1 && y == 10)
            {
                ForegroundColor = ConsoleColor.White;
                Write("Indicator:    ");
            }


            if (x == 2 && y == 10)
            {
                if (zasoby.IsCarrierzatopiony)
                {
                    ForegroundColor = ConsoleColor.Red;
                    Write("otniskowiec [5]   ");
                }
                else
                {
                    ForegroundColor = ConsoleColor.DarkGreen;
                    Write("lotniskowiec [5]   ");
                }

            }

            if (x == 3 && y == 10)
            {
                if (zasoby.IsBattleshipzatopiony)
                {
                    ForegroundColor = ConsoleColor.Red;
                    Write("okręt wojenny [4]");
                }
                else
                {
                    ForegroundColor = ConsoleColor.DarkGreen;
                    Write("okręt wojenny [4]");
                }
            }

            if (x == 4 && y == 10)
            {

                if (zasoby.IsDestroyerzatopiony)
                {
                    ForegroundColor = ConsoleColor.Red;
                    Write("Niszczyciel [3] ");
                }
                else
                {
                    ForegroundColor = ConsoleColor.DarkGreen;
                    Write("Niszczyciel [3] ");
                }
            }

            if (x == 5 && y == 10)
            {

                if (zasoby.IsSubmarinezatopiony)
                {
                    ForegroundColor = ConsoleColor.Red;
                    Write("Łódź podwodna [3] ");
                }
                else
                {
                    ForegroundColor = ConsoleColor.DarkGreen;
                    Write("Łódź podwodna [3] ");
                }
            }

            if (x == 6 && y == 10)
            {

                if (zasoby.IsPatrolBoatzatopiony)
                {
                    ForegroundColor = ConsoleColor.Red;
                    Write("Łódka patrolująca [2]");
                }
                else
                {
                    ForegroundColor = ConsoleColor.DarkGreen;
                    Write("Łódka patrolująca [2]");
                }

            }


            if (x > 6 && y == 10)
            {
                for (int i = 0; i < 14; i++)
                {
                    Write(" ");
                }
            }

        }

        static void PrintMap(List<Position> positions, zasoby mzasoby, zasoby pzasoby, bool pokazStatkiP)
        {
            naglowek();
            WriteLine();
            if (!pokazStatkiP)
                pokazStatkiP = mzasoby.IsObliteratedAll;

            List<Position> sortpozstrzalu = positions.OrderBy(o => o.x).ThenBy(n => n.y).ToList();
            List<Position> SortedPozycjeStatkow = pzasoby.AllShipsPosition.OrderBy(o => o.x).ThenBy(n => n.y).ToList();

            SortedPozycjeStatkow = SortedPozycjeStatkow.Where(FP => !sortpozstrzalu.Exists(ShipPos => ShipPos.x == FP.x && ShipPos.y == FP.y)).ToList();


            int liczniktrafien = 0;
            int iloscstatkowp = 0;
            int iloscstatkowm = 0;
            int pliczniktrafien = 0;

            char row = 'A';
            try
            {
                for (int x = 1; x < 11; x++)
                {
                    for (int y = 1; y < 11; y++)
                    {
                        bool keepGoing = true;

                        #region row indicator
                        if (y == 1)
                        {
                            ForegroundColor = ConsoleColor.DarkYellow;
                            Write("[" + row + "]");
                            row++;
                        }
                        #endregion


                        if (sortpozstrzalu.Count != 0 && sortpozstrzalu[liczniktrafien].x == x && sortpozstrzalu[liczniktrafien].y == y)
                        {

                            if (sortpozstrzalu.Count - 1 > liczniktrafien)
                                liczniktrafien++;

                            if (pzasoby.AllShipsPosition.Exists(ShipPos => ShipPos.x == x && ShipPos.y == y))
                            {

                                ForegroundColor = ConsoleColor.Red;
                                Write("[*]");

                                keepGoing = false;

                            }
                            else
                            {
                                ForegroundColor = ConsoleColor.Black;
                                Write("[X]");

                                keepGoing = false;


                            }

                        }

                        if (keepGoing && pokazStatkiP && SortedPozycjeStatkow.Count != 0 && SortedPozycjeStatkow[iloscstatkowp].x == x && SortedPozycjeStatkow[iloscstatkowp].y == y)

                        {

                            if (SortedPozycjeStatkow.Count - 1 > iloscstatkowp)
                                iloscstatkowp++;

                            ForegroundColor = ConsoleColor.DarkGreen;
                            Write("[O]");
                            keepGoing = false;
                        }

                        if (keepGoing)
                        {
                            ForegroundColor = ConsoleColor.Blue;
                            Write("[~]");
                        }


                        PrintStatistic(x, y, mzasoby);


                        if (y == 10)
                        {
                            Write("      ");

                            PrintMapOfEnemy(x, row, mzasoby, pzasoby, ref iloscstatkowm, ref pliczniktrafien);
                        }
                    }

                    WriteLine();
                }

            }
            catch (Exception e)
            {
                string error = e.Message.ToString();
            }
        }

        static void PrintMapOfEnemy(int x, char row, zasoby mzasoby, zasoby pzasoby, ref int iloscstatkowm, ref int pliczniktrafien)
        {
            List<Position> EnemyFirePositions = new List<Position>();
            row--;
            Random random = new Random();
            List<Position> SortedLFirePositions = pzasoby.FirePositions.OrderBy(o => o.x).ThenBy(n => n.y).ToList();
            List<Position> SortedLStatkiPozycja = mzasoby.AllShipsPosition.OrderBy(o => o.x).ThenBy(n => n.y).ToList();

            SortedLStatkiPozycja = SortedLStatkiPozycja.Where(FP => !SortedLFirePositions.Exists(ShipPos => ShipPos.x == FP.x && ShipPos.y == FP.y)).ToList();


            try
            {

                for (int y = 1; y < 11; y++)
                {
                    bool keepGoing = true;

                    #region row indicator
                    if (y == 1)
                    {
                        ForegroundColor = ConsoleColor.DarkYellow;
                        Write("[" + row + "]");
                        row++;
                    }
                    #endregion


                    if (SortedLFirePositions.Count != 0 && SortedLFirePositions[pliczniktrafien].x == x && SortedLFirePositions[pliczniktrafien].y == y)
                    {

                        if (SortedLFirePositions.Count - 1 > pliczniktrafien)
                            pliczniktrafien++;

                        if (mzasoby.AllShipsPosition.Exists(ShipPos => ShipPos.x == x && ShipPos.y == y))
                        {

                            ForegroundColor = ConsoleColor.Red;
                            Write("[*]");

                            keepGoing = false;

                        }
                        else
                        {
                            ForegroundColor = ConsoleColor.Black;
                            Write("[X]");

                            keepGoing = false;

                        }

                    }

                    if (keepGoing && SortedLStatkiPozycja.Count != 0 && SortedLStatkiPozycja[iloscstatkowm].x == x && SortedLStatkiPozycja[iloscstatkowm].y == y)

                    {

                        if (SortedLStatkiPozycja.Count - 1 > iloscstatkowm)
                            iloscstatkowm++;

                        ForegroundColor = ConsoleColor.DarkGreen;
                        Write("[O]");

                        // PrintStatistic(x, y, navyAsset, true);
                        keepGoing = false;
                        //continue;

                    }

                    if (keepGoing)
                    {
                        ForegroundColor = ConsoleColor.Blue;
                        Write("[~]");
                    }


                    PrintStatistic(x, y, pzasoby);

                }


            }
            catch (Exception e)
            {
                string error = e.Message.ToString();
            }
        }

        static Position AnalyzeInput(string input, Dictionary<char, int> kordy)
        {
            Position pos = new Position();

            char[] inputSplit = input.ToUpper().ToCharArray();


            if (inputSplit.Length < 2 || inputSplit.Length > 4)
            {
                return pos;
            }




            if (kordy.TryGetValue(inputSplit[0], out int value))
            {
                pos.x = value;
            }
            else
            {
                return pos;
            }


            if (inputSplit.Length == 3)
            {

                if (inputSplit[1] == '1' && inputSplit[2] == '0')
                {
                    pos.y = 10;
                    return pos;
                }
                else
                {
                    return pos;
                }

            }


            if (inputSplit[1] - '0' > 9)
            {
                return pos;
            }
            else
            {
                pos.y = inputSplit[1] - '0';
            }

            return pos;
        }

        static void naglowek()
        {
            ForegroundColor = ConsoleColor.DarkYellow;
            Write("[ ]");
            for (int i = 1; i < 11; i++)
                Write("[" + i + "]");


        }


        static Dictionary<char, int> PopulateDictionary()
        {
            Dictionary<char, int> Coordinate =
                     new Dictionary<char, int>
                     {
                     { 'A', 1 },
                     { 'B', 2 },
                     { 'C', 3 },
                     { 'D', 4 },
                     { 'E', 5 },
                     { 'F', 6 },
                     { 'G', 7 },
                     { 'H', 8 },
                     { 'I', 9 },
                     { 'J', 10 }
                     };

            return Coordinate;
        }

        static void Commentator(zasoby zasoby, bool MojStatek)
        {

            string title = MojStatek ? "Twój" : "Przeciwnik";

            if (zasoby.CheckPBattleship && zasoby.IsBattleshipzatopiony)
            {
                ForegroundColor = ConsoleColor.DarkRed;
                WriteLine("{0} {1} jest zatopiony", title, nameof(zasoby.Battleship));
                zasoby.CheckPBattleship = false;
            }

            if (zasoby.CheckCarrier && zasoby.IsCarrierzatopiony)
            {
                ForegroundColor = ConsoleColor.DarkRed;
                WriteLine("{0} {1} jest zatopiony", title, nameof(zasoby.Carrier));
                zasoby.CheckCarrier = false;
            }

            if (zasoby.CheckDestroyer && zasoby.IsDestroyerzatopiony)
            {
                ForegroundColor = ConsoleColor.DarkRed;
                WriteLine("{0} {1} jest zatopiony", title, nameof(zasoby.Destroyer));
                zasoby.CheckDestroyer = false;
            }

            if (zasoby.CheckPatrolBoat && zasoby.IsPatrolBoatzatopiony)
            {
                ForegroundColor = ConsoleColor.DarkRed;
                WriteLine("{0} {1} jest zatopiony", title, nameof(zasoby.PatrolBoat));
                zasoby.CheckPatrolBoat = false;
            }

            if (zasoby.CheckSubmarine && zasoby.IsSubmarinezatopiony)
            {
                ForegroundColor = ConsoleColor.DarkRed;
                WriteLine("{0} {1} jest zatopiony", title, nameof(zasoby.Submarine));
                zasoby.CheckSubmarine = false;
            }

        }
    }
}
