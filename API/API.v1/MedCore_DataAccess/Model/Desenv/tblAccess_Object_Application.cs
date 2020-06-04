using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblAccess_Object_Application
    {
        public int intobjectApplication { get; set; }
        public int? intObjectId { get; set; }
        public int? intApplicationId { get; set; }
        public string txtMinVersion { get; set; }
        public bool bitPermiteOffline { get; set; }
        public string txtMinVersionOffline { get; set; }
    }
}
