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

        public virtual void Add(RandomlyPlate plate)
        {
            if ((heightColumn + plate.Height) <= WidthMainPlate)
            {
                Plates.Add(plate);
                plate.YOffset = heightColumn;
                plate.XOffset = ColumnOffset;
                widthColumn = Math.Max(widthColumn, plate.Width);
                heightColumn += plate.Height;
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
            else if((heightColumn + plate.Height) <= WidthMainPlate)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int Displacement()
        {
            return (widthColumn + ColumnOffset);
        }
    }
}
