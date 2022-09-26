using System;
using System.Diagnostics;
using System.Linq;

namespace Crosscutting.Common.Tools.Extensions
{
    public static class ExceptionHelper
    {
        
        public static object GetInfoException(this Exception e)
        {
            var st = new StackTrace(e, true); // create the stack trace
            var query = st.GetFrames() // get the frames
                .Select(frame => new
                {
                    // get the info
                    FileName = frame.GetFileName(),
                    LineNumber = frame.GetFileLineNumber(),
                    ColumnNumber = frame.GetFileColumnNumber(),
                    Method = frame.GetMethod(),
                    Class = frame.GetMethod().DeclaringType
                });
            return query;
        }

        public static int LineNumber(this Exception e)
        {
            var linenum = 0;
            try
            {
                //linenum = Convert.ToInt32(e.StackTrace.Substring(e.StackTrace.LastIndexOf(":line") + 5));

                //For Localized Visual Studio ... In other languages stack trace  doesn't end with ":Line 12"
                linenum = Convert.ToInt32(e.StackTrace.Substring(e.StackTrace.LastIndexOf(' ')));
            }


            catch
            {
                //Stack trace is not available!
            }
            return linenum;
        }
    }
}