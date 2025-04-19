using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonelYonetim.Server.Domain.PersonelDetaylar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonelYonetim.Server.Infrastructure.Configurations;
internal class PersonelDetayConfiguration : IEntityTypeConfiguration<PersonelDetay>
{
    public void Configure(EntityTypeBuilder<PersonelDetay> builder)
    {
        builder.Property(p => p.TCKN).HasColumnType("varchar(11)");
        builder.Property(p => p.NufusIl).HasColumnType("varchar(50)");
        builder.Property(p => p.NufusIlce).HasColumnType("varchar(50)");
        builder.Property(p => p.AnaAdi).HasColumnType("varchar(50)");
        builder.Property(p => p.BabaAdi).HasColumnType("varchar(50)");
        builder.Property(p => p.DogumYeri).HasColumnType("varchar(50)");
        builder.Property(p => p.MedeniHali).HasColumnType("varchar(20)");
        builder.Property(p => p.Cinsiyet).HasColumnType("varchar(10)");
        builder.Property(p => p.Uyruk).HasColumnType("varchar(30)");

        builder.Property(p => p.CepTelefonu).HasColumnType("varchar(20)");
        builder.Property(p => p.IsTelefonu).HasColumnType("varchar(20)");
        builder.Property(p => p.Eposta).HasColumnType("varchar(100)");
        builder.Property(p => p.EpostaIs).HasColumnType("varchar(100)");
        builder.Property(p => p.Adres).HasColumnType("varchar(250)");
        builder.Property(p => p.IkametIl).HasColumnType("varchar(50)");
        builder.Property(p => p.IkametIlce).HasColumnType("varchar(50)");
        builder.Property(p => p.PostaKodu).HasColumnType("varchar(10)");

        builder.Property(p => p.EgitimDurumu).HasColumnType("varchar(50)");
        builder.Property(p => p.MezuniyetOkulu).HasColumnType("varchar(100)");
        builder.Property(p => p.MezuniyetBolumu).HasColumnType("varchar(100)");

        builder.Property(p => p.AskerlikDurumu).HasColumnType("varchar(20)");

        builder.Property(p => p.EhliyetSinifi).HasColumnType("varchar(10)");

        builder.Property(p => p.SaglikDurumu).HasColumnType("varchar(100)");
        builder.Property(p => p.KanGrubu).HasColumnType("varchar(5)");

        builder.Property(p => p.AcilDurumKisiAdi).HasColumnType("varchar(100)");
        builder.Property(p => p.AcilDurumKisiTelefon).HasColumnType("varchar(20)");
        builder.Property(p => p.AcilDurumKisiYakinlik).HasColumnType("varchar(50)");

        builder.Property(p => p.BankaAdi).HasColumnType("varchar(100)");
        builder.Property(p => p.IBAN).HasColumnType("varchar(34)");

        builder.Property(p => p.Notlar).HasColumnType("varchar(500)");

        builder.HasIndex(p => p.TCKN).IsUnique(true); 
    }
}
