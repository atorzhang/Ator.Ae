﻿// <auto-generated />
using Ator.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Ator.Entity.Migrations
{
    [DbContext(typeof(AeDbContext))]
    partial class AeDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .HasAnnotation("ProductVersion", "2.0.3-rtm-10026");

            modelBuilder.Entity("Ator.Entity.Sys.SysCmsColumn", b =>
                {
                    b.Property<string>("SysCmsColumnId")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(32);

                    b.Property<string>("ColumnDescript")
                        .HasMaxLength(1000);

                    b.Property<string>("ColumnLogo")
                        .HasMaxLength(255);

                    b.Property<string>("ColumnName")
                        .HasMaxLength(50);

                    b.Property<string>("ColumnParent")
                        .HasMaxLength(32);

                    b.Property<DateTime?>("CreateTime");

                    b.Property<string>("CreateUser")
                        .HasMaxLength(32);

                    b.Property<string>("Remark")
                        .HasMaxLength(200);

                    b.Property<int?>("Sort");

                    b.Property<int?>("Status");

                    b.HasKey("SysCmsColumnId");

                    b.ToTable("Sys_Cms_Column");
                });

            modelBuilder.Entity("Ator.Entity.Sys.SysCmsInfo", b =>
                {
                    b.Property<string>("SysCmsInfoId")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(32);

                    b.Property<DateTime?>("CreateTime");

                    b.Property<string>("CreateUser")
                        .HasMaxLength(32);

                    b.Property<string>("InfoAbstract")
                        .HasMaxLength(200);

                    b.Property<string>("InfoAuthor")
                        .HasMaxLength(50);

                    b.Property<DateTime?>("InfoCheckTime");

                    b.Property<string>("InfoCheckUser")
                        .HasMaxLength(50);

                    b.Property<int?>("InfoClicks");

                    b.Property<int?>("InfoComments");

                    b.Property<string>("InfoContent")
                        .HasColumnType("text");

                    b.Property<DateTime?>("InfoEditTime");

                    b.Property<string>("InfoEditUser")
                        .HasMaxLength(50);

                    b.Property<int?>("InfoGoods");

                    b.Property<string>("InfoImage")
                        .HasMaxLength(255);

                    b.Property<string>("InfoLable")
                        .HasMaxLength(200);

                    b.Property<DateTime?>("InfoPublishTime");

                    b.Property<string>("InfoSecTitle")
                        .HasMaxLength(255);

                    b.Property<string>("InfoSource")
                        .HasMaxLength(50);

                    b.Property<string>("InfoTitle")
                        .HasMaxLength(255);

                    b.Property<int?>("InfoTop");

                    b.Property<string>("InfoType")
                        .HasMaxLength(20);

                    b.Property<string>("Remark")
                        .HasMaxLength(200);

                    b.Property<int?>("Sort");

                    b.Property<int?>("Status");

                    b.Property<string>("SysCmsColumnId")
                        .HasMaxLength(32);

                    b.HasKey("SysCmsInfoId");

                    b.HasIndex("SysCmsColumnId");

                    b.ToTable("Sys_Cms_Info");
                });

            modelBuilder.Entity("Ator.Entity.Sys.SysCmsInfoComment", b =>
                {
                    b.Property<string>("SysCmsInfoCommentId")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(32);

                    b.Property<string>("Address")
                        .HasMaxLength(100);

                    b.Property<string>("Comment")
                        .HasMaxLength(2000);

                    b.Property<DateTime?>("CommentTime");

                    b.Property<string>("Ip")
                        .HasMaxLength(100);

                    b.Property<int?>("Status");

                    b.Property<string>("SysCmsColumnId")
                        .HasMaxLength(50);

                    b.Property<string>("SysCmsInfoId")
                        .HasMaxLength(32);

                    b.Property<string>("SysUserId")
                        .HasMaxLength(32);

                    b.Property<string>("ToCommentId")
                        .HasMaxLength(32);

                    b.Property<string>("ToUserId")
                        .HasMaxLength(32);

                    b.Property<string>("UserLable");

                    b.HasKey("SysCmsInfoCommentId");

                    b.HasIndex("SysCmsInfoId");

                    b.HasIndex("SysUserId");

                    b.HasIndex("ToUserId");

                    b.ToTable("Sys_Cms_InfoComment");
                });

            modelBuilder.Entity("Ator.Entity.Sys.SysCmsInfoGood", b =>
                {
                    b.Property<string>("SysCmsInfoGoodId")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(32);

                    b.Property<DateTime?>("CreateTime");

                    b.Property<string>("CreateUser")
                        .HasMaxLength(32);

                    b.Property<string>("Ip")
                        .HasMaxLength(100);

                    b.Property<string>("Remark")
                        .HasMaxLength(200);

                    b.Property<int?>("Sort");

                    b.Property<int?>("Status");

                    b.Property<string>("SysCmsInfoId")
                        .HasMaxLength(32);

                    b.Property<string>("SysUserId")
                        .HasMaxLength(32);

                    b.HasKey("SysCmsInfoGoodId");

                    b.HasIndex("SysCmsInfoId");

                    b.HasIndex("SysUserId");

                    b.ToTable("Sys_Cms_InfoGood");
                });

            modelBuilder.Entity("Ator.Entity.Sys.SysDictionary", b =>
                {
                    b.Property<string>("SysDictionaryId")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(32);

                    b.Property<DateTime?>("CreateTime");

                    b.Property<string>("CreateUser")
                        .HasMaxLength(32);

                    b.Property<string>("Remark")
                        .HasMaxLength(200);

                    b.Property<int?>("Sort");

                    b.Property<int?>("Status");

                    b.Property<string>("SysDictionaryGroup")
                        .HasMaxLength(32);

                    b.Property<string>("SysDictionaryName")
                        .HasMaxLength(32);

                    b.HasKey("SysDictionaryId");

                    b.ToTable("Sys_Dictionary");
                });

            modelBuilder.Entity("Ator.Entity.Sys.SysDictionaryItem", b =>
                {
                    b.Property<string>("SysDictionaryItemId")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(32);

                    b.Property<DateTime?>("CreateTime");

                    b.Property<string>("CreateUser")
                        .HasMaxLength(32);

                    b.Property<string>("Remark")
                        .HasMaxLength(200);

                    b.Property<int?>("Sort");

                    b.Property<int?>("Status");

                    b.Property<string>("SysDictionaryId")
                        .HasMaxLength(32);

                    b.Property<string>("SysDictionaryItemName")
                        .HasMaxLength(32);

                    b.Property<string>("SysDictionaryItemValue")
                        .HasMaxLength(255);

                    b.HasKey("SysDictionaryItemId");

                    b.HasIndex("SysDictionaryId");

                    b.ToTable("Sys_Dictionary_Item");
                });

            modelBuilder.Entity("Ator.Entity.Sys.SysLinkItem", b =>
                {
                    b.Property<string>("SysLinkItemId")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(32);

                    b.Property<DateTime?>("CreateTime");

                    b.Property<string>("CreateUser")
                        .HasMaxLength(32);

                    b.Property<string>("Remark")
                        .HasMaxLength(200);

                    b.Property<int?>("Sort");

                    b.Property<int?>("Status");

                    b.Property<string>("SysLinkGroup")
                        .HasMaxLength(255);

                    b.Property<string>("SysLinkImg")
                        .HasMaxLength(255);

                    b.Property<string>("SysLinkName")
                        .HasMaxLength(32);

                    b.Property<string>("SysLinkTypeId")
                        .HasMaxLength(32);

                    b.Property<string>("SysLinkUrl")
                        .HasMaxLength(255);

                    b.HasKey("SysLinkItemId");

                    b.HasIndex("SysLinkTypeId");

                    b.ToTable("Sys_LinkItem");
                });

            modelBuilder.Entity("Ator.Entity.Sys.SysLinkType", b =>
                {
                    b.Property<string>("SysLinkTypeId")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(32);

                    b.Property<DateTime?>("CreateTime");

                    b.Property<string>("CreateUser")
                        .HasMaxLength(32);

                    b.Property<string>("Group")
                        .HasMaxLength(32);

                    b.Property<string>("Remark")
                        .HasMaxLength(200);

                    b.Property<int?>("Sort");

                    b.Property<int?>("Status");

                    b.Property<string>("SysLinkTypeLogo")
                        .HasMaxLength(200);

                    b.Property<string>("SysLinkTypeName")
                        .HasMaxLength(32);

                    b.HasKey("SysLinkTypeId");

                    b.ToTable("Sys_LinkType");
                });

            modelBuilder.Entity("Ator.Entity.Sys.SysOperateRecord", b =>
                {
                    b.Property<string>("SysOperateRecordId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClassName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<DateTime?>("CreateTime");

                    b.Property<string>("MethodName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("Operate")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<short>("Result");

                    b.Property<string>("SysUserId")
                        .HasMaxLength(32);

                    b.Property<string>("TableName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<short>("Type");

                    b.Property<string>("UserName")
                        .HasMaxLength(50);

                    b.HasKey("SysOperateRecordId");

                    b.HasIndex("SysUserId");

                    b.ToTable("Sys_Operate_Record");
                });

            modelBuilder.Entity("Ator.Entity.Sys.SysPage", b =>
                {
                    b.Property<string>("SysPageId")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(32);

                    b.Property<DateTime?>("CreateTime");

                    b.Property<string>("CreateUser")
                        .HasMaxLength(32);

                    b.Property<string>("Remark")
                        .HasMaxLength(200);

                    b.Property<int?>("Sort");

                    b.Property<int?>("Status");

                    b.Property<string>("SysPageImg")
                        .HasMaxLength(255);

                    b.Property<string>("SysPageName")
                        .HasMaxLength(50);

                    b.Property<string>("SysPageNum")
                        .HasMaxLength(50);

                    b.Property<string>("SysPageParent")
                        .HasMaxLength(32);

                    b.Property<string>("SysPageUrl")
                        .HasMaxLength(255);

                    b.HasKey("SysPageId");

                    b.ToTable("Sys_Page");
                });

            modelBuilder.Entity("Ator.Entity.Sys.SysRole", b =>
                {
                    b.Property<string>("SysRoleId")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(32);

                    b.Property<DateTime?>("CreateTime");

                    b.Property<string>("CreateUser")
                        .HasMaxLength(32);

                    b.Property<string>("Remark")
                        .HasMaxLength(200);

                    b.Property<string>("RoleName")
                        .HasMaxLength(32);

                    b.Property<int?>("Sort");

                    b.Property<int?>("Status");

                    b.HasKey("SysRoleId");

                    b.ToTable("Sys_Role");
                });

            modelBuilder.Entity("Ator.Entity.Sys.SysRolePage", b =>
                {
                    b.Property<string>("SysRolePageId")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(32);

                    b.Property<DateTime?>("CreateTime");

                    b.Property<string>("CreateUser")
                        .HasMaxLength(32);

                    b.Property<string>("Remark")
                        .HasMaxLength(200);

                    b.Property<int?>("Sort");

                    b.Property<int?>("Status");

                    b.Property<string>("SysPageId")
                        .HasMaxLength(32);

                    b.Property<string>("SysRoleId")
                        .HasMaxLength(32);

                    b.HasKey("SysRolePageId");

                    b.HasIndex("SysPageId");

                    b.HasIndex("SysRoleId");

                    b.ToTable("Sys_RolePage");
                });

            modelBuilder.Entity("Ator.Entity.Sys.SysSetting", b =>
                {
                    b.Property<string>("SysSettingId")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(32);

                    b.Property<DateTime?>("CreateTime");

                    b.Property<string>("CreateUser")
                        .HasMaxLength(32);

                    b.Property<string>("Remark")
                        .HasMaxLength(200);

                    b.Property<string>("SetValue")
                        .HasMaxLength(500);

                    b.Property<int?>("Sort");

                    b.Property<int?>("Status");

                    b.Property<string>("SysSettingGroup")
                        .HasMaxLength(50);

                    b.Property<string>("SysSettingName")
                        .HasMaxLength(50);

                    b.Property<string>("SysSettingType")
                        .HasMaxLength(50);

                    b.HasKey("SysSettingId");

                    b.ToTable("Sys_Setting");
                });

            modelBuilder.Entity("Ator.Entity.Sys.SysUser", b =>
                {
                    b.Property<string>("SysUserId")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(32);

                    b.Property<string>("Avatar")
                        .HasMaxLength(255);

                    b.Property<DateTime?>("CreateTime");

                    b.Property<string>("CreateUser")
                        .HasMaxLength(32);

                    b.Property<string>("Email")
                        .HasMaxLength(100);

                    b.Property<string>("Mobile")
                        .HasMaxLength(20);

                    b.Property<string>("NikeName")
                        .HasMaxLength(32);

                    b.Property<string>("Password")
                        .HasMaxLength(255);

                    b.Property<string>("QQ")
                        .HasMaxLength(200);

                    b.Property<string>("Remark")
                        .HasMaxLength(200);

                    b.Property<string>("Sex")
                        .HasMaxLength(1);

                    b.Property<int?>("Sort");

                    b.Property<int?>("Status");

                    b.Property<string>("TrueName")
                        .HasMaxLength(32);

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(32);

                    b.Property<string>("UserType")
                        .HasMaxLength(1);

                    b.Property<string>("WX")
                        .HasMaxLength(200);

                    b.HasKey("SysUserId");

                    b.ToTable("Sys_User");
                });

            modelBuilder.Entity("Ator.Entity.Sys.SysUserRole", b =>
                {
                    b.Property<string>("SysUserRoleId")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(32);

                    b.Property<DateTime?>("CreateTime");

                    b.Property<string>("CreateUser")
                        .HasMaxLength(32);

                    b.Property<string>("Remark")
                        .HasMaxLength(200);

                    b.Property<int?>("Sort");

                    b.Property<int?>("Status");

                    b.Property<string>("SysRoleId")
                        .HasMaxLength(32);

                    b.Property<string>("SysUserId")
                        .HasMaxLength(32);

                    b.HasKey("SysUserRoleId");

                    b.HasIndex("SysRoleId");

                    b.HasIndex("SysUserId");

                    b.ToTable("Sys_UserRole");
                });

            modelBuilder.Entity("Ator.Entity.Sys.SysCmsInfo", b =>
                {
                    b.HasOne("Ator.Entity.Sys.SysCmsColumn", "SysCmsColumn")
                        .WithMany()
                        .HasForeignKey("SysCmsColumnId");
                });

            modelBuilder.Entity("Ator.Entity.Sys.SysCmsInfoComment", b =>
                {
                    b.HasOne("Ator.Entity.Sys.SysCmsInfo", "SysCmsInfo")
                        .WithMany()
                        .HasForeignKey("SysCmsInfoId");

                    b.HasOne("Ator.Entity.Sys.SysUser", "SysUser")
                        .WithMany()
                        .HasForeignKey("SysUserId");

                    b.HasOne("Ator.Entity.Sys.SysUser", "ToUser")
                        .WithMany()
                        .HasForeignKey("ToUserId");
                });

            modelBuilder.Entity("Ator.Entity.Sys.SysCmsInfoGood", b =>
                {
                    b.HasOne("Ator.Entity.Sys.SysCmsInfo", "SysCmsInfo")
                        .WithMany()
                        .HasForeignKey("SysCmsInfoId");

                    b.HasOne("Ator.Entity.Sys.SysUser", "SysUser")
                        .WithMany()
                        .HasForeignKey("SysUserId");
                });

            modelBuilder.Entity("Ator.Entity.Sys.SysDictionaryItem", b =>
                {
                    b.HasOne("Ator.Entity.Sys.SysDictionary", "SysDictionary")
                        .WithMany()
                        .HasForeignKey("SysDictionaryId");
                });

            modelBuilder.Entity("Ator.Entity.Sys.SysLinkItem", b =>
                {
                    b.HasOne("Ator.Entity.Sys.SysLinkType", "SysLinkType")
                        .WithMany()
                        .HasForeignKey("SysLinkTypeId");
                });

            modelBuilder.Entity("Ator.Entity.Sys.SysOperateRecord", b =>
                {
                    b.HasOne("Ator.Entity.Sys.SysUser", "SysUser")
                        .WithMany()
                        .HasForeignKey("SysUserId");
                });

            modelBuilder.Entity("Ator.Entity.Sys.SysRolePage", b =>
                {
                    b.HasOne("Ator.Entity.Sys.SysPage", "SysPage")
                        .WithMany()
                        .HasForeignKey("SysPageId");

                    b.HasOne("Ator.Entity.Sys.SysRole", "SysRole")
                        .WithMany()
                        .HasForeignKey("SysRoleId");
                });

            modelBuilder.Entity("Ator.Entity.Sys.SysUserRole", b =>
                {
                    b.HasOne("Ator.Entity.Sys.SysRole", "SysRole")
                        .WithMany("SysUserRoles")
                        .HasForeignKey("SysRoleId");

                    b.HasOne("Ator.Entity.Sys.SysUser", "SysUser")
                        .WithMany("SysUserRoles")
                        .HasForeignKey("SysUserId");
                });
#pragma warning restore 612, 618
        }
    }
}
