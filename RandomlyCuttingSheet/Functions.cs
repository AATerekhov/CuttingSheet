using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;
using UI = Tekla.Structures.Model.UI;


namespace RandomlyCuttingSheet
{
    class Functions
    {
        #region Creating parts
        public static void CreatingMainPlate()
        {
            var model = new Model();

            if (model.GetConnectionStatus())
            {
                var startPoint = PickAPoint();
                
                var mainPlate = Plate.MainPlare(startPoint, 6000, 2000);
                                
                mainPlate.Insert();

            }
            model.CommitChanges();
        }

        public static void CreatingPart()
        {
            var model = new Model();

            if (model.GetConnectionStatus())
            {
                var startPoint = PickAPoint();

                var rnd = new Random();
                for (int i = 0; i < rnd.Next(5,15); i++)
                {
                    var plate1 = Plate.RandomPoligon(rnd.Next(3, 8), startPoint, rnd.Next(500, 1000), rnd.Next(1000, 2000));
                    plate1.Insert();
                    startPoint.X += 1000;
                }   
            }
            model.CommitChanges();
        }

        public static void CreatingCutting()
        {
            var model = new Model();

            if (model.GetConnectionStatus())
            {
                var startPoint = PickAPoint();

                var mainPlate = Plate.MainPlare(startPoint, 6000, 2000);
                mainPlate.Insert();

                var startPointPart = new Point(startPoint.X + 500, startPoint.Y + 1000, startPoint.Z + 6);
                var rnd = new Random();
                for (int i = 0; i < rnd.Next(5, 15); i++)
                {
                    var plate1 = Plate.RandomPoligon(rnd.Next(3, 8), startPointPart, rnd.Next(500, 1000), rnd.Next(1000, 2000));
                    plate1.Insert();
                    startPointPart.X += 1000;
                }

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
