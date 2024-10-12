﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Word;

namespace MakeProtocols
{
    public class WordMaker
    {
        ObservableCollection<Automat> Automats = new ObservableCollection<Automat>();
        ProtocolDocument protocolDocument = new ProtocolDocument();

        string path = Environment.CurrentDirectory + "\\";

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
                    path += protocolDocument.ObjectProt + "\\";
                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                }
            }

            //Полный путь будущего файла протокола
        }

        //Создание файла Word и общие настройки
        
        //Начало 
    }
}
