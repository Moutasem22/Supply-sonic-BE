using Core;
using Core.Enums;
using Core.Models;
using Core.Models.Identity;
using Core.Models.Products;
using Core.WorkFlow;
using Core.WorkFlow.Configuration;
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
        private readonly IPasswordHasher<SupplierAppUser> _passwordHasherSupplier;
        public DBContext(DbContextOptions<DBContext> options, IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor, IPasswordHasher<AppUser> passwordHasher,
            IPasswordHasher<SupplierAppUser> passwordHasherSupplier)
            : base(options)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _passwordHasher = passwordHasher;
            _passwordHasherSupplier = passwordHasherSupplier;
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
            modelBuilder.ApplyConfiguration(new WFRequestConfiguration());
            modelBuilder.ApplyConfiguration(new WorkFlowConfiguration());
            modelBuilder.ApplyConfiguration(new WFActionConfiguration());
            modelBuilder.ApplyConfiguration(new WFActivityConfiguration());
            modelBuilder.ApplyConfiguration(new WFRequestConfiguration());
            modelBuilder.ApplyConfiguration(new WFStateConfiguration());
            modelBuilder.ApplyConfiguration(new WFTransitionConfiguration());
            modelBuilder.ApplyConfiguration(new WFTransitionActivityConfiguration());



            var setQueryFilterMethod = new Action<ModelBuilder>(SetQueryFilter<BaseEntity<int>>).Method.GetGenericMethodDefinition();

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (entityType.BaseType == null && typeof(BaseEntity<int>).IsAssignableFrom(entityType.ClrType))
                {
                    setQueryFilterMethod
                        .MakeGenericMethod(entityType.ClrType)
                        .Invoke(this, new object[] { modelBuilder });
                }
            }

            //For Guid in Base Entity
            var setQueryGuidFilterMethod = new Action<ModelBuilder>(SetQueryGuidFilter<BaseEntity<Guid>>).Method.GetGenericMethodDefinition();

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (entityType.BaseType == null && typeof(BaseEntity<Guid>).IsAssignableFrom(entityType.ClrType))
                {
                    setQueryGuidFilterMethod
                        .MakeGenericMethod(entityType.ClrType)
                        .Invoke(this, new object[] { modelBuilder });
                }
            }

        }
        void SetQueryFilter<TEntity>(ModelBuilder modelBuilder) where TEntity : BaseEntity<int>
        {
            modelBuilder.Entity<TEntity>().HasQueryFilter(e => e.IsActive && !e.IsDeleted);
        }
        void SetQueryGuidFilter<TEntity>(ModelBuilder modelBuilder) where TEntity : BaseEntity<Guid>
        {
            modelBuilder.Entity<TEntity>().HasQueryFilter(e => e.IsActive && !e.IsDeleted);
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
        public virtual DbSet<WFRejectReason> WFRejectReason { get; set; }
        public virtual DbSet<WorkFlow> WorkFlow { get; set; }
        public virtual DbSet<WFActivity> WFActivity { get; set; }
        public virtual DbSet<WFTransition> WFTransition { get; set; }
        public virtual DbSet<WFTransitionAction> WFTransitionAction { get; set; }
        public virtual DbSet<WFTransitionActivity> WFTransitionActivity { get; set; }
        public virtual DbSet<WFAction> WFAction { get; set; }
        public virtual DbSet<WFRequest> WFRequest { get; set; }
        public virtual DbSet<WFRequestAction> WFRequestAction { get; set; }
        public virtual DbSet<WFState> WFState { get; set; }
        public virtual DbSet<WFStateActivity> WFStateActivitie { get; set; }
        public virtual DbSet<WFStateAssignedUser> WFStateAssignedUser { get; set; }
        public virtual DbSet<WFStateUserStatus> WFStateUserStatus { get; set; }
        public virtual DbSet<WFRequestHistory> WFRequestHistory { get; set; }
        public virtual DbSet<WFRequestPosition> WFRequestPosition { get; set; }
        public virtual DbSet<WFTransitionActivityNotification> WFTransitionActivityNotification { get; set; }
        public virtual DbSet<WFStateUser> WFStateUser { get; set; }
        public virtual DbSet<WFStateRole> WFStateRole { get; set; }
        public virtual DbSet<APIPermission> APIPermissions { get; set; }
        public virtual DbSet<AttachmentExtension> AttachmentExtension { get; set; }


        public virtual DbSet<Group> Groups { get; set; }



        public virtual DbSet<AppearancSetting> AppearancSettings { get; set; }

        public virtual DbSet<Priority> Priorities { get; set; }

        public virtual DbSet<NotificationSetting> NotificationSettings { get; set; }



        public DbSet<Client> Clients { get; set; }



        public DbSet<SupplierAppUser> SupplierAppUsers { get; set; }

        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }




        public DbSet<Nationality> Nationalities { get; set; }

        #region Products

        public DbSet<ProductMainCategory> ProductMainCategories { get; set; }
        public DbSet<ProductSubCategory> ProductSubCategories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Core.Models.Products.Attribute> Attributes { get; set; }
        public DbSet<SubAttribute> SubAttributes { get; set; }
        public DbSet<ProductAttachment> ProductAttachments { get; set; }
        public DbSet<ProductAttribute> ProductAttributes { get; set; }

        public DbSet<ProductOffer> ProductOffers { get; set; }
        public DbSet<ProductOfferAttachment> ProductOfferAttachments { get; set; }
        public DbSet<ProductOfferAttribute> ProductOfferAttributes { get; set; }

        public DbSet<Unit> Units { get; set; }

        public DbSet<ProductWeight> ProductWeights { get; set; }
        public DbSet<ProductOfferPriceSchedule> ProductOfferPriceSchedules { get; set; }


        #endregion


        public DbSet<BindingRoom> BindingRooms { get; set; }
        public DbSet<BindingRoomAttribute> BindingRoomAttributess { get; set; }
        public DbSet<BindingRoomAttachment> BindingRoomAttachments { get; set; }
        public DbSet<BindingRoomRequest> BindingRoomRequests { get; set; }
        public DbSet<UserAddress> UserAddresss { get; set; }
        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetails> OrderDetails { get; set; }


    }
}
