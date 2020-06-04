using System;
using System.Collections.Generic;

namespace MedCore_DataAccess.Model
{
    public partial class tblLinkEsqueciSenha
    {
        public int intId { get; set; }
        public int? intApplicationId { get; set; }
        public string txtLink { get; set; }
    }
}
