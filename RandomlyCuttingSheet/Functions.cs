using System;
using System.Collections.Generic;
using System.Linq;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;
using UI = Tekla.Structures.Model.UI;


namespace RandomlyCuttingSheet
{
    class Functions
    {
        public static Random  rnd = new Random();

        #region Creating parts
      
        public static void CreatingCuttingFCNR()
        {
            var model = new Model();
            if (model.GetConnectionStatus())
            {
                var startPoint = PickAPoint();

                var startPointPart = new Point(startPoint.X, startPoint.Y, startPoint.Z + 6);

                #region Create_Main_Plate
                const int widthMainPlate = 2000;
                var mainPlate = Plate.MainPlate(startPoint, 6000, widthMainPlate);
                mainPlate.Insert();

                #endregion

                #region Создание случайных пластин
                List<RandomlyPlate> plateList = new List<RandomlyPlate>();
                for (int i = 0; i < rnd.Next(30, 60); i++)
                {
                    plateList.Add(new RandomlyPlate(startPointPart, rnd));
                }

                var orderByPlateList = plateList.OrderByDescending(RandomlyPlate => RandomlyPlate.Width);

                var n = 1;
                foreach (var item in orderByPlateList)
                {
                    item.Number = n;
                    n++;
                }

                var sortOrderByPlateList = Helper.SortingFCNR(orderByPlateList, widthMainPlate);

                foreach (var item in sortOrderByPlateList)
                {
                    var plate = new ContourPlate();
                    foreach (var point in item.ContourPoints)
                    {
                        plate.AddContourPoint(new ContourPoint(point, null));
                    }
                    plate.Name = "Пластина";
                    plate.Profile.ProfileString = "—6";
                    plate.Material.MaterialString = "С255";
                    plate.Class = $"{item.Number+1}";
                    plate.Insert();
                }
                #endregion
            }
            model.CommitChanges();
        }

        #endregion

        #region Select an item

        public static Point PickAPoint(string prompt = "Pick a point")
        {
            Point myPoint = null;
            try
            {
                var picker = new UI.Picker();
                myPoint = picker.PickPoint(prompt);
            }
            catch (Exception ex)
            {
                if (ex.Message != "User interrupt")
                {
                    Console.WriteLine(ex.Message + ex.StackTrace);
                }
            }

            return myPoint;
        }
        #endregion
 
    }
}