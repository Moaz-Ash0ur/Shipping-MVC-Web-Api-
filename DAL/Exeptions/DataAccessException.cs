using DAL.Repositoreis;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Exeptions
{
    public class DataAccessException : Exception
    {


        public DataAccessException(Exception ex , string customeMessage,ILogger logger)
        {
            logger.LogError($"main exception {ex.Message} , developer custome exception " +
                $"{customeMessage}");
        }

    }
}
