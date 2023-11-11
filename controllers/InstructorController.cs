using Ejercicio3.Models;
using Ejercicio3.Models.ViewModels;
using Ejercicio4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ejercicio3.Controllers;

public class InstructorController : Controller {
    private readonly PracticaCSharpContext _context;
    private Encriptacion _encrypt;

    public InstructorController(PracticaCSharpContext context) {
        _context = context;
        _encrypt = new Encriptacion();
    }
    
    public async Task<IActionResult> Index() {
        ViewData["ActivePage"] = "Instructor";
        var instructor = await _context.Personas
            .Where(persona => persona.Instructor != null)
            .Include(persona => persona.Instructor)
            .ToListAsync();

        return View(instructor);
    }
    
    public async Task<IActionResult> Edit(int id)
    { // Esta función trae los datos correspondientes al alumno con el ID que recibe la función y los inyecta al formulario en la vista Edit.
        ViewData["ActivePage"] = "Instructor"; // Indicamos que la página activa es Alumno. Con esto podemos aplicar la clase 'active' al link en el navbar.
        var instructor = await _context.Personas
            .Where(persona => persona.IdPersona == id)
            .Include(persona => persona.Instructor)
            .FirstOrDefaultAsync();

        // Declaramos a la variable alumno 
        // await _context.Personas es una operación asincrónica para obtener todos los registros de la
        // tabla "Personas" de la base de datos.
        
        // Where(persona => persona.IdPersona == id) filtra la colección de personas seleccionando a aquella
        // cuyo id_persona sea igual al id que está recibiendo la función Edit.
        
        //  El método .Include() se utiliza para cargar de manera anticipada la propiedad de navegación "Alumno"
        // de las personas seleccionadas. Esto significa que, cuando se recuperen los estudiantes, también se cargarán
        // en memoria las propiedades relacionadas de "Alumno" para evitar cargarlas de manera diferida más adelante.
        // La carga anticipada se conoce como "eager loading" y la carga diferida como "lazy loading".
        
        // .FirstOrDefaultAsync(): Este método se utiliza para seleccionar la primera persona que cumple con el
        // filtro establecido. La operación se realiza de manera asincrónica, lo que significa que se espera la
        // respuesta de la base de datos de manera asincrónica y, una vez que se completa, se obtiene el primer
        // objeto que cumple con el filtro o null si no se encuentra ninguna coincidencia.
        
        if (instructor != null)
        { // Si alumno no es núlo...
            var model = new InstructorViewModel() // Declaramos una variable llamada model y usando la clase AlumnoViewModel
            {
                // Los campos de AlumnoViewModel(lado izquierdo) serán iguales a lo recibido en la variable alumno(lado derecho).
                IdPersona = instructor.IdPersona,
                NombreUno = _encrypt.Desencriptar(instructor.NombreUno),
                NombreDos = instructor.NombreDos,
                ApellidoUno = instructor.ApellidoUno,
                ApellidoDos = instructor.ApellidoDos,
                FechaNacimiento = instructor.DNacimiento,
                Folio = instructor.Instructor.Folio
            };

            return View("Edit", model); // Generamos la vista de nombre Edit y redirigimos a ella y además, le pasamos el modelo con los datos ya asignados.
        }

        return RedirectToAction("Index"); // Retornamos al usuario a la acción Index.
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(InstructorViewModel model)
    { // Esta función actualiza los datos del alumno en la base de datos usando el modelo que recibe.
        if (ModelState.IsValid)
        { // Si el estado del modelo es válido...
            var instructor = await _context.Personas
                .Where(persona => persona.IdPersona == model.IdPersona)
                .Include(persona => persona.Instructor)
                .FirstOrDefaultAsync();
            
            // Declaramos a la variable alumno 
            // await _context.Personas es una operación asincrónica para obtener todos los registros de la
            // tabla "Personas" de la base de datos.
            
            // Where(persona => persona.IdPersona == model.IDPersona) filtra la colección de personas seleccionando
            // a aquella cuyo id_persona sea igual al id_persona que está recibiendo en el modelo.
            
            //  El método .Include() se utiliza para cargar de manera anticipada la propiedad de navegación "Alumno"
            // de las personas seleccionadas. Esto significa que, cuando se recuperen los estudiantes, también se cargarán
            // en memoria las propiedades relacionadas de "Alumno" para evitar cargarlas de manera diferida más adelante.
            // La carga anticipada se conoce como "eager loading" y la carga diferida como "lazy loading".
            
            // .FirstOrDefaultAsync(): Este método se utiliza para seleccionar la primera persona que cumple con el
            // filtro establecido. La operación se realiza de manera asincrónica, lo que significa que se espera la
            // respuesta de la base de datos de manera asincrónica y, una vez que se completa, se obtiene el primer
            // objeto que cumple con el filtro o null si no se encuentra ninguna coincidencia.
            
            if (instructor != null)
            { // Si alumno es distinto de núlo...
                // Los campos de la variable alumno(lado izquierdo), que recibe los datos directamente de la base de datos,
                // serán igual a los datos que se recibieron en el modelo(lado derecho).
                instructor.NombreUno = _encrypt.Encriptar(model.NombreUno);
                instructor.NombreDos = model.NombreDos;
                instructor.ApellidoUno = model.ApellidoUno;
                instructor.ApellidoDos = model.ApellidoDos;
                instructor.DNacimiento = model.FechaNacimiento;

                instructor.Instructor.Folio = model.Folio;

                await _context.SaveChangesAsync();
                // await _context.SaveChangesAsync(); significa que se están guardando todos los cambios pendientes
                // en el contexto de datos de Entity Framework de manera asincrónica. Esto significa que que no bloqueará
                // el hilo de ejecución principal, permitiendo que el programa continúe ejecutándose de manera eficiente
                // mientras se espera la respuesta de la base de datos.

                return RedirectToAction("Index"); // Retornamos al usuario a la acción Index.
            }
        }

        return View("Edit", model); // Si hay errores de validación, muestra la vista de edición nuevamente.
    }
    
    public IActionResult add()
    {
        ViewData["ActivePage"] = "Instructor";
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> add(InstructorViewModel model)
    {
        if (ModelState.IsValid)
        {
            // Crear una instancia de Persona y llenarla con los datos del modelo AlumnoViewModel
            //var encriptado = _encrypt.Encriptar(model.NombreUno);
            var persona = new Persona
            {
                NombreUno = _encrypt.Encriptar(model.NombreUno),
                NombreDos = model.NombreDos,
                ApellidoUno = model.ApellidoUno,
                ApellidoDos = model.ApellidoDos,
                DNacimiento = model.FechaNacimiento,
                TipoRol = "Instructor"
            };
    
            // Agregar el objeto Persona al contexto y guardar los cambios en la base de datos
            _context.Personas.Add(persona);
            await _context.SaveChangesAsync();
    
            // Obtener el ID generado para la nueva persona
            var idPersona = persona.IdPersona;
    
            // Crear una instancia de Alumno y llenarla con los datos del modelo AlumnoViewModel
            var instructor = new Instructor
            {
                IdPersona = idPersona, // Asociar el ID de la persona con el alumno
                Folio = model.Folio
            };
    
            // Agregar el objeto Alumno al contexto y guardar los cambios en la base de datos
            _context.Instructors.Add(instructor);
            await _context.SaveChangesAsync();
    
            return RedirectToAction(nameof(Index));
        }
    
        return View(model);
    }
    
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var instructor = await _context.Personas
            .Where(persona => persona.IdPersona == id)
            .Include(persona => persona.Instructor)
            .FirstOrDefaultAsync();

        if (instructor != null)
        {
            _context.Personas.Remove(instructor);
            await _context.SaveChangesAsync();
        }
        
        return RedirectToAction("Index");
    }
}