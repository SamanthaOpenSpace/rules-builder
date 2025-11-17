using System;
using System.Collections.Generic;
using System.Linq;

namespace Prosperity.Api.Infrastructure.RulesEngine.Models;

public class CptCodeOutput
{
    public CptCodeOutput()
    {
        CptCodes = new List<string>();
    }

    public CptCodeOutput(List<string> cptCodes)
    {
        CptCodes = cptCodes;
    }

    public CptCodeOutput(string cptCodes)
    {
        if (string.IsNullOrWhiteSpace(cptCodes))
        {
            CptCodes = new List<string>();
        }
        else
        {
            CptCodes = cptCodes.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(code => code.Trim())
                .Where(code => !string.IsNullOrWhiteSpace(code))
                .ToList();
        }
    }

    public List<string> CptCodes { get; set; } = new();
}
