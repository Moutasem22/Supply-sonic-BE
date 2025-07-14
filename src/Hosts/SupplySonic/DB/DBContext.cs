using Core.Enums;
using Core.Models;
using Core.Models.Attachments;
using Core.Models.Identity;
using Core.Models.Notifications;
using Core.Models.Setting;
using Core.Models.StoredProcedures;
using Core.Models.Tracker;
using DB.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DB
{
    public class DBContext : IdentityDbContext<AppUser, Role, int, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPasswordHasher<AppUser> _passwordHasher;
        public DBContext(DbContextOptions<DBContext> options, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IPasswordHasher<AppUser> passwordHasher)
            : base(options)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _passwordHasher = passwordHasher;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("name=DefaultConnection");
            }
        }
        private void SetShadowProperties(DateTime dateTimeNow, int userId, EntityEntry entry)
        {
            var ModifiedDate = entry.CurrentValues.Properties.FirstOrDefault(x => x.Name.ToLower() == "ModifiedDate".ToLower());
            var ModifiedBy = entry.CurrentValues.Properties.FirstOrDefault(x => x.Name.ToLower() == "ModifiedBy".ToLower());
            var IsActive = entry.CurrentValues.Properties.FirstOrDefault(x => x.Name.ToLower() == "IsActive".ToLower());
            var IsDeleted = entry.CurrentValues.Properties.FirstOrDefault(x => x.Name.ToLower() == "IsDeleted".ToLower());
            var CreatedDate = entry.CurrentValues.Properties.FirstOrDefault(x => x.Name.ToLower() == "CreatedDate".ToLower());
            var CreatedBy = entry.CurrentValues.Properties.FirstOrDefault(x => x.Name.ToLower() == "CreatedBy".ToLower());

            switch (entry.State)
            {
                case EntityState.Modified:


                    if (ModifiedDate != null)
                    {
                        entry.CurrentValues[ModifiedDate.Name] = dateTimeNow;
                    }
                    if (userId != default(int))
                    {

                        if (ModifiedBy != null)
                        {
                            entry.CurrentValues[ModifiedBy.Name] = userId;
                        }
                    }
                    break;
                case EntityState.Added:

                    //if (IsActive != null)
                    //{
                    //    entry.CurrentValues[IsActive.Name] = true;
                    //}
                    if (IsDeleted != null)
                    {
                        entry.CurrentValues[IsDeleted.Name] = false;
                    }
                    if (CreatedDate != null)
                    {
                        entry.CurrentValues[CreatedDate.Name] = dateTimeNow;
                    }
                    if (userId != default(int))
                    {
                        if (CreatedBy != null)
                        {
                            entry.CurrentValues[CreatedBy.Name] = userId;
                        }
                    }
                    break;
            }
        }

        private int GetUserId(ClaimsPrincipal principal)
        {
            if (principal != null && principal.HasClaim(c => c.Type.ToLower() == "UserId".ToLower()))
            {
                var activeUserId = principal.FindFirst(c => c.Type.ToLower() == "UserId".ToLower()).Value;
                return Convert.ToInt32(activeUserId);
            }
            return 0;
        }

        public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            ChangeTracker.DetectChanges();
            var entries = ChangeTracker.Entries();
            var dateTimeNow = DateTime.UtcNow;
            var userId = GetUserId(_httpContextAccessor.HttpContext.User);
            foreach (var entry in entries)
            {
                SetShadowProperties(dateTimeNow, userId, entry);
            }
            //////////////////////////////////////////////////////////////////
            var trackerList = this.SaveTracking(ChangeTracker.Entries());
            //////////////////////////////////////////////////////////////////


            var saveChanges = await base.SaveChangesAsync(cancellationToken);


            ////////////////////////////////////////////////////////////
            var trackerList2 = this.setNewItemsData(trackerList);
            Trackers.AddRange(trackerList2);
            await base.SaveChangesAsync();
            ///////////////////////////////////////////////////////////
            return saveChanges;
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();
            var entries = ChangeTracker.Entries();
            var dateTimeNow = DateTime.UtcNow;
            var userId = GetUserId(_httpContextAccessor?.HttpContext?.User);
            foreach (var entry in entries)
            {
                SetShadowProperties(dateTimeNow, userId, entry);
            }
            //////////////////////////////////////////////////////////////////
            var trackerList = this.SaveTracking(ChangeTracker.Entries());
            //////////////////////////////////////////////////////////////////
            var saveChanges = base.SaveChanges();
            ////////////////////////////////////////////////////////////
            var trackerList2 = this.setNewItemsData(trackerList);
            Trackers.AddRange(trackerList2);
            base.SaveChanges();
            ///////////////////////////////////////////////////////////
            return saveChanges;
        }
        public int SaveChangesForHangFire()
        {
            return base.SaveChanges();
        }
        protected List<Tracker> setNewItemsData(List<Tracker> trackerList)
        {
            foreach (var tracker in trackerList.Where(x => x.MethodType == EnumTrackMethodType.insert))
            {
                var idprop = tracker.CurrentValues.Properties.Where(x => x.Name == "Id").FirstOrDefault();
                tracker.rowId = (idprop != null ? tracker.CurrentValues[idprop].ToString() : "0");
                foreach (var property in tracker.CurrentValues.Properties)
                {
                    var trackerDetail = new TrackerDetail();
                    trackerDetail.ColumnName = property.Name;

                    trackerDetail.MyProperty = property;
                    trackerDetail.NewValue = tracker.CurrentValues[property]?.ToString();
                    tracker.TrackerDetails.Add(trackerDetail);
                }
            }
            return trackerList;
        }

        protected List<Tracker> SaveTracking(IEnumerable<EntityEntry> Entries)
        {
            List<Tracker> trackerList = new List<Tracker>();
            var changes = from e in Entries
                          where e.State != EntityState.Unchanged
                          select e;


            foreach (var change in changes.ToList())
            {
                var tracker = new Tracker();
                tracker.TblName = change.Entity.GetType().Name;
                tracker.CreatedDate = DateTime.UtcNow;
                tracker.CreatedBy = GetUserId(_httpContextAccessor?.HttpContext?.User);
                if (change.State == EntityState.Added)
                {
                    tracker.MethodType = EnumTrackMethodType.insert;
                    var currentValues = change.CurrentValues;
                    tracker.CurrentValues = change.CurrentValues;
                }
                else if (change.State == EntityState.Modified)
                {
                    // Log Modified
                    tracker.MethodType = EnumTrackMethodType.update;
                    var item = change.Entity;
                    var originalValues = change.OriginalValues;
                    var currentValues = change.CurrentValues;

                    var idprop = change.OriginalValues.Properties.Where(x => x.Name == "Id").FirstOrDefault();
                    tracker.rowId = (idprop != null ? change.OriginalValues[idprop].ToString() : "0");

                    var appclientProp = change.OriginalValues.Properties.Where(x => x.Name == "AppClientId").FirstOrDefault();
                    foreach (var property in originalValues.Properties)
                    {
                        var trackerDetail = new TrackerDetail();
                        var original = originalValues[property];
                        var current = currentValues[property];

                        //if (!Equals(original, current))
                        //{
                        trackerDetail.isChanged = (!Equals(original, current));
                        trackerDetail.ColumnName = property.Name;
                        trackerDetail.OldValue = original?.ToString();
                        trackerDetail.NewValue = current?.ToString();
                        // log propertyName: original --> current
                        // }
                        tracker.TrackerDetails.Add(trackerDetail);
                    }

                }
                else if (change.State == EntityState.Deleted)
                {
                    tracker.MethodType = EnumTrackMethodType.delete;

                    var idprop = change.OriginalValues.Properties.Where(x => x.Name == "Id").FirstOrDefault();
                    tracker.rowId = (idprop != null ? change.OriginalValues[idprop].ToString() : "0");
                    var appclientProp = change.OriginalValues.Properties.Where(x => x.Name == "AppClientId").FirstOrDefault();
                    // log deleted
                }
                trackerList.Add(tracker);
            }
            return trackerList;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserToken>()
           .ToTable("UserTokens");

            modelBuilder.Entity<UserRole>()
            .ToTable("UserRoles");

            modelBuilder.Entity<UserClaim>()
            .ToTable("UserClaims");

            modelBuilder.Entity<RoleClaim>()
            .ToTable("RoleClaims");


            modelBuilder.Entity<UserLogin>()
            .ToTable("UserLogins");

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Roles").HasMany(x => x.UserRoles).WithOne(x => x.Role).HasForeignKey(x => x.RoleId).OnDelete(DeleteBehavior.Cascade);
                entity.HasQueryFilter(e => e.IsActive != false);
                entity.Property(e => e.IsActive).HasDefaultValueSql("((1))");
                entity.HasMany(e => e.Permissions).WithOne(e => e.Role).HasForeignKey(e => e.RoleId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.ToTable("AppUsers").HasMany(x => x.UserRoles).WithOne(x => x.AppUser).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
                //entity.HasQueryFilter(e => e.IsActive != false);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");

            });

            

            modelBuilder.Entity<SPAuthorizePagesWithAction>().HasNoKey().ToView(null);

            modelBuilder.ApplyConfiguration(new EmailNotificationConfiguration());

        }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
        public virtual DbSet<Tracker> Trackers { get; set; }
        public virtual DbSet<TrackerDetail> TrackerDetails { get; set; }
        public virtual DbSet<Page> Pages { get; set; }
        public virtual DbSet<PageCategory> PageCategories { get; set; }
        public virtual DbSet<Core.Models.Identity.Action> Actions { get; set; }
        public virtual DbSet<PageAction> PageActions { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<SysSetting> SysSettings { get; set; }
        public virtual DbSet<UserSetting> UserSettings { get; set; }
        public virtual DbSet<UserConnection> UserConnections { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<UserNotification> UserNotifications { get; set; }
        public virtual DbSet<NotificationTemplate> NotificationTemplates { get; set; }
        public virtual DbSet<NotificationTemplateParams> NotificationTemplateParams { get; set; }
        public virtual DbSet<Attachment> Attachments { get; set; }
  
        public virtual DbSet<APIPermission> APIPermissions { get; set; }
        public virtual DbSet<AttachmentExtension> AttachmentExtension { get; set; }

       
        public virtual DbSet<Group> Groups { get; set; }

      

        public virtual DbSet<AppearancSetting> AppearancSettings { get; set; }
      
        public virtual DbSet<Priority> Priorities { get; set; }

        public virtual DbSet<NotificationSetting> NotificationSettings { get; set; }

       

        public DbSet<Client> Clients { get; set; }


        
    }
}
