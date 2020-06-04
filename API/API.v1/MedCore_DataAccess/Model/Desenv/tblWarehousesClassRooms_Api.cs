using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblWarehousesClassRooms_Api
    {
        public int intID { get; set; }
        public int intWarehouseID { get; set; }
        public int intClassRoomID { get; set; }
        public int intProductID { get; set; }
        public int intYear { get; set; }
    }
}
