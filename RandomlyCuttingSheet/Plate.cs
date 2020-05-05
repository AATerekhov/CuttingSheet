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
        public static ContourPlate MainPlare(Point startPoint, double length, double width)
        {
            var contourPoint1 = new ContourPoint(startPoint, null);
            var contourPoint2 = new ContourPoint(new Point(startPoint.X + length, startPoint.Y, startPoint.Z), null);
            var contourPoint3 = new ContourPoint(new Point(startPoint.X + length, startPoint.Y + width, startPoint.Z), null);
            var contourPoint4 = new ContourPoint(new Point(startPoint.X, startPoint.Y + width, startPoint.Z), null);
            
            var plate = new ContourPlate();

            plate.Name = "Главная пластина";
            plate.Profile.ProfileString = "—6";
            plate.Material.MaterialString = "С255";
            plate.Class = "4";
            plate.AddContourPoint(contourPoint1);
            plate.AddContourPoint(contourPoint2);
            plate.AddContourPoint(contourPoint3);
            plate.AddContourPoint(contourPoint4);
            return plate;
           
        }

        /// <summary>
        /// Создание произвольной пластины.
        /// </summary>
        /// <param name="num_vertices"></param>
        /// <param name="startPoint"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static ContourPlate RandomPoligon(int num_vertices, Point startPoint, double width, double height)
        {

            var rand = new Random();
            // Выбор случайных радиусов.
            double[] radii = new double[num_vertices];
            const int min_radius = 50;
            const int max_radius = 100;
            for (int i = 0; i < num_vertices; i++)
            {
                radii[i] = rand.Next(min_radius, max_radius)/100.0;

            }

            // Выбор случайных угловых весов.
            int[] angle_weights = new int[num_vertices];
            const int min_weight = 1;
            const int max_weight = 10;
            double total_weight = 0;
            for (int i = 0; i < num_vertices; i++)
            {
                angle_weights[i] = rand.Next(min_weight, max_weight);
                total_weight += angle_weights[i];
            }

            // Преобразование весов во фракции 2 * Pi радианов.
            double[] angles = new double[num_vertices];
            double to_radians = 2 * Math.PI / total_weight;
            for (int i = 0; i < num_vertices; i++)
            {
                angles[i] = angle_weights[i] * to_radians;
            }

            double rx = width / 2;
            double ry = height / 2;
            double cx = startPoint.X;
            double cy = startPoint.Y;

            double theta = 0;

            var listContourPoints = new List<ContourPoint>();
            for (int i = 0; i < num_vertices; i++)
            {
                listContourPoints.Add(new ContourPoint(new Point(cx + (int)(rx * radii[i] * Math.Cos(theta)), cy + (int)(ry * radii[i] * Math.Sin(theta)), startPoint.Z), null));
                theta += angles[i];
            }
            var plate = new ContourPlate();
            foreach (var point in listContourPoints)
            {
                plate.AddContourPoint(point);
            }

            plate.Name = "Пластина";
            plate.Profile.ProfileString = "—6";
            plate.Material.MaterialString = "С255";
            plate.Class = "6";

            return plate;

        }


    }
}
