using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RandomlyCuttingSheet
{
    public class ColumnPlate
    {
        public List<RandomlyPlate> Plates = new List<RandomlyPlate>();

        public int Count => Plates.Count;

        public virtual void Add(ref RandomlyPlate plate)
        {
            if (heightColumn == 0)
            {
                plate.YOffset = heightColumn;
                plate.XOffset = ColumnOffset;

                TransferCoordinates(ref plate);

                Plates.Add(plate);
                widthColumn = plate.Width;
                heightColumn += plate.Height;
                Fullness = heightColumn / (double)WidthMainPlate;
            }
            else
            {
                var delta = Rapprochement(ref plate);
                heightColumn = (heightColumn + plate.Height - delta);
                Plates.Add(plate);
                Fullness = heightColumn / (double)WidthMainPlate;
            }
        }
        
        protected int widthColumn = 0; //Ширина колонки.

        protected int heightColumn = 0; //Высота колонки.

        public int HeightColumn { get { return heightColumn; } }

        public double Fullness { get; protected set; }

        public int ColumnOffset { get; set; } //Отступ колонки от начала пластины.

        public int WidthMainPlate { get; set;
        }

        public int Number { get; set; } //Номер колонки от 1.

        public ColumnPlate(int number)
        {
            Number = number;
        }

        public bool CapacityCheck(RandomlyPlate plate)
        {
            if (plate == null)
            {
                throw new ArgumentNullException(nameof(plate), "Plate is Null");
            }
            else if (heightColumn == 0 && plate.Height <= WidthMainPlate)
            {
                return true;
            }
            else if( PlannedHeight(plate) <= WidthMainPlate)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Перемещение колонки
        /// </summary>
        /// <returns></returns>
        public int Displacement()
        {
            return (widthColumn + ColumnOffset);
        }

        private int PlannedHeight( RandomlyPlate plate)
        {
            var plannedHeightColumn = heightColumn;

            if (heightColumn != 0)
            {
                var delta = Rapprochement(plate);//величина сближения
                plate.YOffset = -heightColumn + delta;
                plate.XOffset = -ColumnOffset;
                TransferCoordinates(ref plate);

                plannedHeightColumn = (plannedHeightColumn + plate.Height - delta);
                
            }
            else
            {
                plannedHeightColumn += plate.Height;
            }
            
            return plannedHeightColumn;
        }

        /// <summary>
        /// Сближение деталей по вертикали до пересечения, шаг 1 мм.
        /// Возвращает дистанцию сближения, мм.
        /// </summary>
        /// <param name="plate"></param>
        /// <returns></returns>
        public virtual int Rapprochement(ref RandomlyPlate plate)
        {
            var rapprochement = 0;

            plate.YOffset = heightColumn;
            plate.XOffset = ColumnOffset;

            TransferCoordinates(ref plate);
            

            while (!Helper.GetIntersectionPlate(plate.ContourPoints, Plates[Plates.Count-1].ContourPoints))
            {
                plate.YOffset--;
                TransferCoordinates(ref plate);
                rapprochement++;
            }
            plate.YOffset++;
            TransferCoordinates(ref plate);
            rapprochement--;

            return rapprochement;
        }

        /// <summary>
        /// Сближение пластин друг к другу по вертикале
        /// </summary>
        /// <param name="plate"></param>
        /// <returns></returns>
        public virtual int Rapprochement(RandomlyPlate plate)
        {
            var rapprochement = 0;

            plate.YOffset = heightColumn;
            plate.XOffset = ColumnOffset;

            TransferCoordinates(ref plate);


            while (!Helper.GetIntersectionPlate(plate.ContourPoints, Plates[Plates.Count - 1].ContourPoints))
            {
                plate.YOffset--;
                TransferCoordinates(ref plate);
                rapprochement++;
            }
            plate.YOffset++;
            TransferCoordinates(ref plate);
            rapprochement--;

            return rapprochement;
        }

        /// <summary>
        ///  Перенос координат углов многоугольника
        /// </summary>
        /// <param name="plate"></param>
        public static void TransferCoordinates(ref RandomlyPlate plate)
        {
            for (int i = 0; i < plate.ContourPoints.Count; i++)
            {
                plate.ContourPoints[i].X += plate.XOffset;
                plate.ContourPoints[i].Y += plate.YOffset;
            }
            plate.YOffset = 0;
            plate.XOffset = 0;
        }
    }
}
