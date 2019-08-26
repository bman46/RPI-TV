﻿// <auto-generated />
using Controller;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Controller.Migrations
{
    [DbContext(typeof(Model.DeviceChannels))]
    partial class DeviceChannelsModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity("Controller.Model+Channel", b =>
                {
                    b.Property<int>("ChannelID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ChannelName");

                    b.Property<string>("Url");

                    b.HasKey("ChannelID");

                    b.ToTable("Channels");
                });

            modelBuilder.Entity("Controller.Model+Device", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DeviceName");

                    b.Property<int>("SetChannel");

                    b.HasKey("ID");

                    b.ToTable("Devices");
                });
#pragma warning restore 612, 618
        }
    }
}
