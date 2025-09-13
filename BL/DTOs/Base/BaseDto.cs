using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DTOs.Base
{
    public class BaseDto
    {
        public Guid Id { get; set; }
        public int CurrentState { get; set; }
    }

}

