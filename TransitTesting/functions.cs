using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoByPDX
{
    public static class functions
    {
        public static string myComboVal(object combo1, object combo2)
        {
            string outItem = null;
            if (combo1 != null)
            {
                outItem = combo1.ToString();
            }
            else if (combo2 != null)
            {
                outItem = combo2.ToString();
            }
            return outItem;
        }

        public static object PopulateComboIndex(object combo1, object combo2)
        {
            object comboIndex = null;
            if (Convert.ToInt32(combo1) > -1)
            {
                comboIndex = combo1;
            }
            else if (Convert.ToInt32(combo2) > -1)
            {
                comboIndex = combo2;
            }
            return comboIndex;
            //App.routeListViewModel.routeComboIndex = routeComboBox.SelectedIndex;
            //App.routeListViewModel.dirComboIndex = directionComboBox.SelectedIndex;
            //App.routeListViewModel.stopComboIndex = stopsComboBox.SelectedIndex;
        }

        public static bool whichBox(object combo1)
        {
            bool view = false;
            if (Convert.ToInt32(combo1) > -1)
            {
                view = true;
            }

            return view;
            //App.routeListViewModel.routeComboIndex = routeComboBox.SelectedIndex;
            //App.routeListViewModel.dirComboIndex = directionComboBox.SelectedIndex;
            //App.routeListViewModel.stopComboIndex = stopsComboBox.SelectedIndex;
        }

        public static double findMax(double val1, double val2)
        {
            double maxVal = val2;
            if(val1 > val2)
            {
                maxVal = val1;
            }
            return maxVal;
        }
        public static double findMin(double val1, double val2)
        {
            double minVal = val2;
            if (val1 < val2)
            {
                minVal = val1;
            }
            return minVal;
        }
    }
}
