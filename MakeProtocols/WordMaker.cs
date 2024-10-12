using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Word;

namespace MakeProtocols
{
    public class WordMaker
    {
        ObservableCollection<Automat> Automats = new ObservableCollection<Automat>();

        string path = Environment.CurrentDirectory + "\\";

        public WordMaker(ObservableCollection<Automat> automats)
        {
            for (int i = 0; i < automats.Count; i++)
                Automats.Add(automats[i].Clone());

            if(Automats!=null && automats.Count > 0)
            {
                path+=
            }
        }

        
    }
}
