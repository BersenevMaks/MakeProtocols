using System.Text.RegularExpressions;

namespace MakeProtocols
{
    public class ProtocolDocument
    {
        string modules = "";
        public string Modules
        {
            get
            { return modules; }
            set
            {
                string[] strs = value.Split(',');
                for (int i = 0; i < strs.Length; i++)
                {
                    modules += "М" + strs[i].Trim() + ", ";
                }
                modules = modules.Remove(modules.Length - 2, 2);
                Regex regex = new Regex(@"\.\.\.(?=\d)");
                modules = regex.Replace(modules, "...М");
            }
        }
        public string Shifr { get; set; }
        public string NumbProt { get; set; }
        public string KartUstav { get; set; }
        public string ObjectProt { get; set; }
        public string DateProt { get; set; }
    }
}
