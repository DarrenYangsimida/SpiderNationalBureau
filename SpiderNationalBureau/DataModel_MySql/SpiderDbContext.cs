using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SpiderNationalBureau.DataModel_MySql
{
    public partial class SpiderDbContext : DbContext
    {
        public SpiderDbContext()
        {
        }

        public SpiderDbContext(DbContextOptions<SpiderDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<City> City { get; set; }
        public virtual DbSet<Community> Community { get; set; }
        public virtual DbSet<District> District { get; set; }
        public virtual DbSet<Province> Province { get; set; }
        public virtual DbSet<Street> Street { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>(entity =>
            {
                entity.HasKey(e => e.RunId)
                    .HasName("PRIMARY");

                entity.ToTable("city");

                entity.HasIndex(e => e.CityCode)
                    .HasName("unique_index_city_code")
                    .IsUnique();

                entity.Property(e => e.RunId)
                    .HasColumnName("run_id")
                    .HasComment("自增主键");

                entity.Property(e => e.CityCode)
                    .HasColumnName("city_code")
                    .HasComment("city code - unique");

                entity.Property(e => e.CityName)
                    .IsRequired()
                    .HasColumnName("city_name")
                    .HasColumnType("varchar(255)")
                    .HasComment("city name")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.CreatedTime)
                    .HasColumnName("created_time")
                    .HasColumnType("datetime")
                    .HasComment("created time");

                entity.Property(e => e.DeletedFlag)
                    .HasColumnName("deleted_flag")
                    .HasComment("deleted flag - default 0");

                entity.Property(e => e.DeletedTime)
                    .HasColumnName("deleted_time")
                    .HasColumnType("datetime")
                    .HasComment("deleted time");

                entity.Property(e => e.ModifiedTime)
                    .HasColumnName("modified_time")
                    .HasColumnType("datetime")
                    .HasComment("modified time")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.ProvinceCode)
                    .HasColumnName("province_code")
                    .HasComment("province code - 父节点");
            });

            modelBuilder.Entity<Community>(entity =>
            {
                entity.HasKey(e => e.RunId)
                    .HasName("PRIMARY");

                entity.ToTable("community");

                entity.HasIndex(e => e.CommunityCode)
                    .HasName("unique_index_community_code")
                    .IsUnique();

                entity.Property(e => e.RunId)
                    .HasColumnName("run_id")
                    .HasComment("自增主键");

                entity.Property(e => e.CommunityCode)
                    .HasColumnName("community_code")
                    .HasComment("community code - unique");

                entity.Property(e => e.CommunityName)
                    .IsRequired()
                    .HasColumnName("community_name")
                    .HasColumnType("varchar(255)")
                    .HasComment("community name")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.CreatedTime)
                    .HasColumnName("created_time")
                    .HasColumnType("datetime")
                    .HasComment("created time");

                entity.Property(e => e.DeletedFlag)
                    .HasColumnName("deleted_flag")
                    .HasComment("deleted flag - default 0");

                entity.Property(e => e.DeletedTime)
                    .HasColumnName("deleted_time")
                    .HasColumnType("datetime")
                    .HasComment("deleted time");

                entity.Property(e => e.ModifiedTime)
                    .HasColumnName("modified_time")
                    .HasColumnType("datetime")
                    .HasComment("modified time")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.StreetCode)
                    .HasColumnName("street_code")
                    .HasComment("street code - 父节点");
            });

            modelBuilder.Entity<District>(entity =>
            {
                entity.HasKey(e => e.RunId)
                    .HasName("PRIMARY");

                entity.ToTable("district");

                entity.HasIndex(e => e.DistrictCode)
                    .HasName("unique_index_district_code")
                    .IsUnique();

                entity.Property(e => e.RunId)
                    .HasColumnName("run_id")
                    .HasComment("自增主键");

                entity.Property(e => e.CityCode)
                    .HasColumnName("city_code")
                    .HasComment("city code - 父节点");

                entity.Property(e => e.CreatedTime)
                    .HasColumnName("created_time")
                    .HasColumnType("datetime")
                    .HasComment("created time");

                entity.Property(e => e.DeletedFlag)
                    .HasColumnName("deleted_flag")
                    .HasComment("deleted flag - default 0");

                entity.Property(e => e.DeletedTime)
                    .HasColumnName("deleted_time")
                    .HasColumnType("datetime")
                    .HasComment("deleted time");

                entity.Property(e => e.DistrictCode)
                    .HasColumnName("district_code")
                    .HasComment("district code - unique");

                entity.Property(e => e.DistrictName)
                    .IsRequired()
                    .HasColumnName("district_name")
                    .HasColumnType("varchar(255)")
                    .HasComment("district name")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");

                entity.Property(e => e.ModifiedTime)
                    .HasColumnName("modified_time")
                    .HasColumnType("datetime")
                    .HasComment("modified time")
                    .ValueGeneratedOnAddOrUpdate();
            });

            modelBuilder.Entity<Province>(entity =>
            {
                entity.HasKey(e => e.RunId)
                    .HasName("PRIMARY");

                entity.ToTable("province");

                entity.HasIndex(e => e.ProvinceCode)
                    .HasName("unique_index_province_code")
                    .IsUnique();

                entity.Property(e => e.RunId)
                    .HasColumnName("run_id")
                    .HasComment("自增主键");

                entity.Property(e => e.CreatedTime)
                    .HasColumnName("created_time")
                    .HasColumnType("datetime")
                    .HasComment("created time");

                entity.Property(e => e.DeletedFlag)
                    .HasColumnName("deleted_flag")
                    .HasComment("deleted flag - default 0");

                entity.Property(e => e.DeletedTime)
                    .HasColumnName("deleted_time")
                    .HasColumnType("datetime")
                    .HasComment("deleted time");

                entity.Property(e => e.ModifiedTime)
                    .HasColumnName("modified_time")
                    .HasColumnType("datetime")
                    .HasComment("modified time")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.ProvinceCode)
                    .HasColumnName("province_code")
                    .HasComment("province code - unique");

                entity.Property(e => e.ProvinceName)
                    .IsRequired()
                    .HasColumnName("province_name")
                    .HasColumnType("varchar(255)")
                    .HasComment("province name")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");
            });

            modelBuilder.Entity<Street>(entity =>
            {
                entity.HasKey(e => e.RunId)
                    .HasName("PRIMARY");

                entity.ToTable("street");

                entity.HasIndex(e => e.StreetCode)
                    .HasName("unique_index_street_code")
                    .IsUnique();

                entity.Property(e => e.RunId)
                    .HasColumnName("run_id")
                    .HasComment("自增主键");

                entity.Property(e => e.CreatedTime)
                    .HasColumnName("created_time")
                    .HasColumnType("datetime")
                    .HasComment("created time");

                entity.Property(e => e.DeletedFlag)
                    .HasColumnName("deleted_flag")
                    .HasComment("deleted flag - default 0");

                entity.Property(e => e.DeletedTime)
                    .HasColumnName("deleted_time")
                    .HasColumnType("datetime")
                    .HasComment("deleted time");

                entity.Property(e => e.DistrictCode)
                    .HasColumnName("district_code")
                    .HasComment("district code - 父节点");

                entity.Property(e => e.ModifiedTime)
                    .HasColumnName("modified_time")
                    .HasColumnType("datetime")
                    .HasComment("modified time")
                    .ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.StreetCode)
                    .HasColumnName("street_code")
                    .HasComment("street code - unique");

                entity.Property(e => e.StreetName)
                    .IsRequired()
                    .HasColumnName("street_name")
                    .HasColumnType("varchar(255)")
                    .HasComment("street name")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_unicode_ci");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
