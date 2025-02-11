using TMPro;

namespace Utils.Extensions
{
    public static class TextMeshProExtensions
    {
        private static readonly char[] _buffer = new char[50];
        
        //string must be constant for zero allocation
        public static void SetTextNonAlloc(this TMP_Text textLabel, string description, float value, int maxFractionDigits)
        {
            _buffer.Reset();
            StringExtensions.FloatToStringNonAlloc(value, _buffer, maxFractionDigits);
            _buffer.InsertStringAtStart(description);
            textLabel.SetText(_buffer);
        }
        
        //string must be constant for zero allocation
        public static void SetTextNonAlloc(this TMP_Text textLabel, string description, int value)
        {
            _buffer.Reset();
            StringExtensions.IntToStringNonAlloc(value, _buffer);
            _buffer.InsertStringAtStart(description);
            textLabel.SetText(_buffer);
        }
    }
}