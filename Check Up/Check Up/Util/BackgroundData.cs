using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Check_Up.Util {
 
    public class BackgroundData : IEquatable<BackgroundData> {
        
        // Ex. CPU, Mem, C:
        private string counterName;

        private float cpu;

        // Ex. 888, 1001, 90.2
        private float mem;

        public string CounterName {
            get { return counterName; }
            set { counterName = value; }
        }

        public float Cpu {
            get { return cpu; }
            set { cpu = value; }
        }

        public float Mem {
            get { return mem; }
            set { mem = value; }
        }

        public bool Equals(BackgroundData other) {
            if (other == null) {
                return false;
            }

            if (this.CounterName == other.counterName) {
                return true;
            }
            else {
                return false;
            }
        }

        public override bool Equals(Object obj) {
            if (obj == null)
                return false;

            BackgroundData backgroundDataObj = obj as BackgroundData;
            if (backgroundDataObj == null)
                return false;
            else
                return Equals(backgroundDataObj);
        }   


    }
}
