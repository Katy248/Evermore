namespace Evermore.Models
{
    public class Characteristics
    {
        public int StrengthValue { get; set; } = 20;
        public bool StrengthProficient { get; set; } = false;

        public int DexterityValue { get; set; } = 0;
        public bool DexterityProficient { get; set; } = false;

        public int ConstitutionValue { get; set; } = 0;
        public bool ConstitutionProficient { get; set; } = false;

        public int IntelligenceValue { get; set; } = 0;
        public bool IntelligenceProficient { get; set; } = false;

        public int WisdomValue { get; set; } = 0;
        public bool WisdomProficient { get; set; } = false;

        public int CharismaValue { get; set; } = 0;
        public bool CharismaProficient { get; set; } = false;
    }
}
