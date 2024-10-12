namespace MakeProtocols
{
    public class SF
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Character { get; set; }
        public string Inom { get; set; }
        public string Phase { get; set; }
        public string Ioverload { get; set; }
        public string Toverload { get; set; }
        public string Ito { get; set; }
        public string Tto { get; set; }

        public SF Clone()
        {
            SF sF = new SF()
            {
                Name = Name,
                Type = Type,
                Character = Character,
                Inom = Inom,
                Phase = Phase,
                Ioverload = Ioverload,
                Toverload = Toverload,
                Ito = Ito,
                Tto = Tto
            };
            return sF;
        }
    }


}
