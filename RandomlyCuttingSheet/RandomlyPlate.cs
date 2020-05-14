using System;
using System.Collections.Generic;
using Tekla.Structures.Geometry3d;

namespace RandomlyCuttingSheet
{
    public class RandomlyPlate
    {
        public int NumVertices { get;}

        public int Width { get;}

        public int Height { get;}

        public int BestColumn { get; set; }

        public int TypeSurface { get; set; }//1-Floor; 2-Ceiling

        public int Number { get; set; }

        public int XOffset { get; set; }

        public int YOffset { get; set; }

        private Point startPoint;
        private double widthRegion;
        private double heightRegion;
        

        public List<Point> ContourPoints { get; set; }

        public RandomlyPlate(Point point, Random rand)
        {
            NumVertices = rand.Next(3,8);
            startPoint = point;
            widthRegion = rand.Next(200, 500);
            heightRegion = rand.Next(500, 1000);


            // Выбор случайных радиусов.
            double[] radii = new double[NumVertices];
            const int min_radius = 50;
            const int max_radius = 100;
            for (int i = 0; i < NumVertices; i++)
            {
                radii[i] = rand.Next(min_radius, max_radius) / 100.0;

            }

            // Выбор случайных угловых весов.
            int[] angle_weights = new int[NumVertices];
            const int min_weight = 1;
            const int max_weight = 10;
            double total_weight = 0;
            for (int i = 0; i < NumVertices; i++)
            {
                angle_weights[i] = rand.Next(min_weight, max_weight);
                total_weight += angle_weights[i];
            }

            // Преобразование весов во фракции 2 * Pi радианов.
            double[] angles = new double[NumVertices];
            double to_radians = 2 * Math.PI / total_weight;
            for (int i = 0; i < NumVertices; i++)
            {
                angles[i] = angle_weights[i] * to_radians;
            }

            double rx = widthRegion / 2;
            double ry = heightRegion / 2;
            double cx = startPoint.X;
            double cy = startPoint.Y;

            double theta = 0;
            //Вычисление координат узлов.
            int[] xCoordinate = new int[NumVertices];
            int xMin = 0;
            int xMax = 0;
            int[] yCoordinate = new int[NumVertices];
            int yMin = 0;
            int yMax = 0;
            for (int i = 0; i < NumVertices; i++)
            {
                xCoordinate[i] = (int)(rx * radii[i] * Math.Cos(theta));
                xMax = Math.Max(xMax, xCoordinate[i]);
                xMin = Math.Min(xMin, xCoordinate[i]);
                yCoordinate[i] = (int)(ry * radii[i] * Math.Sin(theta));
                yMax = Math.Max(yMax, yCoordinate[i]);
                yMin = Math.Min(yMin, yCoordinate[i]);
                theta += angles[i];
            }
            Width = xMax - xMin;
            Height = yMax - yMin;

            //Создание контурных точек для пластины.
            ContourPoints = new List<Point>();
            for (int i = 0; i < NumVertices; i++)
            {
                ContourPoints.Add( new Point(cx - xMin + xCoordinate[i], cy - yMin + yCoordinate[i], startPoint.Z));
            }
        }

    }
}
