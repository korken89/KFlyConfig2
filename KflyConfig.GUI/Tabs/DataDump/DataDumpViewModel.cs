using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapControl;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace KFly.GUI
{

    public class ChoiseKFlyCommand : VMBase
    {
        private KFlyCommandType _type;

        public KFlyCommandType Type
        {
            get { return _type; }
            set { _type = value; NotifyPropertyChanged("Type"); }
        }
        private Boolean _isChecked;

        public Boolean IsChecked
        {
            get { return _isChecked; }
            set { _isChecked = value; NotifyPropertyChanged("IsChecked"); }
        }

        public String Name
        {
            get { return Enum.GetName(typeof(KFlyCommandType), _type); }
        }

    }
    /// <summary>
    /// This is the viewmodel used for the navigation tab
    /// (data behind)
    /// </summary>
    public class DataDumpViewModel : VMBase
    {
        private ObservableCollection<ChoiseKFlyCommand> _collection;

        public ObservableCollection<ChoiseKFlyCommand> Collection
        {
            get { return _collection; }
            set { _collection = value; NotifyPropertyChanged("Collection"); }
        }

        private Boolean _running = false;

        public Boolean Running
        {
            get { return _running; }
            set { _running = value; NotifyPropertyChanged("Running"); }
        }

        private DateTime _start;

        public DateTime Start
        {
            get { return _start; }
            set { _start = value; NotifyPropertyChanged("Start"); }
        }

        private DateTime _stop;

        public DateTime Stop
        {
            get { return _stop; }
            set { _stop = value; NotifyPropertyChanged("Stop"); }
        }

        private TimeSpan RunTime
        {
            get
            {
                if (Running)
                    return DateTime.Now - _start;
                else
                    return _stop - _start;
            }
        }

        private ConcurrentStack<KeyValuePair<DateTime,KFlyCommand>> _data;

        public ConcurrentStack<KeyValuePair<DateTime, KFlyCommand>> Data
        {
            get { return _data; }
            set { 
                _data = value; 
                NotifyPropertyChanged("Data"); 
                NotifyPropertyChanged("DataCount"); 
            }
        }

        private HashSet<KFlyCommandType> _subscriptions;

        public HashSet<KFlyCommandType> Subscriptions
        {
            get { return _subscriptions; }
            set { _subscriptions = value; NotifyPropertyChanged("Subscriptions"); }
        }

        private UInt32 _deltaTime;

        public UInt32 DeltaTime
        {
            get { return _deltaTime; }
            set { _deltaTime = value; NotifyPropertyChanged("DeltaTime"); }
        }


        public int DataCount
        {
            get
            {
                return _data.Count;
            }
        }


        public DataDumpViewModel()
        {
            _collection = new ObservableCollection<ChoiseKFlyCommand>();
            foreach (KFlyCommandType type in Enum.GetValues(typeof(KFlyCommandType)))
            {
                _collection.Add(new ChoiseKFlyCommand()
                {
                    Type = type,
                    IsChecked = false
                });
            }
            _subscriptions = new HashSet<KFlyCommandType>();
        }
    }

}
