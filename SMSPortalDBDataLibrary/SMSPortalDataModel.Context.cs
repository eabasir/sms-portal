﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SMSPortalDBDataLibrary
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class SMSPortalDBEntities : DbContext
    {
        public SMSPortalDBEntities()
            : base("name=SMSPortalDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<Inbox> Inboxes { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<Phone> Phones { get; set; }
        public virtual DbSet<Queue_Phone> Queue_Phone { get; set; }
        public virtual DbSet<Queue> Queues { get; set; }
        public virtual DbSet<SendBox_Phone> SendBox_Phone { get; set; }
        public virtual DbSet<SendBox> SendBoxes { get; set; }
        public virtual DbSet<SIM_User> SIM_User { get; set; }
        public virtual DbSet<SIM> SIMs { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}
