using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Models;

public partial class ModelContext :DbContext
{
    public ModelContext()
    {
    }

    public ModelContext(DbContextOptions<ModelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Aboutusdatum> Aboutusdata { get; set; }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<Homedatum> Homedata { get; set; }

    public virtual DbSet<Hotel> Hotels { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<Testimonial> Testimonials { get; set; }
    public virtual DbSet<UserLogin> UserLogins { get; set; }
    public virtual DbSet<Customer> Customers { get; set; }
    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Contactusdatum> Contactusdata { get; set; }



    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseOracle("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SID=xe)));User Id=C##Shaimaa2;Password=Test321;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
     .HasDefaultSchema("C##SHAIMAA2")
     .UseCollation("USING_NLS_COMP");

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.Bookingid).HasName("SYS_C008699");

            entity.ToTable("BOOKING");

            entity.Property(e => e.Bookingid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("BOOKINGID");
            entity.Property(e => e.Checkin)
                .HasColumnType("DATE")
                .HasColumnName("CHECKIN");
            entity.Property(e => e.Checkout)
                .HasColumnType("DATE")
                .HasColumnName("CHECKOUT");
            entity.Property(e => e.Customerid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("CUSTOMERID");
            entity.Property(e => e.Numberofpersons)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("NUMBEROFPERSONS");
            entity.Property(e => e.Roomid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ROOMID");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("STATUS");
            entity.Property(e => e.Totalprice)
                .HasColumnType("NUMBER")
                .HasColumnName("TOTALPRICE");

            entity.HasOne(d => d.Customer).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.Customerid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("SYS_C008701");

            entity.HasOne(d => d.Room).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.Roomid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("SYS_C008700");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Customerid).HasName("SYS_C008686");

            entity.ToTable("CUSTOMERS");

            entity.Property(e => e.Customerid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("CUSTOMERID");
            entity.Property(e => e.Email)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Firstname)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("FIRSTNAME");
            entity.Property(e => e.Lastname)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("LASTNAME");
            entity.Property(e => e.Phonenumber)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("PHONENUMBER");
        });

        modelBuilder.Entity<Hotel>(entity =>
        {
            entity.HasKey(e => e.Hotelid).HasName("SYS_C008615");

            entity.ToTable("HOTEL");

            entity.Property(e => e.Hotelid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("HOTELID");
            entity.Property(e => e.Address)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("ADDRESS");
            entity.Property(e => e.City)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("CITY");
            entity.Property(e => e.Country)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("COUNTRY");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.Email)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Imagepath)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("IMAGEPATH");
            entity.Property(e => e.Name)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("NAME");
            entity.Property(e => e.Phonenumber)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("PHONENUMBER");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Roleid).HasName("SYS_C008689");

            entity.ToTable("ROLES");

            entity.Property(e => e.Roleid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ROLEID");
            entity.Property(e => e.Rolename)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("ROLENAME");
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.Roomid).HasName("SYS_C008620");

            entity.ToTable("ROOM");

            entity.HasIndex(e => e.Hotelid, "IX_ROOM_HOTELID");

            entity.Property(e => e.Roomid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ROOMID");
            entity.Property(e => e.Availabilty)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("AVAILABILTY");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.Hotelid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("HOTELID");
            entity.Property(e => e.Imagepath)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("IMAGEPATH");
            entity.Property(e => e.Maxcapacity)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("MAXCAPACITY");
            entity.Property(e => e.Name)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("NAME");
            entity.Property(e => e.Price)
                .HasColumnType("NUMBER")
                .HasColumnName("PRICE");
            entity.Property(e => e.Roomtype)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("ROOMTYPE");

            entity.HasOne(d => d.Hotel).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.Hotelid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("SYS_C008621");
        });

        modelBuilder.Entity<Testimonial>(entity =>
        {
            entity.HasKey(e => e.Testimonialid).HasName("SYS_C008703");

            entity.ToTable("TESTIMONIAL");

            entity.Property(e => e.Testimonialid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("TESTIMONIALID");
            entity.Property(e => e.Customerid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("CUSTOMERID");
            entity.Property(e => e.Roomid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ROOMID");
            entity.Property(e => e.Reviewtext)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("REVIEWTEXT");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("STATUS");
            entity.Property(e => e.TDate)
               .HasColumnType("DATE")
               .HasColumnName("TDATE");

            entity.HasOne(d => d.Customer).WithMany(p => p.Testimonials)
                .HasForeignKey(d => d.Customerid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("SYS_C008705");

            entity.HasOne(d => d.Room).WithMany(p => p.Testimonials)
                .HasForeignKey(d => d.Roomid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("SYS_C008704");
        });

        modelBuilder.Entity<UserLogin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008695");

            entity.ToTable("USERLOGINS");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID");
            entity.Property(e => e.Customerid)
                .HasColumnType("NUMBER")
                .HasColumnName("CUSTOMERID");
            entity.Property(e => e.Hashedpassword).HasColumnName("HASHEDPASSWORD");
            entity.Property(e => e.Roleid)
                .HasColumnType("NUMBER")
                .HasColumnName("ROLEID");
            entity.Property(e => e.Username)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("USERNAME");

            entity.HasOne(d => d.Customer).WithMany(p => p.Userlogins)
                .HasForeignKey(d => d.Customerid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("SYS_C008697");

            entity.HasOne(d => d.Role).WithMany(p => p.UserLogins)
                .HasForeignKey(d => d.Roleid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("SYS_C008696");
        });
        modelBuilder.Entity<Aboutusdatum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008631");

            entity.ToTable("ABOUTUSDATA");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.Imgpath1)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("IMGPATH1");
            entity.Property(e => e.Imgpath2)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("IMGPATH2");
            entity.Property(e => e.Imgpath3)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("IMGPATH3");
            entity.Property(e => e.Imgpath4)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("IMGPATH4");
            entity.Property(e => e.Title)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("TITLE");
        });

        modelBuilder.Entity<Contactusdatum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008707");

            entity.ToTable("CONTACTUSDATA");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID");
            entity.Property(e => e.Address)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("ADDRESS");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.Email)
                .HasMaxLength(35)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Locationurl)
                .HasMaxLength(400)
                .IsUnicode(false)
                .HasColumnName("LOCATIONURL");
            entity.Property(e => e.Phonenumber)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("PHONENUMBER");
        });

        modelBuilder.Entity<Homedatum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008629");

            entity.ToTable("HOMEDATA");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ID");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.Imgpath1)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("IMGPATH1");
            entity.Property(e => e.Imgpath2)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("IMGPATH2");
            entity.Property(e => e.Imgpath3)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("IMGPATH3");
            entity.Property(e => e.Logopath)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("LOGOPATH");
            entity.Property(e => e.Title)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("TITLE");
        });
        modelBuilder.HasSequence("S_ROLL_SEQ");
        modelBuilder.HasSequence("S_ROLL_SEQ2");
        modelBuilder.HasSequence("S_ROLL_SEQ3");
        modelBuilder.HasSequence("SEQ_TMP_IDENTITY_ID");


        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
