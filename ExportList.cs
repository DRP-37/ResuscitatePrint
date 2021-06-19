using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resuscitate
{
    class ExportList
    {
        private ObservableCollection<ExportData> _data = new ObservableCollection<ExportData>();

        public ObservableCollection<ExportData> Data
        {
            get { return this._data; }
        }

        public void clearList()
        {
            _data = new ObservableCollection<ExportData>();
        }
    }
}
