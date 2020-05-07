using System;
using System.Collections.Generic;
using System.Linq;

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
                    rooms.First().Add(item);
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
                        if (room.CapacityCheck(item))
                        {
                            if (room.Number == item.BestColumn)
                            {
                                room.Add(item);
                                break;
                            }
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
                        rooms.Last().Add(item);
                        columAdd = false;
                    }
                }
                //Перенос координат углов многоугольника
                for (int i = 0; i < item.ContourPoints.Count; i++)
                {
                    item.ContourPoints[i].X += item.XOffset;
                    item.ContourPoints[i].Y += item.YOffset;
                }
            }

            return orderByPlateList;
        }
    }
}
