using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AppliSoccerClientSide.ViewModel
{
    public class PlaceViewModel : INotifyPropertyChanged
    {

        private string _name;
        private string _description;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }
        public string Description 
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged("Description");
            }
        }
        // TODO - add modelView for Position
        //public Position Position { get; set; }
        
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
