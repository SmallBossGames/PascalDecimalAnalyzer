using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ComplersCourseWork.ViewModels
{
    public class MainPageViewModel: INotifyPropertyChanged
    {
        public string InputData { get; set; } = string.Empty;
        public string OutputData { get; set; } = string.Empty;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}
