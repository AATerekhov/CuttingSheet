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

        public int WidthMainPlate { get; } //Ширина полубесконечной полосы.

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
        public void Add( RandomlyPlate plate)
        {
            if (plate == null)
            {
                throw new ArgumentNullException(nameof(plate), "Plate is null");
            }
            else
            {
                
                if (plate.TypeSurface == 1)
                {
                    if (Floor.Count == 0)
                    {
                        Ceiling.WidthMainPlate -= plate.Height;
                        Ceiling.ColumnOffset += plate.Width;
                        widthRoom = plate.Width;
                    }
                    Floor.Add(ref plate);
                    roomPlates.Add(plate);
                    FullnessFloor = Floor.Fullness;

                }
                else if (plate.TypeSurface == 2)
                {
                    Ceiling.Add(ref plate);
                    roomPlates.Add(plate);
                    FullnessCeiling = Ceiling.Fullness;
                }
            }
        }

        /// <summary>
        /// Проверка вместимости пола и потолка столбца.
        /// </summary>
        /// <param name="plate"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Проверка вместимости потолка
        /// </summary>
        /// <param name="plate"></param>
        /// <returns></returns>
        private bool CeilingCapacityCheck(RandomlyPlate plate, out int typeSurface)
        {
            typeSurface = 2;
            bool search = true; //Поиск.
            if (Ceiling.Plates.Count == 0)
            {
                plate.YOffset = Ceiling.CeilingOffsett - plate.Height;
                plate.XOffset = Ceiling.ColumnOffset - plate.Width;
                ColumnPlate.TransferCoordinates(ref plate);

                foreach (var item in Floor.Plates)
                {
                    if (Helper.GetIntersectionPlate(plate.ContourPoints, item.ContourPoints))
                    {
                        typeSurface = 0;
                        search = false;
                        break;
                    }
                }

                plate.YOffset = -(Ceiling.CeilingOffsett - plate.Height);
                plate.XOffset = -(Ceiling.ColumnOffset - plate.Width);
                ColumnPlate.TransferCoordinates(ref plate);
            }
            else
            {
                var delta = Ceiling.Rapprochement(plate);//величина сближения
               
                foreach (var item in Floor.Plates)
                {
                    if (Helper.GetIntersectionPlate(plate.ContourPoints, item.ContourPoints))
                    {
                        typeSurface = 0;
                        search = false;
                        break;
                    }
                }

                plate.YOffset = -(Ceiling.CeilingOffsett - Ceiling.HeightColumn - plate.Height + delta);
                plate.XOffset = -(Ceiling.ColumnOffset - plate.Width);
                ColumnPlate.TransferCoordinates(ref plate);
            }
            
            return search;
        }


        public int Displacement()
        {
            return Floor.Displacement(); //перемещение
        }

    }
}
