using System;
using System.Collections.Generic;

namespace RandomlyCuttingSheet
{
    public class Room
    {
        public int Number { get; set; }

        private List<RandomlyPlate> roomPlates = new List<RandomlyPlate>();

        public double FullnessFloor { get; set; }

        public double FullnessCeiling { get; set; }

        public int Count => roomPlates.Count;

        public int WidthMainPlate { get; }

        int widthRoom = 0;

        ColumnPlate Floor;
        ColumnBack Ceiling;

        public Room(int number, int offset, int widthMainPlate)
        {
            Number = number;
            WidthMainPlate = widthMainPlate;

            Floor = new ColumnPlate(Number);
            Floor.ColumnOffset = offset;
            Floor.WidthMainPlate = widthMainPlate;

            Ceiling = new ColumnBack(Number);
            Ceiling.ColumnOffset = offset;
            Ceiling.WidthMainPlate = widthMainPlate;
            Ceiling.CeilingOffsett = widthMainPlate;
        }

        /// <summary>
        /// Добавление пластины в столбец.
        /// </summary>
        /// <param name="plate"></param>
        public void Add(RandomlyPlate plate)
        {
            if (plate == null)
            {
                throw new ArgumentNullException(nameof(plate), "Plate is null");
            }
            else
            {
                roomPlates.Add(plate);
                if (Floor.CapacityCheck(plate))
                {
                    if (Floor.Count == 0)
                    {
                        Ceiling.WidthMainPlate -= plate.Height;
                        Ceiling.ColumnOffset += plate.Width;
                        widthRoom = plate.Width;
                    }
                    Floor.Add(plate);
                    FullnessFloor = Floor.Fullness;

                }
                else if (CeilingCapacityCheck(plate))
                {
                    Ceiling.Add(plate);
                    FullnessCeiling = Ceiling.Fullness;
                }
            }
        }

        /// <summary>
        /// Проверка вместимости пола и потолка столбца.
        /// </summary>
        /// <param name="plate"></param>
        /// <returns></returns>
        public bool CapacityCheck(RandomlyPlate plate)
        {
            if (plate == null)
            {
                throw new ArgumentNullException(nameof(plate), "Plate is Null");
            }
            else
            {
                if (Floor.CapacityCheck(plate))
                {
                    return true;
                }
                else
                {
                   return CeilingCapacityCheck(plate);
                }
            }
        }
        
        public bool CapacityCheck(RandomlyPlate plate, out int typeSurface)
        {
            if (plate == null)
            {
                throw new ArgumentNullException(nameof(plate), "Plate is Null");
            }
            else if (Floor.CapacityCheck(plate))
            {
                typeSurface = 1;
                return true;
            }
            else
            {
                return CeilingCapacityCheck(plate, out typeSurface);
            }
        }

        private bool CeilingCapacityCheck(RandomlyPlate plate)
        {
            bool search = false;
            int hightFloor = 0;
            foreach (var item in Floor.Plates)
            {
                if ((hightFloor + plate.Height + Ceiling.HeightColumn <= Floor.WidthMainPlate) && (item.Width + plate.Width <= widthRoom))
                {
                    search = true;
                    break;
                }
                hightFloor += item.Height;
            }

            return search;
        }

        private bool CeilingCapacityCheck(RandomlyPlate plate, out int typeSurface)
        {
            typeSurface = 0;
            bool search = false;
            int hightFloor = 0;
            foreach (var item in Floor.Plates)
            {
                if ((hightFloor + plate.Height + Ceiling.HeightColumn <= Floor.WidthMainPlate) && (item.Width + plate.Width <= widthRoom))
                {
                    typeSurface = 2;
                    search = true;
                    break;
                }
                hightFloor += item.Height;
            }
            return search;
        }

        public int Displacement()
        {
            return Floor.Displacement();
        }

    }
}
