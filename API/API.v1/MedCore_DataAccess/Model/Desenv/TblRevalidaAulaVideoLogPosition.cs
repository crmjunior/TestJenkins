﻿using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblRevalidaAulaVideoLogPosition
    {
        public int intLogPositionId { get; set; }
        public int intClientId { get; set; }
        public int intSecond { get; set; }
        public int intRevalidaAulaVideoId { get; set; }
        public DateTime dteLastUpdate { get; set; }
    }
}
