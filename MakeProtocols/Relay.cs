using System;

namespace MakeProtocols
{
    public class Relay
    {
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

        public string USrabat { get; set; }

        public string UVozvrat { get; set; }
        
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

        public void Generate(Random random)
        {
            if (TypeRelay.Contains("КМ"))
                USrabat = random.Next(143, 153).ToString();
            else USrabat = random.Next(135, 145).ToString();

            if (TypeRelay.Contains("КМ"))
                UVozvrat = random.Next(101, 107).ToString();
            else UVozvrat = random.Next(104, 110).ToString();
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
