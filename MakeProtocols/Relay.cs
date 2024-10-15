using System;

namespace MakeProtocols
{
    public class Relay
    {
        string uSrabat = "";
        string uVozvrat = "";

        string idRelay;
        string mark;

        public string IDrelay
        {
            get { return idRelay; }
            set { idRelay = value; }
        }
        public string TypeRelay { get; set; }
        
        public string NameRelay { get; set; }

        public bool? IsChecked { get; set; }

        public string USrabat
        {
            get { return uSrabat; }
            set
            {
                if (IDrelay.Contains("КМ"))
                    uSrabat = new Random().Next(143, 153).ToString();
                else uSrabat = new Random().Next(135, 145).ToString();
            }
        }

        public string UVozvrat
        {
            get { return uVozvrat; }
            set
            {
                if (IDrelay.Contains("КМ"))
                    uVozvrat = new Random().Next(101, 107).ToString();
                else uVozvrat = new Random().Next(104, 110).ToString();
            }
        }

        public string KM_I1 { get; set; }
        public string KM_I2 { get; set; }

        public string Mark
        {
            get { return mark; }
            set
            {
                if (String.IsNullOrEmpty(value) && !string.IsNullOrEmpty(KM_I1) && !string.IsNullOrEmpty(KM_I2))
                    mark = $"Metasol\rMC - {KM_I1}a, 3p, {KM_I1} A(AC - 3),\r{KM_I2} A(AC - 1) 230 В 50 Гц";
                else mark = value;
            }
        }

        public Relay Clone()
        {
            Relay relay = new Relay
            {
                IDrelay = IDrelay,
                TypeRelay = TypeRelay,
                NameRelay = NameRelay,
                Mark = Mark,
                IsChecked = IsChecked,
                USrabat = USrabat,
                UVozvrat = UVozvrat,
                KM_I1 = KM_I1,
                KM_I2 = KM_I2
            };
            return relay;
        }
    }
}
