using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblWarehousesClassRooms
    {
        public int intWarehouseID { get; set; }
        public int intClassRoomID { get; set; }
        public int intProductGroupID { get; set; }
        public int intYear { get; set; }

        public virtual tblClassRooms intClassRoom { get; set; }
        public virtual tblProductGroups1 intProductGroup { get; set; }
        public virtual tblWarehouses intWarehouse { get; set; }
    }
}
