using System;
using System.Collections.Generic;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;


namespace RandomlyCuttingSheet
{
    class Plate
    {
        /// <summary>
        /// Создание прямоугольной пластины
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="length"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static ContourPlate MainPlate(Point startPoint, double length, double width)
        {
            var contourPoint1 = new ContourPoint(startPoint, null);
            var contourPoint2 = new ContourPoint(new Point(startPoint.X + length, startPoint.Y, startPoint.Z), null);
            var contourPoint3 = new ContourPoint(new Point(startPoint.X + length, startPoint.Y + width, startPoint.Z), null);
            var contourPoint4 = new ContourPoint(new Point(startPoint.X, startPoint.Y + width, startPoint.Z), null);
            
            var plate = new ContourPlate();

            plate.Name = "Главная пластина";
            plate.Profile.ProfileString = "—6";
            plate.Material.MaterialString = "С255";
            plate.Class = "1";
            plate.AddContourPoint(contourPoint1);
            plate.AddContourPoint(contourPoint2);
            plate.AddContourPoint(contourPoint3);
            plate.AddContourPoint(contourPoint4);
            return plate;
           
        }
        
    }
}
