﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Trippin_Website.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(100)]
        public string UsernameCont { get; set; }

        [DataType(DataType.Url)]
        public string ProfilePicture { get; set; }
        public float Quota { get; set; }
        public Guid? Versuri { get; set; }
        public float FileUploadHardLimit { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {

            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Piese> Piese { get; set; }
        public DbSet<Client> Clienti { get; set; }
        public DbSet<MembershipType> MembershipType { get; set; }
        public DbSet<Beat> Beaturi { get; set; }
        public DbSet<StyleOf> StyleOf { get; set; }
        public DbSet<Likes> Likes { get; set; }
        public DbSet<Versuri> Versuri { get; set; }
        public DbSet<Grupuri> Grupuri { get; set; }
        public DbSet<GrupuriMembrii> GrupuriMembrii { get; set; }
        public DbSet<WhoIsOnTheSong> WhoIsOnTheSong { get; set; }
        public DbSet<WhoProducedTheSong> WhoProducedTheSong { get; set; }
        public DbSet<PlayList> PlayList { get; set; }
        public DbSet<PlaylistContent> PlaylistContent { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<ChatMembers> ChatMembers { get; set; }
        public DbSet<ChatContents> ChatContents { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {

        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

    }
}