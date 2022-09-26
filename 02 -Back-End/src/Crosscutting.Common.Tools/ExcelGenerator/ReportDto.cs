using System.Collections.Generic;
using System.Linq;

namespace Crosscutting.Common.Tools.ExcelGenerator
{
    public class ReportDto
    {
        private List<string> _columns;
        public IEnumerable<object> ListData { get; set; }

        public List<string> Columns
        {
            get
            {
                if (_columns != null)
                    return _columns;
                _columns = new List<string>();
                var header = ReportData.FirstOrDefault();
                if (header == null)
                    return _columns;
                foreach (var dictionary in header)
                    _columns.Add(dictionary.Key);
                return _columns;
            }
        }

        public IEnumerable<Dictionary<string, string>> ReportData { get; set; }
    }
}