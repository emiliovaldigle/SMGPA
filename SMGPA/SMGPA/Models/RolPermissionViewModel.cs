using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMGPA.Models
{
    public class RolPermissionViewModel
    {
            public List<Permission> PermissionList;
            public SelectList Permission { get; set; }
            public string SelectedPermission { get; set; }

    }
}