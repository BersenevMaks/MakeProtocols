using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Word;
using System.Text.RegularExpressions;

namespace MakeProtocols
{
    public class WordMaker
    {
        ObservableCollection<Automat> Automats = new ObservableCollection<Automat>();
        ProtocolDocument protocolDocument = new ProtocolDocument();

        string directoryPath = Environment.CurrentDirectory + "\\";
        string protocolPath = "";

        public WordMaker(ObservableCollection<Automat> automats, ProtocolDocument _protocolDocument)
        {
            //Заполнение списка автоматов из конструктора
            for (int i = 0; i < automats.Count; i++)
                Automats.Add(automats[i].Clone());

            //Создание Общих сведений о протоколе из конструктора
            protocolDocument = _protocolDocument;

            //Создание директории для протоколов одного объекта (одного МСС)
            if(protocolDocument !=null)
            {
                if (!string.IsNullOrEmpty(protocolDocument.ObjectProt))
                {
                    directoryPath += protocolDocument.ObjectProt + "\\";
                    if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);
                }
            }

            //Полный путь будущего файла протокола
            Regex regex = new Regex(@"(?<=\d\d\.)\d\d(?=[Ээ])");
            protocolPath = "№" + regex.Match(protocolDocument.Shifr).Value + " Протокол наладки модуля универсального" + protocolDocument.Shifr;
        }

        //Создание файла Word и общие настройки
        void CreateWordFile()
        {
            Application application = new Application();
            Document document = application.Documents.Open(directoryPath + protocolPath);


        }


        //Начало 
    }
}
