using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MakeProtocols
{
    public class Automat
    {
        public string Section { get; set; }
        public string QFQS { get; set; }
        public string PositionNumb { get; set; }

        public string NameAutomat { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string VendorNumb { get; set; }
        public string NominalCurrent { get; set; }
        public string NominalVoltage { get; set; }
        public string TypeBreaker { get;set;}
        public string Ust_Ir { get; set; }
        public string Ust_Tr { get; set; }
        public string Ust_Isd { get; set; }
        public string Ust_Tsd { get; set; }
        public string Ust_Ii { get; set; }
        public string Ust_Ti { get; set; }
        public string Ust_Ig { get; set; }
        public string Ust_Tg { get; set; }

        public string FirstKontaktorType { get; set; }
        public string SecondKontaktorType { get; set; }

        public ObservableCollection<Relay> Relays { get; set; }
        public List<SF> SFs { get; set; }

        public Automat Factory(string section, string qfqs, string posnumb, string description, string type = "-", string vendorNumb = "-", string nominalCurrent = "-", string nominalVoltage = "-", string typeBreaker = "", string ustIr = "-", string ustTr = "-", string ustIsd = "-", string ustTsd = "-", string ustIi = "-", string ustTi = "-",
            string ustIg = "-", string ustTg = "-", string firstKontakorType = "", string secondKontaktorType = "")
        {
            return new Automat()
            {
                Section = section,
                QFQS = qfqs,
                PositionNumb = posnumb,
                NameAutomat = section + qfqs + posnumb,
                Description = description,
                Type = type,
                VendorNumb = vendorNumb,
                NominalCurrent = nominalCurrent,
                NominalVoltage = nominalVoltage,
                TypeBreaker = typeBreaker,
                Ust_Ir = ustIr.Replace(",", "."),
                Ust_Tr = ustTr.Replace(",", "."),
                Ust_Isd = ustIsd.Replace(",", "."),
                Ust_Tsd = ustTsd.Replace(",", "."),
                Ust_Ii = ustIi,
                Ust_Ti = ustTi.Replace(",", "."),
                Ust_Ig = ustIg.Replace(",", "."),
                Ust_Tg = ustTg.Replace(",", "."),
                FirstKontaktorType = firstKontakorType,
                SecondKontaktorType = secondKontaktorType
            };
        }

        public Automat Clone()
        {
            return new Automat()
            {
                Section = Section,
                QFQS = QFQS,
                PositionNumb = PositionNumb,
                NameAutomat = NameAutomat,
                Description = Description,
                Type = Type,
                VendorNumb = VendorNumb,
                NominalCurrent = NominalCurrent,
                NominalVoltage = NominalVoltage,
                TypeBreaker = TypeBreaker,
                Ust_Ir = Ust_Ir,
                Ust_Tr = Ust_Tr,
                Ust_Isd = Ust_Isd,
                Ust_Tsd = Ust_Tsd,
                Ust_Ii = Ust_Ii,
                Ust_Ti = Ust_Ti,
                Ust_Ig = Ust_Ig,
                Ust_Tg = Ust_Tg
            };
        }

        public List<string> PhaseCharacteristics()
        {
            List<string> phaseCharacteristics = new List<string>();

            switch (TypeBreaker)
            {
                case "FTU":
                case "Комбинированный":
                    {
                        phaseCharacteristics.Add("-"); //Ir
                        phaseCharacteristics.Add("-"); //Ir проверки
                        phaseCharacteristics.Add("-"); //tr проверки

                        phaseCharacteristics.Add("-"); //Isd
                        phaseCharacteristics.Add("-"); //Isd проверки
                        phaseCharacteristics.Add("-"); //tsd проверки

                        phaseCharacteristics.Add(Ust_Ii); //Ii
                        double Ii = Convert.ToDouble(Ust_Ii) * 1.3;
                        phaseCharacteristics.Add(Convert.ToString(Convert.ToInt32(Math.Ceiling(Ii)))); //Ii проверки
                        phaseCharacteristics.Add("0,02"); //ti проверки

                        break;
                    }
            }

            return phaseCharacteristics;
        }
    }
}
