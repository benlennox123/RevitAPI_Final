using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPI_Final
{
    public class Utils
    {
        public static List<Level> GetLevels(ExternalCommandData commandData)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;
            List<Level> levels = new FilteredElementCollector(doc)
                .OfClass(typeof(Level))
                .Cast<Level>()
                .ToList();

            return levels;
        }

        public static List<Room> GetRooms(ExternalCommandData commandData)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;
            List<Room> rooms = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_Rooms)
                .Cast<Room>()
                .ToList();

            return rooms;
        }

        public static Phase GetPhase(ExternalCommandData commandData, string phaseName)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;
            Phase phase = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_Phases)
                .Cast<Phase>()
                .Where(x=>x.Name == phaseName)
                .FirstOrDefault();

            return phase;
        }
    }
}
