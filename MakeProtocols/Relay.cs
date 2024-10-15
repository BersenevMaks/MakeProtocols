using System;

namespace MakeProtocols
{
    public class Relay
    {
        string uSrabat = "";
        string uVozvrat = "";

        string idRelay;
        string typeRelay;

        public string IDrelay
        {
            get { return idRelay; }
            set { idRelay = value; }
        }
        public string TypeRelay
        {
            get { return typeRelay; }
            set { typeRelay = value; }
        }

        public string NameRelay { get; set; }

        public string Mark { get; set; }

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

        public Relay Clone()
        {
            Relay relay = new Relay
            {
                IDrelay = IDrelay,
                TypeRelay = TypeRelay,
                NameRelay = NameRelay,
                Mark = Mark,
                IsChecked = IsChecked
            };
            return relay;
        }
    }
}
