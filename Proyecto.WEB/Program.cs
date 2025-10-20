using Microsoft.EntityFrameworkCore;
using Proyecto.BLL.Interfaces;
using Proyecto.BLL.Servicios;
using Proyecto.DAL.DataContext;
using Proyecto.DAL.ModelosRepositorios;
using Proyecto.DAL.Repositorio;
using Proyecto.MODELS;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<PuntoVentaContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlString"));
});


builder.Services.AddScoped<IGenericRepository<Producto>, ProductoRepository>();
builder.Services.AddScoped<IProductoService, ProductoService>();

builder.Services.AddScoped<IGenericRepository<Usuario>, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

builder.Services.AddScoped<IGenericRepository<Categorium>, CategoriaRepository>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();

builder.Services.AddScoped<IGenericRepository<Proveedor>, ProveedorRepository>();
builder.Services.AddScoped<IProveedorService, ProveedorService>();

builder.Services.AddScoped<IGenericRepository<Cliente>, ClienteRepository>();
builder.Services.AddScoped<IClienteService, ClienteService>();

builder.Services.AddScoped<IGenericRepository<Ventum>, VentasRepository>();
builder.Services.AddScoped<IVentasService, VentasService>();

builder.Services.AddScoped<IGenericRepository<Compra>, CompraRepository>();
builder.Services.AddScoped<IGenericRepository<DetalleCompra>, DetalleCompraRepository>();
builder.Services.AddScoped<IGenericRepository<DetalleVentum>, DetalleVentaRepository>();
builder.Services.AddScoped<IGenericRepository<MovimientoStock>, MovimientoStockRepository>();
builder.Services.AddScoped<IGenericRepository<PagoFiado>, PagoFiadoRepository>();





var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
