﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

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
        public string TypeBreaker { get; set; }
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
                Ust_Ir = ustIr,
                Ust_Tr = ustTr,
                Ust_Isd = ustIsd,
                Ust_Tsd = ustTsd,
                Ust_Ii = ustIi,
                Ust_Ti = ustTi,
                Ust_Ig = ustIg,
                Ust_Tg = ustTg,
                FirstKontaktorType = firstKontakorType,
                SecondKontaktorType = secondKontaktorType
            };
        }

        public Automat Clone()
        {
            Automat automat = new Automat()
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
            automat.Relays = new ObservableCollection<Relay>();
            automat.SFs = new List<SF>();
            if (Relays != null)
                for (int r = 0; r < Relays.Count; r++)
                    automat.Relays.Add(Relays[r].Clone());

            if (SFs != null)
                for (int sf = 0; sf < SFs.Count; sf++)
                    automat.SFs.Add(SFs[sf].Clone());

            return automat;
        }

        public List<string> PhaseCharacteristics(Random random)
        {
            List<string> phaseCharacteristics = new List<string>();
            try
            {
                switch (TypeBreaker)
                {
                    case "MTU":
                    case "EXP":
                    case "Комбинированный":
                    case "Хар.С":
                        {
                            phaseCharacteristics.Add(Ust_Ir); //Ir
                            if (!string.IsNullOrEmpty(Ust_Ir) && Ust_Ir != "-")
                            {
                                double Ir = Convert.ToDouble(Ust_Ir) * 1.4;
                                phaseCharacteristics.Add(Convert.ToString(Convert.ToInt32(Math.Ceiling(Ir)))); //Ir проверки
                                phaseCharacteristics.Add(Convert.ToString(random.Next(201, 239) / 10)); //tr проверки
                            }
                            else
                            {
                                phaseCharacteristics.Add("-"); //Ir проверки
                                phaseCharacteristics.Add("-"); //tr проверки
                            }

                            phaseCharacteristics.Add("-"); //Isd
                            phaseCharacteristics.Add("-"); //Isd проверки
                            phaseCharacteristics.Add("-"); //tsd проверки

                            phaseCharacteristics.Add(Ust_Ii); //Ii
                            if (!string.IsNullOrEmpty(Ust_Ii) && Ust_Ii != "-")
                            {
                                double Ii = Convert.ToDouble(Ust_Ii) * 1.3;
                                phaseCharacteristics.Add(Convert.ToString(Convert.ToInt32(Math.Ceiling(Ii)))); //Ii проверки
                                phaseCharacteristics.Add("0,02"); //ti проверки
                            }
                            else
                            {
                                phaseCharacteristics.Add("-"); //Ii проверки
                                phaseCharacteristics.Add("-"); //ti проверки
                            }

                            break;
                        }
                    case "FTU":
                    case "FMU":
                        {
                            phaseCharacteristics.Add(Ust_Ir); //Ir
                            if (!string.IsNullOrEmpty(Ust_Ir) && Ust_Ir != "-")
                            {
                                double Ir = Convert.ToDouble(Ust_Ir) * 2;
                                phaseCharacteristics.Add(Convert.ToString(Convert.ToInt32(Math.Ceiling(Ir)))); //Ir проверки
                                phaseCharacteristics.Add(Convert.ToString(random.Next(201, 300))); //tr проверки
                            }
                            else
                            {
                                phaseCharacteristics.Add("-"); //Ir проверки
                                phaseCharacteristics.Add("-"); //tr проверки
                            }

                            phaseCharacteristics.Add("-"); //Isd
                            phaseCharacteristics.Add("-"); //Isd проверки
                            phaseCharacteristics.Add("-"); //tsd проверки

                            Ust_Ii = Convert.ToString(Convert.ToDouble(NominalCurrent) * 10.0);
                            phaseCharacteristics.Add(Ust_Ii); //Ii
                            if (!string.IsNullOrEmpty(Ust_Ii) && Ust_Ii != "-")
                            {
                                double Ii = Convert.ToDouble(Ust_Ii) * 1.3;
                                phaseCharacteristics.Add(Convert.ToString(Convert.ToInt32(Math.Ceiling(Ii)))); //Ii проверки
                                phaseCharacteristics.Add("0,02"); //ti проверки
                            }
                            else
                            {
                                phaseCharacteristics.Add("-"); //Ii проверки
                                phaseCharacteristics.Add("-"); //ti проверки
                            }

                            break;

                        }
                    case "ATU":
                        {
                            phaseCharacteristics.Add(Ust_Ir); //Ir
                            if (!string.IsNullOrEmpty(Ust_Ir) && Ust_Ir != "-")
                            {
                                double Ir = Convert.ToDouble(Ust_Ir) * 2;
                                phaseCharacteristics.Add(Convert.ToString(Convert.ToInt32(Math.Ceiling(Ir)))); //Ir проверки
                                phaseCharacteristics.Add(Convert.ToString(random.Next(201, 300))); //tr проверки
                            }
                            else
                            {
                                phaseCharacteristics.Add("-"); //Ir проверки
                                phaseCharacteristics.Add("-"); //tr проверки
                            }

                            phaseCharacteristics.Add(Ust_Isd); //Isd
                            if (!string.IsNullOrEmpty(Ust_Isd) && Ust_Isd != "-")
                            {
                                double Isd = Convert.ToDouble(Ust_Isd) * 1.3;
                                phaseCharacteristics.Add(Convert.ToString(Convert.ToInt32(Math.Ceiling(Isd)))); //Isd проверки
                                phaseCharacteristics.Add("0,02"); //tsd проверки
                            }
                            else
                            {
                                phaseCharacteristics.Add("-"); //Isd проверки
                                phaseCharacteristics.Add("-"); //tsd проверки
                            }

                            Ust_Ii = Convert.ToString(Convert.ToDouble(NominalCurrent) * 10.0);
                            phaseCharacteristics.Add(Ust_Ii); //Ii
                            if (!string.IsNullOrEmpty(Ust_Ii) && Ust_Ii != "-")
                            {
                                double Ii = Convert.ToDouble(Ust_Ii) * 1.3;
                                phaseCharacteristics.Add(Convert.ToString(Convert.ToInt32(Math.Ceiling(Ii)))); //Ii проверки
                                phaseCharacteristics.Add("0,02"); //ti проверки
                            }
                            else
                            {
                                phaseCharacteristics.Add("-"); //Ii проверки
                                phaseCharacteristics.Add("-"); //ti проверки
                            }

                            break;

                        }
                    case "ETS23":
                    case "ETS33":
                    case "ETS43":
                        {
                            phaseCharacteristics.Add(Ust_Ir); //Ir
                            if (!string.IsNullOrEmpty(Ust_Ir) && Ust_Ir != "-")
                            {
                                double Ir = Convert.ToDouble(Ust_Ir) * 2.0;
                                if (Ir >= Convert.ToDouble(Ust_Ir))
                                    Ir = Convert.ToDouble(Ust_Ir) * 1.4;
                                phaseCharacteristics.Add(Convert.ToString(Convert.ToInt32(Math.Ceiling(Ir)))); //Ir проверки

                                if (Ir >= Convert.ToDouble(Ust_Ir))
                                    phaseCharacteristics.Add(Convert.ToString(random.Next(201, 239))); //tr проверки
                                else
                                    phaseCharacteristics.Add(Convert.ToString(random.Next(52, 64))); //tr проверки
                            }
                            else
                            {
                                phaseCharacteristics.Add("-"); //Ir проверки
                                phaseCharacteristics.Add("-"); //tr проверки
                            }

                            phaseCharacteristics.Add(Ust_Isd); //Isd
                            if (!string.IsNullOrEmpty(Ust_Isd) && Ust_Isd != "-")
                            {
                                double Isd = Convert.ToDouble(Ust_Isd) * 1.3;
                                phaseCharacteristics.Add(Convert.ToString(Convert.ToInt32(Math.Ceiling(Isd)))); //Isd проверки
                                phaseCharacteristics.Add(Convert.ToString(Math.Ceiling(random.Next(909, 1099) * Convert.ToDouble(Ust_Tsd) / 10.0) / 100.0)); //tsd проверки
                            }
                            else
                            {
                                phaseCharacteristics.Add("-"); //Isd проверки
                                phaseCharacteristics.Add("-"); //tsd проверки
                            }

                            phaseCharacteristics.Add(Ust_Ii); //Ii
                            if (!string.IsNullOrEmpty(Ust_Ii) && Ust_Ii != "-")
                            {
                                double Ii = Convert.ToDouble(Ust_Ii) * 1.3;
                                phaseCharacteristics.Add(Convert.ToString(Convert.ToInt32(Math.Ceiling(Ii)))); //Ii проверки
                                phaseCharacteristics.Add("0,02"); //ti проверки
                            }
                            else
                            {
                                phaseCharacteristics.Add("-"); //Ii проверки
                                phaseCharacteristics.Add("-"); //ti проверки
                            }

                            break;
                        }
                    case "AG0U0AL":
                    case "AC6U0AL":
                    case "AD6U0AL":
                        {
                                phaseCharacteristics.Add(Ust_Ir); //Ir
                                if (!string.IsNullOrEmpty(Ust_Ir) && Ust_Ir != "-")
                                {
                                    double Ir = Convert.ToDouble(Ust_Ir) * 1.5;
                                    phaseCharacteristics.Add(Convert.ToString(Convert.ToInt32(Math.Ceiling(Ir)))); //Ir проверки
                                    phaseCharacteristics.Add(Convert.ToString(random.Next(43, 56))); //tr проверки
                                }
                                else
                                {
                                    phaseCharacteristics.Add("-"); //Ir проверки
                                    phaseCharacteristics.Add("-"); //tr проверки
                                }

                                phaseCharacteristics.Add(Ust_Isd); //Isd
                                if (!string.IsNullOrEmpty(Ust_Isd) && Ust_Isd != "-")
                                {
                                    double Isd = Convert.ToDouble(Ust_Isd) * 1.05;
                                    phaseCharacteristics.Add(Convert.ToString(Convert.ToInt32(Math.Ceiling(Isd)))); //Isd проверки
                                    phaseCharacteristics.Add(Convert.ToString(Math.Ceiling(random.Next(909, 1099) * Convert.ToDouble(Ust_Tsd) / 10.0) / 100.0)); //tsd проверки
                                }
                                else
                                {
                                    phaseCharacteristics.Add("-"); //Isd проверки
                                    phaseCharacteristics.Add("-"); //tsd проверки
                                }

                                phaseCharacteristics.Add(Ust_Ii); //Ii
                                if (!string.IsNullOrEmpty(Ust_Ii) && Ust_Ii != "-")
                                {
                                    double Ii = Convert.ToDouble(Ust_Ii) * 1.3;
                                    if (Ii > 11500.00) Ii = 11500;
                                    phaseCharacteristics.Add(Convert.ToString(Convert.ToInt32(Math.Ceiling(Ii)))); //Ii проверки
                                    phaseCharacteristics.Add("0,02"); //ti проверки
                                }
                                else
                                {
                                    phaseCharacteristics.Add("-"); //Ii проверки
                                    phaseCharacteristics.Add("-"); //ti проверки
                                }

                                phaseCharacteristics.Add(Ust_Ig); //Ig
                                if (!string.IsNullOrEmpty(Ust_Ig) && Ust_Ig != "-")
                                {
                                    double Ig = Convert.ToDouble(Ust_Ig) * 1.3;
                                    if (Ig > 11500.00) Ig = 11500;
                                    phaseCharacteristics.Add(Convert.ToString(Convert.ToInt32(Math.Ceiling(Ig)))); //Ig проверки
                                    if (Ust_Tg != "" && Ust_Tg != "-")
                                        phaseCharacteristics.Add(Convert.ToString(Math.Ceiling(random.Next(909, 1099) * Convert.ToDouble(Ust_Tg) / 1000.0))); //tg проверки
                                    else phaseCharacteristics.Add("0,02");
                                }
                                else
                                {
                                    phaseCharacteristics.Add("-"); //Ig проверки
                                    phaseCharacteristics.Add("-"); //tg проверки
                                }
                            break;
                        }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при обработке " + TypeBreaker + "\n" + ex.ToString());
            }
            return phaseCharacteristics;
        }
    }
}
