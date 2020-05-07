using System;
using System.Collections.Generic;
using System.Linq;

namespace RandomlyCuttingSheet
{
    public class ColumnBack : ColumnPlate
    {
        public ColumnBack(int number) : base(number)
        {
            
        }

        public int CeilingOffsett { get; set; }

        public override void Add(RandomlyPlate plate)
        {
            if ((heightColumn + plate.Height) <= WidthMainPlate)
            {
                Plates.Add(plate);
                plate.YOffset = CeilingOffsett - heightColumn - plate.Height;
                plate.XOffset = ColumnOffset - plate.Width;
                heightColumn += plate.Height;
                Fullness = heightColumn / (double)WidthMainPlate;
            }
        }
    }
}
