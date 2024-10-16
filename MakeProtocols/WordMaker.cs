//using Word = Microsoft.Office.Interop.Word;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
using System;
using System.Collections.Generic;
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

        const double pointsForConvert = 28.328611898;

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
            protocolName = "№" + regex.Match(protocolDocument.Shifr).Value + " Протокол наладки модуля универсального " + protocolDocument.Shifr + ".docx";
        }

        //Создание файла Word и общие настройки
        public void CreateWordFile()
        {
            Document doc = new Document();
            Section section = doc.AddSection();

            try
            {
                //Наборы стилей
                ParagraphStyle regularTextStyle_1 = new ParagraphStyle(doc);  //Обычный текст
                regularTextStyle_1.CharacterFormat.Bold = false;
                regularTextStyle_1.CharacterFormat.FontName = "Times New Roman";
                regularTextStyle_1.CharacterFormat.FontSize = 12;
                regularTextStyle_1.CharacterFormat.TextColor = Color.Black;
                regularTextStyle_1.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Left;
                doc.Styles.Add(regularTextStyle_1);

                ParagraphStyle styleKolonTitle = new ParagraphStyle(doc);  //стиль колонтитула
                styleKolonTitle.CharacterFormat.Bold = false;
                styleKolonTitle.CharacterFormat.FontName = "Times New Roman";
                styleKolonTitle.CharacterFormat.FontSize = 10;
                styleKolonTitle.CharacterFormat.TextColor = Color.Black;
                doc.Styles.Add(styleKolonTitle);

                ParagraphStyle styleTitle14 = new ParagraphStyle(doc); //Титул 14 кегля
                styleTitle14.CharacterFormat.Bold = true;
                styleTitle14.CharacterFormat.FontName = "Times New Roman";
                styleTitle14.CharacterFormat.FontSize = 14;
                styleTitle14.CharacterFormat.TextColor = Color.Black;
                styleTitle14.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Center;
                doc.Styles.Add(styleTitle14);

                ParagraphStyle styleTitle12 = new ParagraphStyle(doc); //Титул 12 кегля
                styleTitle12.CharacterFormat.Bold = true;
                styleTitle12.CharacterFormat.FontName = "Times New Roman";
                styleTitle12.CharacterFormat.FontSize = 12;
                styleTitle12.CharacterFormat.TextColor = Color.Black;
                styleTitle12.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Center;
                doc.Styles.Add(styleTitle12);

                ParagraphStyle regularStyleNumb_1 = new ParagraphStyle(doc);
                regularStyleNumb_1.CharacterFormat.Bold = false;
                regularStyleNumb_1.CharacterFormat.FontName = "Times New Roman";
                regularStyleNumb_1.CharacterFormat.FontSize = 12;
                regularStyleNumb_1.CharacterFormat.TextColor = Color.Black;
                regularStyleNumb_1.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Left;
                regularStyleNumb_1.ParagraphFormat.FirstLineIndent = Convert.ToSingle(0.63 * pointsForConvert);
                doc.Styles.Add(regularStyleNumb_1);

                ParagraphStyle regularStyleTable_1 = new ParagraphStyle(doc);
                regularStyleTable_1.Name = "regularStyleTable_1";
                regularStyleTable_1.CharacterFormat.Bold = false;
                regularStyleTable_1.CharacterFormat.FontName = "Times New Roman";
                regularStyleTable_1.CharacterFormat.FontSize = 11;
                regularStyleTable_1.CharacterFormat.TextColor = Color.Black;
                regularStyleTable_1.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Center;
                regularStyleTable_1.ParagraphFormat.FirstLineIndent = Convert.ToSingle(0 * pointsForConvert);
                doc.Styles.Add(regularStyleTable_1);

                ParagraphStyle regularStyleTable_2 = new ParagraphStyle(doc);
                regularStyleTable_2.Name = "regularStyleTable_2";
                regularStyleTable_2.CharacterFormat.Bold = false;
                regularStyleTable_2.CharacterFormat.FontName = "Times New Roman";
                regularStyleTable_2.CharacterFormat.FontSize = 10;
                regularStyleTable_2.CharacterFormat.TextColor = Color.Black;
                regularStyleTable_2.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Center;
                regularStyleTable_2.ParagraphFormat.FirstLineIndent = Convert.ToSingle(0 * pointsForConvert);
                doc.Styles.Add(regularStyleTable_2);

                section.PageSetup.Margins.All = Convert.ToSingle(1 * pointsForConvert); //Перевод в сантиметры
                section.PageSetup.Margins.Left = Convert.ToSingle(2 * pointsForConvert);

                //Верхний колонтитул с номером страницы
                HeaderFooter headersFooter = section.HeadersFooters.Header;
                Paragraph paragraphHeader = headersFooter.AddParagraph();
                paragraphHeader.AppendField("page number", FieldType.FieldPage);
                paragraphHeader.Format.HorizontalAlignment = HorizontalAlignment.Center;
                paragraphHeader.ApplyStyle(styleKolonTitle);

                //Создание параграфа с таблицей общей информации организации и объекта
                string[] data = {
                    @"Лаборатория: ООО  «ЭНКОН» 28 - ЭТЛ / 765. 454000, Челябинск, Свердловский проспект, д.2, оф.325А",
                    @"Решение о регистрации электролаборатории рег. №28-ЭТЛ\765 от «25» октября 2021 г.",
                    @"Заказчик: ООО «АмурМинералс»",
                    @"Объект: "+ protocolDocument.ObjectProt,
                    @"Дата: " + protocolDocument.DateProt
                    };

                Table orgTable = section.AddTable(true);
                orgTable.ResetCells(5, 1);
                for (int i = 0; i < data.Length; i++)
                {
                    TableRow tr = orgTable.Rows[i];
                    Paragraph p = tr.Cells[0].AddParagraph();
                    tr.Cells[0].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                    p.Format.HorizontalAlignment = HorizontalAlignment.Left;
                    TextRange txtRange = p.AppendText(data[i]);
                    txtRange.CharacterFormat.FontName = "Times New Roman";
                    txtRange.CharacterFormat.FontSize = 11;
                }

                //Параграф с шапкой протокола
                section.AddParagraph();
                section.AddParagraph();
                Paragraph paragraph = section.AddParagraph();
                paragraph.AppendText("ПРОТОКОЛ №" + protocolDocument.NumbProt);
                paragraph.ApplyStyle(styleTitle14);

                paragraph = section.AddParagraph();
                paragraph.AppendText("Наладки модулей унифицированных");
                paragraph.ApplyStyle(styleTitle12);

                paragraph = section.AddParagraph();
                paragraph.AppendText(protocolDocument.Modules);
                paragraph.ApplyStyle(styleTitle12);

                section.AddParagraph();
                section.AddParagraph();

                //Создание автоматического списка с общими данными протокола

                doc.Styles.Add(regularStyleNumb_1);

                data = new string[] {
                    "1.  Протокол касается только объекта, подвергнутого измерениям.",
                    "2.  Протокол оформлен в соответствии с требованиями ГОСТ Р 50571.16-2007.",
                    "3.  Проектная документация: Шифр 3760-4.1.12-ЭМ АО Механобр Инжиниринг.",
                    "4.  Должно соответствовать требованиям ПУЭ 1.8, инструкции по эксплуатации.",
                    "5.  Климатические условия проведения испытаний: температура +20 °С",
                    "6.  Цель проведения испытания: приемо-сдаточные.",
                    "7.  Общие данные:",
                    "    Принципиальные схемы АО «Электронмаш». " + protocolDocument.Shifr,
                    "    Номинальное напряжение 0,4 кВ",
                    "8.  Испытание оборудования",
                    "    8.1. Паспортные данные оборудования",
                    "    8.1.1. Данные автоматических выключателей",

                };
                for (int i = 0; i < data.Length; i++)
                {
                    paragraph = section.AddParagraph();
                    paragraph.AppendText(data[i]);
                    paragraph.ApplyStyle(regularStyleNumb_1);
                }

                //Таблица данных автоматических выключателей

                doc.Styles.Add(regularStyleTable_1);

                section.AddParagraph();
                Table commonAutomatsTable = section.AddTable(true);
                commonAutomatsTable.ResetCells(Automats.Count + 1, 6);

                string[] Header = { @"Фидер/монтаж. символ", "Тип", "Зав. № автомата", "Ном. Ток, А", "Ном. напряж., кВ", "Тип расцепителя" };
                for (int c = 0; c < Header.Length; c++)
                {
                    TableRow tr = commonAutomatsTable.Rows[0];
                    Paragraph p = tr.Cells[c].AddParagraph();
                    tr.Cells[c].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                    p.Format.HorizontalAlignment = HorizontalAlignment.Center;
                    TextRange txtRange = p.AppendText(Header[c]);
                    p.ApplyStyle(regularStyleTable_1);
                }


                for (int r = 0; r < Automats.Count; r++)
                {
                    string[] rowDataCommon = {
                        Automats[r].NameAutomat + "\r" + Automats[r].Description,
                        Automats[r].Type,
                        Automats[r].VendorNumb,
                        Automats[r].NominalCurrent,
                        Automats[r].NominalVoltage,
                        Automats[r].TypeBreaker
                    };
                    TableRow tr = commonAutomatsTable.Rows[r + 1];
                    for (int c = 0; c < rowDataCommon.Length; c++)
                    {
                        Paragraph p = tr.Cells[c].AddParagraph();
                        tr.Cells[c].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                        p.Format.HorizontalAlignment = HorizontalAlignment.Center;
                        TextRange txtRange = p.AppendText(rowDataCommon[c]);
                        p.ApplyStyle(regularStyleTable_1);
                    }

                }

                //Параграфы с общими данными и шифром карты уставок
                data = new string[]{
                    "8.2.Проверка автоматических выключателей",
                    "8.2.1.Автоматические выключатели осмотрены, не имеют механических повреждений.Визуально проверено состояние монтажа, целостность комплектующих изделий. Также проверена затяжка силовых контактных соединений. Состояние автоматических выключателей соответствует заводским нормам.",
                    "8.2.2.Испытание автоматических выключателей.",
                    "Уставки выставлены в соответствии с Таблицей уставок защит АВ МСС, шифр " + protocolDocument.KartUstav + "."
                };
                for (int i = 0; i < data.Length; i++)
                {
                    paragraph = section.AddParagraph();
                    paragraph.AppendText(data[i]);
                    paragraph.ApplyStyle(regularStyleNumb_1);
                }

                //Таблица с уставками для автоматов
                section.AddParagraph();
                Table ustavAutomatsTable = section.AddTable(true);
                ustavAutomatsTable.ResetCells(Automats.Count + 2, 10);
                ustavAutomatsTable.ApplyHorizontalMerge(0, 2, 3); //строка, столбец, конечный столбец
                ustavAutomatsTable.ApplyHorizontalMerge(0, 4, 5);
                ustavAutomatsTable.ApplyHorizontalMerge(0, 6, 7);
                ustavAutomatsTable.ApplyHorizontalMerge(0, 8, 9);

                ustavAutomatsTable.ApplyVerticalMerge(0, 0, 1);  //столбец, строка, конечная строка
                ustavAutomatsTable.ApplyVerticalMerge(1, 0, 1);

                Header = new string[] { @"Фидер", "Iном, А", "Ir", "", "Isd", "", "Ii", "", "Ig", "" }; //первая строка заголовока
                for (int c = 0; c < Header.Length; c++)
                {
                    TableRow tr = ustavAutomatsTable.Rows[0];
                    Paragraph p = tr.Cells[c].AddParagraph();
                    tr.Cells[c].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                    p.Format.HorizontalAlignment = HorizontalAlignment.Center;
                    TextRange txtRange = p.AppendText(Header[c]);
                    p.ApplyStyle(regularStyleTable_1);
                }
                Header = new string[] { @"", "", "Ir, А", "tr, с", "Isd, А", "tsd, с", "Ii, А", "ti, с", "Ig, А", "tg, с" }; //вторая строка заголовка
                for (int c = 0; c < Header.Length; c++)
                {
                    TableRow tr = ustavAutomatsTable.Rows[1];
                    Paragraph p = tr.Cells[c].AddParagraph();
                    tr.Cells[c].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                    p.Format.HorizontalAlignment = HorizontalAlignment.Center;
                    TextRange txtRange = p.AppendText(Header[c]);
                    p.ApplyStyle(regularStyleTable_1);
                }

                for (int r = 0; r < Automats.Count; r++)
                {
                    string[] rowData = {
                        Automats[r].NameAutomat + "\r" + Automats[r].Description,
                        Automats[r].NominalCurrent,
                        Automats[r].Ust_Ir,
                        Automats[r].Ust_Tr,
                        Automats[r].Ust_Isd,
                        Automats[r].Ust_Tsd,
                        Automats[r].Ust_Ii,
                        Automats[r].Ust_Ti,
                        Automats[r].Ust_Ig,
                        Automats[r].Ust_Tg
                    };
                    TableRow tr = ustavAutomatsTable.Rows[r + 2];
                    Paragraph p;
                    TextRange txtRange;
                    for (int c = 0; c < rowData.Length; c++)
                    {
                        p = tr.Cells[c].AddParagraph();
                        tr.Cells[c].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                        p.Format.HorizontalAlignment = HorizontalAlignment.Center;
                        txtRange = p.AppendText(rowData[c]);
                        p.ApplyStyle(regularStyleTable_1);
                    }
                }

                //Таблица Результаты испытаний 
                section.AddParagraph();

                paragraph = section.AddParagraph();
                paragraph.AppendText("Результаты испытаний:");
                paragraph.ApplyStyle(regularTextStyle_1);

                Table ispytanAutomatsTable = section.AddTable(true);
                ispytanAutomatsTable.ResetCells(Automats.Count * 3 + 2, 12);
                ispytanAutomatsTable.ApplyHorizontalMerge(0, 3, 5); //строка, столбец, конечный столбец
                ispytanAutomatsTable.ApplyHorizontalMerge(0, 6, 8);
                ispytanAutomatsTable.ApplyHorizontalMerge(0, 9, 11);

                ispytanAutomatsTable.ApplyVerticalMerge(0, 0, 1);  //столбец, строка, конечная строка
                ispytanAutomatsTable.ApplyVerticalMerge(1, 0, 1);
                ispytanAutomatsTable.ApplyVerticalMerge(2, 0, 1);




                Header = new string[] { @"Фидер", "Фаза", "Ток ном, А", "Ir", "", "", "Isd", "", "", "Ii", "", "" }; //первая строка заголовока

                for (int c = 0; c < Header.Length; c++)
                {
                    TableRow tr = ispytanAutomatsTable.Rows[0];
                    Paragraph p = tr.Cells[c].AddParagraph();
                    tr.Cells[c].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                    p.Format.HorizontalAlignment = HorizontalAlignment.Center;
                    TextRange txtRange = p.AppendText(Header[c]);
                    p.ApplyStyle(regularStyleTable_2);
                }
                Header = new string[] { @"", "", "", "Ток уставки, А", "Ток проверки, А", "Время сраб-ия, с", "Ток уставки, А", "Ток проверки, А", "Время сраб-ия, с", "Ток уставки, А", "Ток проверки, А", "Время сраб-ия, с" }; //вторая строка заголовка
                for (int c = 0; c < Header.Length; c++)
                {
                    TableRow tr = ispytanAutomatsTable.Rows[1];
                    Paragraph p = tr.Cells[c].AddParagraph();
                    tr.Cells[c].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                    p.Format.HorizontalAlignment = HorizontalAlignment.Center;
                    TextRange txtRange = p.AppendText(Header[c]);
                    p.ApplyStyle(regularStyleTable_2);
                    tr.Height = Convert.ToSingle(0.93 * pointsForConvert);
                }
                int row = 0;
                for (int r = 0; r < Automats.Count; r++)
                {
                    Paragraph p;
                    TextRange txtRange;

                    ispytanAutomatsTable.ApplyVerticalMerge(0, row + 2, row + 2 + 2);
                    ispytanAutomatsTable.ApplyVerticalMerge(2, row + 2, row + 2 + 2);

                    TableRow tr = ispytanAutomatsTable.Rows[row + 2];
                    tr.Height = Convert.ToSingle(0.93 * pointsForConvert);
                    tr.HeightType = TableRowHeightType.AtLeast;
                    List<string> characteristics = Automats[r].PhaseCharacteristics();
                    string[] rowData = {
                        Automats[r].NameAutomat + "\r" + Automats[r].Description,
                        "A",
                        Automats[r].NominalCurrent,
                        characteristics[0],
                        characteristics[1],
                        characteristics[2],
                        characteristics[3],
                        characteristics[4],
                        characteristics[5],
                        characteristics[6],
                        characteristics[7],
                        characteristics[8]
                    };

                    for (int c = 0; c < rowData.Length; c++)
                    {
                        p = tr.Cells[c].AddParagraph();
                        tr.Cells[c].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                        p.Format.HorizontalAlignment = HorizontalAlignment.Center;
                        txtRange = p.AppendText(rowData[c]);
                        p.ApplyStyle(regularStyleTable_2);
                    }

                    tr = ispytanAutomatsTable.Rows[row + 2 + 1];
                    tr.Height = Convert.ToSingle(0.93 * pointsForConvert);
                    tr.HeightType = TableRowHeightType.AtLeast;
                    characteristics = Automats[r].PhaseCharacteristics();
                    rowData = new string[] {
                        "",
                        "B",
                        "",
                        characteristics[0],
                        characteristics[1],
                        characteristics[2],
                        characteristics[3],
                        characteristics[4],
                        characteristics[5],
                        characteristics[6],
                        characteristics[7],
                        characteristics[8]
                    };

                    for (int c = 0; c < rowData.Length; c++)
                    {
                        p = tr.Cells[c].AddParagraph();
                        tr.Cells[c].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                        p.Format.HorizontalAlignment = HorizontalAlignment.Center;
                        txtRange = p.AppendText(rowData[c]);
                        p.ApplyStyle(regularStyleTable_2);
                    }

                    tr = ispytanAutomatsTable.Rows[row + 2 + 2];
                    tr.Height = Convert.ToSingle(0.93 * pointsForConvert);
                    tr.HeightType = TableRowHeightType.AtLeast;
                    characteristics = Automats[r].PhaseCharacteristics();
                    rowData = new string[] {
                        "",
                        "C",
                        "",
                        characteristics[0],
                        characteristics[1],
                        characteristics[2],
                        characteristics[3],
                        characteristics[4],
                        characteristics[5],
                        characteristics[6],
                        characteristics[7],
                        characteristics[8]
                    };

                    for (int c = 0; c < rowData.Length; c++)
                    {
                        p = tr.Cells[c].AddParagraph();
                        tr.Cells[c].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                        p.Format.HorizontalAlignment = HorizontalAlignment.Center;
                        txtRange = p.AppendText(rowData[c]);
                        p.ApplyStyle(regularStyleTable_2);
                    }

                    row += 3; //пропускаем две строки, т.к. они уже заполнены
                }

                //Текст про проверку изоляции и проверку реле
                section.AddParagraph();
                data = new string[] {
                    "8.2.3.Проверено сопротивление изоляции автоматических выключателей мегаомметром на 1000В в сборе.Сопротивление изоляции проверялось между полюсами выключателя, между полюсами и землей и сопротивление главных контактов выключателя на разрыв. Наименьшее значение сопротивления изоляции составило 5000Мом.Норма > 1 МОм.",
                                       "\r",
                    "8.3.Проверка промежуточных реле, контакторов и автоматических выключателей в составе модулей унифицированных.",
                    "Проверка реле и контакторов в модулях " + protocolDocument.Modules + "."
                };
                for (int i = 0; i < data.Length; i++)
                {
                    paragraph = section.AddParagraph();
                    paragraph.AppendText(data[i]);
                    paragraph.ApplyStyle(regularStyleNumb_1);
                }

                //Параграф про проверку реле автоматических выключателей

                int countAllRelays = 0; //Подсчет всех реле у всех автоматов
                foreach (Automat automat in Automats)
                    countAllRelays += automat.Relays.Count;

                Table RelayTable = section.AddTable(true);
                RelayTable.ResetCells(countAllRelays + 1, 5);

                Header = new string[]
                {
                    "Место установки",
                    "Обозначение",
                    "Тип",
                    "Uср, В",
                    "Uвозв, В"
                };
                for (int c = 0; c < Header.Length; c++)
                {
                    TableRow tr = RelayTable.Rows[0];
                    Paragraph p = tr.Cells[c].AddParagraph();
                    tr.Cells[c].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                    p.Format.HorizontalAlignment = HorizontalAlignment.Center;
                    TextRange txtRange = p.AppendText(Header[c]);
                    p.ApplyStyle(regularStyleTable_1);
                }

                row = 1;
                for (int a = 0; a < Automats.Count; a++)
                {
                    RelayTable.ApplyVerticalMerge(0, row, row + Automats[a].Relays.Count - 1); //столбец, строка, конечная строка

                    //Записывается в первый столбец первого реле его название
                    TableRow tr = RelayTable.Rows[row];
                    Paragraph p = tr.Cells[0].AddParagraph();
                    tr.Cells[0].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                    tr.Height = 0.93f;
                    p.Format.HorizontalAlignment = HorizontalAlignment.Center;
                    TextRange txtRange = p.AppendText("Релейный отсек\r" + Automats[a].NameAutomat);
                    p.ApplyStyle(regularStyleTable_1);

                    for (int rel = 0; rel < Automats[a].Relays.Count; rel++)
                    {

                        data = new string[]
                        {
                            Automats[a].Relays[rel].NameRelay,
                            Automats[a].Relays[rel].Mark,
                            Automats[a].Relays[rel].USrabat,
                            Automats[a].Relays[rel].UVozvrat
                        };
                        for (int c = 0; c < data.Length; c++)
                        {
                            tr = RelayTable.Rows[row];
                            p = tr.Cells[c + 1].AddParagraph();
                            tr.Cells[c + 1].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                            p.Format.HorizontalAlignment = HorizontalAlignment.Center;
                            txtRange = p.AppendText(data[c]);
                            p.ApplyStyle(regularStyleTable_1);
                        }
                        row++;
                    }
                }
                RelayTable.AutoFit(AutoFitBehaviorType.AutoFitToContents);
                RelayTable.AutoFit(AutoFitBehaviorType.AutoFitToWindow);
                section.AddParagraph();
                section.AddParagraph();

                //Таблица проверки автоматов
                int countAllPhasesForCounterRow = 0;

                foreach (Automat automat in Automats)
                    foreach (SF sF in automat.SFs)
                        countAllPhasesForCounterRow += Convert.ToInt32(sF.CountPhases);

                Table SfTable = section.AddTable(true);
                SfTable.ResetCells(countAllPhasesForCounterRow + 2, 11);
                for (int i = 0; i < 6; i++)
                    SfTable.ApplyVerticalMerge(i, 0, 1);
                SfTable.ApplyVerticalMerge(10, 0, 1);
                SfTable.ApplyHorizontalMerge(0, 6, 7);
                SfTable.ApplyHorizontalMerge(0, 8, 9);

                Header = new string[]
                {
                    "Место\rустановки",
                    "Обозн.\rпо схеме",
                    "Тип",
                    "Хар-\rка",
                    "Ном.\rток In,\rА",
                    "Фаза",
                    "Перегруз",
                    "",
                    "ТО",
                    "",
                    "Заключение"
                };
                for (int c = 0; c < Header.Length; c++)
                {
                    TableRow tr = SfTable.Rows[0];
                    Paragraph p = tr.Cells[c].AddParagraph();
                    tr.Cells[c].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                    p.Format.HorizontalAlignment = HorizontalAlignment.Center;
                    TextRange txtRange = p.AppendText(Header[c]);
                    p.ApplyStyle(regularStyleTable_1);
                }
                Header = new string[]
                {
                    "",
                    "",
                    "",
                    "",
                    "",
                    "",
                    "Ток\rпрог-ки,\rА",
                    "Время,\rс",
                    "Ток\rпрог-\rки, А",
                    "Время,\rс",
                    ""
                };
                for (int c = 0; c < Header.Length; c++)
                {
                    TableRow tr = SfTable.Rows[0];
                    Paragraph p = tr.Cells[c].AddParagraph();
                    tr.Cells[c].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                    p.Format.HorizontalAlignment = HorizontalAlignment.Center;
                    TextRange txtRange = p.AppendText(Header[c]);
                    p.ApplyStyle(regularStyleTable_1);
                }

                row = 2;
                foreach(Automat automat in Automats)
                {
                    TableRow tr = SfTable.Rows[row];
                    Paragraph p = tr.Cells[0].AddParagraph();
                    tr.Cells[0].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                    p.Format.HorizontalAlignment = HorizontalAlignment.Center;
                    TextRange txtRange = p.AppendText(automat.NameAutomat);
                    p.ApplyStyle(regularStyleTable_1);

                    int countPhasesForCounterRow = 0;
                    foreach (SF sF in automat.SFs)
                        countPhasesForCounterRow += Convert.ToInt32(sF.CountPhases);

                    SfTable.ApplyVerticalMerge(0, row, row + countPhasesForCounterRow - 1);

                    for (int s = 0; s < automat.SFs.Count; s++)
                    {
                        SF sF = automat.SFs[s];
                        data = new string[] {
                            sF.Name,
                            sF.Type,
                            sF.Character,
                            sF.Inom
                        };
                        for (int c = 1; c < data.Length; c++)
                        {
                            tr = SfTable.Rows[row];
                            p = tr.Cells[c].AddParagraph();
                            tr.Cells[c].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                            p.Format.HorizontalAlignment = HorizontalAlignment.Center;
                            txtRange = p.AppendText(data[c-1]);
                            p.ApplyStyle(regularStyleTable_1);

                            SfTable.ApplyVerticalMerge(c, row, row + Convert.ToInt32(sF.CountPhases) - 1);
                        }


                    }
                }

                //Текст Проверка действия и состояния схемы

                // таблица Проверка изоляции

                //Текст про испытание повышенным напряжением 

                //Текст применение испытательного оборудования

                //таблица испытательного оборудования

                //Текст заключение и кто выполнил проверку
            }

            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Ошибка при создании файла Word в классе WordMaker\n\n" + ex.ToString(), "Ошибка!");
            }

            try
            {
                doc.SaveToFile(directoryPath + protocolName);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Ошибка при сохранении, возможно файл уже открыт\n\n" + ex.ToString(), "Ошибка");
            }
            doc.Dispose();

            System.Windows.MessageBox.Show("Создание файла успешно завершено!", "Выполнено!");
        }


        //Начало 
    }
}
