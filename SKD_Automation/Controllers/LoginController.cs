using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


using AM.Domain.Entities;
using AM.Persistence;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Am.Persistence.Seeding;

namespace SKD_Automation.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUnitWorkService _service;
        private readonly IMapper _mapper;
        private readonly IValidator<Department> _validator;
        private readonly string _plgnIncludeEntities;


        public LoginController(IUnitWorkService service, IMapper mapper, IValidator<Department> validator)
        {
            _service = service;
            _mapper = mapper;
            _validator = validator;
        }


    }
}
