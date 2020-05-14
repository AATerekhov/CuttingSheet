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

        public override void Add(ref RandomlyPlate plate)
        {
            if (heightColumn == 0)
            {
                plate.YOffset = CeilingOffsett - plate.Height;
                plate.XOffset = ColumnOffset - plate.Width;
                ColumnPlate.TransferCoordinates(ref plate);

                Plates.Add(plate);
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

        public override int Rapprochement(RandomlyPlate plate)
        {
            var rapprochement = 0;

            plate.YOffset = CeilingOffsett - heightColumn - plate.Height;
            plate.XOffset = ColumnOffset - plate.Width;

            TransferCoordinates(ref plate);


            while (!Helper.GetIntersectionPlate(plate.ContourPoints, Plates[Plates.Count - 1].ContourPoints))
            {
                plate.YOffset++;
                TransferCoordinates(ref plate);
                rapprochement++;
            }
            plate.YOffset--;
            TransferCoordinates(ref plate);
            rapprochement--;

            return rapprochement;
        }

        public override int Rapprochement(ref RandomlyPlate plate)
        {
            var rapprochement = 0;

            plate.YOffset = CeilingOffsett - heightColumn - plate.Height;
            plate.XOffset = ColumnOffset - plate.Width;

            TransferCoordinates(ref plate);


            while (!Helper.GetIntersectionPlate(plate.ContourPoints, Plates[Plates.Count - 1].ContourPoints))
            {
                plate.YOffset++;
                TransferCoordinates(ref plate);
                rapprochement++;
            }
            plate.YOffset--;
            TransferCoordinates(ref plate);
            rapprochement--;

            return rapprochement;
        }
    }
}
