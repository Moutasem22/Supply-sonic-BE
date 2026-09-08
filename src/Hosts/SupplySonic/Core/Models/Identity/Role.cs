
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security;
using System.Text;

namespace Core.Models.Identity
{
    public class Role : IdentityRole<int>
    {

        public Role(string NameAr, string name, List<Permission> _Permissions, int Id = 0) : this()
        {
            this.Name = name;
            this.NameAr = NameAr;
            this.Permissions = _Permissions;
            this.Id = Id;
            //this.Permissions = roleDto.Permissions.Select(x => new Permission() { PageActionId = x.PageActionId }).ToList<Permission>();
        }
        public void Update(string NameAr, string Name, List<Permission> _Permissions)
        {

            this.Permissions.Clear();
            //foreach (var item in this.Permissions.ToList())
            //{
            //    this.Permissions.Remove(item);
            //}

            this.Name = Name;
            this.NameAr = NameAr;

            //foreach (var item in Permissions.ToList())
            //{
            //    this.Permissions.Add(new Permission() { PageActionId = item.PageActionId });
            //}

            List<Permission> list = new List<Permission>();
            list.AddRange(_Permissions);
            this.Permissions = list;

            //this.Permissions.ToList().AddRange(roleDto.Permissions.Select(x => new Permission() { PageActionId = x.PageActionId }).ToList<Permission>());
            //this.PageLookupData.ToList().AddRange(roleDto.PageLookupData.Distinct().Select(x => new PageLookupData() { LookupId = x.LookupId, PageLookupId = x.PageLookupId }).ToList<PageLookupData>());      
        }
        public void Delete()
        {
            this.IsDeleted = true;
        }
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public string NameAr { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsActive { get; set; }
        public ICollection<Permission> Permissions { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsMaster { get; set; }
        public bool IsAdmin { get; set; }// for system admin it is not enabled to edit it or ssign it with isMaster Role
        [Timestamp]
        public byte[] RowVersion { get; set; }
        private Role()
        {
            UserRoles = new HashSet<UserRole>();
            Permissions = new HashSet<Permission>();
        }
    }
}
