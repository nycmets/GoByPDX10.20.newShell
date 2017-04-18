using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoByPDX.Models.TripPlannerModels
{
    public class tripPlannerResultsViewModel : INotifyPropertyChanged
    {

        public ObservableCollection<string> bindingTripInfo1 = new ObservableCollection<string>();
        public ObservableCollection<string> tripList { get { return bindingTripInfo1; } set { this.OnPropertyChanged("bindingTripInfo1"); } }

        public tripPlannerResultsViewModel()
        {
            //tripPlanner.
            foreach (tripItinerary tripItinerary in App.tripPlanner.tripItinerary_l)
            {
                string legList = "";
                foreach (tripLeg leg in tripItinerary.tripLeg_l)
                {
                    legList = legList + leg.number + "->";
                }
                legList = legList.Remove(legList.Length - 2);
                //tripList.Add(tripItinerary.startTime + " " + tripItinerary.endTime);
                //_output = tripItinerary.startTime + " " + tripItinerary.endTime;
                //Output = tripItinerary.startTime + " " + tripItinerary.endTime;

                //bindingTripInfo1_test = tripItinerary.startTime + " " + tripItinerary.endTime;
                bindingTripInfo1.Add(tripItinerary.startTime + " ->" + tripItinerary.endTime + ":   " + legList);
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
