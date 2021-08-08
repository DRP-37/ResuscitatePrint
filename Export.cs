using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Resuscitate
{
    interface Export
    {
        void ExportStatusList(string patientId, StatusList statusList, Button exportButton, TextBlock flyout);
    }
}
