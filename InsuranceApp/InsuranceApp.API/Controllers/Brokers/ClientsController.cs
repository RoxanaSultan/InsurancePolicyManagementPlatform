using InsuranceApp.Application.DTOs.Clients;
using InsuranceApp.Application.DTOs.Paging;
using InsuranceApp.Application.Interfaces.Services.Client;
using InsuranceApp.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InsuranceApp.API.Controllers.Brokers;

[ApiController]
[Route("api/brokers/[controller]")]
[Authorize(Roles = Roles.Broker)] // cere un JWT valid, al unui utilizator cu rolul Broker
public class ClientsController(IClientService clientsService) : ControllerBase
{
    private readonly IClientService _clientsService = clientsService;

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ClientResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<ClientResponse>>> Search(
        [FromQuery] ClientSearchRequest request,
        [FromQuery] PageRequest pageRequest,
        CancellationToken cancellationToken
        )
    {
        var result = await _clientsService.SearchAsync(request, pageRequest, cancellationToken);

        return Ok(result);
    }

    [HttpGet("{clientId:int}")]
    [ProducesResponseType(typeof(ClientDetailsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ClientDetailsResponse>> GetById(
        [FromRoute] int clientId,
        CancellationToken cancellationToken
        )
    {
        var client = await _clientsService.GetByIdAsync(clientId, cancellationToken);

        return Ok(client);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ClientResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ClientResponse>> Create(
        [FromBody] ClientCreateRequest request, // FromBody -> spune ASP.NET-ului să transforme JSON-ul primit într-un obiect ClientCreateRequest
        CancellationToken cancellationToken
        )
    {
        var createdClient = await _clientsService.CreateAsync(request, cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { clientId = createdClient.Id },
            createdClient
            ); // CreatedAtAction(..) generează răspunsul corect 201 Created, împreună cu locația noului client
    }
    /* înainte să intre în metoda Create, FluentValidation verifică automat requestul
     * dacă una dintre regulile cerute de validator e încălcată, endpointul nu mai este apelat -> se primește direct un 400 Bad Request în formatul de eroare configurat în API
     * dacă requestul e valid, se ajunge în ClientService.cs -> aici e logica aplicației
     * - verifică dacă CNP-ul/CUI-ul, emailul și telefonul sunt deja folosite
     * - transformă DTO-ul primit din API în entitatea de domeniu Client
     * - îl adaugă prin repository
     * - confirmă salvarea prin SaveChangesAsync
     * - transformă entitatea într-un ClientResponse, fără să expună direct modelul intern al bazei de date
     * 
     * UnitOfWork grupează repository-urile și oferă un singur punct pentru confirmarea modificărilor
     * EF Core ține noul client "în memorie" după AddAsync -> abia SaveChangesAsync trimite de fapt instrucțiunea INSERT către SQL Server
     * 
     * ContactInfo și Address sunt value object în Domain, dar EF Core le salvează în coloane ale tabelului Clients, prin configurația ClientConfiguration.cs (nu au tabele separate)
     * dacă e găsit un duplicat, serviciul aruncă ConflictException -> middleware-ul global o transformă într-un răspuns 409 Conflict
     * 
     * Aceeași informație trece prin 3 forme
     * - JSON primit -> DTO de request
     *               -> entitate de domeniu
     *               -> DTO de response
     *               -> JSON trimis înapoi
     *               
     * 1. DTO (Data Transfer Object) - contractul cu exteriorul
     *    - ClientCreateRequest este un DTO
     *    - el descrie exact datele pe care API-ul le acceptă la cerare
     *    - după salvare, API-ul întoarce ClientResponse
     *    - DTO-urile protejează aplicația de requesturi nedorite
     *    
     * 2. Entitatea - lucrul care are identitate
     *    - Entitatea este Client
     *    - un Client este o entitate deoarece are o identiate proprie: Id
     *    - dacă numele sau adresa se modifică, este tot același client dpdv al aplicației și al bazei de date
     *    - entitățile au de regulă -> un Id, un ciclu de viață, relații cu alte entități, reguli de business asociate
     * 
     * 3. Value object - o valoare fără identitate proprie
     *    - ContactInfo și Address sunt value objects
     *    - ele sunt parte din client (nu au viață independentă)
     *    - două value objects cu aceleași valori sunt echivalente conceptual
     *    - faptul că sunt record oferă egalitate bazată pe conținut, nu pe referința din memorie
     *    - init -> obiectul se setează la creare și nu este modificat accidental proprietate cu proprietate
     *    - în SQL Server, ContactInfo nu devine un tabel separat -> EF Core îl tratează ca obiect owned și îl mapează în coloane
     *    
     * AsNoTracking
     *    - EF Core are în mod normal un mecanism numit change tracking -> păstrează în memorie entitățile încărcate și detectează ce proprietăți s-au modificat
     *    - pentru căutări și pagini de listare, acest lucru este inutil și consumă memorie, deci AsNoTracking() este mai rapid
     *    - GetByIdAsync întoarce astfel un obiect detașat de EF Core
     * 
     * - CancellationToken e propagat până la EF Core -> dacă utilizatorul închide pagina sau requestul este abandonat, operațiile SQL în curs pot fi anulate, în loc să consume resurse inutil
     * 
     * - endpointul de login apelează AuthService.LoginAsync -> acolo aplicația
     *      - caută utilizatorul după email sau username
     *      - verifică dacă are rolul Broker
     *      - dacă este Broker, verifică și profilul de broker din aplicație, inclusiv că este activ
     *      - verifică parola prin SignInManager
     *      - dacă totul este valid, creează un JWT
     *      * tokenul include câteva claims - informații pe care API-ul le poate folosi ulterior
     *          - new(ClaimTypes.NameIdentifier, user.Id)
     *          - new(ClaimTypes.Name, user.UserName ?? user.Email ?? user.Id)
     *          - new(ClaimTypes.Role, role)
     *      * de aceea în ClientsController funcționează User.FindFirstValue(ClaimTypes.NameIdentifier)
     *      * nu se caută din nou utilizatorul în baza de date doar ca să aflăm cine a făcut requestul -> user.Id este deja în token, ca claim
     *      * tokenul expiră după două ore și este semnat cu cheia JWT -> semnătura împiedică un client să modifice tokenul (de exemplu, să schimbe rolul din Broker în Adminsitrator)
     *      * JWT-ul nu este criptat implicit (conținutul poate fi citit de cine îl are)
     *      * la fiecare endpoint protejat, middleware-ul configurat în AddJwtAuthentication verifică
     *          - semnătura cu cheia configurată
     *          - issuer-ul
     *          - audience-ul
     *          - expirarea tokenului
     *      * dacă tokenul e absent, invalid sau expirat, rezultatul este 401 Unauthorized
     *      * dacă tokenul e valid, dar nu are rolul cerut de [Authorize(Roles = Roles.Broker)], rezultatul este 403 Forbidden
     *      
     * - InsuranceDbContext -> reprezentarea bazei de date în cod (unește două lumi - tabele Identity, tabelele aplicației)
     * - moștenește IdentityDbContext<IdentityUser, IdentityRole, string>
     * - același context EF Core gestionează atât autentificarea ASP.NET Identity, cât și modelul aplicației
     * - DbSet<T> = acces la un tip de tabel (T)
     * - DbSet<Client> se poate citi ca tabelul Clients (EF Core îl tratează ca o colecție asupra căreia construiești interogări LINQ)
     * - var xlient = await _context.Clients.FirstOrDefaultAsync(x => x.Id == id, cancellationToken
     *      - EF Core traduce această expresie LINQ într-un SELECT SQL
     * 
     * Configurațiile EF Core
     *      - OnModelCreating conține această linie -> builder.ApplyConfigurationsFromAssembly(typeof(InsuranceDbContext).Assembly);
     *      - ea găsește atomat toate clasele de configurare din proiectul Infrastructure (ClientConfiguration.cs, PolicyConfiguration.cs)
     *      - astfel este evitată o clasă DbContext enormă
     *      - connection string -> înregistrarea se face în AddInfrastructure
     *          - services.AddDbContext<InsuranceDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
     *          
     * Migrations
     *      - în Infrastructure există pachetele necesare pentru migrations
     *      - ex: adaug proprietatea X în entitate
     *          - creez migration
     *          - EF Core generează schimbarea SQL
     *          - aplic migration în baza de date
     *      - nu se modifică schema manual, ci se păstrează schimbarea în cod, revizuibilă și repetabilă pentru orice mediu
     */
    [HttpPut("{clientId:int}")]
    [ProducesResponseType(typeof(ClientResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ClientResponse>> Update(
        [FromRoute] int clientId,
        [FromBody] ClientUpdateRequest request,
        CancellationToken cancellationToken
        )
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var updatedClient = await _clientsService.UpdateAsync(userId!, clientId, request, cancellationToken);

        return Ok(updatedClient);
    }

    [HttpGet("{clientId:int}/audit")]
    [ProducesResponseType(typeof(IEnumerable<ClientIdentificationNumberAuditResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<ClientIdentificationNumberAuditResponse>>> GetIdentificationNumberAudit(
        [FromRoute] int clientId,
        CancellationToken cancellationToken
        )
    {
        var audits = await _clientsService
            .GetIdentificationNumberAuditAsync(clientId, cancellationToken);

        return Ok(audits);
    }
}

// controllerul primește requestul, apelează serviciul și întoarce răspunsul HTTP