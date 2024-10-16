using System;

namespace MakeProtocols
{
    public class SF
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Character { get; set; }
        public string Inom { get; set; }
        public string CountPhases { get; set; }
        public string Ioverload { get; set; }
        public string Toverload { get; set; }
        public string Ito { get; set; }
        public string Tto { get; set; }


        public SF Clone()
        {
            SF sF = new SF()
            {
                ID = ID,
                Name = Name,
                Type = Type,
                Character = Character,
                Inom = Inom,
                CountPhases = CountPhases,
                Ioverload = Ioverload,
                Toverload = Toverload,
                Ito = Ito,
                Tto = Tto
            };
            return sF;
        }

        public void Generate(Random random)
        {
            if(int.TryParse(Inom, out int In)) In = 9999;

            switch (Character)
            {
                case "B":
                    {
                        Ioverload = "2,5In=" + Convert.ToInt32(In * 2.5);
                        break;
                    }
                case "C":
                    {
                        Ioverload = "4In=" + Convert.ToInt32(In * 4);
                        break;
                    }
            }
        }
    }


}
