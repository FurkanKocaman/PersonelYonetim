﻿using PersonelYonetim.Server.Domain.Personeller;
using FluentValidation;
using GenericRepository;
using Mapster;
using MediatR;
using TS.Result;
using PersonelYonetim.Server.Domain.Departmanlar;
using PersonelYonetim.Server.Domain.Pozisyonlar;

namespace PersonelYonetim.Server.Application.Personeller;

public sealed record PersonelCreateCommand(
    string Ad,
    string Soyad,
    DateTimeOffset DogumTarihi,
    bool? Cinsiyet,
    Iletisim Iletisim,
    Adres Adres,
    Guid DepartmanId,
    Guid PozisyonId
    ) : IRequest<Result<string>>;

public sealed class PersonelCreateCommandValidator : AbstractValidator<PersonelCreateCommand>
{
    public PersonelCreateCommandValidator()
    {
        RuleFor(x => x.Ad).NotEmpty().WithMessage("Ad boş olamaz").MaximumLength(50);
        RuleFor(x => x.Soyad).NotEmpty().WithMessage("Soyad boş olamaz").MaximumLength(50);
        RuleFor(x => x.DogumTarihi).NotEmpty().WithMessage("Doğum tarihi boş olamaz");
        RuleFor(x => x.Iletisim.Eposta).NotEmpty().WithMessage("Eposta boş olamaz").EmailAddress();
        RuleFor(x => x.Iletisim.Telefon).NotEmpty().WithMessage("Telefon boş olamaz");
        RuleFor(x => x.DepartmanId).NotEmpty().WithMessage("Departman boş olamaz");
        RuleFor(x => x.PozisyonId).NotEmpty().WithMessage("Pozisyon boş olamaz");
        RuleFor(x => x.Adres.Ulke).NotEmpty().WithMessage("Ülke boş olamaz");
        RuleFor(x => x.Adres.Sehir).NotEmpty().WithMessage("Şehir boş olamaz");
        RuleFor(x => x.Adres.Ilce).NotEmpty().WithMessage("İlçe boş olamaz");
        RuleFor(x => x.Adres.TamAdres).NotEmpty().WithMessage("Tam adres boş olamaz");
    }
}
internal sealed class PersonelCreateCommandHandler(
    IPersonelRepository personelRepository,
    IDepartmanRepository departmanRepository,
    IPozisyonRepository pozisyonRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<PersonelCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(PersonelCreateCommand request, CancellationToken cancellationToken)
    {
        var personelVarMi = await personelRepository.AnyAsync(p => p.Iletisim.Eposta == request.Iletisim.Eposta);
        if (personelVarMi)
            return Result<string>.Failure("Personel zaten mevcut");

        var departman = await departmanRepository.FirstOrDefaultAsync(p => p.Id == request.DepartmanId);
        if (departman is null)
            return Result<string>.Failure("Departman bulunamadı");
        var pozisyon = await pozisyonRepository.FirstOrDefaultAsync(p => p.Id == request.PozisyonId);
        if (pozisyon is null)
            return Result<string>.Failure("Pozisyon bulunamadı");

        Personel personel = request.Adapt<Personel>();

        personelRepository.Add(personel);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Personel başarıyla oluşturuldu";
    }
}

