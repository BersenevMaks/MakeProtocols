using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeProtocols
{
    public class Relay
    {
        string idRelay;
        string typeRelay;

        public string IDrelay {
            get { return idRelay; }
            set { idRelay = value; }
        }
        public string TypeRelay {
            get { return typeRelay; }
            set { typeRelay = value; }
        }
    }
}
