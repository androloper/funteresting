using LanguagePackageAPI.Methods;
using LanguagePackageAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace LanguagePackageAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LanguageController : ControllerBase
    {
        [HttpGet("GetUpdateInfo")]
        public IActionResult GetUpdateInfo()
        {
            try
            {
                var result = LanguageBll.GetUpdateInfo();
                if (result != null)
                {
                    return Ok(result);
                }
                else
                    return NotFound("Check database and/or backend.");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpGet("GetWords")]
        public IActionResult GetWords()
        {
            try
            {
                var result = LanguageBll.GetWords();
                if (result != null)
                {
                    return Ok(result);
                }
                else
                    return NotFound("Check database and/or backend.");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("GetWordsByLanguageCode")]
        public IActionResult GetWordsByLanguageCode(string languageCode)
        {
            try
            {
                var result = LanguageBll.GetWordsByLanguageCode(languageCode);
                if (result != null)
                {
                    return Ok(result);
                }
                else
                    return NotFound("Check database and/or backend.");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
      
        [HttpGet("GetNewWordsByLatestUpdate")]
        public IActionResult GetNewWordsByLatestUpdate(DateTime latestUpdate)
        {
            try
            {
                var result = LanguageBll.GetNewWordsByLatestUpdate(latestUpdate);
                if (result != null)
                {
                    return Ok(result);
                }
                else
                    return NotFound("Check database and/or backend.");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("GetAddedKeyNameList")]
        public IActionResult GetAddedKeyNameList()
        {
            try
            {
                var result = LanguageBll.GetAddedKeyNameList();
                if (result != null)
                {
                    return Ok(result);
                }
                else
                    return NotFound("Check database and/or backend.");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpGet("IsThisKeyNameAdded")]
        public IActionResult IsThisKeyNameAdded(string keyName)
        {
            try
            {
                var result = LanguageBll.IsThisKeyNameAdded(keyName);
                if (result != null)
                {
                    return Ok(result);
                }
                else
                    return NotFound("Check database and/or backend.");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpGet("GetValuesByKeyName")]
        public IActionResult GetValuesByKeyName(string keyName)
        {
            try
            {
                var result = LanguageBll.GetValuesByKeyName(keyName);
                if (result != null)
                {
                    return Ok(result);
                }
                else
                    return NotFound("Check database and/or backend.");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("AddNewWordToAllLanguages")]
        public IActionResult AddNewWordToAllLanguages([FromBody] SendKeyModel model)
        {
            try
            {
                var result = LanguageBll.AddNewWordToAllLanguages(model.KeyName, model.TrValue, model.EnValue, model.DeValue);
                if (result != null)
                {
                    return Ok(result);
                }
                else
                    return NotFound("The word hasn't been added.");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("AddNewWordByLanguageCode")]
        public IActionResult AddNewWordByLanguageCode(string keyName, string value, string languageCode)
        {
            try
            {
                var result = LanguageBll.AddNewWordByLanguageCode(keyName, value, languageCode);
                if (result != null)
                {
                    return Ok(result);
                }
                else
                    return NotFound("The word hasn't been added.");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        private readonly IWebHostEnvironment _env;

        public LanguageController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpGet("{fileName}")]
        public async Task<IActionResult> GetMarkdownFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName) || !fileName.EndsWith(".md"))
            {
                return BadRequest("Invalid file name");
            }

            var filePath = Path.Combine(_env.ContentRootPath, "MarkdownFiles", fileName);
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var content = await System.IO.File.ReadAllTextAsync(filePath);
            return Ok(content);
        }

    }
}
