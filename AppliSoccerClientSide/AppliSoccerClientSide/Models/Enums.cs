using AppliSoccerObjects.Modeling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AppliSoccerClientSide.Models
{
    public class Enums
    {
        public static List<string> RoleNames
        {
            get
            {
                return Enum.GetNames(typeof(Role)).ToList();
            }
        }
    }
}
