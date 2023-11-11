using Ejercicio3.Models;
using System.Linq;
using Ejercicio3.Models.ViewModels;
using Ejercicio4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ejercicio3.Controllers;

public class ReunionesController : Controller {
    private readonly PracticaCSharpContext _context;


    public ReunionesController(PracticaCSharpContext context) {
        _context = context;
    }

    /*public async Task<IActionResult> Index() {
        ViewData["ActivePage"] = "Reuniones";
        return View(await _context.Reunions.ToListAsync());
    }*/
    
    public async Task<IActionResult> Index()
    {
        ViewData["ActivePage"] = "Reuniones";
        
        var reunionData = _context.AlumnoReunionViews
            .Select(a => new AlumnoReunionView
            {
                id_persona = a.id_persona,
                nombre_uno = a.nombre_uno,
                nombre_dos = a.nombre_dos,
                apellido_uno = a.apellido_uno,
                apellido_dos = a.apellido_dos,
                D_nacimiento = a.D_nacimiento,
                matricula = a.matricula,
                carrera = a.carrera,
                semestre = a.semestre,
                especialidad = a.especialidad,
                id_reunion = a.id_reunion,
                fecha = a.fecha,
                hora = a.hora,
                lugar = a.lugar,
                tema = a.tema
            })
            .ToList();

        return View(reunionData);
    }
}