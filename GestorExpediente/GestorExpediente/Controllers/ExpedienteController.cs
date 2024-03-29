﻿using AutoMapper;
using GestorExpediente.DTO;
using GestorExpediente.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using System.Text;
using System.Collections.Generic;

namespace GestorExpediente.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ExpedienteController : ControllerBase
    {
        private GestorExpedienteContext _contexto;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ILogger<ExpedienteController> _logger;

        public ExpedienteController(GestorExpedienteContext context, IConfiguration configuration, IMapper mapper, ILogger<ExpedienteController> logger)
        {
            _contexto = context;
            _configuration = configuration;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("listar/")]
        public ActionResult<IEnumerable<Expediente>> Expedientes()
        {
            var lst = (from tbl in _contexto.Expediente where tbl.Id > 0 select tbl).ToList();

            List<ExpedienteDTO> expedientesDTO = _mapper.Map<List<ExpedienteDTO>>(lst);

            foreach (var item in expedientesDTO)
            {
                var acto = (from h in _contexto.Acto where h.Id == item.IdActo select h).FirstOrDefault();
                if (acto != null)
                {
                    item.ActoNombre = acto.Nombre;
                }

                var caratula = (from h in _contexto.Caratula where h.Id == item.IdCaratula select h).FirstOrDefault();
                if (caratula != null)
                {
                    item.CaratulaNombre = caratula.Nombre;
                }

                var situacionRevista = (from h in _contexto.SituacionRevista where h.Id == item.IdSituacionRevista select h).FirstOrDefault();
                if (situacionRevista != null)
                {
                    item.SituacionRevistaNombre = situacionRevista.Nombre;
                }
            }

            return Accepted(expedientesDTO);
        }


        [HttpGet("buscar/{IdExpediente}")]
        public ActionResult<Expediente> Expedientes(int IdExpediente)
        {
            try
            {
                Expediente item = (from tbl in _contexto.Expediente where tbl.Id == IdExpediente select tbl).FirstOrDefault();
                if (item == null)
                {
                    return NotFound(IdExpediente);
                }

                ExpedienteDTO expedienteDTO = _mapper.Map<ExpedienteDTO>(item);

                var acto = (from h in _contexto.Acto where h.Id == expedienteDTO.IdActo select h).FirstOrDefault();
                if (acto != null)
                {
                    expedienteDTO.ActoNombre = acto.Nombre;
                }

                var caratula = (from h in _contexto.Caratula where h.Id == expedienteDTO.IdCaratula select h).FirstOrDefault();
                if (caratula != null)
                {
                    expedienteDTO.CaratulaNombre = caratula.Nombre;
                }

                var situacionRevista = (from h in _contexto.SituacionRevista where h.Id == expedienteDTO.IdSituacionRevista select h).FirstOrDefault();
                if (situacionRevista != null)
                {
                    expedienteDTO.SituacionRevistaNombre = situacionRevista.Nombre;
                }

                //_logger.LogWarning("Búsqueda de Expediente Id: " + _id + ". Resultados: " + item.Nombre);
                return Accepted(expedienteDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("nuevo")]
        //[Authorize(Roles = "Administrador")]
        public ActionResult nuevo(ExpedienteDTO nuevo)
        {
            try
            {
                Expediente expediente = _mapper.Map<Expediente>(nuevo);

                /*{
                   "id": 0,
                  "nombre": "ferioli martina",
                  "expediente1": "Exp-2022-33230698-GDEBA-hieacsjdmsalgp",
                  "fecha": "2022-10-17T01:04:07.409Z",
                  "documento": "string",
                  "idCaratula": 1,
                  "caratulaNombre": "",
                  "idActo": null,
                  "actoNombre": "",
                  "idSituacionRevista": 1,
                  "situacionRevistaNombre": "",
                  "fechaExpediente": "2022-10-01T01:04:07.409Z",
                  "firmadoSumario": 1,
                  "firmadoLaborales": 1,
                  "enviadoLaborales": true,
                  "avisado": true,
                  "observaciones": ""
                }*/

                _contexto.Expediente.Add(expediente);
                _contexto.SaveChanges();

                nuevo.Id = nuevo.Id;

                //_logger.LogWarning("Se insertó un nuevo Expediente: " + nuevo.Id + ". Nombre: " + nuevo.Nombre);
                return Accepted(nuevo);

            }
            catch (Exception ex)
            {
                //_logger.LogError("Ocurrió un error al insertar el Expediente: " + nuevo.Nombre + ". Detalle: " + ex.Message);
                return BadRequest(ex.Message);
            }


        }

        [HttpPut("actualizar")]
        //[Authorize(Roles = "Administrador")]
        public ActionResult actualizar(ExpedienteDTO actualiza)
        {
            string oldName = "";
            try
            {
                var item = (from h in _contexto.Expediente where h.Id == actualiza.Id select h).FirstOrDefault();

                if (item == null)
                {
                    return NotFound(actualiza);
                }
                oldName = item.Nombre;
                item.Nombre = actualiza.Nombre;

                _contexto.Expediente.Update(item);
                _contexto.SaveChanges();
                //_logger.LogWarning("Se actualizó el Expediente: " + actualiza.Id + ". Nombre anterior: " + oldName + ". Nombre actual: " + actualiza.Nombre);
                return Accepted(actualiza);
            }
            catch (Exception ex)
            {
                //_logger.LogError("Ocurrió un error al actualizar el Expediente: " + oldName + ". Detalle: " + ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("eliminar/{IdExpediente}")]
        //[Authorize(Roles = "Administrador")]
        public ActionResult eliminar(int IdExpediente)
        {
            var item = (from h in _contexto.Expediente where h.Id == IdExpediente select h).FirstOrDefault();

            if (item == null)
            {
                return NotFound(IdExpediente);
            }

            _contexto.Expediente.Remove(item);
            _contexto.SaveChanges();
            //_logger.LogWarning("Se eliminó el Expediente: " + IdActo + ", " + item.Nombre);
            return Accepted(IdExpediente);
        }
    }
}
