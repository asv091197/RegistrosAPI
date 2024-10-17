using Microsoft.AspNetCore.Mvc;
using RegistrosAPI.Models;
using System.Linq;

[ApiController]
[Route("api/[controller]")]
public class DatosController : ControllerBase
{
    private readonly FormulariosDBContext _context;

    public DatosController(FormulariosDBContext context)
    {
        _context = context;
    }

    [HttpPost]
    public IActionResult Enviar([FromBody] Datos data)
    {
        if (string.IsNullOrEmpty(data.Nombre) || !data.Nombre.All(c => char.IsLetter(c) || c == ' '))
        {
            return BadRequest(new { message = "El nombre solo debe contener letras y espacios." });
        }

        if (!IsValidEmail(data.Email))
        {
            return BadRequest(new { message = "El formato del correo no es válido." });
        }

        if (data.Telefono.Length != 10 || !data.Telefono.All(char.IsDigit))
        {
            return BadRequest(new { message = "El número de teléfono debe tener 10 dígitos." });
        }

        if (!data.AceptoLosTerminos)
        {
            return BadRequest(new { message = "Debe aceptar los términos y condiciones." });
        }

        _context.Datos.Add(data);
        _context.SaveChanges();

        return Ok(new { message = "Datos guardados correctamente", data });
    }

    [HttpGet]
    public IActionResult GetAllDatos()
    {
        var datos = _context.Datos.ToList();
        return Ok(datos);
    }

    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}
