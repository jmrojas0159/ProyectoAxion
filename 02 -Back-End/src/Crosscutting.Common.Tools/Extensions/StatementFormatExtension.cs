
namespace Crosscutting.Common.Tools.Extensions
{
    public static class StatementFormatExtension
    {
        public static string ToString(this decimal? value, string format)
        {
            return value == null ? "0" : value.Value.ToString(format);
        }

    }

}
