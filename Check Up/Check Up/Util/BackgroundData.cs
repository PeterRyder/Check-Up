using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Check_Up.Util {
    public class BackgroundDataList : ObservableCollection<BackgroundData> {

        public BackgroundDataList()
            : base() {

        }

    }

    public class BackgroundData {
        
        // Ex. CPU, Mem, C:
        private string counterName { get; set; }

        // Ex. 10.1, 99.9
        private float data { get; set; }

    }
}
