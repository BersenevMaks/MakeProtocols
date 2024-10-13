//using Word = Microsoft.Office.Interop.Word;
using Spire.Doc;
using Spire.Doc.Documents;
using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
//using System.Windows;

namespace MakeProtocols
{
    public class WordMaker
    {
        ObservableCollection<Automat> Automats = new ObservableCollection<Automat>();
        ProtocolDocument protocolDocument = new ProtocolDocument();

        string directoryPath = Environment.CurrentDirectory + "\\";
        string protocolName = "";

        public WordMaker(ObservableCollection<Automat> automats, ProtocolDocument _protocolDocument)
        {
            //Заполнение списка автоматов из конструктора
            for (int i = 0; i < automats.Count; i++)
                Automats.Add(automats[i].Clone());

            //Создание Общих сведений о протоколе из конструктора
            protocolDocument = _protocolDocument;

            //Создание директории для протоколов одного объекта (одного МСС)
            if (protocolDocument != null)
            {
                if (!string.IsNullOrEmpty(protocolDocument.ObjectProt))
                {
                    directoryPath += protocolDocument.ObjectProt + "\\";
                    if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);
                }
            }

            //Полный путь будущего файла протокола
            Regex regex = new Regex(@"(?<=\d\d\.)\d\d(?=[Ээ])");
            protocolName = "№" + regex.Match(protocolDocument.Shifr).Value + " Протокол наладки модуля универсального" + protocolDocument.Shifr + ".docx";
        }

        //Создание файла Word и общие настройки
        public void CreateWordFile()
        {
            Document doc = new Document();
            Section section = doc.AddSection();

            try
            {
                section.PageSetup.Margins.All = 1f;
                section.PageSetup.Margins.Left = 2f;

                //Верхний колонтитул с номером страницы
                HeaderFooter headersFooter = section.HeadersFooters.Header;
                Paragraph paragraphHeader = headersFooter.AddParagraph();
                paragraphHeader.AppendField("page number", FieldType.FieldPage);
                paragraphHeader.Format.HorizontalAlignment = HorizontalAlignment.Center;

                //Форматирование колонтитула со страницей
                ParagraphStyle style = new ParagraphStyle(doc);
                style.CharacterFormat.Bold = true;
                style.CharacterFormat.FontName = "Times New Roman";
                style.CharacterFormat.FontSize = 18;
                style.CharacterFormat.TextColor = Color.Red;
                doc.Styles.Add(style);
                paragraphHeader.ApplyStyle(style);



            }

            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Ошибка при создании файла Word в классе WordMaker\n\n" + ex.ToString(), "Ошибка!");
            }

            doc.SaveToFile(directoryPath + protocolName);

            doc.Dispose();
        }


        //Начало 
    }
}
