using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Statki.Models
{
    class zasoby
    {
        Random random = new Random();
        private const int CARRIER = 5;
        private const int BATTLESHIP = 4;
        private const int DESTROYER = 3;
        private const int SUBMARINE = 3;
        private const int PATROLBOAT = 2;

        public zasoby()
        {

            Carrier = GeneratePosistion(CARRIER, PozAllStatkow);
            Battleship = GeneratePosistion(BATTLESHIP, PozAllStatkow);
            Destroyer = GeneratePosistion(DESTROYER, PozAllStatkow);
            Submarine = GeneratePosistion(SUBMARINE, PozAllStatkow);
            PatrolBoat = GeneratePosistion(PATROLBOAT, PozAllStatkow);
        }

        public int ZrobioneRuchy { get; set; } = 0;

        public List<Position> Carrier { get; set; }//5
        public List<Position> Battleship { get; set; }//4
        public List<Position> Destroyer { get; set; }//3
        public List<Position> Submarine { get; set; }//3
        public List<Position> PatrolBoat { get; set; }//2
        public List<Position> PozAllStatkow { get; set; } = new List<Position>();
        public List<Position> pozstrzalu { get; set; } = new List<Position>();


        public bool IsCarrierzatopiony { get; set; } = false;
        public bool IsBattleshipzatopiony { get; set; } = false;
        public bool IsDestroyerzatopiony { get; set; } = false;
        public bool IsSubmarinezatopiony { get; set; } = false;
        public bool IsPatrolBoatzatopiony { get; set; } = false;
        public bool WszystkieZniszczone { get; set; } = false;


        public bool CheckCarrier { get; set; } = true;
        public bool CheckPBattleship { get; set; } = true;
        public bool CheckDestroyer { get; set; } = true;
        public bool CheckSubmarine { get; set; } = true;
        public bool CheckPatrolBoat { get; set; } = true;

        public zasoby StatekStatus(List<Position> HitPositions)
        {

            IsCarrierzatopiony = Carrier.Where(C => !HitPositions.Any(H => C.x == H.x && C.y == H.y)).ToList().Count == 0;
            IsBattleshipzatopiony = Battleship.Where(B => !HitPositions.Any(H => B.x == H.x && B.y == H.y)).ToList().Count == 0;
            IsDestroyerzatopiony = Destroyer.Where(D => !HitPositions.Any(H => D.x == H.x && D.y == H.y)).ToList().Count == 0;
            IsSubmarinezatopiony = Submarine.Where(S => !HitPositions.Any(H => S.x == H.x && S.y == H.y)).ToList().Count == 0;
            IsPatrolBoatzatopiony = PatrolBoat.Where(P => !HitPositions.Any(H => P.x == H.x && P.y == H.y)).ToList().Count == 0;


            WszystkieZniszczone = IsCarrierzatopiony && IsBattleshipzatopiony && IsDestroyerzatopiony && IsSubmarinezatopiony && IsPatrolBoatzatopiony;
            return this;
        }


        public List<Position> GeneratePosistion(int size, List<Position> AllPosition)
        {
            List<Position> positions = new List<Position>();

            bool IsExist = false;

            do
            {
                positions = GeneratePositionRandomly(size);
                IsExist = positions.Where(AP => AllPosition.Exists(ShipPos => ShipPos.x == AP.x && ShipPos.y == AP.y)).Any();
            }
            while (IsExist);

            AllPosition.AddRange(positions);


            return positions;
        }
        public List<Position> GeneratePositionRandomly(int size)
        {
            List<Position> positions = new List<Position>();

            int direction = random.Next(1, size);

            int col = random.Next(1, 11);

            if (direction % 2 != 0)
            {
                if (row - size > 0)
                {
                    for (int i = 0; i < size; i++)
                    {
                        Position pos = new Position();
                        pos.x = row - i;
                        pos.y = col;
                        positions.Add(pos);
                    }
                }
                else
                {
                    for (int i = 0; i < size; i++)
                    {
                        Position pos = new Position();
                        pos.x = row + i;
                        pos.y = col;
                        positions.Add(pos);
                    }
                }
            }
            else
            {

                if (col - size > 0)
                {
                    for (int i = 0; i < size; i++)
                    {
                        Position pos = new Position();
                        pos.x = row;
                        pos.y = col - i;
                        positions.Add(pos);
                    }
                }
                else
                {
                    for (int i = 0; i < size; i++)
                    {
                        Position pos = new Position();
                        pos.x = row;
                        pos.y = col + i;
                        positions.Add(pos);
                    }
                }
            }
            return positions;
        }

        public zasoby Fire()
        {
            Position EnemyShotPos = new Position();
            bool alreadyShot = false;
            do
            {
                EnemyShotPos.x = random.Next(1, 11);
                EnemyShotPos.y = random.Next(1, 11);
                alreadyShot = pozstrzalu.Any(EFP => EFP.x == EnemyShotPos.x && EFP.y == EnemyShotPos.y);
            }
            while (alreadyShot);

            pozstrzalu.Add(EnemyShotPos);
            return this;
        }
        class Position
        {
            public int x { get; set; } = -1;
            public int y { get; set; } = -1;
        }
    }
}
