using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Identity
{
    //if controller and action not exist in this table then its available
    public class APIPermission:BaseEntity<int>
    { 
        public string ControllerName { get; set; }
        public string ActionName { get; set; }// if * this controller all of its actions are available for specific action
        public int? PageActionId { get; set; }
        public bool IsPublic { get; set; }// always return for each action not need for PageActionId
    }
}
