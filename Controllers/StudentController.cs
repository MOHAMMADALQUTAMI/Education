using System;
using ManyToMany.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace ManyToMany.Controllers;
[ApiController]
[Route("[controller]/[action]")]

public class StudentController : ControllerBase
{
    private readonly AppDbContext _context;
    public StudentController(AppDbContext context)
    {
        _context = context;
    }
    [HttpPost]
    public async Task<IActionResult> CreateStudent(String name, string? address)
    {
        if (ModelState.IsValid)
        {
            var student = new Student { Name = name };
            if (address is not null)
            {
                student.Address = new Address { Location = address };
            }
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return Ok(student);
        }
        return BadRequest(ModelState);
    }
    [HttpPut]
    public async Task<ActionResult> UpdateStudentAddress(int studentId, [FromBody] string address)
    {
        var student = await _context.Students.FindAsync(studentId);
        if (student == null)
        {
            return NotFound();
        }
        student.Address = new Address { Location = address };
        await _context.SaveChangesAsync();
        return Ok(student);
    }
    [HttpPost]
    public async Task<ActionResult> AssignStudentToCourse(int courseId, int studentId)
    {
        var course = await _context.Courses.FindAsync(courseId);
        if (course == null)
        {
            return NotFound();
        }
        var student = await _context.Students.FindAsync(studentId);
        if (student == null)
        {
            return NotFound();
        }
        course.Students.Add(student);
        await _context.SaveChangesAsync();
        return Ok("Student assigned to course");
    }
    [HttpGet]
    public async Task<Object> GetStudentWithAddress(int studentId)
    {
        var query =
        from student in _context.Students
        join address in _context.Addresses on student.AdressId equals address.Id into addressj
        from address in addressj.DefaultIfEmpty()
        where student.Id == studentId
        select new
        {
            studentId = student.Id,
            studentName = student.Name,
            adaressId = address.Id,
            location = address.Location

        };
        return await query.SingleAsync();
    }
    [HttpGet]
    public async Task<IEnumerable<Course>> GetCoursesWithAssignedStudents(int courseId)
    {
        var query =
        from course in _context.Courses.Include(c => c.Students).ThenInclude(s => s.Address)
        where course.Id == courseId
        select course;
        return await query.ToListAsync();
    }
    [HttpGet]
    public async Task<IEnumerable<Student>> GetStudentAndTheirCourses(int studentId)
    {
        var query =
        from student in _context.Students.Include(s => s.Courses)
        where student.Id == studentId
        select student;
        return await query.ToListAsync();
    }
    [HttpGet]
    public async Task<IEnumerable<Student>> GetStudentAndTheirCoursesAndAddress(int studentId)
    {
        var query =
        from student in _context.Students.Include(s => s.Courses).Include(s => s.Address)
        where student.Id == studentId
        select student;
        return await query.ToListAsync();
    }
    [HttpPost]
    public async Task<IActionResult> CreateCourse(string name)
    {
        if (ModelState.IsValid)
        {
            _context.Courses.Add(new Course { Title = name });
            await _context.SaveChangesAsync();
            return Ok();
        }
        return BadRequest(ModelState);
    }

}
