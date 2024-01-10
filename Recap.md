Pour la creation d'une Application REST API en c# :

# -o pour faire quoi?

-   creation d'un projet ASP.NET Core Web Application grace a la commande : dotnet new webapi -o <NomDuProjet>

-   Ajout de la dependance Microsoft.EntityFrameworkCore.SqlServer grace a la commande : dotnet add package Microsoft.EntityFrameworkCore.SqlServer
-   Ajout de la dependance Microsoft.EntityFrameworkCore.Design grace a la commande : dotnet add package Microsoft.EntityFrameworkCore.Design
-   Ajout de la dependance Microsoft.EntityFrameworkCore.Tools grace a la commande : dotnet add package Microsoft.EntityFrameworkCore.Tools

-   Creation d'un dossier Models

    -   le dossier Models contient les classes qui representent les tables de la base de donnees

-   Creation d'un dossier Data

    -   le dossier Data contient la classe DataContext qui herite de DbContext
    -   DataContext est la classe qui permet de faire le lien entre les classes du dossier Models et la base de donnees

    '''csharp
    public class DataContext : DbContext
    {
    // constructeur de la classe DataContext
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        // DbSet est une collection d'objets qui represente une table de la base de donnees
        public DbSet<Classe> Classes { get; set; }
        public DbSet<Eleve> Eleves { get; set; }

        // on peut ensuite gerer la configuration de la base de donnees dans la methode OnConfiguring
        # Ou est appelé cette methode? Comment le lien est fait?
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        	// on utilise la methode UseSqlServer pour configurer la base de donnees
        	optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=NomDeLaBaseDeDonnees;Trusted_Connection=True;");
        }

    }
    '''

-   Creation d'un dossier Controllers

    -   le dossier Controllers contient les classes qui permettent de gerer les requetes HTTP

    ```csharp
    [Route("api/[controller]")]
    [ApiController]
    public class ClassesController : ControllerBase
    {
    	# Pourquoi? Peut-être préciser ce que fait chaque étape de chaques méthodes plus bas pour ceux qui ont du mal avec le code
    	// on declare une variable de type DataContext
    	private readonly DataContext _context;

    	// on initialise la variable _context dans le constructeur de la classe
    	public ClassesController(DataContext context)
    	{
    		_context = context;
    	}

    	// on declare une methode qui permet de recuperer toutes les classes
    	[HttpGet]
    	public async Task<ActionResult<IEnumerable<Classe>>> GetClasses()
    	{
    		return await _context.Classes.ToListAsync();
    	}

    	# Class qui représente toujours une table de la bdd dans le dossier Models
    	// on declare une methode qui permet de recuperer une classe en fonction de son id
    	[HttpGet("{id}")]
    	public async Task<ActionResult<Classe>> GetClasse(int id)
    	{
    		return await _context.Classes.FindAsync(id);
    	}

    	// on declare une methode qui permet de modifier une classe en fonction de son id
    	[HttpPut("{id}")]
    	public async Task<IActionResult> PutClasse(int id, Classe classe)
    	{
    		if (id != classe.Id)
    		{
    			return BadRequest();
    		}

    		_context.Entry(classe).State = EntityState.Modified;

    		try
    		{
    			await _context.SaveChangesAsync();
    		}
    		catch (DbUpdateConcurrencyException)
    		{
    			if (!ClasseExists(id))
    			{
    				return NotFound();
    			}
    			else
    			{
    				throw;
    			}
    		}

    		# peut etre interessant de renvoyer la classe modifiée
    		return NoContent();
    	}

    	// on declare une methode qui permet de creer une classe
    	[HttpPost]
    	public async Task<ActionResult<Classe>> PostClasse(Classe classe)
    	{
    		_context.Classes.Add(classe);
    		await _context.SaveChangesAsync();

    		return CreatedAtAction("GetClasse", new { id = classe.Id }, classe);
    	}

    	// on declare une methode qui permet de supprimer une classe en fonction de son id
    	[HttpDelete("{id}")]
    	public async Task<ActionResult<Classe>> DeleteClasse(int id)
    	{
    		var classe = await _context.Classes.FindAsync(id);
    		if (classe == null)
    		{
    			return NotFound();
    		}

    		_context.Classes.Remove(classe);
    		await _context.SaveChangesAsync();

    		return classe;
    	}

    }
    ```

# comment (détailler comme fait pour sqlServer)? pourquoi?

-   Une fois le backend pret, Nous allons deployer le serveur SQL sur Azure, une fois fait nous pourrons s'y connecter depuis le backend.

# ajouter des screens et dire ce que fais chaque champs à chaque étape et pourquoi on laisse par defaut où on changenge la valeur

-   Pour deployer le serveur SQL sur Azure, il faut se rendre sur le portail Azure :

    -   Creer une ressource SQL Server

        -   le groupe de ressources pourra posseder le meme groupe de ressources que le backend
        -   il devra autoriser les connexions entrantes publiques pour :
            -   l'adresse IP de l'ordinateur actuel(client)
            -   l'adresse IP du serveur de l'application
        -   l'authentification devra se faire avec user et mot de passe SQL

    -   Creer une ressource SQL Database
        et deployer la base de donnees sur le serveur SQL Server

    -   Une fois la base de donnees deployee, il faut se rendre sur la bdd et recuperer la chaine de connexion sql.
        Vous pouvez la trouver dans la section "Chaine de connexion ADO.NET" de la page "Parametres de la base de donnees"

    Vous pouvez ensuite remplacer la chaine de connexion dans la methode OnConfiguring de la classe DataContext, y rajouter le mot de passe.

Une fois que tout est pret, vous pouvez lancer le backend et tester les requetes HTTP avec Postman.
