using System.Text;
using BibliotecaApi.Autorizacao;
using BibliotecaApi.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Acervo em memória (compartilhado por toda a aplicação).
builder.Services.AddSingleton<BibliotecaStore>();

// ── Autenticação JWT — exatamente como ficou na Aula 08 ─────────────
var jwt = builder.Configuration.GetSection("Jwt");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwt["Issuer"],
        ValidAudience = jwt["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!)),
        ClockSkew = TimeSpan.Zero
    };
});

// ── Autorização: o lugar CENTRAL das regras de acesso ───────────────
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("GerenciarAcervo",
        policy => policy.RequireRole("Bibliotecario", "Admin"));
    options.AddPolicy("RemoverLivro",
        policy => policy.RequireRole("Admin"));
    options.AddPolicy("VerRelatorios",
        policy => policy.RequireRole("Admin"));

    // ABAC: a policy que decide olhando o RECURSO.
    options.AddPolicy("PodeRenovarEmprestimo",
        p => p.Requirements.Add(new DonoDoEmprestimoRequirement()));

    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});
builder.Services.AddSingleton<IAuthorizationHandler, DonoDoEmprestimoHandler>();

// ── PBAC: políticas fora do código, recarregadas a quente ────────────
builder.Configuration.AddJsonFile("politicas.json", optional: false, reloadOnChange: true);
builder.Services.Configure<PoliticasOptions>(builder.Configuration.GetSection("Politicas"));
builder.Services.AddSingleton<MotorDePoliticas>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
