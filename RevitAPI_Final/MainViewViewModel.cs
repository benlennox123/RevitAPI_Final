using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPI_Final
{
    class MainViewViewModel
    {
        private ExternalCommandData _commandData;

        public List<Level> Levels { get; } = new List<Level>();
        public Level SelectedLevel { get; set; }

        public int StartNumber { get; set; }

        public DelegateCommand SaveCommand { get; }

        public MainViewViewModel(ExternalCommandData commandData)
        {
            _commandData = commandData;
            Levels = Utils.GetLevels(commandData);
            StartNumber = 1;


            SaveCommand = new DelegateCommand(OnSaveCommand);
        }

        private void OnSaveCommand()
        {
            UIApplication uiapp = _commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            List<Room> rooms = Utils.GetRooms(_commandData);

            using (Transaction transaction = new Transaction(doc, "Автонумерация"))
            {
                transaction.Start();

                if (rooms.Count == 0)
                {
                    string phaseName = doc.ActiveView.get_Parameter(BuiltInParameter.VIEW_PHASE).AsValueString();
                    Phase phase = Utils.GetPhase(_commandData, phaseName);

                    if(SelectedLevel != null)
                    {
                        doc.Create.NewRooms2(SelectedLevel, phase);
                    }
                    else
                    {
                        foreach (Level level in Levels)
                        {
                            doc.Create.NewRooms2(level, phase);
                        }
                    }
                    rooms = Utils.GetRooms(_commandData);
                }

                if (SelectedLevel != null)
                {
                    rooms = rooms
                        .Where(x => x.Level.Id == SelectedLevel.Id)
                        .ToList();
                }

                foreach (Room room in rooms)
                {
                    //Parameter parameter = room.get_Parameter(BuiltInParameter.ROOM_NUMBER);
                    //parameter.Set(StartNumber.ToString());
                    room.Number = StartNumber.ToString();
                    StartNumber++;
                }

                transaction.Commit();
            }
               

            RaiseCloseRequest();
        }

        public event EventHandler CloseRequest;

        private void RaiseCloseRequest()
        {
            CloseRequest?.Invoke(this, EventArgs.Empty);
        }
    }
}
