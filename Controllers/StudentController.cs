using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentApi.Application.Services;
using StudentApi.Web.Authorize;
using StudentApi.Web.Framework;

namespace StudentApi.Web.Controllers
{
    [Route("api/")]
    [AuthorizeApi]
    [ApiController]
    public class StudentController : BaseController
    {
        private readonly IStudentService _studentService;
        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }
        [HttpGet]
        [Route("Student")]
        public IActionResult GetAllStudent()
        {
            return Response(_studentService.GetStudents());
        }
        [HttpGet]
        [Route("StudentEnrollment")]
        public IActionResult GetEnrollment(int Id)
        {
            return Response(_studentService.GetEntrollment(Id));
        } 
        [HttpGet]
        [Route("StudentGrade")]
        public IActionResult GetGrade(int Id)
        {
            return Response(_studentService.GetGrade(Id));
        }
        [HttpGet]
        [Route("StudentDetails")]
        public IActionResult GetStudentDetById(int Id)
        {
            return Response(_studentService.GetStudentsInfo(Id));
        }
    }
}
