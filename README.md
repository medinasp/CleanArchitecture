# CleanArchitecture
Structure for work on Clean Architecture

<h1 align="center"> Estrutura Clean para iniciar desenvolvimento </h1>

Esta é uma estrutura pronta para desenvolver com Clean Architecture facilitando a aplicação de um código desacoplado e observando princípios SOLID, escalável e responsável.

Você pode pegar o código e desenvolver a partir dele, ou pode observar o tutorial abaixo de como deenvolve-lo você mesmo.

É possivel e até provável que haja erros, porque estou aprendendo, fique a vontade para apontá-los se quiser, será de grande ajuda:

Este projeto é baseado nas aulas do professor Macoratti.


Projeto Estrutura Clean para reaproveitar
Pegue pronto ou faça você mesmo seguindo o passo a passo:

Clean Structure Project
Take to go or make yourself following the instruction step by step.


1 - Abrir Visual Studio - New Project / Blank Solution - vídeo 19 min
2 - Criar diretorios - dentro da Blank Solution criada - botão direito no projeto/Add/New Solution Folder
----- 0 - Presentation
----- 1 - Services
----- 2 - Application
----- 3 - Domain
----- 4 - Infra

3 - Dentro da Camada "Presentation" adicionar projeto MVC:
Add
  |__New Project
	|__Asp Net Core Web-App (Model-View-Controller)
		|__Project Name - CleanArch.UI.MVC (Sem Authentication, Sem Docker) .net5.0

4 - Dentro da Camada de aplicação "Application", adicionar novo projeto:
Add
  |__New Project
	|__Class Library
		|__Project Name - CleanArch.Application .net5.0

	4.1 - Remover a classe criada automaticamente "Class1.cs"

	5.2 - Criar Diretórios dentro de Application/CleanArch.Application
		1 - Interfaces
		2 - Services
		3 - ViewModels

5 - Dentro da Camada de domínio "Domain", adicionar novo projeto:
Add
  |__New Project
	|__Class Library
		|__Project Name - CleanArch.Domain .net5.0

	5.1 - Remover a classe criada automaticamente "Class1.cs"

	5.2 - Criar Diretórios dentro de Domain/ProjetoModeloDDD.Domain
		Entities
		Interfaces

	5.3 - Criar classe para representar o modelo de domínio no diretório Entities, exemplo classe "Product"
		Botão direito no diretório Entities
		  |__Add
			|__Class
				|__Product.cs
		
			namespace CleanArch.Domain.Entities
			{
			    public class Product
			    {
			        public int Id { get; set; }
			        public string Name { get; set; }
			        public string Description { get; set; }
			        public decimal Price { get; set; }
			    }
			}
	5.4 - Adicionar na camada Presentation, uma referência a este projeto CleanArch.Domain
		|__Add
	           |__Project Reference
	              |__CleanArch.Domain

	5.5 - Adicionar na camada Application, uma referência a este projeto CleanArch.Domain
		|__Add
	           |__Project Reference
	              |__CleanArch.Domain

6  Dentro da camada Infra/Data, adicionar novo projeto
Add
  |__New Project
	|__Class Library
		|__Project Name - CleanArch.Infra.Data .net5.0

	6.1 - Remover a classe criada automaticamente "Class1.cs"

	6.2 - Adicionar nesta camada, uma referência ao projeto CleanArch.Domain
		|__Add
           |__Project Reference
              |__CleanArch.Domain

	6.3 - Criar Diretórios dentro de Infra/CleanArch.Infra.Data
		Context
		Repositories

	6.4 - Instalar os seguintes pacotes nesta camada de Infra:
		Microsoft.EntityFrameworkCore.Design 5.0.17 (Instalar este também na camada Presentation, mesma versão)
		Microsoft.EntityFrameworkCore.SqlServer 5.0.17
		Microsoft.EntityFrameworkCore.Tools 5.0.17

		|__Tools
	           |__NuGet Package Manager
	              |__Manage NuGet Package for Solutions
		              |__Buscar pacotes e versões mencionados acima e instalar

	6.5 - No diretório Context, criar classe de contexto para nosso domínio Product
		Botão direito no diretório Context
		  |__Add
			|__Class
				|__ProductDbContext.cs

		using CleanArch.Domain.Entities;
		using Microsoft.EntityFrameworkCore;

		namespace CleanArch.Infra.Data.Context
		{
			public  class ProductDbContext : DbContext
			{
			   public ProductDbContext(DbContextOptions options) : base(options)
			   { }
			   public DbSet<Product> Products { get; set; }
			}
		}

	6.5 - Voltar a camada Presentation e Configurar o Startup e o appsettings.json
		Inserir no StartUp:

		using CleanArch.Infra.Data.Context;
		using Microsoft.AspNetCore.Builder;
		using Microsoft.AspNetCore.Hosting;
		using Microsoft.EntityFrameworkCore;
		using Microsoft.Extensions.Configuration;
		using Microsoft.Extensions.DependencyInjection;
		using Microsoft.Extensions.Hosting;

			 services.AddDbContext<ProductDbContext>(options =>
            		   options.UseSqlServer(
           		      Configuration.GetConnectionString("DefaultConnection")));
      			      services.AddControllersWithViews();

		Inserir no appsettings.json:
		{
  			"ConnectionStrings": {
 			   "DefaultConnection": "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=ProductDB;Integrated Security=True"
 			 },

 			 "Logging": {
 			   "LogLevel": {
  			    "Default": "Information",
   			   "Microsoft": "Warning",
  			    "Microsoft.Hosting.Lifetime": "Information"
  			  }
 			 },
 		 "AllowedHosts": "*"
		}

	6.6 - Rodar Migration no PM da camada Infra:
		|__Tools
	           |__NuGet Package Manager
	              |__Package Manager Console

		Dentro do Console do Package Manager, no menu suspenso, escolher "Default Project" Infra Data.
		Digitar comandos 
			add-migration InicialProduct
			update-database

		Se tiver mais de um contexto e pedir para especificar:
			add-migration InicialProduct -Context ProductDbContext
			update-database -Context ProductDbContext

7 - Voltando a camada Application:
	7.1 - Criar ViewModel "ProductViewModel" para representar os dados que eu desejo exibir na minha página/view:
		|__Botão direito no diretório ViewModels
	           |__Add
	              |__Class
		              |__ProductViewModel

		using CleanArch.Domain.Entities;
		using System.Collections.Generic;
		namespace CleanArch.Application.ViewModels
		{
			public class ProductViewModel
			{
				public IEnumerable<Product> Produtos { get; set; }
			}
		}

	7.2 Criar Interface de Products na pasta Interfaces
		|__Botão direito no diretório Interfaces
	           |__Add
	              |__Class
		              |__IProductService
		
		using CleanArch.Application.ViewModels;
		namespace CleanArch.Application.Interfaces
		{
			public interface IProductService
			{
				ProductViewModel GetProdutcs();
			}
		}

		Quando o método GetProducts() for implementado, ele retornará uma lista de produtos baseado nesta ViewModel, abstraindo o modelo de domínio da entidade Product.

8 - Voltando à camada Domain
	8.1 Criar Interface para chamar o Repositório que será criado na camada Infra para intermediar o acesso ao Banco de Dados que será feito pelo Entity Framework.
		|__Botão direito no diretório Interfaces
	           |__Add
	              |__Class
		              |__IProductRepository
		
		using CleanArch.Domain.Entities;
		using System.Collections.Generic;
		namespace CleanArch.Domain.Interfaces
		{
			public interface IProductRepository
			{
				IEnumerable<Product> GetProducts();
			}
		}

9 - Voltando à camada Infra.Data
	9.1 Criar Repositório
		|__Botão direito no diretório Repositories
	           |__Add
	              |__Class
		              |__ProductRepository

		using CleanArch.Domain.Entities;
		using CleanArch.Domain.Interfaces;
		using CleanArch.Infra.Data.Context;
		using System.Collections.Generic;

		namespace CleanArch.Infra.Data.Repositories
		{
			public class ProductRepository : IProductRepository
			{
				private ProductDbContext _context;

				public ProductRepository(ProductDbContext context)
				{
					_context = context;
				}

				public IEnumerable<Product> GetProducts()
				{
					return _context.Products;
				}
			}
		}

		Note que injetamos uma instância do contexto para a seguir ter acesso a entidade Products que esta mapeada para a tabela Products.

10 - Voltando à camada Application
	Finalmente criar o ProductService na pasta Services:
		10.1 Criar ProductService no diretório Services
		|__Botão direito no diretório Services
	           |__Add
	              |__Class
		              |__ProductService

		using CleanArch.Application.Interfaces;
		using CleanArch.Application.ViewModels;
		using CleanArch.Domain.Interfaces;

		namespace CleanArch.Application.Services
		{
			public class ProductService : IProductService
			{
				private IProductRepository _productRepository;

				public ProductService(IProductRepository productRepository)
				{
					_productRepository = productRepository;
				}

				public ProductViewModel GetProducts()
				{
					return new ProductViewModel()
					{
						Produtos = _productRepository.GetProducts()
					};
				}
			}
		}

		Observe que injetamos uma instância da nossa implementação do padrão Repository para acessar o método GetProducts() a partir do Repositório.

11 - Dentro da camada INFRA, Implementar o projeto CleanArch.Infra.IoC para separar as dependências
	11.1 Add
		  |__New Project
			|__Class Library
				|__Project Name - CleanArch.Infra.IoC .net5.0

	11.2 Instalar pacote Microsoft.Extensions.DependencyInjection via Nuget. 6.0.0

	11.3 Adicionar referências aos projetos : Application, Domain e Infra.Data
			|__Add
	           |__Project Reference
	              |__CleanArch.Application
				  |__CleanArch.Domain
				  |__CleanArch.Infra.Data

	11.4 Criar classe do contêiner de dependências DependencyContainer para registrar os serviços para repositório e produto e suas interfaces:
		Add
		  |__Class
			|__DependencyContainer
		
		using CleanArch.Application.Interfaces;
		using CleanArch.Application.Services;
		using CleanArch.Domain.Interfaces;
		using CleanArch.Infra.Data.Repositories;
		using Microsoft.Extensions.DependencyInjection;
		namespace CleanArch.Infra.IoC
		{
			public class DependencyContainer
			{
				public static void RegisterServices(IServiceCollection services)
				{
					services.AddScoped<IProductService, ProductService>();
					services.AddScoped<IProductRepository, ProductRepository>();
				}
			}
		}

		Usamos o tempo de vida "AddScoped" para os serviços o que significa que os objetos usados serão iguais dentro do mesmo request mas diferentes em requisições distintas,
		ou seja, cada request obtém uma nova instância do serviço.

12 - Adicionar referência e registrar o container acima no StartUp da camada presentation:
	12.1 - Adicionar referência ao projeto Infra.IoC
			|__Add
	           |__Project Reference
				  |__CleanArch.Infra.IoC
	
	...
			public void RegisterServices(IServiceCollection services)
			{
				DependencyContainer.RegisterServices(services);
			}
	...

	Incluir também a chamada do RegisterServices no StartUp Configure Services:
	
        public void ConfigureServices(IServiceCollection services)
        {
			...
            RegisterServices(services);
			...
        }

	Com isso concluímos a implementação dos projetos que representam as camadas da nossa aplicação em nossa solução.

********************************************************************************************************************

1 - Tratamento da Interface com o usuário - Definição de Controllers e Views
	1 - Adicionar Controller ao projeto Presentation
			|__Botão direito na pasta Controllers - Add / Controller
	           |__ProductsController
				  |__Add
				  
		Injetamos uma instância do serviço do produto e passamos  a View Model ProductViewModel para a nossa view Index.

		using Microsoft.AspNetCore.Mvc;
		using CleanArch.Application.Interfaces;
		using CleanArch.Application.ViewModels;

		namespace CleanArch.UI.MVC.Controllers
		{
			public class ProductsController : Controller
			{
				private IProductService _productService;

				public ProductsController(IProductService productService)
				{
					_productService = productService;
				}

				public IActionResult Index()
				{
					ProductViewModel model = _productService.GetProducts();
					return View(model);
				}
			}
		}

	1.2 - Criar view Index para exibir a interface de Products
		|__Botão direito na pasta Views
	        |__Add / New Folder
				|__Products

		|__Botão direito na pasta Products
	        |__Add / View
				|__Razor View Empty
					|__Index

			@*
				For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
			*@
			@{
			@model CleanArch.Application.ViewModels.ProductViewModel

			<table id="book" class="table table-bordered table-hover">
				<thead>
					<tr class="">
						<th>Name</th>
						<th>Description</th>
						<th>Price</th>
					</tr>
				</thead>
				<tbody>
					@foreach (var product in Model.Produtos)
					{
						<tr style="height: 60px;">
							<td>@product.Name</td>
							<td>@product.Description</td>
							<td>@product.Price.ToString("c")</td>
						</tr>
					}
				</tbody>
			</table>
			}

	1.3 - Adicionar link de navegação para Products no _Layout.cshtml, na pasta Views/Shared:
		...
			<li class="nav-item">
				<a class="nav-link text-dark" asp-area="" asp-controller="Products" asp-action="Index">Products</a>
			</li>
		...
		

	Podemos executar o projeto usando o comando "dotnet watch run" no terminal, a partir da pasta onde esta o projeto MVC.
	Com isso podemos fazer alterações no código sem ter que interromper a execução do projeto.
