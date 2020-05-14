using System;
using System.Collections.Generic;
using System.Linq;
using Tekla.Structures.Geometry3d;

namespace RandomlyCuttingSheet
{
    class Helper
    {
        
        public static IOrderedEnumerable<RandomlyPlate> SortingFCNR(IOrderedEnumerable<RandomlyPlate> orderByPlateList, int widthMainPlate)
        {
            List<Room> rooms = new List<Room>();
            
            rooms.Add(new Room(1, 0, widthMainPlate));

            bool columAdd = false;

            foreach (var item in orderByPlateList)
            {
                if (rooms.First().Count == 0)
                {
                    item.TypeSurface = 1;
                    rooms.First().Add( item);
                }
                else
                {
                    //нахождение наилучшего столбца
                    item.BestColumn = -1;
                    item.TypeSurface = 0;
                    int typeSurface;
                    foreach (var room in rooms)
                    {
                        if (room.CapacityCheck(item,out typeSurface))
                        {
                            if (item.BestColumn == -1)
                            {
                                item.BestColumn = room.Number;
                                item.TypeSurface = typeSurface;
                            }
                            else if ((rooms[item.BestColumn - 1].FullnessFloor < room.FullnessFloor) && (typeSurface == 1) && (item.TypeSurface == typeSurface))
                            {
                                item.BestColumn = room.Number;
                            }
                            else if ((rooms[item.BestColumn - 1].FullnessCeiling < room.FullnessCeiling) && (typeSurface == 2) && (item.TypeSurface == typeSurface))
                            {
                                item.BestColumn = room.Number;
                            }
                        }
                    }
                    //размещение в столбце или создание нового
                    foreach (var room in rooms)
                    {
                        if (room.Number == item.BestColumn)
                        {
                            room.Add(item);
                            break;
                        }
                        else
                        {
                            if ((room.Number) != rooms.Count)
                            {
                                continue;
                            }
                            else
                            {
                                columAdd = true;
                                break;
                            }
                        }
                    }
                    //создание нового столбца
                    if (columAdd)
                    {
                        rooms.Add(new Room(rooms.Last().Number + 1,rooms.Last().Displacement(), widthMainPlate));
                        item.TypeSurface = 1;
                        rooms.Last().Add(item);
                        columAdd = false;
                    }
                }
                
            }

            return orderByPlateList;
        }

        /// <summary>
        /// Определение пересекаются ли многоугольники.
        /// </summary>
        /// <param name="pointListOne"></param>
        /// <param name="pointListTwo"></param>
        /// <returns></returns>
        public static bool GetIntersectionPlate(List<Point> pointListOne, List<Point> pointListTwo)
        {
            var intersectionPlate = false;

            List<LineSegment> segmentListOne = TransformationOfPointsInLineSegment(pointListOne);
            List<LineSegment> segmentListTwo = TransformationOfPointsInLineSegment(pointListTwo);

            foreach (var item in segmentListOne)
            {
                foreach (var segmentTwo in segmentListTwo)
                {
                    intersectionPlate = IntersectionSegmentToSegment(item, segmentTwo);
                    if (intersectionPlate)
                    {
                        break;
                    }
                }
                if (intersectionPlate)
                {
                    break;
                }

            }

            
            return intersectionPlate;
        }

        /// <summary>
        /// Из списка точек возвращает список отрезков.
        /// </summary>
        /// <param name="pointList"></param>
        /// <returns></returns>
        public static List<LineSegment> TransformationOfPointsInLineSegment(List<Point> pointList)
        {
            List<LineSegment> segmentList = new List<LineSegment>();
            var i = 1;
            foreach (var point in pointList)
            {
                if (i != pointList.Count())
                {
                    segmentList.Add(new LineSegment(point,pointList[i]));
                }
                else
                {
                    segmentList.Add(new LineSegment(point, pointList[0]));
                    break;
                }
                i++;
            }
            return segmentList;
        }

        /// <summary>
        /// Определение пересекаются ли отрезки.
        /// возвращает bool.
        /// </summary>
        /// <param name="segmentOne"></param>
        /// <param name="segmentTwo"></param>
        /// <returns></returns>
        public static bool IntersectionSegmentToSegment(LineSegment segmentOne, LineSegment segmentTwo)
        {
            
            Vector vectorOne = new Vector(segmentOne.Point2.X - segmentOne.Point1.X, segmentOne.Point2.Y - segmentOne.Point1.Y, segmentOne.Point2.Z - segmentOne.Point1.Z);
            Vector vectorTwo = new Vector(segmentTwo.Point2.X - segmentTwo.Point1.X, segmentTwo.Point2.Y - segmentTwo.Point1.Y, segmentTwo.Point2.Z - segmentTwo.Point1.Z);

            Vector vectorZ = vectorOne.Cross(vectorTwo);

            GeometricPlane planeOne = new GeometricPlane(segmentOne.Point1, vectorOne, vectorZ);
            GeometricPlane planeTwo = new GeometricPlane(segmentTwo.Point1,vectorTwo,vectorZ);

            bool conditionOne;
            bool conditionTwo;
            var pointIntersectionOne = Intersection.LineSegmentToPlane(segmentOne, planeTwo);
            var pointIntersectionTwo = Intersection.LineSegmentToPlane(segmentTwo,planeOne);
            if (pointIntersectionOne == null)
            {
                conditionOne = false;
            }
            else 
            {
                conditionOne = true;
            }

            if (pointIntersectionTwo == null)
            {
                conditionTwo = false;
            }
            else 
            {
                conditionTwo = true;
            }

            if (conditionOne && conditionTwo)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
