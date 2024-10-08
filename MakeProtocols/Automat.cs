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
        public string Ust_Ir { get; set; }
        public string Ust_Tr { get; set; }
        public string Ust_Isd { get; set; }
        public string Ust_Tsd { get; set; }
        public string Ust_Ii { get; set; }
        public string Ust_Ti { get; set; }
        public string Ust_Ig { get; set; }
        public string Ust_Tg { get; set; }

        public Automat Factory(string section, string qfqs, string posnumb, string description, string type="-", string vendorNumb="-", string nominalCurrent="-", string nominalVoltage="-", string ustIi="-", string ustTi="-", string ustIr = "-", string ustTr = "-", string ustIsd = "-", string ustTsd = "-", 
            string ustIg = "-", string ustTg = "-")
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
                Ust_Ir = ustIr,
                Ust_Tr = ustTr,
                Ust_Isd = ustIsd,
                Ust_Tsd = ustTsd,
                Ust_Ii = ustIi,
                Ust_Ti = ustTi,
                Ust_Ig = ustIg,
                Ust_Tg = ustTg
            };
        }

    }
}
