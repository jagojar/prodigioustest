using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace ProdigiousJuanOsorioTest.Source.BusinessLayer
{
    public class ProdigiousPrincipal : IPrincipal
    {
        public string Name { get; set; }
        public string[] Roles { get; set; }

        public ProdigiousPrincipal(string name, string[] roles)
        {
            this.Name = name;
            this.Roles = roles;
        }

        public IIdentity Identity
        {
            get
            {
                return new GenericIdentity(this.Name);
            }

            set
            {
                this.Name = value.Name;
            }
        }

        public bool IsInRole(string role)
        {
            if (this.Roles.Contains(role))
            {
                return true;
            }

            return false;
        }
    }
}