using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cae.Utils.MappedExceptions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Cae.Utils.Trier
{
    public interface IUnexpectedExceptionHandler
    {
        /// <summary>
        /// Contract of handling the unexpected exception: receives it and
        /// returns out of it a new, mapped-typed, exception instance.
        /// </summary>
        /// <param name="unexpectedException">the exception that does not extend any kind of MappedException</param>
        /// <returns>the MappedException as a result of handling the unexpected</returns>
        MappedException Handle(Exception unexpectedException);
    }
}
