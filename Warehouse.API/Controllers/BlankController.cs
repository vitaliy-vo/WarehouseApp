using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Warehouse.BLL;
using Warehouse.Core.InputModels;
using Warehouse.Core.OutPutModels;

namespace Warehouse.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BlankController : ControllerBase
    {
        private BlankServise _blankServise;

        public BlankController(BlankServise blankServise)
        {
            _blankServise = blankServise;
        }

        [HttpGet("all", Name = "Получить все заготовки")]
        public ActionResult<IEnumerable<BlankOutPutModel>> GetAll()
        {
            var result = _blankServise.GetAll();

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public ActionResult<BlankOutPutModel> GetById(int id)
        {
            try
            {
                var result = GetBlankById(id);
                return Ok(result);
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }

        private object GetBlankById(int id)
        {
            var tmp = _blankServise.GetAll();

            var result = tmp.Single(p => p.Id == id);

            return result;
        }

        [HttpPost]
        public ActionResult<BlankInputModel> Add(BlankInputModel blank)
        {
            var result = _blankServise.Add(blank);

            return Ok(result);
        }

    }
}
