using System;
using System.Collections.Generic;
using System.Text;

namespace ContosoRest.Models.Domain
{
    public class LegoColorModel
    {
        #region Properties
        public int Id { get; set; }
        public string Material { get; set; }
        public int LegoId { get; set; }
        public string LegoName { get; set; }
        public int BlId { get; set; }
        public string BlName { get; set; }
        public string BoName { get; set; }
        public int LdrawId { get; set; }
        public string LdrawName { get; set; }
        public string PeeronName { get; set; }
        public string Other { get; set; }
        public int YearsActiveStart { get; set; }
        public int YearsActiveEnd { get; set; }
        public string Notes { get; set; }
        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }
        public string Hex { get; set; }
        //public int Hex { get; set; }
        public int C { get; set; }
        public int M { get; set; }
        public int Y { get; set; }
        public int K { get; set; }
        public string Pantone { get; set; }
        public string Textile { get; set; }
        public string NCS { get; set; }
        public string RAL { get; set; }
        public string LabL { get; set; }
        public string LabA { get; set; }
        public string LabB { get; set; }
        #endregion
    }
}
